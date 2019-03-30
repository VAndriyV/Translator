import React, { Component } from 'react';

export default class RelationTable extends Component {

    state = {
        hasError: false
    }   

    makeTableHeaders(terms) {        
        return terms.map((term) => {
            return <td key={term}>{term}</td>;
        });        
    }

    makeRelations(terms, relationMatrix) {      

        var i = -1;
        var j = -1;
        var conflicts = 0;
        var result = terms.map((term) => {           
            { i++; }
            { j = -1; }
            return (                
                <tr>
                    <td>{term}</td>                   
                    {terms.map((term_) => {

                        { j++; }
                      
                        if (String(relationMatrix[i][j]).trim().length > 1) {
                            if (!this.state.hasError) {
                                this.setState({
                                    hasError: true
                                });                               
                            }
                            conflicts++;
                            return <td className="relation-cell error">{relationMatrix[i][j]}</td>;
                        }
                        else
                            return <td className="relation-cell">{relationMatrix[i][j]}</td>;
                    })}                    
                </tr>
            );
        });

        if (this.state.hasError && conflicts === 0) {
            this.setState({
                hasError: false
            });
        }
        return result;
        
    }

    render() {        
     
        const { allTerms, relationMatrix } = this.props.relationData;

        const { tableTitle } = this.props;

        return (            
            <div style={{ overflowX: 'auto' }}>
                {this.state.hasError ? (<div className="alert alert-danger" role="alert">
                    Grammar is not correct!
                </div>) : null}
                <h4>{tableTitle}</h4>
                <table className="table table-bordered table-hover table-sm table-responsive">
                    <thead className="thead-dark">
                        <tr>
                            <td />
                            {this.makeTableHeaders(allTerms)}
                        </tr>
                    </thead>
                    <tbody>
                        {this.makeRelations(allTerms, relationMatrix)}
                    </tbody>
                </table>
            </div>
            );
    }
}