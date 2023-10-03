import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { SideNav } from './SideNav';
import {MainContainer} from "./mainDiv/MainContainer";

export class Layout extends Component {
  static displayName = Layout.name;

    constructor(props) {
        super(props);
        this.state = {
            mounted: false,
            groups: [],
        };
    }

    componentDidMount() {
        if(!this.state.mounted) {
            this.fetchGroupList();
            this.setState({ mounted : true});
        }
    }
    
    fetchGroupList = async () => {
        const ownerId = '00000000-0000-0000-0000-000000000000';

        try {
            const response = await fetch(`http://localhost:5268/api/group?ownerId=${ownerId}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const responseData = await response.json();

            const groupData = responseData.groups.map(group => ({
                name: group.name,
                id: group.id
            }));
            
            this.setState({ groups: groupData });
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }
    
    render() {
    return (
      <div>
          <SideNav groups={this.state.groups}/>
          <MainContainer fetchGroupList={this.fetchGroupList}/>
      </div>
    );
  }
}