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

export class MainContainer extends Component {
    static displayName = MainContainer.name;

    constructor(props) {
        super(props);
        this.state = {
            displayGroupCreateMenu: this.props.displayGroupEditMenu,
            groupConfigMenuType: 'create',
            displayLoginMenu: false,
            displaySignInMenu: false,
            notes: [],
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
                        
                        <NoteDisplay currentGroupId={this.props.currentGroupId}
                                     currentUserId={this.props.currentUserId}
                                     createNote={this.state.createNote}
                                     noteCreated={() => this.handleCreateNote(false)}
                        />
                    </>
                )}

                
                {this.props.currentUserId === 0 && (
                    <div className="top-nav">
                        <CreatingLoginButtons toggleMenu={this.toggleSignInMenu} buttonName={{name: "Sign In"}}/>
                        <CreatingLoginButtons toggleMenu={this.toggleLoginMenu} buttonName={{name: "Login"}}/>
                    </div>
                )}
                        
                {this.props.isOwner && this.state.displayGroupCreateMenu &&
                    <GroupCreateMenu 
                        configType = {this.state.groupConfigMenuType}
                        toggledGroup={this.props.toggledGroup}
                        fetchGroupList={this.props.fetchGroupList} toggleGroupCreateMenu={this.toggleGroupConfigMenu} 
                        currentUserId={this.props.currentUserId}
                        isOwner={this.props.isOwner}/>
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
