import React, { Component } from 'react';
import InputDataForm from '../InputDataForm/InputDataForm';
import TranslatorService from '../../services/TranslatorService';
import LexicalAnalyzeTablesSet from '../LexicalAnalyzeTablesSet/LexicalAnalyzeTablesSet';
import ErrorsInfo from '../ErrorsInfo/ErrorsInfo';


export default class LexicalAnalyzerPage extends Component {

    state = {
        hasError: false,
        errorMsg: null,
        analyzeResult: null
    };

    translatorService = new TranslatorService();

    analyzeType = "Lexical analyze";   

    doLexicalAnalyze = (sourceCode) => {
        const result = this.translatorService.getLexicalAnalyzeResult(sourceCode);
        result.then((r) => {
            console.log(r);
            this.setState({
                analyzeResult: r,
                hasError:false
            });
        })
            .catch((e) => {
                console.log(e);
                this.setState({
                    errorMsg: e,
                    hasError: true
                });
            });
    }



    render() {

        const content = (this.state.hasError)
            ? <ErrorsInfo errors={this.state.errorMsg} />
            : <LexicalAnalyzeTablesSet analyzeResult={this.state.analyzeResult} />;

        return (
            <div>
                <InputDataForm handleSubmit={this.doLexicalAnalyze} analyzeType={this.analyzeType} />                
                {content}
            </div>
            );
    }
}
