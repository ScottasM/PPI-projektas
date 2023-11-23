import React, {Component} from 'react';
import './NoteDisplay.css'
import {NoteDisplayElement} from './NoteDisplayElement'

export class NoteDisplay extends Component {
    constructor(props) {
        super(props)
    }
        
    state = {
        mounted: false,
        notes: [],
        isLoading: true,
        defaultCheck: true,
        tags: [],
        nameFilter: '',
        searchType: 0
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
        const parameters = `search?UserId=${this.props.currentUserId}`
            + `&SearchType=${this.state.searchType}`
            + (this.state.tags.length > 0 ? `&Tags=${this.state.tags}` : '')
            + (this.state.nameFilter !== '' ? `&NameFilter=${this.state.nameFilter}` : '')
            + (this.props.currentGroupId !== 0 ? `&GroupId=${this.props.currentGroupId}` : '');
        
        fetch(`http://localhost:5268/api/note/` + parameters)
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
    
    handleNameFilterChange = (event) => {
        this.setState({
            nameFilter: event.target.value
        })
    }

    handleTypeChange = (event) => {
        const enumToInt = {
            'All': 0,
            'Any': 1
        }
        
        this.setState({
            defaultCheck: false,
            SearchType: enumToInt[event.target.value]
        });
    }
    
    handleSearch = () => {
        this.setState({
            isLoading: true,
            defaultCheck: true,
            tags: [],
            searchType: 0,
            nameFilter: ''
        })
        this.fetchNotes();
    }

    render() {
        return (
            <div className="noteDisplay">
                <input type='search' width='100px' value={this.state.nameFilter} onChange={this.handleNameFilterChange}></input>
                <input type='radio' name='searchType' value='All' defaultChecked={this.state.defaultCheck} onClick={this.handleTypeChange}></input>
                <input type='radio' name='searchType' value='Any' onClick={this.handleTypeChange}></input>
                <button onClick={this.handleSearch}>Search</button>
                <br/>
                {this.state.isLoading
                    ? <p>Loading...</p>
                    : this.state.notes.length > 0
                        ? this.state.notes.map((note) => (
                            <NoteDisplayElement
                                noteName={note.name}
                                noteId={note.id}
                                openNote={this.props.openNote}
                                key={note.id}
                            />
                        ))
                        : <p>No notes found.</p>
                }
            </div>
        )
    }
}