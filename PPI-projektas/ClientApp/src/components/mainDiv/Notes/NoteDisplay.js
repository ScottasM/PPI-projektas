import React, {Component} from 'react';
import './NoteDisplay.css'
import {NoteDisplayElement} from './NoteDisplayElement'

export class NoteDisplay extends Component {
    constructor(props) {
        super(props)
    }
    
    state= {
        mounted: false,
        notes: []
    }
    
    componentDidMount() {
        if (!this.state.mounted)
        {
            this.fetchNotes();
            this.setState({
                mounted: true
            });
        }
    }

    fetchNotes = async () => {
        try {
            fetch('http://localhost:5268/api/note')
                .then(async response => {
                    if (!response.ok)
                        throw new Error(`Network response was not ok`);
                    return await response.json();
                })
                .then(data => {
                    const notes = data.map(note => ({
                        name: note.name,
                        id: note.id
                    }));
                    this.setState({
                        notes: notes
                    });
                })
        }
        catch (error) {
            console.error('There was a problem with the fetch operation:', error);
        }
    }

    render() {
        return (
            <div className='noteDisplay'>
                {this.state.notes.length > 0 ?
                    <ul className='noteList'>
                        {this.state.notes.map(note => (
                            <NoteDisplayElement
                                noteName={note.name}
                                noteId={note.id}
                                openNote={this.props.openNote}
                            />
                        ))}
                    </ul> :
                    <p>No notes found.</p>}
            </div>
        )
    }

}