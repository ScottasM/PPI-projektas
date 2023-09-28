import React, { Component } from 'react';
import './Group.css';

export class Group extends Component {
    static displayName = Group.name;
    
    constructor(props) {
        super(props);
        this.state = {
            isContextMenuVisible: false,
        }
    }

    showContextMenu = (event) => {
        event.preventDefault();
        this.setState({ isContextMenuVisible: true });
        
        document.addEventListener('click', this.hideContextMenu);
    }
    hideContextMenu = () => {
        this.setState({ isContextMenuVisible: false });
        document.removeEventListener('click', this.hideContextMenu);
    }
    
    render() {
        const { isContextMenuVisible } = this.state;

        return (
            <div className="group-container">
                <button className="group bg-white rounded-circle" onContextMenu={this.showContextMenu}>
                    {this.props.groupInitials}
                </button>
                {isContextMenuVisible && (
                    <div className="context-menu">
                        <ul>
                            <li>
                                <button className="context-button">Edit</button>
                            </li>
                            <li>
                                <button className="context-button">Delete</button>
                            </li>
                        </ul>
                    </div>
                )}
            </div>
        );
    }
}