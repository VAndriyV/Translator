import React, { Component } from 'react';
import { saveAs } from 'file-saver';
import FileLoadService from '../../services/FileLoadService';


export default class InputDataForm extends Component {      
            
    state = {
        textareaValue : ''
    }  
 
    fileInput = React.createRef();

    fileLoader = new FileLoadService();
    
    onTextContentChange = (e) => {        
        this.setState({
            textareaValue: e.target.value
        });
    }   

    loadFile = (e) => {
        e.preventDefault();
        var data = this.fileLoader.loadFile(this.fileInput.current.files[0].name);        
        data.then((r) => {
            console.log(r);
            this.setState({
                textareaValue: r
            }
            );
        })
            .catch((e) => console.log(e));
        alert(
            `Selected file - ${
            this.fileInput.current.files[0].name
            }`
        );
    }

    onFormSubmit = (e) => {
        e.preventDefault();
        const { handleSubmit } = this.props;
        handleSubmit(this.state.textareaValue);
    }

    saveSourceCodeToFile = () => {
        var text = this.state.textareaValue;
        var filename = "source code";
        var blob = new Blob([text], { type: "text/plain;charset=utf-8" });
        saveAs(blob, filename + ".txt");
    }

    render() {
        const { analyzeType } = this.props;      
        
        return (
            <div className="form-group">
                <form onSubmit={this.onFormSubmit} >
                    <textarea className="form-control" rows="25" cols="100" id ="dataInput"
                        onChange={this.onTextContentChange} value={this.state.textareaValue} required />
                <div className="btn-group" role="group">
                    <button type="submit" className="btn btn-primary">{analyzeType}</button>

                    <button type="button" className="btn btn-success" onClick={this.saveSourceCodeToFile}>
                        Save source code to file</button>                   
                </div>
                </form>               
            <form onSubmit={this.loadFile}>
                <div className="form-group file-upload">
                    <label htmlFor="uploadFile">Choose the file with a source code</label>
                        <input type="file" className="form-control-file"
                            id="uploadFile" ref={this.fileInput} accept=".txt" />
                        <button type="submit" className="btn btn-primary">Load!</button>
                </div>
                </form>
            </div>
            );
    }
}

