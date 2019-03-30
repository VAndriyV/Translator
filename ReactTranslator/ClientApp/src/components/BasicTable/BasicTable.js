import React, { Component } from 'react';

export default class BasicTable extends Component{

    makeTableHeaders(arr) {
        var id = 0;
        return arr.map((item) => {            
            return (
                <th scope="col" key={id++}>{item}</th>
            );
        });
    }

    makeTableBody(arr) {        
        var id = 0;
        return arr.map((item) => {
            const values = Object.values(item);

            return (
                <tr key={item.id} >                    
                    {values.map((value) => {                        
                        return (<td key={id++}>{value}</td>);
                    })}                     
                </tr>
            );
        });
    }
    render() {
        const { tableData, tableTitle } = this.props;
       
        if (tableData !== null) {
            if (tableData !== undefined && tableData.length !== 0) {
                const keys = Object.keys(tableData[0]);

                return (
                    <div>
                        <h4>{tableTitle}</h4>
                        <table className="table table-bordered table-hover">
                            <thead className="thead-dark">
                                <tr>
                                    {this.makeTableHeaders(keys)}
                                </tr>
                            </thead>
                            <tbody>
                                {this.makeTableBody(tableData)}
                            </tbody>
                        </table>
                    </div>
                );
            }
        }
        return (null);
    }
}