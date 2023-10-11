import React, { Component } from 'react';

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
            displayGroupCreateMenu: false,
            groupConfigMenuType: 'create',
            displayLoginMenu: false,
            displaySignInMenu: false,
        };
    }

    componentDidUpdate(prevProps) {
        if (this.props.toggledGroupId !== prevProps.toggledGroupId || this.props.displayGroupEditMenu !== prevProps.displayGroupEditMenu) {
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
    
    render() {
        return (
            <div className="bg-white">
                <CreatingButtons toggleMenu={this.toggleGroupConfigMenu}/>
                {this.state.displayGroupCreateMenu && 
                    <GroupCreateMenu 
                        configType = {this.state.groupConfigMenuType}
                        fetchGroupList={this.props.fetchGroupList} toggleGroupCreateMenu={this.toggleGroupConfigMenu} />
                }
                        
                <CreatingLoginButtons toggleMenu={this.toggleSignInMenu} buttonName={{name: "Sign In"}} />
                {this.state.displaySignInMenu && <UserSignInMenu toggleMenu={this.toggleSignInMenu}/>}

                <CreatingLoginButtons toggleMenu={this.toggleLoginMenu} buttonName={{name: "Login"}} />
                {this.state.displayLoginMenu && <UserLoginMenu />}

            </div>
        );
    }
}
