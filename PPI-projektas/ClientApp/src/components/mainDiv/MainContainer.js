import React, { Component } from 'react';
import {GroupCreateMenu} from "./GroupCreateMenu";
import {CreatingButtons} from "./CreatingButtons";
export class MainContainer extends Component {
    static displayName = MainContainer.name;


    state = {
        displayGroupCreateMenu: false,
    };
    
    toggleGroupCreateMenu = () => {
        this.setState((prevState) => ({
            displayGroupCreateMenu: !prevState.displayGroupCreateMenu,
        }));
    }
    
    render() {
        return (
            <div className="bg-white">
                <CreatingButtons toggleMenu={this.toggleGroupCreateMenu}/>
                {this.state.displayGroupCreateMenu && <GroupCreateMenu />}
            </div>
        );
    }
}