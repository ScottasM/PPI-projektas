import React, { Component } from 'react';
import '../../Group.css'
import {UserSelection} from "./UserSelection";

export class GroupCreateMenu extends Component {
    static displayName = GroupCreateMenu.name;

    constructor(props) {
        super(props);
        this.state = {
            groupName: '',
            members: [],
        };
    }
    
    componentDidMount() {
        if(this.props.configType === 'edit'){
            this.setState({
                groupName: this.props.toggledGroup.name,
            }, () => {
                this.handleMemberGet();
            });
        }
        else{
            this.setState({
                groupName: '',
                members: [],
            })
        }
    }

    handleInputChange = (event) => {
        this.setState({ groupName: event.target.value });
    };

    handleMemberGet = async () => { //TODO: Group member fetching
        try {
            const response = await fetch(`http://localhost:5268/api/group/group-members/${this.props.toggledGroup.id}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const responseData = await response.json();

            const memberData = responseData.map(user => ({
                id: user.id,
                name: user.name
            }));

            this.setState({ members: memberData});
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }
    
    updateMembers = (updatedMembers) => {
        this.setState({
            members: updatedMembers
        });
    }

    handleSubmit = (event) => {
        event.preventDefault();

        const { groupName } = this.state;
        this.handlePost(groupName)
        this.setState({ groupName: '' });
    };
    
   async handlePost(groupName) {
       
       const groupData = {
           GroupName: groupName,
           Id: this.props.configType === 'create' ? '0f8fad5b-d9cb-469f-a165-70867728950e' : this.props.toggledGroup.id, // temporary static user id
           MemberIds : this.state.members.map(member => member.id)
       };

       await fetch(`http://localhost:5268/api/group/${this.props.configType}group`, { // temporary localhost api url
           method: this.props.configType === 'create' ? 'POST' : 'PUT',
           headers: {
               'Content-Type': 'application/json',
           },
           body: JSON.stringify(groupData),
       })
           .then((response) => {
               if (!response.ok) {
                   throw new Error('Network response was not ok');
               }
           })
           .catch((error) => {
               console.error('There was a problem with the fetch operation:', error);
           });

       this.props.fetchGroupList();
       await this.props.toggleGroupCreateMenu();
    }
    
    render() {
        const { groupName } = this.state;
        
        return (
            <div className="groupCreateMenu position-absolute translate-middle text-white">
                <div className="title">
                    <h2>{this.props.configType.charAt(0).toUpperCase() + this.props.configType.slice(1)} Group</h2>
                </div>
                <form onSubmit={this.handleSubmit}>
                    <label>Group Name:</label>
                    <input
                        type="text" 
                        id="group-name" 
                        name="group-name" 
                        value={groupName}
                        onChange={this.handleInputChange}
                    />
                    <br />
                    <br />
                    <UserSelection
                        members = {this.state.members}
                        updateMembers = {this.updateMembers}/>
                    <br />
                    <input type="submit" name="createButton" value={this.props.configType.charAt(0).toUpperCase() + this.props.configType.slice(1)} />
                </form>
            </div>
        );
    }
}
