import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from '../Layout/Layout';
import TranslatorService from '../../services/TranslatorService';
import LexicalAnalyzerPage from '../LexicalAnalyzerPage/LexicalAnalyzerPage';
import RecursiveAnalyzerPage from '../RecursiveAnalyzerPage/RecursiveAnalyzerPage';
import AutomaticAnalyzerPage from '../AutomaticAnalyzerPage/AutomaticAnalyzerPage'; 
import RelationsPage from '../RelationsPage/RelationsPage';
import AscendingAnalyzerPage from '../AscendingAnalyzerPage/AscendingAnalyzerPage';
import GrammarAndRelations from '../GrammarAndRelations/GrammarAndRelations';
import PRNExpressionPage from '../PRNExpressionPage/PRNExpressionPage';
import RPNBuilderPage from '../RPNBuilderPage/RPNBuilderPage';
import ExecutorPage from '../ExecutorPage/ExecutorPage';

export default class App extends Component {
    displayName = App.name;

    translatorService = new TranslatorService();

    doLexicalAnalyze = (sourceCode) => {
        var result = this.translatorService.getLexicalAnalyzeResult(sourceCode);
        result.then((r) => console.log(r)).catch((c) => console.log(c));
    }

    doAutomaticAnalyze = (sourceCode) => {
        var result = this.translatorService.getAutomaticAnalyzeResult(sourceCode);
        result.then((r) => console.log(JSON.stringify(r))).catch((c) => console.log(c));
    }

    doRecursiveAnalyze = (sourceCode) => {
        var result = this.translatorService.getRecursiveAnalyzeResult(sourceCode);
        result.then((r) => console.log(r)).catch((c) => console.log(c));
    }

    setRelations = (grammarText) => {
        var result = this.translatorService.getRelationTable(grammarText);
        result.then((r) => console.log(JSON.stringify(r))).catch((c) => console.log(c));
    }   
    
    render() {
        return (
            <Layout>
                <Route path="/lexicalAnalyzer" component={LexicalAnalyzerPage} />
                <Route path="/recursiveAnalyzer" component={RecursiveAnalyzerPage} />
                <Route path="/automaticAnalyzer" component={AutomaticAnalyzerPage} />
                <Route path="/relationsTable" component={RelationsPage} />
                <Route path="/ascendingAnalyzer" component={AscendingAnalyzerPage} />
                <Route path="/grammarAndRelation/:type" component={GrammarAndRelations} />
                <Route path="/rpnExpression" component={PRNExpressionPage} />
                <Route path="/rpnBuilder" component={RPNBuilderPage} />
                <Route path="/executeProgram" component={ExecutorPage} />
            </Layout>
        );
    }
}
