import React, {Component} from 'react';
import {Dropdown, DropdownMenu, DropdownToggle} from "reactstrap";

export class NotePrivilegeMenu extends Component {
    constructor(props) {
        super(props);
    }
    
    state = {
        dropdownOpen: false,
        currentPrivileges: {},
        updatePrivileges: {},
        isLoading: true
    }
    
    handleGetPrivileges = async () => {
        await fetch(`http://localhost:5268/api/note/getPrivileges/${this.props.noteId}`)
            .then(response => {
                if (!response.ok)
                    throw new Error('Network response was not ok');
            })
            .then(data => {
                const privileges = data
                    .map(privilegeData => ({
                        id: privilegeData.id,
                        username: privilegeData.username,
                        privilege: privilegeData.privilege
                    }));
                this.setState({
                    currentPrivileges: privileges,
                    isLoading: false
                });
            })
            .catch(error =>
                console.log('There was a problem with the fetch operation:', error));
    }
    
    handleSetPrivileges = async () => {
        const data = {
            UserId: this.props.currentUserId,
            UpdatePrivileges: this.state.updatePrivileges
        }
        
        await fetch(`http://localhost:5268/api/note/updatePrivileges${this.props.noteId}`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (!response.ok)
                    throw new Error('Network response was not okay');
            })
            .then(data => {
                const failureMessage = data
                    .filter(update => update.result === 1)
                    .reduce((message, errorMessage) => (message + '\n' + errorMessage), '');
                
                if (failureMessage !== '')
                    alert(failureMessage);
                
                this.handleGetPrivileges();
            })
            .catch(error =>
                console.log('There was a problem with the fetch operation: ', error));
    }
    
    toggleDropdown = () => {
        this.setState(prevState => {
            return {
                dropdownOpen: !prevState.dropdownOpen
            }
        });
    }
    
    render() {
        return (
            <div>
                <h1>{this.props.name} user privileges</h1>
                <ul>{this.state.currentPrivileges.map(privilege => {
                    return (
                        <li>
                            <Dropdown>
                                <DropdownToggle isOpen={this.state.dropdownOpen} toggle={this.toggleDropdown}>{privilege.user}</DropdownToggle>
                                <DropdownMenu
                            </Dropdown>
                        </li>
                    )
                })}
                </ul>
            </div>
        )
    }
}