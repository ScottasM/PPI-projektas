import React, {Component} from 'react';
import './NoteDisplay.css'
import {NoteDisplayElement} from './NoteDisplayElement'

export class NoteDisplay extends Component {
    constructor(props) {
        super(props)
    }
    
    render() {
        return (
            <div className='noteDisplay'>
                <ul className='noteList'>
                    {this.props.notes.map(note => (
                        <NoteDisplayElement noteName={note.name} noteId={note.id} openNote={this.props.openNote}/>
                    ))}
                </ul>
            </div>
        )
    }

}