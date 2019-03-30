import React, { Component } from 'react';

export default class AutomaticStepsTable extends Component {

    makeSingleListCollection = (arr) => {
        return arr.map((item) => {
            return <td>{item}</td>;
        });
    }

    makeMultipleListCollection = (arr) => {
        return arr.map((item) => {
            return (<td>
                <ul>
                    {this.makeListItemsMultipleListCollectionItem(item)}
                </ul>
            </td>);
        });
    }

    makeListItemsMultipleListCollectionItem = (itemArr) => {       
        if (itemArr.lenght === 0)
            return <li />;        
        return itemArr.map((itemNode) => {
            return <li>{itemNode}</li>;
        });
    }
       

    render() {
        if (this.props.automaticSteps !== null) {
            const { numbers, lexemes, states, stack } = this.props.automaticSteps;
            const { tableTitle } = this.props;
            return (
                <div >
                    <h4>{tableTitle}</h4>
                    <table className="table table-bordered table-hover">
                        <tbody>
                            <tr>
                                <th>Number</th>
                                {this.makeSingleListCollection(numbers)}
                            </tr>
                            <tr scope="row">
                                <th>Lexeme</th>
                                {this.makeSingleListCollection(lexemes)}
                            </tr>
                            <tr id="state">
                                <th>State</th>
                                {this.makeMultipleListCollection(states)}
                            </tr>
                            <tr id="stack">
                                <th>Stack</th>
                                {this.makeMultipleListCollection(stack)}
                            </tr>
                        </tbody>
                    </table>
                </div>
            );
        }
        return null;
    }
}