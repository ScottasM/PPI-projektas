import React, { Component } from 'react';
import '../../LoginWindow.css'

export class UserLoginMenu extends Component {
    static displayName = UserLoginMenu.name;

    constructor() {
        super();
        this.state = {
            username: '',
            password: '',
        };
    }

    handleUsernameInputChange = (event) => {
        this.setState({ username: event.target.value });
    };

    handlePasswordInputChange = (event) => {
        this.setState({ password: event.target.value });
    };

    handleSubmit = (event) => {
        //to do
    };

    render() {
        const { username, password } = this.state;

        return (
            <div className="userLoginMenu position-fixed translate-middle text-white">
                <div className="title">
                    <h2>Login</h2>
                </div>
                <form onSubmit={this.handleSubmit}>
                    <label><b>Username: </b></label>
                    <br />
                    <input
                        type = "text"
                        id = "user-name"
                        name = "user-name"
                        value = { username }
                        onChange = { this.handleUsernameInputChange }
                    />
                    <br />
                    <label><b>Password: </b></label>
                    <br />
                    <input
                        type = "text"
                        id = "password"
                        name = "password"
                        value = { password }
                        onChange = { this.handlePasswordInputChange }
                    />
                    <br />
                    <input className="submitButton" type="submit" value="Login" />
                </form>
            </div>
        );
    }
}
