using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.Util.Interfaces;

namespace Domain.Util
{
    public class CharacterClassDeterminant : ICharacterClassDeterminant
    {
        private readonly ILexemeClassRepository _lexemeClassRepository;

        public CharacterClassDeterminant(ILexemeClassRepository lexemeClassRepository)
        {
            _lexemeClassRepository = lexemeClassRepository;
        }

        public async Task<bool> IsLetter(char ch)
        {
            var lexemeClass = await _lexemeClassRepository.Find(c => c.Name.Equals("Letter"));
            return lexemeClass.Values.Contains(ch);
        }
        public async Task<bool> IsDigit(char ch)
        {
            var lexemeClass = await _lexemeClassRepository.Find(c => c.Name.Equals("Digit"));
            return lexemeClass.Values.Contains(ch);
        }
        public async Task<bool> IsExclamation(char ch)
        {
            var lexemeClass = await _lexemeClassRepository.Find(c => c.Name.Equals("Exclamation"));            
            return lexemeClass.Values.Contains(ch);
        }
        public async Task<bool> IsLess(char ch)
        {
            var lexemeClass = await _lexemeClassRepository.Find(c => c.Name.Equals("Less"));
            return lexemeClass.Values.Contains(ch);
        }
        public async Task<bool> IsMore(char ch)
        {
            var lexemeClass = await _lexemeClassRepository.Find(c => c.Name.Equals("More"));
            return lexemeClass.Values.Contains(ch);
        }
        public async Task<bool> IsEqual(char ch)
        {
            var lexemeClass = await _lexemeClassRepository.Find(c => c.Name.Equals("Equal"));
            return lexemeClass.Values.Contains(ch);
        }
        public async Task<bool> IsSeparator(char ch)
        {

            if (ch.Equals('\n'))
            {
                var lexemeClassNewLine = await _lexemeClassRepository.Find(c => c.Name.Equals("Separator"));
                return lexemeClassNewLine.Values.Contains("\\n");
            }
            var lexemeClass = await _lexemeClassRepository.Find(c => c.Name.Equals("Separator"));
            return lexemeClass.Values.Contains(ch);
        }
        public async Task<bool> IsSign(char ch)
        {
            var lexemeClass = await _lexemeClassRepository.Find(c => c.Name.Equals("Sign"));
            return lexemeClass.Values.Contains(ch);
        }
    }
}
