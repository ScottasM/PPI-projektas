import React, { Component } from 'react';
import './SideNav.css';
import { Group } from './Group';
import { IconContext } from "react-icons";
import { FiSettings } from 'react-icons/fi';

export class SideNav extends Component {
    static displayName = SideNav.name;

    constructor(props) {
        super(props);
        this.state = {
            groupNames: [],
        };
    }
    
    async componentDidMount() {
        const ownerId = '00000000-0000-0000-0000-000000000000';
        
        try {
            const response = await fetch(`http://localhost:5268/api/group?ownerId=${ownerId}`); // temporary url with temporary owner id
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json(); // Assuming the API returns JSON data with group names
            this.setState({ groupNames: data });
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }

    render() {
        const { groupNames } = this.state;
        
        return (
            <nav className="sidenav">
                {groupNames.map((groupName, index) => (
                    <Group key={index} groupInitials={groupName.substring(0, 3)} />
                ))}
                <div className="settings-div position-absolute">
                    <IconContext.Provider value={{ size: "2em" }}>
                        <button className="settings-button bg-transparent border-0"><FiSettings /></button>
                    </IconContext.Provider>
                </div>
            </nav>
        );
    }
}
