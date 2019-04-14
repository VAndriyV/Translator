using Microsoft.Extensions.DependencyInjection;
using Domain.Repositories;
using DataEFCore.Repositories;
using Domain.Translator;
using Domain.Services.Interfaces;
using Domain.Services;
using TranslatorLogic.SyntaxAnalyzer;
using TranslatorLogic.LexicalAnalyzer;
using TranslatorLogic.RelationManager;
using Domain.Util.Interfaces;
using Domain.Util;
using TranslatorLogic.ReversePolishNotation;
using TranslatorLogic.Executor;
using TranslatorLogic.Hash;

namespace ReactTranslator.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAutomaticRuleRepository, AutomaticRuleRepository>()
                .AddTransient<ILexemeClassRepository, LexemeClassRepository>()
                .AddTransient<ILexemeRepository, LexemeRepository>()
                .AddTransient<IOutputIdnRepository, OutputIdnRepository>()
                .AddTransient<IOutputConstantRepository, OutputConstantRepository>()
                .AddTransient<IOutputLexemeRepository, OutputLexemeRepository>();

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<ILexemeService, LexemeService>()
                .AddTransient<IAutomaticConfigurationService, AutomaticConfigurationService>()
                .AddTransient<ICharacterClassDeterminant, CharacterClassDeterminant>()
                .AddTransient<IFileService, FileService>();

            return services;
        }

        public static IServiceCollection ConfigureTranslatorComponents(this IServiceCollection services)
        {
            services.AddTransient<IAutomaticAnalyzer, AutomaticAnalyzer>()
                .AddTransient<ILexicalAnalyzer,StateDiagramLexicalAnalyzer>()
                .AddTransient<IRelationManager, RelationManager>()
                .AddTransient<IRecursiveAnalyzer, RecursiveAnalyzer>()
                .AddTransient<IAscendingAnalyzer, AscendingAnalyzer>()
                .AddSingleton<IRPNExpressionBuilder, RPNExpressionBuilder>()
                .AddTransient<IRPNExpressionCalculator, RPNExpressionCalculator>()
                .AddTransient<IRPNBuilder, RPNBuilder>()
                .AddSingleton<IExecutor,Executor>()
                .AddTransient<IHashManager,HashManager>();
               

            return services;
        }
    }
}
