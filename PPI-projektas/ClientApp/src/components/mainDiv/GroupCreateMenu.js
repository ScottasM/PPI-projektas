import React, { Component } from 'react';
import axios from 'axios';
import '../Group.css'

export class GroupCreateMenu extends Component {
    static displayName = GroupCreateMenu.name;

    constructor() {
        super();
        this.state = {
            groupName: '',
        };
    }

    handleInputChange = (event) => {
        this.setState({ groupName: event.target.value });
    };

    handleSubmit = (event) => {
        event.preventDefault();
        const { groupName } = this.state;
        
        const data = {
            name: groupName,
            owner: {
                // Owner data
            }
        }
        
        console.log('Group Name:', groupName);
        
        this.handlePost(groupName)
        
        this.setState({ groupName: '' });
    };
    
   async handlePost(groupName) {
       
       const groupData = {
           GroupName: groupName,
       };
       
       let data = new FormData();
       data.append("groupData", JSON.stringify(groupData));
       
       fetch('http://localhost:5268/api/group/creategroup',
           {
               method: "Post",
               body: data
           })
       
       console.log('Posted');
    }
    
    render() {
        const { groupName } = this.state;
        
        return (
            <div className="groupCreateMenu position-absolute translate-middle text-white">
                <div className="title">
                    <h2>Create New Group</h2>
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
                    <input type="submit" value="Create" />
                </form>
            </div>
        );
    }
}
