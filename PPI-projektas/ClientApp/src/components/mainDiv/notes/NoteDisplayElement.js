import React, {Component} from 'react'
import './NoteDisplay.css'

export class NoteDisplayElement extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="noteDisplayElement" onClick={() => this.props.openNote(this.props.noteId, 1)}>
                <h3>{this.props.noteName}</h3>
            </div>
        )
    }
}