using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Translator;
using Domain.ViewModels;
using Domain.Exceptions;
using Domain.Converters;
using Domain.Entities;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Domain.Util;
using TranslatorLogic.ReversePolishNotation;
using TranslatorLogic.LexicalAnalyzer;

namespace ReactTranslator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class TranslatorController : Controller
    {
        private readonly ILexicalAnalyzer _lexicalAnalyzer;

        private readonly IRecursiveAnalyzer _recursiveAnalyzer;

        private readonly IAutomaticAnalyzer _automaticAnalyzer;

        private readonly IRelationManager _relationManager;

        private readonly IAscendingAnalyzer _ascendingAnalyzer;

        private readonly IAutomaticConfigurationService _automaticConfiguration;

        private readonly IRPNExpressionBuilder _RPNExpressionBuilder;

        private readonly IRPNExpressionCalculator _RPNExpressionCalculator;

        private readonly IHostingEnvironment _enviroment;

        private readonly IFileService _fileService;

        private readonly IRPNBuilder _RPNBuilder;

        private const string FULL_GRAMMAR_TEXT_FILE = "GrammarConfiguration.txt";

        private const string EXPRESSION_FULL_GRAMMAR_TEXT_FILE = "ExpressionGrammar.txt";


        public TranslatorController(ILexicalAnalyzer lexicalAnalyzer
            , IRecursiveAnalyzer recursiveAnalyzer
            , IAutomaticAnalyzer automaticAnalyzer
            , IRelationManager relationManager
            , IAutomaticConfigurationService automaticConfiguration
            , IHostingEnvironment environment
            , IFileService fileService
            , IAscendingAnalyzer ascendingAnalyzer
            ,IRPNExpressionBuilder RPNExpressionBuilder
            ,IRPNExpressionCalculator RNPExpressionCalculator
            ,IRPNBuilder RPNBuilder)
        {
            _lexicalAnalyzer = lexicalAnalyzer;
            _recursiveAnalyzer = recursiveAnalyzer;
            _automaticAnalyzer = automaticAnalyzer;
            _relationManager = relationManager;
            _automaticConfiguration = automaticConfiguration;
            _enviroment = environment;
            _fileService = fileService;
            _ascendingAnalyzer = ascendingAnalyzer;
            _RPNExpressionBuilder = RPNExpressionBuilder;
            _RPNExpressionCalculator = RNPExpressionCalculator;
            _RPNBuilder = RPNBuilder;
        }       

        [HttpGet("Index")]
        public IActionResult Index()
        {
            string webRootPath = _enviroment.WebRootPath;
            string contentRootPath = _enviroment.ContentRootPath;

            return Content(webRootPath + "\n" + contentRootPath);
        }

        [HttpGet("LoadFile/{fileName}")]
        public async Task<string> LoadFile(string fileName)
        {
            string webRootPath = _enviroment.WebRootPath;

            var path = webRootPath + "\\Files\\" + fileName;

            var result = await _fileService.ReadTextFromFile(path);

            return result;
        }

        [HttpPost("LexicalAnalyzeResult")]       
        public async Task<IActionResult> LexicalAnalyzeResult([FromBody] string sourceCode)
        {                   
            try
            {
                return new ObjectResult(await LexicalAnalyze(sourceCode));
            }
            catch(LexicalAnalyzeException laEx)
            {
                var exInfo = new ErrorDetails { Message = laEx.Message
                        , AnalyzeErrors = laEx.AnalyzeErrors};              
                return StatusCode(400, exInfo);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("RecursiceAnalyzeResult")]
        public async Task<IActionResult> RecursiceAnalyzeResult([FromBody] string sourceCode)
        {
            try
            {
                var laResult = await LexicalAnalyze(sourceCode);

                var outputLexemes = AutoMapperListConventer
                    .MapList<OutputLexemeViewModel, OutputLexeme>(laResult.OutputLexemes);

                _recursiveAnalyzer.DoAnalyze(outputLexemes);

                return new ObjectResult(laResult);

            }
            catch (LexicalAnalyzeException laEx)
            {
                var exInfo = new ErrorDetails { Message = laEx.Message
                        , AnalyzeErrors = laEx.AnalyzeErrors};

                return StatusCode(400, exInfo);
            }
            catch(SyntaxAnalyzeException saEx)
            {
                var exInfo = new ErrorDetails { Message = saEx.Message
                        , AnalyzeErrors = saEx.AnalyzeErrors};

                return StatusCode(400, exInfo);
            }
        }

        [HttpPost("AutomaticAnalyzeResult")]
        public async Task<IActionResult> AutomaticAnalyzeResult([FromBody] string sourceCode)
        {
            try
            {
                var laResult = await LexicalAnalyze(sourceCode);

                var outputLexemes = AutoMapperListConventer
                    .MapList<OutputLexemeViewModel, OutputLexeme>(laResult.OutputLexemes);             
                
                var automaticSteps = await _automaticAnalyzer.DoAnalyze(outputLexemes);

                var analyzeResult = new AutomaticAnalyzerResult { LAResult = laResult
                    , AutomaticSteps = automaticSteps };

                return new ObjectResult(analyzeResult);
            }
            catch (LexicalAnalyzeException laEx)
            {
                var exInfo = new ErrorDetails { Message = laEx.Message
                        , AnalyzeErrors = laEx.AnalyzeErrors};
                
                return StatusCode(400, exInfo);
            }
            catch (SyntaxAnalyzeException saEx)
            {
                var exInfo = new ErrorDetails { Message = saEx.Message
                        , AnalyzeErrors = saEx.AnalyzeErrors};

                return StatusCode(400, exInfo);
            }
        }

        [HttpPost("AutomaticConfiguration")]
        public async Task<IActionResult> AutomaticConfiguration()
        {
            return new ObjectResult(await _automaticConfiguration.GetAutomaticRules());
        }

        [HttpPost("RelationTable")]
        public async Task<IActionResult> RelationTable([FromBody] string grammarText)
        {
            var relationTableData = await SetRelations(grammarText);

            return new ObjectResult(relationTableData);            
        }

        [HttpPost("AscendingAnalyzeResult")]
        public async Task<IActionResult> AscendingAnalyzer([FromBody] string sourceCode)
        {

            var grammarText = await ReadGrammar(FULL_GRAMMAR_TEXT_FILE);

            _relationManager.SetRelations(grammarText);

            if (_relationManager.IsGrammarCorrect())
            {
                LAResult laResult;
                try
                {
                    laResult = await _lexicalAnalyzer.DoLexicalAnalyze(sourceCode);
                }
                catch (LexicalAnalyzeException laEx)
                {
                    var exInfo = new ErrorDetails { Message = laEx.Message
                        , AnalyzeErrors = laEx.AnalyzeErrors};

                    return StatusCode(400, exInfo);
                }

                var outputLexemes = AutoMapperListConventer
                    .MapList<OutputLexemeViewModel, OutputLexeme>(laResult.OutputLexemes);

                outputLexemes.ForEach((l) =>
                {
                    if (l.LexemeId == 100)
                    {
                        l.LexemeName = "IDN";
                    }
                    if (l.LexemeId == 101)
                    {
                        l.LexemeName = "CON";
                    }
                });

                try
                {
                    var digitRelationMatrix = 
                        RelationMatrixTransformer.GetTransformedRelationMatrix(_relationManager.GetRelationMatrix());

                    var ascendingAnalyzeTable = await _ascendingAnalyzer
                        .DoSyntaxAnalyze(_relationManager.GetGrammar()
                        , _relationManager.GetAllTerms(),digitRelationMatrix, outputLexemes);

                    var ascendingAnalyzeResult = new AscendingAnalyzerResult
                    {
                        LAResult = laResult                   
                        ,AscendingAnalyzerTable = ascendingAnalyzeTable
                    };

                    return new ObjectResult(ascendingAnalyzeResult);
                }
                catch(SyntaxAnalyzeException saEx)
                {
                    var exInfo = new ErrorDetails { Message = saEx.Message
                        , AnalyzeErrors = saEx.AnalyzeErrors};

                    return StatusCode(400, exInfo);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            var grammarErrorMsg = "Grammar is not correct! ";

            return StatusCode(400,grammarErrorMsg);

        }

        [HttpPost("BuildExpressionRPN")]
        public async Task<IActionResult> RPNExpressionBuilder([FromBody] string expression)
        {           

            var grammarText = await ReadGrammar(EXPRESSION_FULL_GRAMMAR_TEXT_FILE);

            _relationManager.SetRelations(grammarText);

            var digitRelationMatrix =
                        RelationMatrixTransformer.GetTransformedRelationMatrix(_relationManager.GetRelationMatrix());
            try
            {
                LAResult laResult = null;                
                try
                {
                   laResult = await LexicalAnalyze(expression);
                }
                catch(LexicalAnalyzeException laEx)
                {
                    var exInfo = new ErrorDetails
                    {
                        Message = laEx.Message
                       ,
                        AnalyzeErrors = laEx.AnalyzeErrors
                    };

                    return StatusCode(400, exInfo);
                }

                var outputLexemes = AutoMapperListConventer
                    .MapList<OutputLexemeViewModel, OutputLexeme>(laResult.OutputLexemes);

                _RPNExpressionBuilder.BuildRPN(_relationManager.GetGrammar(),
                    _relationManager.GetAllTerms(), digitRelationMatrix, outputLexemes);               

                var result = new RPNExpressionBuilderResult
                {
                    ResultTable = _RPNExpressionBuilder.GetResultTable(),
                    Idns = _RPNExpressionBuilder.GetOutputIdns()
                };

                return new ObjectResult(result);
                
            }
            catch(RPNException ex)
            {
                var exInfo = new ErrorDetails
                {
                    Message = ex.Message,
                      
                    AnalyzeErrors = ex.ErrorDetails
                };

                return StatusCode(400, exInfo);
            }          
           
        }

        [HttpPost("CalculateExpressionRPN")]
        public async Task<IActionResult> RPNCalculator([FromBody] Dictionary<string,int> variables)
        {           
            var calculationResult =  _RPNExpressionCalculator.Calculate(_RPNExpressionBuilder.GetRPN()
                , variables, _RPNExpressionBuilder.GetOutputIdns());

            var result = new { calculationResult
                , calculationTable=_RPNExpressionCalculator.GetResultTable() };
            
            return new ObjectResult(result);
        }

        [HttpPost("BuildRPN")]
        public async Task<IActionResult> RPNBuilder([FromBody] string sourceCode)
        {
            try
            {
                var laResult = await LexicalAnalyze(sourceCode);

                var outputLexemes = AutoMapperListConventer
                    .MapList<OutputLexemeViewModel, OutputLexeme>(laResult.OutputLexemes);

                _recursiveAnalyzer.DoAnalyze(outputLexemes);

                _RPNBuilder.BuildRPN(outputLexemes);

                string singleLineRPN = string.Join(" ", _RPNBuilder.GetRPN());

                var result = new RPNBuilderResult
                {
                    LaResult = laResult,
                    RPNBuilderTable = _RPNBuilder.GetResultTable(),
                    SingleLineRPN = singleLineRPN
                };

                return new ObjectResult(result);

            }
            catch (LexicalAnalyzeException laEx)
            {
                var exInfo = new ErrorDetails
                {
                    Message = laEx.Message
                        ,
                    AnalyzeErrors = laEx.AnalyzeErrors
                };

                return StatusCode(400, exInfo);
            }
            catch (SyntaxAnalyzeException saEx)
            {
                var exInfo = new ErrorDetails
                {
                    Message = saEx.Message
                        ,
                    AnalyzeErrors = saEx.AnalyzeErrors
                };

                return StatusCode(400, exInfo);
            }
        }

        [HttpGet("FullGrammar")]
        public async Task<IActionResult> FullGrammar()
        {
            var result = await GrammarAndRelation(FULL_GRAMMAR_TEXT_FILE);
            return new ObjectResult(result);
        }

        [HttpGet("ExpressionGrammar")]
        public async Task<IActionResult> ExpressionGrammar()
        {
            var result = await GrammarAndRelation(EXPRESSION_FULL_GRAMMAR_TEXT_FILE);
            return new ObjectResult(result);
        }

        private async Task<GrammarAndRelations> GrammarAndRelation(string grammarFileName)
        {

            var grammar = await ReadGrammar(grammarFileName);

            _relationManager.SetRelations(grammar);

            var grammarAndRelations = new GrammarAndRelations
            {
                Grammar = grammar,
                RelationTable = new RelationTableData
                {
                    AllTerms = _relationManager.GetAllTerms(),
                    RelationMatrix = _relationManager.GetRelationMatrix()
                }
            };

            return grammarAndRelations;

        }

        private async Task<RelationTableData> SetRelations(string grammarText)
        {
            _relationManager.SetRelations(grammarText);

            var relationMatrix = _relationManager.GetRelationMatrix();

            var allTerms = _relationManager.GetAllTerms();

            var relationTableData = new RelationTableData
            {
                RelationMatrix = relationMatrix,
                AllTerms = allTerms
            };

            return relationTableData;
        }

        private async Task<LAResult> LexicalAnalyze(string sourceCode)
        {
            var laResult =await _lexicalAnalyzer.DoLexicalAnalyze(sourceCode);

            return laResult;
        }

        private async Task<string> ReadGrammar(string fileName)
        {
            string webRootPath = _enviroment.WebRootPath;

            var path = webRootPath + "\\Files\\" + fileName;

            var grammarText = await _fileService.ReadTextFromFile(path);

            return grammarText;
        }
    }
}