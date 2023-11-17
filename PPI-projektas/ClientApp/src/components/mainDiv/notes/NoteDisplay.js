import React, {Component} from 'react';
import './NoteDisplay.css'
import {NoteDisplayElement} from './NoteDisplayElement'
import {Note} from "./Note";

export class NoteDisplay extends Component {
    constructor(props) {
        super(props)
        this.state = {
            mounted: false,
            notes: [],
            isLoading: true,
            selectedNote: 0,
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

        document.addEventListener('click', this.handleGlobalClick);
    }

    componentWillUnmount() {
        document.removeEventListener('click', this.handleGlobalClick);
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
    
    handleNoteSelect = (noteId) => {
        this.setState({
            selectedNote: noteId,
        })
    }

    handleGlobalClick = (event) => {
        const noteCard = document.querySelector('.note-card.selected');
        if (noteCard && !noteCard.contains(event.target)) {
            this.setState({
                selectedNote: 0,
            });
        }
    };
    
    render() {
        const {selectedNote} = this.state;
        
        return (
            <div className="note-display">
                {this.props.currentGroupId ?
                    (this.state.isLoading ? (
                        <p>Loading...</p>
                    ) : this.state.notes.length > 0 ? (
                        this.state.notes.map((note) => (
                            <Note
                                key={note.id}
                                id={note.id}
                                title={note.name}
                                selected={note.id === selectedNote}
                                handleSelect={this.handleNoteSelect}
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