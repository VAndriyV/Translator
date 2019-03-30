import React, { Component } from 'react';
import TranslatorService from '../../services/TranslatorService';
import InputDataForm from '../InputDataForm/InputDataForm';
import RelationTable from '../RelationsTable/RelationTable';

export default class RelationsPage extends Component {
        
    state = {
        relationData: null,
        hasErros: false
    }

    translatorService = new TranslatorService();

    setRelations = (grammarText) => {
        const result = this.translatorService.getRelationTable(grammarText);

        result.then((r) => {
            this.setState({
                relationData: r
            });
        });
    }
  
    render() {
        return (
            <div>
                <InputDataForm handleSubmit={this.setRelations} analyzeType='Set relations' />
                {this.state.relationData === null
                    ? null
                    : <RelationTable relationData={this.state.relationData} tableTitle='Relations table' />}
            </div>);    
        }
}