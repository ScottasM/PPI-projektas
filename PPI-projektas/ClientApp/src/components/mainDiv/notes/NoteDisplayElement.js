import React, {Component} from 'react'
import './NoteDisplayElement.css'

export class NoteDisplayElement extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <li>
                <p className='noteDisplayElement' onClick={() => this.props.openNote(this.props.noteId, 1)}>{this.props.noteName}</p>
            </li>
        )
    }
}