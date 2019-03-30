import React, { Component } from 'react';
import TranslatorService from '../../services/TranslatorService';
import RelationTable from '../RelationsTable/RelationTable';

export default class GrammarAndRelations extends Component {    

    state = {
        grammar: '',
        relationData: null
    }

    componentDidMount() {
        var type = this.props.match.params.type;       
        let result;
        if (type === 'expression')
            result = this.translatorService.getExpressionGrammar();
        else
            result = this.translatorService.getFullGrammar();

        result.then((r) => {            
            this.setState({
                grammar: r.grammar,
                relationData: r.relationTable
            });
        });
    }

    translatorService = new TranslatorService();

    render() {
        console.log(this.state.grammar);
        if (this.state.relationData !== null) {
            return (
                <div style={{ whiteSpace: "pre" }}>
                    <h4>Grammar</h4>
                    <p>{this.state.grammar}</p>                    
                    <RelationTable relationData={this.state.relationData} tableTitle='Relations table' />
                </div>
            );
        }
        else {
            return null;
        }
    }
}