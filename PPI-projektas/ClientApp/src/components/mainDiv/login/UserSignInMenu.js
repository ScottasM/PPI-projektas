import React, { Component } from 'react';
import '../../LoginWindow.css'

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
        event.preventDefault();
        const { username, email, password } = this.state;

        this.handlePost(username, email, password)

        this.setState({
            username: '',
            email: '',
            password: '',
        });
    }

    async handlePost(username, email, password) {

        const userData = {
            Username: username,
            Email: email,
            Password: password,
            OwnerId: '00000000-0000-0000-0000-000000000000',
        };

        await fetch(`http://localhost:5268/api/user/postuser`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userData),
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
            })
            .catch((error) => {
                console.error('There was a problem with the fetch operation:', error);
            });

        this.props.toggleMenu();
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
