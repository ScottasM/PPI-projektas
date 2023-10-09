import React, { Component } from 'react';
import '../LoginWindow.css'

export class UserSignInMenu extends Component {
    static displayName = UserSignInMenu.name;

    constructor() {
        super();
        this.state = {
            username: '',
            email: '',
            password: '',
        };
    }

    handleUsernameInputChange = (event) => {
        this.setState({ username: event.target.value });
    };

    handleEmailInputChange = (event) => {
        this.setState({ email: event.target.value });
    };

    handlePasswordInputChange = (event) => {
        this.setState({ password: event.target.value });
    };

    handleSubmit = (event) => {
        //to do
    };

    render() {
        const { username, email, password } = this.state;

        return (
            <div className="userLoginMenu position-absolute translate-middle text-white">
                <div className="title">
                    <h2>Sign In</h2>
                </div>
                <form onSubmit={this.handleSubmit}>
                    <label><b>Username: </b></label>
                    <br />
                    <input
                        type="text"
                        id="user-name"
                        name="user-name"
                        value={username}
                        onChange={this.handleUsernameInputChange}
                    />
                    <br />
                    <label><b>Email: </b></label>
                    <br />
                    <input
                        type="text"
                        id="email"
                        name="email"
                        value={email}
                        onChange={this.handleEmailInputChange}
                    />
                    <br />
                    <label><b>Password: </b></label>
                    <br />
                    <input
                        type="text"
                        id="password"
                        name="password"
                        value={password}
                        onChange={this.handlePasswordInputChange}
                    />
                    <br />
                    <input className="submitButton" type="submit" value="Sign In" />
                </form>
            </div>
        );
    }
}