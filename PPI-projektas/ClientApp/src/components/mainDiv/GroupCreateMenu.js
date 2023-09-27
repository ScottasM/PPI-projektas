import React, { Component } from 'react';
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
        
        this.handlePost(groupName)
        
        this.setState({ groupName: '' });
    };
    
   handlePost(groupName) {
       
       const groupData = {
           GroupName: groupName,
           OwnerId: '0f8fad5b-d9cb-469f-a165-70867728950e', // temporary static user id
       };

       fetch('http://localhost:5268/api/group/creategroup', { // temporary localhost api url
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
               console.log('Posted');
           })
           .catch((error) => {
               console.error('There was a problem with the fetch operation:', error);
           });
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
