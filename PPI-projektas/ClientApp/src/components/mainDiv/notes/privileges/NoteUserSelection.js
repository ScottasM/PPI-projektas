import React, {Component} from "react";
import '../../buttons.css'
import {MdOutlinePersonRemove, MdPersonAddAlt} from "react-icons/md";
import {ScrollContainer} from "../../ScrollContainer.js";

export class NoteUserSelection extends Component {
    static displayName = NoteUserSelection.name;

    constructor(props) {
        super(props);
        this.state = {
            userSearch: '',
            users: []
        }
    }

    handleUserSearch = (event) => {
        this.setState({userSearch: event.target.value }, () => {
            if(this.state.userSearch){
                this.handleUserGet();
            }
        });
    }

    handleUserGet = async () => {
        try {
            const response = await fetch(`http://localhost:5268/api/user/${this.state.userSearch}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const responseData = await response.json();

            let userData = responseData.map(user => ({
                id: user.id,
                name: user.name
            }));

            userData = userData.filter(el =>
                !this.props.editors.some(editor => editor.id === el.id) &&
                el.id !== this.props.currentUserId
            );

            this.setState({ users: userData});
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }

    addEditor = (id) => {
        const userToAdd = this.state.users.find(user => user.id === id)

        if(userToAdd) {
            const newEditors = [...this.props.editors, userToAdd];
            const newUsers = this.state.users.filter(user => user.id !== id);

            this.setState({
                users: newUsers
            });

            this.props.updateEditors(newEditors);
        }
    }

    removeEditor = (id) => {
        const editorToRemove = this.props.editors.find(editor => editor.id === id);

        if(editorToRemove) {
            const newEditors = this.props.editors.filter(editor => editor.id !== id);

            if(this.state.users.find(user => user.name.includes(this.state.userSearch))) {

                const newUsers = [...this.state.users, editorToRemove];

                this.setState({
                    users: newUsers
                });
            }

            this.props.updateEditors(newEditors);
        }
    }

    render() {

        return (
            <div className="user-selection">
                <label className="m-0">Search for users:</label>
                <br />
                <input
                    type="text"
                    id="user-search"
                    name="user-search"
                    value={this.state.userSearch}
                    onChange={this.handleUserSearch}/>
                <br />
                <ScrollContainer
                    elements={this.state.users}
                    buttonClassName={"add-user rounded-circle"}
                    behaviour={this.addEditor}
                    iconType={(<MdPersonAddAlt/>)}/>
                <p className="m-0">Note Editors</p>
                <ScrollContainer
                    elements={this.props.editors}
                    buttonClassName={"remove-user rounded-circle"}
                    behaviour={this.removeEditor}
                    iconType={(<MdOutlinePersonRemove/>)}/>
            </div>
        );
    }

    static defaultProps = {
        editors: [],
    };
}