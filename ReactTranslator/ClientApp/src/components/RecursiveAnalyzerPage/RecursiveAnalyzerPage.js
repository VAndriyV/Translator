import React, { Component } from 'react';
import InputDataForm from '../InputDataForm/InputDataForm';
import TranslatorService from '../../services/TranslatorService';
import LexicalAnalyzeTablesSet from '../LexicalAnalyzeTablesSet/LexicalAnalyzeTablesSet';
import ErrorsInfo from '../ErrorsInfo/ErrorsInfo';


export default class RecursiveAnalyzerPage extends Component {

    state = {
        hasError: false,
        errorMsg: null,
        laAnalyzeResult: null,
        showLAResult: false
    };

    translatorService = new TranslatorService();

    analyzeType = "Recursive analyze";

    doRecursiveAnalyze = (sourceCode) => {
        var result = this.translatorService.getRecursiveAnalyzeResult(sourceCode);
        result.then((r) => {
            console.log(r);
            this.setState({
                laAnalyzeResult: r,
                hasError: false
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

    displayLexicalAnalyzeTablesOption = () => {
        if (this.state.laAnalyzeResult !== null) {
            this.setState({
                showLAResult: !this.state.showLAResult
            });
        }
        else {
            alert("No lexical analyze result! Please,first of all, do analyze");
        }
    }

    render() {

        const content = (this.state.hasError)
            ? <ErrorsInfo errors={this.state.errorMsg} />
            : null;

        return (
            <div>
                <InputDataForm handleSubmit={this.doRecursiveAnalyze} analyzeType={this.analyzeType} />

                <div className="btn-group" role="group">
                 <button type="button" className="btn btn-info toggler" onClick={this.displayLexicalAnalyzeTablesOption}>
                        Show/hide lexical analyze tables</button>
                </div>
                
                {this.state.showLAResult
                    ? <LexicalAnalyzeTablesSet analyzeResult={this.state.laAnalyzeResult} />
                    : null}
                {content}                
            </div>
        );
    }
}
