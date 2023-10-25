import React, { Component } from 'react';
import { NoteDisplay } from "./Notes/NoteDisplay";
import {NoteHub} from "./Notes/NoteHub";
import { GroupCreateMenu } from "./group/GroupCreateMenu";
import { UserLoginMenu } from "./login/UserLoginMenu";
import { UserSignInMenu } from "./login/UserSignInMenu";
import { CreatingButtons } from "./CreatingButtons";
import { CreatingLoginButtons } from "./login/CreatingLoginButtons";

export class MainContainer extends Component {
    static displayName = MainContainer.name;

    constructor(props) {
        super(props);
        this.state = {
            mounted: false,
            displayGroupCreateMenu: this.props.displayGroupEditMenu,
            groupConfigMenuType: 'create',
            displayLoginMenu: false,
            displaySignInMenu: false,
            noteId: '',
            notes: [],
            showNote: false
        }
    }
    
    componentDidMount() {
       if(!this.state.mounted) {
           this.fetchNotes();
           this.setState({
               mounted: true
           });
       }
    }

    componentDidUpdate(prevProps) {
        if (this.props.toggledGroup !== prevProps.toggledGroup || this.props.displayGroupEditMenu !== prevProps.displayGroupEditMenu) {
            if(this.props.displayGroupEditMenu){
                this.setState(() => ({
                    groupConfigMenuType: 'edit'
                    }), () => {
                        this.toggleGroupConfigMenu();
                });
            }
            else {
                this.setState(() => ({
                    groupConfigMenuType: 'create'
                }));
                if(this.state.displayGroupCreateMenu)
                    this.toggleGroupConfigMenu();
            }
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
                        id: note.id,
                        name: note.name,
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
    
    toggleGroupConfigMenu = () => {
        if (!(this.state.displayGroupCreateMenu)) {
            this.setState({ displayLoginMenu: false, displaySignInMenu: false })
        }
        else{
            this.setState((prevState) => ({
                groupConfigMenuType: 'create',
            }));
            if(this.props.displayGroupEditMenu) this.props.toggleGroupEditMenu();
        }
        
        this.setState((prevState) => ({
                displayGroupCreateMenu: !prevState.displayGroupCreateMenu,
            }));
    }

    toggleLoginMenu = () => {
        if (!(this.state.displayLoginMenu)) {
            this.setState({ displayGroupCreateMenu: false, displaySignInMenu: false })
        }

        this.setState((prevState) => ({
            displayLoginMenu: !prevState.displayLoginMenu,
        }));
    }

    toggleSignInMenu = () => {
        if (!(this.state.displaySignInMenu)) {
            this.setState({ displayGroupCreateMenu: false, displayLoginMenu: false })
        }

        this.setState((prevState) => ({
            displaySignInMenu: !prevState.displaySignInMenu,
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
                <CreatingButtons toggleMenu={this.toggleGroupConfigMenu}/>
                {this.state.displayGroupCreateMenu && 
                    <GroupCreateMenu 
                        configType = {this.state.groupConfigMenuType}
                        toggledGroup={this.props.toggledGroup}
                        fetchGroupList={this.props.fetchGroupList} toggleGroupCreateMenu={this.toggleGroupConfigMenu} />
                }
                        
                <CreatingLoginButtons toggleMenu={this.toggleSignInMenu} buttonName={{name: "Sign In"}} />
                {this.state.displaySignInMenu && <UserSignInMenu toggleMenu={this.toggleSignInMenu}/>}

                <CreatingLoginButtons toggleMenu={this.toggleLoginMenu} buttonName={{name: "Login"}} />
                {this.state.displayLoginMenu && <UserLoginMenu />}

                {this.state.notes == null || this.state.notes.length === 0 ? <p>No notes found.</p> : !this.state.showNote && <NoteDisplay notes={this.state.notes} openNote={this.openNote}/>}
                {this.state.showNote && <NoteHub noteId={this.state.noteId} exitNote={this.exitNote}/>}

            </div>
        );
    }
}
