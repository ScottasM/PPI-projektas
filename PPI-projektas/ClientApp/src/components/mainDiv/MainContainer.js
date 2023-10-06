import React, { Component } from 'react';
import {GroupCreateMenu} from "./group/GroupCreateMenu";
import {CreatingButtons} from "./CreatingButtons";
export class MainContainer extends Component {
    static displayName = MainContainer.name;

    constructor(props) {
        super(props);
    }
    

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
                {this.state.displayGroupCreateMenu && <GroupCreateMenu fetchGroupList={this.props.fetchGroupList} toggleGroupCreateMenu={this.toggleGroupCreateMenu} />}
            </div>
        );
    }
}