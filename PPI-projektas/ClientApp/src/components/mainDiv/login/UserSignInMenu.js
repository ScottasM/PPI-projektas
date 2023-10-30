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

        this.handlePost(username, email, password);

        this.setState({
            username: '',
            email: '',
            password: '',
        });
    }

    async handlePost(username, email, password) {
        const userData = {
            username: username,
            password: password,
        };

        await fetch(`http://localhost:5268/api/Authentication/tryregister`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userData),
        })
            .then(async (response) => {
                if(response.status === 400){
                    alert(await response.text());
                }
                
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                
                if (data)
                    this.props.setCurrentUser(data);
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
