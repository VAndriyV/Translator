import React, { Component } from 'react';
import TranslatorService from '../../services/TranslatorService';

export default class Hashing extends Component {

    state = {
        tableSize: '',
        data:null
    };

    onTextContentChange = (e) => {
        this.setState({
            tableSize: e.target.value
        });
    } 

    translatorService = new TranslatorService();

    handeSubmit = (e) => {
        e.preventDefault();
        const result = this.translatorService.performHashing(this.state.tableSize);
        result.then((r) => {
            console.log(r);
            this.setState({
                data: r
            });
        })
            .catch((e) => {
                this.setState({
                    data:null
                });
            });
    };

    mapArrayToHtmlList = (arr) => {
        return (
            <ul>
                {arr.map(item => {
                    return <li>{item}</li>;
                })
                }
            </ul>
        );
    };

    render() {
        const { data } = this.state;
        return (
            <div>
                <form onSubmit={this.handeSubmit} >
                    <label htmlFor="tableSize">Table size</label>
                    <input onChange={this.onTextContentChange}  type="number" pattern="\d*" className="form-control" id="tableSize" required />
                    <button type="submit" className="btn btn-primary">Submit</button>
                </form>
                {data !== null ?
                    <div>
                        <h3>Середня кількість спроб для</h3>
                        <ul>
                            <li>10% заповненості таблиці</li>
                            <li>50% заповненості таблиці</li>
                            <li>90% заповненості таблиці</li>
                        </ul>
                        <h5>Лінійне рехешування  </h5>
                        {this.mapArrayToHtmlList(data.linearStatistic)}
                        <h5>Лінійне рехешування (спроба занесення 100елементів)  </h5>
                        {this.mapArrayToHtmlList(data.fakeLinearStatistic)}
                        <h5>Квадратичне рехешування  </h5>
                        {this.mapArrayToHtmlList(data.squareStatistic)}
                        <h5>Квадратичне рехешування (спроба занесення 100елементів)  </h5>
                        {this.mapArrayToHtmlList(data.fakeSquareStatistic)}
                    </div>
                    : null}              
                
            </div>);
    }
}