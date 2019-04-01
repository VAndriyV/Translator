import React, { Component } from 'react';
import InputDataForm from '../InputDataForm/InputDataForm';
import TranslatorService from '../../services/TranslatorService';
import RPNBuilderTable from '../RPNBuilderTable/RPNBuilderTable';
import ErrorsInfo from '../ErrorsInfo/ErrorsInfo';
import IdnValuesForm from '../IdnValuesForm/IdnValuesForm';
import ExecutionTable from '../ExecutionTable/ExecutionTable';
export default class ExecutorPage extends Component {

    state = {
        isFinish: false,       
        idns: null,
        executionResult: '',
        resultTable: null,
        hasError: false,
        errorMsg: ''
    };

    translatorService = new TranslatorService();
    tableTitle = "Execution result table";

    execute = (sourceCode) => {
        const result = this.translatorService.executeProgram(sourceCode);
        
        result.then((r) => {
            if (Array.isArray(r)) {
                this.setState({
                    idns: r,
                    isFinish: false,
                    executionResult: '',
                    hasError: false,
                    resultTable: null,
                    errorMsg: ''
                });
            }
            else {
                this.setState({                    
                    isFinish: true,
                    executionResult: r.executionResult,
                    resultTable: r.resultTable,
                    idns: null,
                    errorMsg: '',
                    hasError: ''
                });
            }
            console.log(r);
        })
            .catch((e) => {
                this.setState({
                    isFinish: false,
                    executionResult: '',
                    resultTable: null,
                    idns: null,
                    errorMsg: e,
                    hasError: true
                });
            });
    };

    sendIdnsValue = (e) => {
        let data = new FormData(e.target);
        data = data.entries();
        let obj = data.next();
        let retrieved = {};
        while (undefined !== obj.value) {
            retrieved[obj.value[0]] = parseInt(obj.value[1]);
            obj = data.next();
        }

        console.log(retrieved);

        const result = this.translatorService.continueExecuting(retrieved);
        result.then((r) => {
            if (Array.isArray(r)) {
                this.setState({
                    idns: r,
                    isFinish: false,
                    executionResult: '',
                    hasError: false,
                    resultTable: null,
                    errorMsg: ''
                });
            }
            else {
                this.setState({
                    isFinish: true,
                    executionResult: r.executionResult,
                    resultTable: r.resultTable,
                    idns: null,
                    errorMsg: '',
                    hasError: ''
                });
            }
            console.log(r);
        })
            .catch((e) => {
                this.setState({
                    isFinish: false,
                    executionResult: '',
                    resultTable: null,
                    idns: null,
                    errorMsg: e,
                    hasError: true
                });
            });
    };

    analyzeType = "Execute program";

    render() {

        const { idns, executionResult, isFinish, errorMsg, hasError, resultTable } = this.state;
        const content = hasError
            ? <ErrorsInfo errors={errorMsg} />
            : <h4>{executionResult}</h4>;      
        return (
            <React.Fragment>
                <InputDataForm handleSubmit={this.execute} analyzeType={this.analyzeType} />
                {content}
                {(idns !== null && idns !== undefined) ?
                    <IdnValuesForm handleSubmit={this.sendIdnsValue} idns={idns} /> : null}
                <div style={{ overflowX: 'auto' }}>
                    <ExecutionTable tableData={resultTable} tableTitle={this.tableTitle} />
                </div>
            </React.Fragment>
            );
    }
    
}