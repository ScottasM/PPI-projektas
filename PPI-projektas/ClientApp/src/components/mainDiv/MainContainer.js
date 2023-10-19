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
    }
    
    state = {
        displayGroupCreateMenu: false,
        groupConfigMenuType: 'create',
        displayLoginMenu: false,
        displaySignInMenu: false,
        noteId: '',
        displayNote: false
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

    openNote = id => {
        this.setState(prevState => ({
            noteId: id,
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
                
                {this.state.displayNote ? <NoteHub display={1} noteId={this.state.noteId} exitNote={this.exitNote}/> : <NoteDisplay openNote={this.openNote}/>}
            </div>
        );
    }
}
