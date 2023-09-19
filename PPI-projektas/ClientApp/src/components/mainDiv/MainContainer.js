import React, { Component } from 'react';
import {GroupCreateMenu} from "../GroupCreateMenu";
import {CreatingButtons} from "./CreatingButtons";
export class MainContainer extends Component {
    static displayName = MainContainer.name;


    state = {
        displayGroupCreateMenu: false,
    };
    
    toggleGroupCreateMenu = () => {
        console.log('I was summoned');
        this.setState((prevState) => ({
            displayGroupCreateMenu: !prevState.displayGroupCreateMenu,
        }));
    }
    
    render() {
        console.log(this.state.displayGroupCreateMenu);
        return (
            <div className="bg-white">
                <CreatingButtons toggleMenu={this.toggleGroupCreateMenu}/>
                {this.state.displayGroupCreateMenu && <GroupCreateMenu />}
            </div>
        );
    }
}