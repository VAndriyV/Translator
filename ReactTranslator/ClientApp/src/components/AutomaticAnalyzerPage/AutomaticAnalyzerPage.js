import React, { Component } from 'react';
import InputDataForm from '../InputDataForm/InputDataForm';
import TranslatorService from '../../services/TranslatorService';
import LexicalAnalyzeTablesSet from '../LexicalAnalyzeTablesSet/LexicalAnalyzeTablesSet';
import ErrorsInfo from '../ErrorsInfo/ErrorsInfo';
import AutomaticStepsTable from '../AutomaticStepsTable/AutomaticStepsTable';
import BasicTable from '../BasicTable/BasicTable';

export default class AutomaticAnalyzerPage extends Component {
     
    state = {
        hasError: false,
        errorMsg: null,
        laResult: null,
        automaticSteps: null,
        automaticConfiguration: null,
        showAutomaticConfiguration: false,
        showLAResult:false
    };

    componentDidMount() {
        this.loadAutomaticConfiguration();
    }

    translatorService = new TranslatorService();

    analyzeType = "Automatic analyze";
   
    loadAutomaticConfiguration = () => {
        var result = this.translatorService.getAutomaticConfiguration();
        result.then((r) => this.setState({
            automaticConfiguration: r
        }))
            .catch((e) => console.log(e));
    }

    doAutomaticAnalyze = (sourceCode) => {
        var result = this.translatorService.getAutomaticAnalyzeResult(sourceCode);
        result.then((r) => {            
            this.setState({
                laResult: r.laResult,
                hasError: false,
                automaticSteps: r.automaticSteps
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

    displayAutomaticConfigurationOption = () => {       
        this.setState({
            showAutomaticConfiguration: !this.state.showAutomaticConfiguration
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

        const content = this.state.hasError
            ? <ErrorsInfo errors={this.state.errorMsg} />
            : <AutomaticStepsTable automaticSteps={this.state.automaticSteps} tableTitle='Analyze steps' />;
               
               

        return (
            <div>
                <InputDataForm handleSubmit={this.doAutomaticAnalyze} analyzeType={this.analyzeType} />

                <div className="btn-group" role="group">

                    <button type="button" className="btn btn-info toggler" onClick={this.displayAutomaticConfigurationOption}>
                    Show/hide automatic configuration</button>

                    <button type="button" className="btn btn-info toggler" onClick={this.displayLexicalAnalyzeTablesOption}>
                        Show/hide lexical analyze tables</button>
                </div>

                <div style={{ overflowX: 'auto' }}>

                {this.state.showAutomaticConfiguration
                    ? <BasicTable tableData={this.state.automaticConfiguration} />
                        : null}

                </div>

                {this.state.showLAResult
                    ? <LexicalAnalyzeTablesSet analyzeResult={this.state.laResult} />
                    : null}
                {content}
            </div>
        );
    }
}
