import React, {Component} from 'react'
import {TagList} from "../../TagList";

export class NoteViewer extends Component {
    constructor (props) {
        super(props)
    }
    
    render() {
        return <div>
            <h2>{this.props.name}</h2>
            <button onClick={this.props.toggleEditor}>
                Edit
            </button>
            <button onClick={this.props.exitNote}>
                Exit
            </button>
            <br/>
            <TagList tags={this.props.tags}/>
            <br/>
            <p>{this.props.text}</p>
        </div>
    }
}