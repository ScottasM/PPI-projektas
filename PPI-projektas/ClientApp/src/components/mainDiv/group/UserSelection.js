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
                    {this.props.users.map((user, index) => (
                        <div key={user.id} className="scroll-item">
                            {user.name}
                        </div>
                    ))}
                </div>
            </div>
        );
    }
}