import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { SideNav } from './SideNav';
import {MainContainer} from "./mainDiv/MainContainer";

export class Layout extends Component {
  static displayName = Layout.name;

    constructor(props) {
        super(props);
        this.state = {
            displayGroupEditMenu: false,
            mounted: false,
            groups: [],
            toggledGroupId: 0,
            currentUserId: 0,
            currentGroupId: 0,
        };
    }

    componentDidMount() {
        if(!this.state.mounted) {
            this.fetchGroupList();
            this.setState({ mounted : true});
        }
    }
    
    setCurrentUser = (id) => {
        this.setState({
            currentUserId: id,
        })
        
        this.fetchGroupList();
    }

    toggleGroupEditMenu = (groupId) => {
        this.setState((prevState) => ({
            displayGroupEditMenu: !prevState.displayGroupEditMenu,
            toggledGroupId: groupId,
        }));
    }
    
    toggleGroup = (groupId) => {
        if(this.state.groups.find(group => group.id === groupId))
        {
            this.setState({
                currentGroupId: groupId,
            })
        }
        else 
            this.fetchGroupList();
    }
    
    fetchGroupList = async () => {
        const ownerId = this.state.currentUserId;
        
        if(ownerId === 0)
            return;
        
        try {
            const response = await fetch(`http://localhost:5268/api/group?ownerId=${ownerId}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const responseData = await response.json();

            const groupData = responseData.map(group => ({
                id: group.id,
                name: group.name
            }));
            
            this.setState({ groups: groupData });
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }
    
    render() {
    return (
      <div>
          <SideNav fetchGroupList={this.fetchGroupList} 
                   toggleGroupEditMenu={this.toggleGroupEditMenu}
                   toggleGroup={this.toggleGroup}
                   groups={this.state.groups}
          />
          <MainContainer fetchGroupList={this.fetchGroupList} toggleGroupEditMenu={this.toggleGroupEditMenu}
                         toggledGroup={this.state.groups.find(group => group.id === this.state.toggledGroupId)}
                         displayGroupEditMenu={this.state.displayGroupEditMenu}
                         setCurrentUser={this.setCurrentUser}
                         currentUserId={this.state.currentUserId}
                         currentGroupId={this.state.currentGroupId}
          />
      </div>
    );
  }
}