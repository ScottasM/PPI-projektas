import React, { Component } from 'react';
import './SideNav.css';
import { Group } from './Group';
import { IconContext } from "react-icons";
import { FiSettings } from 'react-icons/fi';

export class SideNav extends Component {
    static displayName = SideNav.name;

    constructor(props) {
        super(props);
    }
    
    render() {
        
        return (
            <nav className="sidenav">
                {this.props.groups.map((group, index) => (
                    <Group fetchGroupList={this.props.fetchGroupList} groupId={group.id} key={group.id} groupInitials={group.name.substring(0, 3)} />
                ))}
                <div className="settings-div position-absolute">
                    <IconContext.Provider value={{ size: "2em" }}>
                        <button className="settings-button bg-transparent border-0"><FiSettings /></button>
                    </IconContext.Provider>
                </div>
            </nav>
        );
    }


    static defaultProps = {
        groups: [],
    };
}
