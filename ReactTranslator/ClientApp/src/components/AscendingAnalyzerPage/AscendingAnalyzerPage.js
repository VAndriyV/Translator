import React, { Component } from 'react';
import InputDataForm from '../InputDataForm/InputDataForm';
import TranslatorService from '../../services/TranslatorService';
import LexicalAnalyzeTablesSet from '../LexicalAnalyzeTablesSet/LexicalAnalyzeTablesSet';
import ErrorsInfo from '../ErrorsInfo/ErrorsInfo';
import BasicTable from '../BasicTable/BasicTable';
import GrammarAndRelations from '../GrammarAndRelations/GrammarAndRelations';
import { Link } from "react-router-dom";

export default class AscendingAnalyzerPage extends Component {

    state = {
        hasError: false,
        errorMsg: null,
        laResult: null, 
        showLAResult: false,
        ascendingAnalyzerTable: null
    };

    translatorService = new TranslatorService();

    analyzeType = "Ascending analyze";

    tableTitle = "Ascending analyze steps";

    doAscendingAnalyze = (sourceCode) => {
        let result = this.translatorService.getAscendingAnalyzeResult(sourceCode);
        result.then((r) => {            
            console.clear();            
            this.setState({
                laResult: r.laResult,
                ascendingAnalyzerTable: r.ascendingAnalyzerTable,
                hasError: false,
                errorMsg: null
            });
        })
        .catch((e) => {
            console.clear();           
            this.setState({
                errorMsg: e,
                hasError: true,
                laResult: null,
                showLAResult: false,
                ascendingAnalyzerTable: null
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

    addSpaceBetweenElements(arr) {

        let array = JSON.parse(JSON.stringify(arr));

        array.forEach(el => {
            el.stack.forEach((o, i, a) => a[i] = a[i] + " ");

            el.lexemes.forEach((o, i, a) => a[i] = a[i] + " ");
        });

        return array;
    }

    render() {

        const content = (this.state.hasError)
            ? <ErrorsInfo errors={this.state.errorMsg} />
            : null;        

        let modifiedArray = null;

        if (this.state.ascendingAnalyzerTable !== null)
            modifiedArray =  this.addSpaceBetweenElements(this.state.ascendingAnalyzerTable);

        return (
            <div>               
                <InputDataForm handleSubmit={this.doAscendingAnalyze} analyzeType={this.analyzeType} />

                <div>
                    <Link to="/grammarAndRelation/full" target="_blank" exact>Grammar and relation table</Link>
                </div>
                {content}

                <div className="btn-group" role="group">
                    <button type="button" className="btn btn-info toggler" onClick={this.displayLexicalAnalyzeTablesOption}>
                        Show/hide lexical analyze tables</button>                  
                </div>

                <div style={{ overflowX: 'auto' }}>

                    <BasicTable tableData={modifiedArray} tableTitle={this.tableTitle} /> 

                </div>

                {this.state.showLAResult
                    ? <LexicalAnalyzeTablesSet analyzeResult={this.state.laResult} />
                    : null}
              
            </div>
        );
    }
}