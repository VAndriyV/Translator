import React, { Component } from 'react';

export default class RPNBuilderTable extends Component {
    

    mapArray = (arr) => {
        return arr.map((item) => {
            return <td>{item}</td>;
        });
    };

    mapArrayOfArrays = (arr) => {
        return arr.map((item) => {            
            if (item.length === 0) {
                return <td />;
            }
            else {
                return (<td>{item.map((i) => {
                    return i;
                })}</td>);
            }
        });
    }

    render() {       

        const { tableTitle, tableData } = this.props;

        if (tableData !== null) {
            const { inputLexemes, loopArgument, loopIndicator, rpn, stack } = tableData;
            return (
                <React.Fragment>
                    <h4>{tableTitle}</h4>
                    <table className="table table-bordered table-hover table-sm table-responsive">                      
                        <tbody>
                            <tr>
                                <th>input lexemes</th>
                                {this.mapArray(inputLexemes)}
                            </tr>
                            <tr>
                                <th>Stack</th>
                                {this.mapArrayOfArrays(stack)}
                            </tr>
                            <tr>
                                <th>loop argument</th>
                                {this.mapArray(loopArgument)}
                            </tr>
                            <tr>
                                <th>loop indicator</th>
                                {this.mapArray(loopIndicator)}
                            </tr>
                            <tr>
                                <th>output</th>
                                {this.mapArrayOfArrays(rpn)}
                            </tr>
                        </tbody>
                    </table>
                </React.Fragment>
            );
        }
        else
            return null;
    }
}