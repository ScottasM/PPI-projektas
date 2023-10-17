import React, { Component } from 'react';
import {GroupCreateMenu} from "./GroupCreateMenu";
import {CreatingButtons} from "./CreatingButtons";
import {NoteDisplay} from "./Notes/NoteDisplay";
import {NoteHub} from "./Notes/NoteHub";

export class MainContainer extends Component {
    static displayName = MainContainer.name;

    constructor(props) {
        super(props);
    }
    
    state = {
        mounted: false,
        notes: [],
        displayGroupCreateMenu: false,
        noteId: '',
        showNote: false
    }
    
    componentDidMount() {
       if(!this.state.mounted) {
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
    
    toggleGroupCreateMenu = () => {
        this.setState((prevState) => ({
            displayGroupCreateMenu: !prevState.displayGroupCreateMenu,
        }));
    }
    
    openNote = id => {
        this.setState(prevState => ({
            noteId: id,
            showNote: !prevState.showNote
        }));
    }
    
    exitNote = () => {
        this.fetchNotes();
        this.setState(prevState => ({
            noteId: '',
            showNote: !prevState.showNote
        }));
    }
    
    render() {
        return (
            <div className="bg-white">
                <CreatingButtons toggleMenu={this.toggleGroupCreateMenu}/>
                {this.state.notes == null || this.state.notes.length === 0 ? <p>No notes found.</p> : !this.state.showNote && <NoteDisplay notes={this.state.notes} openNote={this.openNote}/>}
                {this.state.showNote && <NoteHub noteId={this.state.noteId} exitNote={this.exitNote}/>}
                {this.state.displayGroupCreateMenu && <GroupCreateMenu fetchGroupList={this.props.fetchGroupList} toggleGroupCreateMenu={this.toggleGroupCreateMenu} />}
            </div>
        );
    }
}