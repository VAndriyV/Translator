import React, { Component } from 'react';

export default class ExecutionTable extends Component {


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
            const { rpn, stack, description } = tableData;
            return (
                <React.Fragment>
                    <h4>{tableTitle}</h4>
                    <table className="table table-bordered table-hover table-sm table-responsive">
                        <tbody>
                            <tr>
                                <th>Polish</th>
                                {this.mapArrayOfArrays(rpn)}
                            </tr>
                            <tr>
                                <th>Stack</th>
                                {this.mapArrayOfArrays(stack)}
                            </tr>                        
                            
                            <tr>
                                <th>Description</th>
                                {this.mapArray(description)}
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