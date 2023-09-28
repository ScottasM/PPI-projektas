import React, { Component } from 'react';
import { GroupCreateMenu } from "./GroupCreateMenu";
import { UserLoginMenu } from "./UserLoginMenu";
import { UserSignInMenu } from "./UserSignInMenu";
import { CreatingButtons } from "./CreatingButtons";
import { CreatingLoginButtons } from "./CreatingLoginButtons";
import { CreatingSignInButtons } from "./CreatingSignInButtons";
export class MainContainer extends Component {
    static displayName = MainContainer.name;


    state = {
        displayGroupCreateMenu: false,
        displayLoginMenu: false,
        displaySignInMenu: false,
    };
    
    toggleGroupCreateMenu = () => {
        if (!(this.displayGroupCreateMenu)) {
            this.setState({ displayLoginMenu: false, displaySignInMenu: false })
        }

        this.setState((prevState) => ({
            displayGroupCreateMenu: !prevState.displayGroupCreateMenu,
        }));
    }

    toggleLoginMenu = () => {
        if (!(this.displayLoginMenu)) {
            this.setState({ displayGroupCreateMenu: false, displaySignInMenu: false })
        }

        this.setState((prevState) => ({
            displayLoginMenu: !prevState.displayLoginMenu,
        }));
    }

    toggleSignInMenu = () => {
        if (!(this.displaySignInMenu)) {
            this.setState({ displayGroupCreateMenu: false, displayLoginMenu: false })
        }

        this.setState((prevState) => ({
            displaySignInMenu: !prevState.displaySignInMenu,
        }));
 
    }
    
    render() {
        return (
            <div className="bg-white">
                <CreatingButtons toggleMenu={this.toggleGroupCreateMenu} />
                {this.state.displayGroupCreateMenu && <GroupCreateMenu />}

                <CreatingSignInButtons toggleMenu={this.toggleSignInMenu} />
                {this.state.displaySignInMenu && <UserSignInMenu />}

                <CreatingLoginButtons toggleMenu={this.toggleLoginMenu} />
                {this.state.displayLoginMenu && <UserLoginMenu />}
            </div>
        );
    }
}
