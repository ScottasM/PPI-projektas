import React, {Component} from 'react'
import './NoteDisplay.css'

export class NoteDisplayElement extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="note-display-element" onClick={() => this.props.openNote(this.props.noteId, 1)}>
                <h3>{this.props.noteName ? this.props.noteName : '?Could not load note name?'}</h3>
            </div>
        )
    }
}