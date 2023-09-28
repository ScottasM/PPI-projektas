import React, { Component } from "react";
import './buttons.css'

export class CreatingSignInButtons extends Component {
    static displayName = CreatingSignInButtons.name;

    render() {
        return (
            <div className="registerButtonsDiv">
                <button className="create-button" onClick={this.props.toggleMenu}>Sign In</button>
            </div>
        );
    }

}