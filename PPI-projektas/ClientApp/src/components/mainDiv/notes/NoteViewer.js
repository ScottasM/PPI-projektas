import React, {Component} from 'react'
import {TagList} from "../../TagList";

export class NoteViewer extends Component {
    constructor (props) {
        super(props)
    }
    
    render() {
        return <div>
            <h2>{this.props.noteName}</h2>
            <button onClick={() => this.props.changeDisplay(2, '')}>
                Edit
            </button>
            <button onClick={this.props.exitNote}>
                Exit
            </button>
            <br/>
            <TagList noteTags={this.props.noteTags}/>
            <br/>
            <p>{this.props.noteText}</p>
        </div>
    }
}