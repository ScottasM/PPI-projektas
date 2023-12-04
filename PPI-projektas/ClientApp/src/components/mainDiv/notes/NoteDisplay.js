import React, {Component} from 'react';
import './NoteDisplay.css'
import {Note} from "./Note";
import {NoteHub} from "./NoteHub";

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

    noteHubRef = React.createRef();
    
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
                const notes = data
                    .filter(note => note.name !== null)
                    .map(note => ({
                    id: note.id,
                    name: note.name,
                    tags: note.tags,
                    text: note.text,
                }));
                this.setState({
                    notes: notes,
                    isLoading: false,
                });
            })
            .catch(error =>
                console.error('There was a problem with the fetch operation:', error));
        
        if(this.props.createNote)
            this.props.noteCreated();
    }
    
    handleNoteSelect = (event, noteId) => {
        this.setState({
            selectedNote: noteId,
        })
        event.stopPropagation();
    }

    handleGlobalClick = (event) => {
        const noteCard = document.querySelector('.note-card.selected');
        const isNoteHubClick = event.target.closest('.note-hub');
        const isNoCloseButtonClick = event.target.classList.contains('no-close-button');
        
        if (noteCard && !noteCard.contains(event.target) && !isNoteHubClick && !isNoCloseButtonClick) {
            this.setState({
                selectedNote: 0,
            });
        }
    };
    
    render() {
        const {selectedNote, notes} = this.state;
        
        return (
            <div>
                <div className="note-display">
                    {this.props.currentGroupId ?
                        (this.state.isLoading ? (
                            <p>Loading...</p>
                        ) : notes.length > 0 ? (
                            notes.map((note) => (
                                <Note
                                    key={note.id}
                                    noteData={note}
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
                {(selectedNote !== 0 || this.props.createNote) &&
                    <NoteHub
                        display={selectedNote !== 0 ? 1 : 2}
                        ref={this.noteHubRef}
                        noteData={notes.find(note => note.id === selectedNote)}
                        currentGroupId={this.props.currentGroupId}
                        currentUserId={this.props.currentUserId}
                        fetchNotes={this.fetchNotes}
                        handleClose={() => this.setState({selectedNote: 0}, () => {
                            this.fetchNotes();
                        })}
                        
                    />
                }
            </div>
        );
    }

}