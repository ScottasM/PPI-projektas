import React, {Component} from "react";
import {CreateNewGroup} from "./group/CreateNewGroup";
import './buttons.css'

export class CreatingButtons extends Component {
    static displayName = CreatingButtons.name;

    render() {
        return (
            <div className="creating-buttons-div">
                <CreateNewGroup toggleMenu={this.props.toggleMenu}/>
            </div>
        );
    }
    
}