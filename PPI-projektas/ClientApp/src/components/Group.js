import React, { Component } from 'react';
import './Group.css';

export class Group extends Component {
    static displayName = Group.name;
    
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="group bg-white rounded-circle">
                <p>{this.props.groupInitials}</p>
            </div>
        );
    }
}