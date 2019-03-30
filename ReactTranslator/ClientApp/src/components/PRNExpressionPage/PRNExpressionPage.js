import React, { Component } from 'react';
import InputDataForm from '../InputDataForm/InputDataForm';
import TranslatorService from '../../services/TranslatorService';
import BasicTable from '../BasicTable/BasicTable';
import IdnValuesForm from '../IdnValuesForm/IdnValuesForm';
import ErrorsInfo from '../ErrorsInfo/ErrorsInfo';
import { Link } from "react-router-dom";
export default class PRNExpressionPage extends Component {

    state = {
        rpnBuildTable: null,
        rpnCalculationTable: null,
        idns: null,
        hasError: false,
        errorMsg: null,
        calculationResult:null
    };

    translatorService = new TranslatorService();

    analyzeType = 'Build expression';

    buildTableTitle = 'Reverse Polish notation build';

    calculationTableTitle = 'Calculation table';

    buildExpression = (expression) => {

        let result = this.translatorService.getRPNExpression(expression);

        result.then((r) => {
            
            this.setState({
                rpnBuildTable: r.resultTable,
                idns: r.idns,
                hasError: false,
                errorMsg: null,
                rpnCalculationTable: null,
                calculationResult: null
            });

        }).catch((e) => this.setState({
            hasError: true,
            errorMsg: e,
            rpnBuildTable: null,
            idns: null,
            rpnCalculationTable: null,
            calculationResult: null
            }));

    }

    calculateExpression = (e) => {

        let data = new FormData(e.target);
        data = data.entries();
        let obj = data.next();
        let retrieved = {};
        while (undefined !== obj.value) {
            retrieved[obj.value[0]] = parseInt(obj.value[1]);
            obj = data.next();
        }

        var result = this.translatorService.getRPNCalculation(retrieved);

        result.then((r) => {           
            this.setState({
                rpnCalculationTable: r.calculationTable,
                calculationResult: r.calculationResult
            });
        });
    };

    addSpaceBetweenBuildElements(arr) {

       let array = JSON.parse(JSON.stringify(arr));

        array.forEach(el => {
            el.stack.forEach((o, i, a) => a[i] = a[i] + " ");

            el.inputTokens.forEach((o, i, a) => a[i] = a[i] + " ");

            el.rpn.forEach((o, i, a) => a[i] = a[i] + " ");
        });

        return array;
    }

    addSpaceBetweenCalculationElements(arr) {

        let array = JSON.parse(JSON.stringify(arr));

        array.forEach(el => {
            el.stack.forEach((o, i, a) => a[i] = a[i] + " ");            

            el.rpn.forEach((o, i, a) => a[i] = a[i] + " ");
        });

        return array;
    }


    render() {

        let modifiedBuildTable = null;
        let modifiedCalculationTable = null;

        const { idns, rpnCalculationTable, rpnBuildTable, calculationResult, hasError, errorMsg } = this.state;       

        const content = hasError
            ? <ErrorsInfo errors={errorMsg} />
            : null;      
        

        if (rpnBuildTable !== null) {
            modifiedBuildTable = this.addSpaceBetweenBuildElements(rpnBuildTable);
        }

        if (rpnCalculationTable !== null) {
            modifiedCalculationTable = this.addSpaceBetweenCalculationElements(rpnCalculationTable);
        }

        return (
            <React.Fragment>
                <InputDataForm handleSubmit={this.buildExpression} analyzeType={this.analyzeType} />

                {content}

                <div>
                    <Link to="/grammarAndRelation/expression" target="_blank" exact>Grammar and relation table</Link>
                </div>

                {(idns !== null && idns !== undefined) ?
                    <IdnValuesForm handleSubmit={this.calculateExpression} idns={idns} /> : null}

                <div style={{ overflowX: 'auto' }}>

                    <BasicTable tableData={modifiedBuildTable} tableTitle={this.buildTableTitle} />

                </div>

                <div style={{ overflowX: 'auto' }}>
                    <h4>Result: {calculationResult}</h4>
                    <BasicTable tableData={modifiedCalculationTable} tableTitle={this.calculationTableTitle} />

                </div>

            </React.Fragment>
            );
    }
}