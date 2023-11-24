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
        searchType: 0,
        tagFilter: '',
        nameFilter: ''
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
        return (
            <div className="noteDisplay">
                <div className='searchDiv'>
                    <input className='searchBar' type='search' value={this.state.nameFilter} onChange={this.handleNameFilterChange}></input>
                    <button onClick={this.handleSearch}>Search</button>
                    <br/>
                    <input className='searchBar' type='search' value={this.state.tagFilter} onChange={this.handleTagFilterChange}></input>
                    <br/>
                    <div className='tagFilterOptions'>
                        <label className='tagFilterLabel'>
                            All
                            <input type='radio' name='searchType' value='All' defaultChecked={this.state.defaultCheck} onClick={this.handleTypeChange}></input>
                        </label>
                        <label className='tagFilterOptions'>
                            Any
                            <input type='radio' name='searchType' value='Any' onClick={this.handleTypeChange}></input>
                        </label>
                    </div>
                </div>
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