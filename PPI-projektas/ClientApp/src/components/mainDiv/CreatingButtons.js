import React, {Component} from "react";
import {CreateNewGroup} from "../CreateNewGroup";
import './buttons.css'

export class CreatingButtons extends Component {
    static displayName = CreatingButtons.name;

    render() {
        return (
            <div className="creatingButtonsDiv">
                <CreateNewGroup toggleMenu={this.props.toggleMenu}/>
            </div>
        );
    }
    
}