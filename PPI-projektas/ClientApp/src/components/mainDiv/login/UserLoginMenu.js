import React, { Component } from 'react';
import '../InputWindow.css';

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
        event.preventDefault();
        const { username, password } = this.state;

        this.handlePost(username, password);

        this.setState({
            username: '',
            email: '',
            password: '',
        });
    };

    async handlePost(username, password) {
        const userData = {
            username: username,
            password: password,
        };

        await fetch(`http://localhost:5268/api/Authentication/trylogin`, {
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
                else if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                else{
                    const data = await response.json();
                    
                    if (data)
                    {
                        this.props.setCurrentUser(data);
                        this.props.setUserName(username);
                    }
                }

            })
            .catch((error) => {
                console.error('There was a problem with the fetch operation:', error);
            });

        this.props.toggleMenu();
    };

    render() {
        const { username, password } = this.state;

        return (
            <div className="userLoginMenu position-fixed translate-middle text-white">
                <div className="title">
                    <h2>Login</h2>
                </div>
                <form onSubmit={this.handleSubmit}>
                    <label>Username: </label>
                    <br />
                    <input
                        type = "text"
                        id = "user-name"
                        name = "user-name"
                        value = { username }
                        onChange = { this.handleUsernameInputChange }
                    />
                    <br />
                    <label>Password: </label>
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
