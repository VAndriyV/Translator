import React from 'react';
import BasicTable from '../BasicTable/BasicTable';

const LexicalAnalyzeTablesSet = ({ analyzeResult }) => {
    if (analyzeResult !== null) {
        return (
            <div>                
                <BasicTable tableData={analyzeResult.outputLexemes} tableTitle='Lexemes'/>                
                <BasicTable tableData={analyzeResult.outputIdns} tableTitle='IDNs'/>                
                <BasicTable tableData={analyzeResult.outputConstants} tableTitle='Constants'/>
            </div>
        );
    }
    return null;
};

export default LexicalAnalyzeTablesSet;