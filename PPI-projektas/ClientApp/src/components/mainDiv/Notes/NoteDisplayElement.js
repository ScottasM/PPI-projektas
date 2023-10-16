import React, {Component} from 'react'
import './NoteDisplayElement.css'

export class NoteDisplayElement extends Component {
    constructor(props) {
        super(props);
    }

    handleOpenNote = () => {
        this.props.openNote(this.props.noteId);
    }

    render() {
        return (
            <li>
                <p className='noteDisplayElement' onClick={this.handleOpenNote}>{this.props.noteName}</p>
            </li>
        )
    }
}