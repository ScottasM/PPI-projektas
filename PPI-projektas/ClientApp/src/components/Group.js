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
    
    handleDelete = async () => {
        try {
            await fetch(`http://localhost:5268/api/group/delete/${this.props.groupId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            
            this.props.fetchGroupList();
        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
        }
    }
    
    render() {
        const { isContextMenuVisible } = this.state;

        return (
            <div className="group-container">
                <button className="group rounded-circle" onContextMenu={this.showContextMenu}>
                    {this.props.groupInitials}
                </button>
                {isContextMenuVisible && (
                    <div className="context-menu">
                        <ul>
                            <li>
                                <button className="context-button" onClick={() => this.props.toggleGroupEditMenu(this.props.groupId)}>Edit</button>
                            </li>
                            <li>
                                <button className="context-button" onClick={this.handleDelete}>Delete</button>
                            </li>
                        </ul>
                    </div>
                )}
            </div>
        );
    }
}