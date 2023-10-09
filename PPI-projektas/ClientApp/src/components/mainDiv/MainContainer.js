import React, { Component } from 'react';
import {GroupCreateMenu} from "./GroupCreateMenu";
import {CreatingButtons} from "./CreatingButtons";
import {TagList} from "./openNote/NOteDisplay";
export class MainContainer extends Component {
    static displayName = MainContainer.name;

    constructor(props) {
        super(props);
        this.setState({
            mounted: false
        });
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
            const response = fetch('http://localhost:5268/api/notes/');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const responseData = await response.json();
            const noteData = responseData.notes.map(note => ({
                name: note.name, 
                id: note.id
            }));
            this.setState({
                notes: noteData
            })
        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
        }
    }

    state = {
        displayGroupCreateMenu: false,
        toggleNoteListOrNote: false
    };
    
    toggleGroupCreateMenu = () => {
        this.setState((prevState) => ({
            displayGroupCreateMenu: !prevState.displayGroupCreateMenu,
        }));
    }
    
    openNote = id => {
        this.setState({
            noteId: id
        });
    }
    
    render() {
        return (
            <div className="bg-white">
                <CreatingButtons toggleMenu={this.toggleGroupCreateMenu}/>
                {!this.state.toggleNoteListOrNote && <NoteList/>}
                {this.state.toggleNoteListOrNote && <NoteHub id={this.state.noteId}/>}
                {this.state.displayGroupCreateMenu && <GroupCreateMenu fetchGroupList={this.props.fetchGroupList} toggleGroupCreateMenu={this.toggleGroupCreateMenu} />}
            </div>
        );
    }
}