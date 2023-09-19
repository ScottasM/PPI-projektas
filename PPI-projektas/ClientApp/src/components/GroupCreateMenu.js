import React, { Component } from 'react';
import './Group.css'

export class GroupCreateMenu extends Component {
    static displayName = GroupCreateMenu.name;

    render() {
        return (
            <div className="groupCreateMenu position-absolute translate-middle bg-black text-white">
                <p>Group Name goes here!</p>
            </div>
        );
    }
}