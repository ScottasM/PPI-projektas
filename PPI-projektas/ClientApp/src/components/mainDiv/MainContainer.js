import React, { Component } from 'react';
import { NoteDisplay } from "./notes/NoteDisplay";
import { NoteHub } from "./notes/NoteHub";
import { GroupCreateMenu } from "./group/GroupCreateMenu";
import { UserLoginMenu } from "./login/UserLoginMenu";
import { UserSignInMenu } from "./login/UserSignInMenu";
import { CreatingButtons } from "./CreatingButtons";
import { CreatingLoginButtons } from "./login/CreatingLoginButtons";
import { CreatingNotesButton } from "./notes/CreatingNotesButton";
import './MainContainer.css';
import {NotePrivilegeMenu} from "./notes/privileges/NotePrivilegeMenu";

export class MainContainer extends Component {
    static displayName = MainContainer.name;

    constructor(props) {
        super(props);
        this.state = {
            displayGroupCreateMenu: this.props.displayGroupEditMenu,
            groupConfigMenuType: 'create',
            displayLoginMenu: false,
            displaySignInMenu: false,
            displayNotePrivilegeMenu: false,
            notes: [],
            selectedNote: 0,
            displayNote: false,
            noteHubDisplay: 1,
            currentUserName: '',
            createNote: false,
        }
    }
    
    componentDidMount() {
        // this.fetchNotes();
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
    handleCreateNote = (createNote) => {
        this.setState({
            createNote: createNote,
        });
    }
    
    toggleNotePrivilegeMenu = () => {
        this.setState((prevState) => ({
            displayNotePrivilegeMenu: !prevState.displayNotePrivilegeMenu
        }));
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

    handleLogout = () => {
        this.props.setCurrentUser(0);
        this.setState({ displayGroupCreateMenu: false });
    }
    
    setUserName = (username) => {
        this.setState({
            currentUserName: username,
        })
    }
    
    handleNoteSelect = (event, noteId) => {
        this.setState({
            selectedNote: noteId,
        })

        event.stopPropagation();
    }
    
    resetSelectedNote = () => {
        this.setState({
            selectedNote: 0
        });
    }
    
    render() {
        return (
            <div className="main-container">
                {this.props.currentUserId !== 0 && (
                    <>
                        <div className="top-nav">
                        <CreatingButtons toggleMenu={this.toggleGroupConfigMenu} />
                        <CreatingLoginButtons toggleMenu={this.handleLogout} buttonName={{ name: 'Log out' }} />
                        <div className="register-buttons-div">
                            <h6>Logged in as: {this.state.currentUserName}</h6>
                        </div>

                        {this.props.currentGroupId !== 0 &&
                            <CreatingNotesButton
                                handleCreateNote={this.handleCreateNote}
                                groupId={this.props.currentGroupId}
                            />
                        }
                        </div>
                        
                        <NoteDisplay selectedNote={this.state.selectedNote}
                                     handleNoteSelect={this.handleNoteSelect}
                                     resetSelectedNote={this.resetSelectedNote}
                                     currentGroupId={this.props.currentGroupId}
                                     currentUserId={this.props.currentUserId}
                                     createNote={this.state.createNote}
                                     noteCreated={() => this.handleCreateNote(false)}
                        />
                    </>
                )}

                
                {this.props.currentUserId === 0 && (
                    <>
                        <div className="top-nav">
                            <CreatingLoginButtons toggleMenu={this.toggleSignInMenu} buttonName={{name: "Sign Up"}}/>
                            <CreatingLoginButtons toggleMenu={this.toggleLoginMenu} buttonName={{name: "Login"}}/>
                        </div>
                        <h1><b>Welcome to NoteNest</b><br /> Login or sign up to start sharing notes</h1>
                    </>
                )}
                        
                {this.props.isOwner && this.state.displayGroupCreateMenu &&
                    <GroupCreateMenu 
                        configType = {this.state.groupConfigMenuType}
                        toggledGroup={this.props.toggledGroup}
                        fetchGroupList={this.props.fetchGroupList} toggleGroupCreateMenu={this.toggleGroupConfigMenu} 
                        currentUserId={this.props.currentUserId}
                        isOwner={this.props.isOwner}/>
                }

                {this.state.displayNotePrivilegeMenu &&
                    <NotePrivilegeMenu
                        noteId={this.state.selectedNote}
                        currentUserId={this.props.currentUserId}
                        toggleNotePrivilegeMenu={this.toggleNotePrivilegeMenu}/>
                }
                
                {this.state.displaySignInMenu && 
                    <UserSignInMenu 
                        toggleMenu={this.toggleSignInMenu}
                        setUserName={this.setUserName}
                        setCurrentUser={this.props.setCurrentUser}
                    />}

                {this.state.displayLoginMenu && 
                    <UserLoginMenu
                        toggleMenu={this.toggleLoginMenu}
                        setUserName={this.setUserName}
                        setCurrentUser={this.props.setCurrentUser}
                    />}
            </div>
        );
    }
}
