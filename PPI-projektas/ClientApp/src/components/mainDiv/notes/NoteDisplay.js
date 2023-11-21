import React, {Component} from 'react';
import './NoteDisplay.css'
import {NoteDisplayElement} from './NoteDisplayElement'

export class NoteDisplay extends Component {
    constructor(props) {
        super(props)
        this.state = {
            mounted: false,
            notes: [],
            isLoading: true,
        }
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

    componentDidUpdate(prevProps) {
        if (this.props.currentGroupId !== prevProps.currentGroupId) {
            this.fetchNotes();
        }
    }

    fetchNotes = async () => {
        if(this.props.currentGroupId === 0){
            return;
        }
        
        fetch(`http://localhost:5268/api/note/${this.props.currentGroupId}`)
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
                    notes: notes,
                    isLoading: false,
                });
            })
            .catch(error =>
                console.error('There was a problem with the fetch operation:', error));
    }

    render() {
        return (
            <div className="noteDisplay">
                {this.props.currentGroupId ? 
                    (this.state.isLoading ? (
                        <p>Loading...</p>
                    ) : this.state.notes.length > 0 ? (
                        this.state.notes.map((note) => (
                            <NoteDisplayElement
                                noteName={note.name}
                                noteId={note.id}
                                openNote={this.props.openNote}
                                key={note.id}
                            />
                        ))
                    ) : (
                        <p>No notes found.</p>
                    )) : (
                        <p>Please select a group</p>
                    )
                }
                
            </div>
        );
    }

}