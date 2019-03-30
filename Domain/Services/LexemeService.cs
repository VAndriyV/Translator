using Domain.Services.Interfaces;
using Domain.Util.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Repositories;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ViewModels;
using AutoMapper.QueryableExtensions;
using Domain.Converters;

namespace Domain.Services
{
    public class LexemeService : ILexemeService
    {    
        private readonly ILexemeRepository _lexemeRepository;

        private readonly IOutputConstantRepository _constantRepository;

        private readonly IOutputIdnRepository _idnRepository;

        private readonly IOutputLexemeRepository _outputLexemeRepository;

        private const string CONSTANT = "CON";

        private const string IDN = "IDN";

        public LexemeService(ILexemeRepository lexemeRepository
            ,IOutputConstantRepository constantRepository
            ,IOutputIdnRepository idnRepository
            ,IOutputLexemeRepository outputLexemeRepository)
        {
            _lexemeRepository = lexemeRepository;
            _constantRepository = constantRepository;
            _idnRepository = idnRepository;
            _outputLexemeRepository = outputLexemeRepository;
        }

        public async Task<bool> IsKeyWord(string lexeme)
        {
            return await _lexemeRepository.LexemeExist(lexeme);
        }

        public async Task<bool> IsInOutputIdns(string idn)
        {
            return await _idnRepository.IdnExist(idn);
        }

        public async Task<bool> IsInOutputConstants(string constant)
        {
            return await _constantRepository.ConstantExist(constant);
        }

        public async Task<bool> IsVariableType(string lexeme)
        {
            var lex = await _lexemeRepository.Find(c => c.Name.Equals(lexeme));
            if (lex == null)
                return false;
            return (lex.Id == 1 || lex.Id == 2);
        }

        public async Task<int> GetIdnCode(string idn)
        {
            return await _idnRepository.GetCode(idn);
        }

        public async Task AddToOutputIdns(string idn, string type, int row, bool isAlreadyAdded)
        {            

            var idnEntity = new OutputIdn
            {
                Name = idn,
                Type = type
            };

            if (!isAlreadyAdded)
            {
                await _idnRepository.Add(idnEntity);
            }

            var lexeme = new OutputLexeme
            {
                LexemeName = idn,
                Idncode = await GetIdnCode(idn),
                Row = row,
                LexemeId = await GetLexemeCode(IDN)
            };

            await _outputLexemeRepository.Add(lexeme);
        }

        public async Task AddToOutputLexemes(string lexeme, int row)
        {

            var lexemeEntity = new OutputLexeme
            {
                LexemeName = lexeme,
                Row = row
            };
            if (lexeme == "¶")
            {
                lexemeEntity.LexemeId = await GetLexemeCode("\\n");
            }
            else
            {
                lexemeEntity.LexemeId = await GetLexemeCode(lexeme);
            }

            await _outputLexemeRepository.Add(lexemeEntity);
        }
        public async Task<int> GetLexemeCode(string lexeme)
        {
            return await _lexemeRepository.GetCode(lexeme);
        }
        
        public async Task<int> GetConstantCode(string constant)
        {
            return await _constantRepository.GetCode(constant);
        }
        public async Task AddToOutputConstants(string constant, int row, bool isAlreadyAdded)
        {          
            var constantEntity = new OutputConstant
            {
                Name = constant
            };

            if (!isAlreadyAdded)
            {
                await _constantRepository.Add(constantEntity);
            }

            var lexeme = new OutputLexeme
            {
                LexemeName = constant,
                ConstantCode = await GetConstantCode(constant),
                Row = row,
                LexemeId = await GetLexemeCode(CONSTANT)
            };

            await _outputLexemeRepository.Add(lexeme);
        }
        public async Task<bool> IsInDeclatationSegment()
        {
            var lexeme = await _outputLexemeRepository.Find(c => c.LexemeName.Equals("{"));
            return  lexeme == null;
        }
        public async Task<LAResult> GetResultTables()
        {
            LAResult result = new LAResult();

            var outputLexemes = await _outputLexemeRepository.GetAll();
            var outputIdns = await _idnRepository.GetAll();
            var outputConstants = await _constantRepository.GetAll();

            result.OutputLexemes = AutoMapperListConventer
                .MapList<OutputLexeme, OutputLexemeViewModel>(outputLexemes);
            result.OutputIdns = AutoMapperListConventer
                .MapList<OutputIdn, OutputIdnViewModel>(outputIdns);
            result.OutputConstants = AutoMapperListConventer
                .MapList<OutputConstant, OutputConstantViewModel>(outputConstants);

            return result;
        }
        public async Task<bool> IsNextSignBinary()
        {
            var lexeme =await _outputLexemeRepository.GetLast();
            return (lexeme!=null&&(lexeme.Idncode != null || lexeme.ConstantCode != null || lexeme.LexemeName.Equals(")")));
        }

        public async Task ClearOutputTables()
        {
            await _outputLexemeRepository.DeleteAllRows();
            await _constantRepository.DeleteAllRows();
            await _idnRepository.DeleteAllRows();
        }
    }
}
