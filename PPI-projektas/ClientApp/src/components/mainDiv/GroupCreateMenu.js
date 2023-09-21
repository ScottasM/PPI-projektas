import React, { Component } from 'react';
import '../Group.css'

export class GroupCreateMenu extends Component {
    static displayName = GroupCreateMenu.name;

    render() {
        return (
            <div className="groupCreateMenu position-absolute translate-middle text-white">
                <div className="title">
                    <h2>Create New Group</h2>
                </div>
                <form>
                    <label>Group Name:</label>
                    <input type="text" id="group-name" name="group-name" />
                    <br />
                    <input type="submit" value="Create" />
                </form>
            </div>
        );
    }
}