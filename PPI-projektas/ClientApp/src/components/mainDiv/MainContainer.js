import React, { Component } from 'react';
import { NoteDisplay } from "./notes/NoteDisplay";
import { NoteHub } from "./notes/NoteHub";
import { GroupCreateMenu } from "./group/GroupCreateMenu";
import { UserLoginMenu } from "./login/UserLoginMenu";
import { UserSignInMenu } from "./login/UserSignInMenu";
import { CreatingButtons } from "./CreatingButtons";
import { CreatingLoginButtons } from "./login/CreatingLoginButtons";
import { CreatingNotesButton } from "./notes/CreatingNotesButton";

export class MainContainer extends Component {
    static displayName = MainContainer.name;

    constructor(props) {
        super(props);
    }
    
    state = {
        displayGroupCreateMenu: false,
        groupConfigMenuType: 'create',
        displayLoginMenu: false,
        displaySignInMenu: false,
        noteId: '',
        displayNote: false,
        noteHubDisplay: 1
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
    
    handleCreateNote = async () => {
        fetch(`http://localhost:5268/api/note/createNote/0f8fad5b-d9cb-469f-a165-70867728950e`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(async response => {
                if (!response.ok)
                    throw new Error('Network response was not ok');
                return await response.json();
            })
            .then(noteId =>
                this.openNote(noteId, 2)
            )
            .catch(error =>
                console.error('There was a problem with the fetch operation:', error));
    }
    
    toggleGroupConfigMenu = () => {
        if (!(this.state.displayGroupCreateMenu)) {
            this.setState({ displayLoginMenu: false, displaySignInMenu: false })
        }
        else{
            this.setState({
                groupConfigMenuType: 'create',
            })
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

    openNote = (noteId, display) => {
        this.setState(prevState => ({
            noteId: noteId,
            noteHubDisplay: display,
            displayNote: true
        }));
    }

    exitNote = () => {
        this.setState(prevState => ({
            noteId: '',
            displayNote: false
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
                
                <CreatingNotesButton handleCreateNote={this.handleCreateNote} />
                
                {this.state.displayNote ? <NoteHub display={this.state.noteHubDisplay} noteId={this.state.noteId} exitNote={this.exitNote} /> : <NoteDisplay openNote={this.openNote} />}
            </div>
        );
    }
}
