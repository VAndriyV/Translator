using Domain.Exceptions;
using Domain.Services.Interfaces;
using Domain.Translator;
using Domain.Util.Interfaces;
using Domain.ViewModels;
using System.Threading.Tasks;


namespace TranslatorLogic.LexicalAnalyzer
{
    public class StateDiagramLexicalAnalyzer : ILexicalAnalyzer
    {
        private readonly ILexemeService _lexemeService;

        private readonly ICharacterClassDeterminant _characterClassDeterminant;

        private int _position;

        private bool _hasToRead;

        private char _ch;

        private string _lexeme;

        private int _currentRow;

        private string _lastVariableType;

        private const string NEW_LINE_SYMBOL = "¶";

        public StateDiagramLexicalAnalyzer(ILexemeService lexemeService
            ,ICharacterClassDeterminant characterClassDeterminant)
        {
            _lexemeService = lexemeService;
            _characterClassDeterminant = characterClassDeterminant;
            _position = 0;
            _hasToRead = true;
            _lexeme = string.Empty;
            _currentRow = 1;
        }

        public async Task<LAResult> DoLexicalAnalyze(string sourceCode)
        {
            await _lexemeService.ClearOutputTables();

            if (sourceCode.Trim().Equals(""))
            {
                throw new LexicalAnalyzeException("Lexical analyze error! ", "No source code");
            }

            try
            {
                while (_position < sourceCode.Length)
                {
                   await FirstState(sourceCode);
                }
            }
            catch (LexicalAnalyzeException e)
            {
                await _lexemeService.ClearOutputTables();
                throw e;
            }

            return await _lexemeService.GetResultTables();
        }

        private async Task FirstState(string code)
        {
            if (_hasToRead)
            {
                _ch =  code[_position];
            }

            while (_position < code.Length && (code[_position].Equals(' ') || code[_position].Equals('\r')))
                _position++;

            if (_position >= code.Length)
            {
                return;
            }

            _ch =  code[_position];
            _lexeme = "";

            if (await _characterClassDeterminant.IsLetter(_ch))
            {
                _position++;
                _lexeme += _ch;                
                await SecondState(code);
            }

            else if (await _characterClassDeterminant.IsDigit(_ch))
            {
                _position++;
                _lexeme += _ch;                
                await ThirdState(code);
            }

            else if (await _characterClassDeterminant.IsSeparator(_ch))
            {
                _lexeme += _ch;
                _hasToRead = true;
                _position++;
                if (_ch.Equals('\n'))
                {
                    await _lexemeService.AddToOutputLexemes(NEW_LINE_SYMBOL, _currentRow);
                    _currentRow++;
                }
                else
                {
                    await _lexemeService.AddToOutputLexemes(_lexeme, _currentRow);
                }
                    
            }

            else if (await _characterClassDeterminant.IsLess(_ch))
            {
                _position++;
                _lexeme += _ch;
                await FourthState(code);
                _hasToRead = true;
            }

            else if (await _characterClassDeterminant.IsMore(_ch))
            {
                _position++;
                _lexeme += _ch;
                await FifthState(code);
                _hasToRead = true;
            }

            else if (await _characterClassDeterminant.IsEqual(_ch))
            {
                _position++;
                _lexeme += _ch;
                await SixthState(code);
                _hasToRead = true;
            }

            else if (await _characterClassDeterminant.IsExclamation(_ch))
            {
                _position++;
                _lexeme += _ch;
                await SeventhState(code);
                _hasToRead = true;
            }
            else if (await _characterClassDeterminant.IsSign(_ch))
            {
                _position++;
                _lexeme += _ch;
                await EighthState(code);
            }
            else
            {
                var status = "Can not recognize the characterClass: " + _ch + " Line: " + _currentRow + ".\n";
                throw new LexicalAnalyzeException("Lexical analyze error! ", status);
            }

        }

        private async Task SecondState(string code)
        {
            if (_position < code.Length 
                && (await _characterClassDeterminant.IsLetter(code[_position]) 
                || await _characterClassDeterminant.IsDigit(code[_position])))
            {                
                _ch =  code[_position];
                _position++;
                _lexeme += _ch;
                await SecondState(code);
            }
            else
            {
                if (await _lexemeService.IsVariableType(_lexeme))
                {
                    _lastVariableType = _lexeme;
                }
                if (!await _lexemeService.IsKeyWord(_lexeme))
                {
                    if (await _lexemeService.IsInDeclatationSegment())
                    {
                        if (!await _lexemeService.IsInOutputIdns(_lexeme))
                        {
                            await _lexemeService.AddToOutputIdns(_lexeme, _lastVariableType, _currentRow, false);
                        }
                        else
                        {
                            var status = "Dubplicate identify declaration: " + _lexeme + " Line: " + _currentRow + ".\n";
                            throw new LexicalAnalyzeException("Lexical analyze error! ", status);
                        }
                    }
                    else if (!await _lexemeService.IsInOutputIdns(_lexeme))
                    {
                        var status = "Undefined identify used: " + _lexeme + " Line: " + _currentRow + ".\n";
                        throw new LexicalAnalyzeException("Lexical analyze error!", status);
                    }
                    else
                    {
                        await _lexemeService.AddToOutputIdns(_lexeme, _lastVariableType,_currentRow,true);
                    }
                }
                else
                {
                    await _lexemeService.AddToOutputLexemes(_lexeme, _currentRow);
                }
                _hasToRead = false;

            }
        }

        private async Task ThirdState(string code)
        {
            if (_position < code.Length 
                && await _characterClassDeterminant.IsDigit(code[_position]))
            {                
                _ch =  code[_position];
                _position++;
                _lexeme += _ch;
                await ThirdState(code);
            }
            else
            {
                var isAlreadyAdded = await _lexemeService.IsInOutputConstants(_lexeme);
                await _lexemeService.AddToOutputConstants(_lexeme, _currentRow,isAlreadyAdded);
                _hasToRead = false;
            }
        }

        private async Task FourthState(string code)
        {
            if (_position < code.Length 
                && (await _characterClassDeterminant.IsLess(code[_position]) 
                || await _characterClassDeterminant.IsEqual(code[_position])))
            {
                _ch =  code[_position];
                _position++;
                _lexeme += _ch;
            }

            await _lexemeService.AddToOutputLexemes(_lexeme, _currentRow);
        }

        private async Task FifthState(string code)
        {
            if (_position < code.Length 
                && (await _characterClassDeterminant.IsMore(code[_position]) 
                || await _characterClassDeterminant.IsEqual(code[_position])))
            {
                _ch =  code[_position];
                _position++;
                _lexeme += _ch;
            }

            await _lexemeService.AddToOutputLexemes(_lexeme, _currentRow);
        }

        private async Task SixthState(string code)
        {
            if (_position < code.Length 
                && await _characterClassDeterminant.IsEqual(code[_position]))
            {
                _ch =  code[_position];
                _position++;
                _lexeme += _ch;
            }

            await _lexemeService.AddToOutputLexemes(_lexeme, _currentRow);
        }

        private async Task SeventhState(string code)
        {
            if (_position < code.Length 
                && await _characterClassDeterminant.IsEqual(code[_position]))
            {
                _ch =  code[_position];
                _position++;
                _lexeme += _ch;
                await _lexemeService.AddToOutputLexemes(_lexeme, _currentRow);
            }
            else
            {
                var status = "Can not recognize the lexeme: " + _lexeme + code[_position] + " Line: " + _currentRow + ".\n";
                throw new LexicalAnalyzeException("Lexical analyze error! ", status);
            }
        }

        private async Task EighthState(string code)
        {
            if (_position < code.Length
                && !await _lexemeService.IsNextSignBinary()
                && await _characterClassDeterminant.IsDigit(code[_position]))
            {
                await ThirdState(code);
            }
            else
            {
                await _lexemeService.AddToOutputLexemes(_lexeme, _currentRow);
            }

            _hasToRead = true;
        }       
        
    }
}
