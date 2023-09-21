import React, { Component } from 'react';
import './Group.css';

export class Group extends Component {
    static displayName = Group.name;

    render() {
        return (
            <div className="group bg-white rounded-circle">
                <p>Group</p>
            </div>
        );
    }
}