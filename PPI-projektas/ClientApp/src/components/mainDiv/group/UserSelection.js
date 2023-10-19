import React, {Component} from "react";
import '../../Group.css'
import '../buttons.css'
import {MdOutlinePersonRemove, MdPersonAddAlt} from "react-icons/md";

export class UserSelection extends Component {
    static displayName = UserSelection.name;

    constructor(props) {
        super(props);
        this.scrollContainerRef = React.createRef();
        this.memberScrollContainerRef = React.createRef();
        this.scrollPosition = 0;
        this.memberScrollPosition = 0;
``        
        this.state = {
            userSearch: '',
            users: []
        }
    }
    
    componentDidMount() {
        this.scrollContainerRef.current.addEventListener("wheel", this.handleScroll);
        this.memberScrollContainerRef.current.addEventListener("wheel", this.handleMemberScroll);
    }
    
    componentWillUnmount() {
        this.scrollContainerRef.current.removeEventListener("wheel", this.handleScroll);
        this.memberScrollContainerRef.current.addEventListener("wheel", this.handleMemberScroll);
    }

    handleScroll = (e) => {
        const scrollContainer = this.scrollContainerRef.current;
        const scrollAmount = 100;
        if (e.deltaY > 0) {
            this.scrollPosition -= this.scrollPosition > 0 ? scrollAmount : 0;
        } else {
            this.scrollPosition += (this.scrollPosition + scrollAmount) <= scrollContainer.offsetWidth ? scrollAmount : 0;
        }

        scrollContainer.style.transition = "transform 0.3s ease";
        scrollContainer.style.transform = `translateX(-${this.scrollPosition}px)`;
        
        setTimeout(() => {
            scrollContainer.style.transition = "none";
        }, 300);
    };

    handleMemberScroll = (e) => {
        const scrollContainer = this.memberScrollContainerRef.current;
        const scrollAmount = 100;
        if (e.deltaY > 0) {
            this.memberScrollPosition -= this.memberScrollPosition > 0 ? scrollAmount : 0;
        } else {
            this.memberScrollPosition += (this.memberScrollPosition + scrollAmount) <= scrollContainer.offsetWidth ? scrollAmount : 0;
        }

        scrollContainer.style.transition = "transform 0.3s ease";
        scrollContainer.style.transform = `translateX(-${this.memberScrollPosition}px)`;

        setTimeout(() => {
            scrollContainer.style.transition = "none";
        }, 300);
    };

    handleUserSearch = (event) => {
        this.setState({userSearch: event.target.value }, () => {
            if(this.state.userSearch){
                this.handleUserGet();
            }
        });
    }

    handleUserGet = async () => {
        try {
            const response = await fetch(`http://localhost:5268/api/user/${this.state.userSearch}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const responseData = await response.json();

            let userData = responseData.map(user => ({
                id: user.id,
                name: user.name
            }));
            
            userData = userData.filter((el) => !this.props.members.includes(el));

            this.setState({ users: userData});
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }
    
    addMember = (id) => {
        const userToAdd = this.state.users.find(user => user.id === id)
        
        if(userToAdd) {
            const newMembers = [...this.props.members, userToAdd];
            const newUsers = this.state.users.filter(user => user.id !== id);
            
            this.setState({
                users: newUsers
            });
            
            this.props.updateMembers(newMembers);
        }
    }
    
    removeMember = (id) => {
        const memberToRemove = this.props.members.find(member => member.id === id);

        if(memberToRemove) {
            const newMembers = this.props.members.filter(member => member.id !== id);
            
            if(this.state.users.find(user => user.name.includes(this.state.userSearch))) {
                
                const newUsers = [...this.state.users, memberToRemove];
                
                this.setState({
                    users: newUsers
                });
            }
            
            this.props.updateMembers(newMembers);
        }
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
                    onChange={this.handleUserSearch}
                />
                <br/>
                <div className="scroll-container" ref={this.scrollContainerRef}>
                    {this.state.users.map((user) => (
                        <div key={user.id} className="scroll-item">
                            <div className="item-content">
                                <p>{user.name}</p>
                                    <button type="button" className="add-user rounded-circle" onClick={() => this.addMember(user.id)}>
                                        <MdPersonAddAlt/>
                                    </button>
                            </div>
                        </div>
                    ))}
                </div>
                <p>Group Members</p>
                <div className="scroll-container" ref={this.memberScrollContainerRef}>
                    {this.props.members.map((member) => (
                        <div key={member.id} className="scroll-item">
                            <div className="item-content">
                                <p>{member.name}</p>
                                <button type="button" className="remove-user rounded-circle" onClick={() => this.removeMember(member.id)}>
                                    <MdOutlinePersonRemove/>
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        );
    }

    static defaultProps = {
        members: [],
    };
}