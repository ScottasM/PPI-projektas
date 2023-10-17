import React, {Component} from 'react'
import './NoteDisplayElement.css'

export class NoteDisplayElement extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <li>
                <p className='noteDisplayElement' onClick={() => this.props.openNote(this.props.noteId)}>{this.props.noteName}</p>
            </li>
        )
    }
}