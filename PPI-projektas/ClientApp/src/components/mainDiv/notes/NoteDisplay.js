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
            defaultCheck: true,
            searchType: 0,
            tagFilter: '',
            nameFilter: ''
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
        const parameters = `search?UserId=${this.props.currentUserId}`
            + `&SearchType=${this.state.searchType}`
            + (this.state.tagFilter !== '' ? `&TagFilter=${this.state.tagFilter}` : '')
            + (this.state.nameFilter !== '' ? `&NameFilter=${this.state.nameFilter}` : '')
            + (this.props.currentGroupId !== 0 ? `&GroupId=${this.props.currentGroupId}` : '');
        
        fetch(`http://localhost:5268/api/note/` + parameters)
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
                    tags: note.tags === null ? [] : note.tags,
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
    

    handleGlobalClick = (event) => {
        const noteCard = document.querySelector('.note-card.selected');
        const isNoteHubClick = event.target.closest('.note-hub');
        const isNoCloseButtonClick = event.target.classList.contains('no-close-button');
        
        if (noteCard && !noteCard.contains(event.target) && !isNoteHubClick && !isNoCloseButtonClick) {
            this.props.resetSelectedNote();
        }
    };

    handleNameFilterChange = (event) => {
        this.setState({
            nameFilter: event.target.value
        })
    }
    
    handleTagFilterChange = (event) => {
        this.setState({
            tagFilter: event.target.value
        })
    }

    handleTypeChange = (event) => {
        if (event.target.value === "Any")
            this.setState({
                searchType: 1 
            })
        else
            this.setState({
                searchType: 0
            })
    }
    
    handleSearch = () => {
        this.setState({
            isLoading: true,
            defaultCheck: true,
            tagFilter: [],
            searchType: 0,
            nameFilter: ''
        })
        this.fetchNotes();
    }
    
    render() {
        const {notes} = this.state;
        const {selectedNote} = this.props.selectedNote;
        
        return (
            <div>
                <div className='note-search'>
                    <div className='note-search-bar'>
                        <input type='search' value={this.state.nameFilter} onChange={this.handleNameFilterChange}></input>
                        <br/>
                        <input type='search' value={this.state.tagFilter} onChange={this.handleTagFilterChange}></input>
                    </div>
                    <button className='create-button' onClick={this.handleSearch}>Search</button>
                        <label className='tagFilterLabel'>
                            All
                            <input type='radio' name='searchType' value='All' defaultChecked={this.state.defaultCheck} onClick={this.handleTypeChange}></input>
                        </label>
                        <label className='tagFilterOptions'>
                            Any
                            <input type='radio' name='searchType' value='Any' onClick={this.handleTypeChange}></input>
                        </label>
                </div>
                <div className="note-display">
                    {this.props.currentGroupId ?
                        (this.state.isLoading ? (
                            <p>Loading...</p>
                        ) : notes.length > 0 ? (
                            notes.map((note) => (
                                <Note
                                    key={note.id}
                                    noteData={note}
                                    handleSelect={this.props.handleNoteSelect}
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
                        handleClose={() => {
                            this.props.resetSelectedNote();
                            this.fetchNotes();
                        }}
                        
                    />
                }
            </div>
        )
    }
}

