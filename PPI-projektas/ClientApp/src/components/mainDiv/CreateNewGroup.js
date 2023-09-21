import React, { Component } from 'react';
import '../Group.css';

export class CreateNewGroup extends Component {
    static displayName = CreateNewGroup.name;

    constructor(props) {
        super(props);
    }

    render() {

        return (
                <button className="create-button" onClick={this.props.toggleMenu}>Create New Group</button>
        );
    }
}