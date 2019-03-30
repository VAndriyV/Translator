import React, { Component } from 'react';
import InputDataForm from '../InputDataForm/InputDataForm';
import TranslatorService from '../../services/TranslatorService';
import RPNBuilderTable from '../RPNBuilderTable/RPNBuilderTable';
import ErrorsInfo from '../ErrorsInfo/ErrorsInfo';
import LexicalAnalyzeTablesSet from '../LexicalAnalyzeTablesSet/LexicalAnalyzeTablesSet';

export default class RPNBuilderPage extends Component {

    state = {
        hasError: false,
        errorMsg: null,
        laResult: null,
        showLAResult: false,
        rpnResult: null,
        singleLineRPN: ''
    };

    translatorService = new TranslatorService();

    analyzeType = "Build Reverse Polish notation";

    tableTitle = 'Reverse Polish notation build';

    buildRPN = (sourceCode) => {
       
        let result = this.translatorService.getFullRPN(sourceCode);
        result.then((r) => {
            console.log(r);
            this.setState({
                hasError: false,
                errorMsg: null,
                laResult: r.laResult,
                rpnResult: r.rpnBuilderTable,
                singleLineRPN: r.singleLineRPN
            });
        })
            .catch((e) => {
                this.setState({
                    hasError: true,
                    errorMsg: e                   
                });
            });
    }

    displayLexicalAnalyzeTablesOption = () => {
        if (this.state.laResult !== null) {
            this.setState({
                showLAResult: !this.state.showLAResult
            });
        }
        else {
            alert("No lexical analyze result! Please,first of all, do successfull analyze");
        }
    }

    render() {

        const { hasError, errorMsg, laResult, rpnResult, singleLineRPN } = this.state;        
        const content = hasError
            ? <ErrorsInfo errors={errorMsg} />
            : null;      

        return (
            <React.Fragment>
                <InputDataForm handleSubmit={this.buildRPN} analyzeType={this.analyzeType} />
                {content}

                <div className="btn-group" role="group">
                    <button type="button" className="btn btn-info toggler" onClick={this.displayLexicalAnalyzeTablesOption}>
                        Show/hide lexical analyze tables</button>
                </div>
                <h4 className="result-header">Result: {singleLineRPN}</h4>
                <div style={{ overflowX: 'auto' }}>
                    <RPNBuilderTable tableData={rpnResult} tableTitle={this.tableTitle} />                      
                </div>
               
                {this.state.showLAResult
                    ? <LexicalAnalyzeTablesSet analyzeResult={laResult} />
                    : null}
            </React.Fragment>
        );
    }
}