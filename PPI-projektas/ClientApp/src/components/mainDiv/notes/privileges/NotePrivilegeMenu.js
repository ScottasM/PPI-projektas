import React, { Component } from 'react';
import '../../Group.css';
import '../InputWindow.css';
import {NoteUserSelection} from "./NoteUserSelection";

export class NotePrivilegeMenu extends Component {
    static displayName = NotePrivilegeMenu.name;

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        this.handleEditorGet();
    }

    handleEditorGet = async () => {
        try {
            const response = await fetch(`http://localhost:5268/api/note/getPrivileges/${this.props.noteId}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const responseData = await response.json();

            const editorData = responseData.map(user => ({
                id: user.id,
                name: user.name
            }));

            this.setState({ editors: editorData});
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }

    updateEditors = (updatedEditors) => {
        this.setState({
            editors: updatedEditors
        });
    }

    handleSubmit = (event) => {
        event.preventDefault();

        if(this.state.groupName)
        {
            const { groupName } = this.state;
            this.handlePost(groupName);
        }
        else
        {
            alert('Group name must not be empty');
        }
    };

    async handlePost(groupName) {

        let privilegeData = {
            Id: this.props.currentUserId,
            EditorIds : this.state.members.map(member => member.id)
        };

        await fetch(`http://localhost:5268/api/note/updatePrivilege/${this.props.noteId}`, { // temporary localhost api url
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(privilegeData),
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
            })
            .catch((error) => {
                console.error('There was a problem with the fetch operation:', error);
            });
    
        await this.props.toggleNotePrivilegeMenu();
    }

    render() {
        const { groupName } = this.state;

        return (
            <div className="note-privilege-menu position-fixed translate-middle text-white">
                <div className="title">
                    <h2>Edit Note Privileges</h2>
                </div>
                <form onSubmit={this.handleSubmit}>
                    <br />
                    <NoteUserSelection
                        currentUserId={this.props.currentUserId}
                        editors = {this.state.editors}
                        updateEditors = {this.updateEditors}/>
                    <br />
                    <input className="submit-button" type="submit" name="createButton" value={'Save'} />
                </form>
            </div>
        );
    }
}
