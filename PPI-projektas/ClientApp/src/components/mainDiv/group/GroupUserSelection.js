import React, {Component} from "react";
import '../../Group.css'
import '../buttons.css'
import {MdOutlinePersonRemove, MdPersonAddAlt} from "react-icons/md";
import {ScrollContainer} from "../ScrollContainer";

export class GroupUserSelection extends Component {
    static displayName = GroupUserSelection.name;

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
                !this.props.members.some(member => member.id === el.id) &&
                el.id !== this.props.currentUserId
            );

            this.setState({ users: userData});
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }
    
    addMember = (id) => {
        const userToAdd = this.state.users.find(user => user.id === id)
        
        if(userToAdd) {
            const newMembers = [...this.props.members, userToAdd];
            const newUsers = this.state.users.filter(user => user.id !== id);
            
            this.setState({
                users: newUsers
            });
            
            this.props.updateMembers(newMembers);
        }
    }
    
    removeMember = (id) => {
        const memberToRemove = this.props.members.find(member => member.id === id);

        if(memberToRemove) {
            const newMembers = this.props.members.filter(member => member.id !== id);
            
            if(this.state.users.find(user => user.name.includes(this.state.userSearch))) {
                
                const newUsers = [...this.state.users, memberToRemove];
                
                this.setState({
                    users: newUsers
                });
            }
            
            this.props.updateMembers(newMembers);
        }
    }
    
    addAdministrator = (id) => {
        const administratorToAdd = this.props.members.find(member => member.id === id);
        
        if (administratorToAdd) {
            const newAdministrators = [...this.props.administrators, administratorToAdd];
            const newMembers = this.props.members.filter(member => member.id !== id);
            
            this.props.updateAdministrators(newAdministrators);
            this.props.updateMembers(newMembers);
        }
    }
    
    removeAdministrator = (id) => {
        const administratorToRemove = this.props.administrators.find(administrator => administrator.id === id);

        if (administratorToRemove) {
            const newAdministrators = this.props.administrators.filter(administrator => administrator.id !== id);
            const newMembers = [...this.props.members, administratorToRemove];

            this.props.updateAdministrators(newAdministrators);
            this.props.updateMembers(newMembers);
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
                    behaviour={this.addMember}
                    iconType={(<MdPersonAddAlt/>)}/>
                <p className="m-0">Group Members</p>
                <ScrollContainer
                    elements={this.props.members}
                    buttonClassName={"remove-user rounded-circle"}
                    behaviour={this.removeMember}
                    iconType={(<MdOutlinePersonRemove/>)}/>
                {this.props.isOwner && <div>
                    <p className="m-0">Group Administrators</p>
                    <ScrollContainer
                        elements={this.props.members}
                        buttonClassName={"add-user rounded-circle"}
                        behaviour={this.addAdministrator}
                        iconType={(<MdPersonAddAlt/>)}/>
                    <ScrollContainer
                        elements={this.props.administrators}
                        buttonClassName={"remove-user rounded-circle"}
                        behaviour={this.removeAdministrator}
                        iconType={(<MdOutlinePersonRemove/>)}/>
                </div>}
            </div>
        );
    }

    static defaultProps = {
        members: [],
    };
}