import React, { Component } from "react";
import './buttons.css'

export class CreatingLoginButtons extends Component {
    static displayName = CreatingLoginButtons.name;

    render() {
        return (
            <div className="registerButtonsDiv">
                <button className="create-button" onClick={this.props.toggleMenu}>Log In</button>
            </div>
        );
    }

}