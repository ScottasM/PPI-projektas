import React, { Component } from 'react';
import {GroupCreateMenu} from "./group/GroupCreateMenu";
import {CreatingButtons} from "./CreatingButtons";
export class MainContainer extends Component {
    static displayName = MainContainer.name;

    constructor(props) {
        super(props);
        this.state = {
            displayGroupCreateMenu: false,
            groupConfigMenuType: 'create',
        };
    }
    
    toggleGroupCreateMenu = () => { // TODO: get argument and change groupConfigMenuType accordingly
        this.setState((prevState) => ({
            displayGroupCreateMenu: !prevState.displayGroupCreateMenu,
        }));
    }
    
    render() {
        return (
            <div className="bg-white">
                <CreatingButtons toggleMenu={this.toggleGroupCreateMenu}/>
                {this.state.displayGroupCreateMenu && 
                    <GroupCreateMenu 
                        configType = {this.state.groupConfigMenuType}
                        fetchGroupList={this.props.fetchGroupList} toggleGroupCreateMenu={this.toggleGroupCreateMenu} />
                }
            </div>
        );
    }
}