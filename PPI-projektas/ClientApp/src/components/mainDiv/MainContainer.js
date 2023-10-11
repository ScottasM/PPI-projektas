import React, { Component } from 'react';
import {GroupCreateMenu} from "./GroupCreateMenu";
import {CreatingButtons} from "./CreatingButtons";
import {NoteList} from "./Notes/NoteDisplay";
import {NoteHub} from "./Notes/NoteHub";

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
        showNote: false
    };
    
    toggleGroupCreateMenu = () => {
        this.setState((prevState) => ({
            displayGroupCreateMenu: !prevState.displayGroupCreateMenu,
        }));
    }
    
    exitNote = () => {
        this.setState(prevState => ({
            showNote: !prevState.showNote,
            noteId: ''
        }));
    }
    
    openNote = id => {
        this.setState(prevState => ({
            noteId: id,
            showNote: !prevState.showNote
        }));
    }
    
    render() {
        return (
            <div className="bg-white">
                <CreatingButtons toggleMenu={this.toggleGroupCreateMenu}/>
                {!this.state.showNote && <NoteList notes={this.state.notes} openNote={this.openNote}/>}
                {this.state.showNote && <NoteHub id={this.state.id} exitNote={this.exitNote}/>}
                {this.state.displayGroupCreateMenu && <GroupCreateMenu fetchGroupList={this.props.fetchGroupList} toggleGroupCreateMenu={this.toggleGroupCreateMenu} />}
            </div>
        );
    }
}