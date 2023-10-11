import React, {Component} from "react";
import '../../Group.css'

export class UserSelection extends Component {
    static displayName = UserSelection.name;

    constructor(props) {
        super(props);
    }

    render() {

        return (
            <div className="user-selection">
                <p className="m-0">Search for users:</p>
                <input
                    type="text"
                    id="user-search"
                    name="user-search"
                    value={this.props.userSearch}
                    onChange={this.props.handleUserSearch}
                />
                <div className="scroll-container">
                    {this.props.users.map((user) => (
                        <div key={user.id} className="scroll-item">
                            <p>{user.username}</p>
                        </div>
                    ))}
                </div>
                <p>Group Members</p>
                <div className="scroll-container">
                    {this.props.members.map((member) => (
                        <div key={member.id} className="scroll-item">
                            <p>{member.username}</p>
                        </div>
                    ))}
                </div>
            </div>
        );
    }

    static defaultProps = {
        users: [],
        members: [],
    };
}