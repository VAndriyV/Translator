import React, { Component } from 'react';

export default class IdnValuesForm extends Component{

    onFormSubmit = (e) => {
        e.preventDefault();
        this.props.handleSubmit(e);
    };

    mapIdnsList = (idns) => {       

        return idns.map((i) => {           
            if (i.hasOwnProperty('name')) {
                return (<React.Fragment>
                    <label>{i.name} </label>
                    <input type='number' name={i.name} pattern="\d*" className="form-control idn-input" required />
                </React.Fragment>);
            }
            else {
                return (<React.Fragment>
                    <label>{i} </label>
                    <input type='number' name={i} pattern="\d*" className="form-control idn-input" required />
                </React.Fragment>);
            }
            
        });
    };

    render() {

        const { idns } = this.props;         

        return (
            <div className="form-group">
                <form onSubmit={this.onFormSubmit} >
                    {this.mapIdnsList(idns)}
                    <button type="submit" className="btn btn-primary">Calculate</button>
                </form>
            </div>
        );
    }
}