using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataEFCore
{
    public interface IDbContext
    {
         DbSet<AutomaticRule> AutomaticRules { get; set; }
         DbSet<LexemeClass> LexemeClasses { get; set; }
         DbSet<Lexeme> Lexemes { get; set; }
         DbSet<OutputConstant> OutputConstants { get; set; }
         DbSet<OutputIdn> OutputIdns { get; set; }
         DbSet<OutputLexeme> OutputLexemes { get; set; }
         Task TruncateTable(string tableName);
         Task SaveChangesAsync();
    }
}
