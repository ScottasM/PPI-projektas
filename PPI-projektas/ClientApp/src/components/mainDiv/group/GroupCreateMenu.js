import React, { Component } from 'react';
import '../../Group.css'
import {UserSelection} from "./UserSelection";

export class GroupCreateMenu extends Component {
    static displayName = GroupCreateMenu.name;

    constructor(props) {
        super(props);
        this.state = {
            groupName: '',
            userSearch: '',
            users: [],
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
    }

    handleInputChange = (event) => {
        this.setState({ groupName: event.target.value });
    };
    
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

            const userData = responseData.map(user => ({
                id: user.id,
                username: user.username
            }));

            this.setState({ users: userData});
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }

    handleMemberGet = async () => { //TODO: Group member fetching
        try {
            const response = await fetch(`http://localhost:5268/api/group/group-members/${this.props.toggledGroup.id}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const responseData = await response.json();

            const memberData = responseData.map(user => ({
                id: user.id,
                username: user.name
            }));

            this.setState({ members: memberData});
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
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
           OwnerId: '0f8fad5b-d9cb-469f-a165-70867728950e', // temporary static user id
           // MemberIds : this.state.members.map(member => member.id), TODO: Get selected members from editing menu
       };

       await fetch(`http://localhost:5268/api/group/${this.props.configType}group`, { // temporary localhost api url
           method: 'POST',
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
       this.props.toggleGroupCreateMenu();
    }
    
    render() {
        const { groupName } = this.state.groupName;
        const { userSearch } = this.state.userSearch;
        
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
                        handleUserSearch={this.handleUserSearch} userSearch={this.userSearch} 
                        users={this.state.users} members={this.state.members}/>
                    <br />
                    <input type="submit" value="Create" />
                </form>
            </div>
        );
    }
}
