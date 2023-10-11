import React, {Component} from "react";
import '../../Group.css'
import '../buttons.css'
import {MdPersonAddAlt} from "react-icons/md";

export class UserSelection extends Component {
    static displayName = UserSelection.name;

    constructor(props) {
        super(props);
        this.scrollContainerRef = React.createRef();
        this.scrollPosition = 0;
    }
    
    componentDidMount() {
        this.scrollContainerRef.current.addEventListener("wheel", this.handleScroll);
    }
    
    componentWillUnmount() {
        this.scrollContainerRef.current.removeEventListener("wheel", this.handleScroll);
    }

    handleScroll = (e) => {
        const scrollContainer = this.scrollContainerRef.current;
        const scrollAmount = 200;
        if (e.deltaY > 0) {
            this.scrollPosition -= this.scrollPosition > 0 ? scrollAmount : 0;
        } else {
            this.scrollPosition += (this.scrollPosition + scrollAmount) <= (this.props.users.length/2 * scrollAmount) ? scrollAmount : 0;
        }

        scrollContainer.style.transition = "transform 0.3s ease";
        scrollContainer.style.transform = `translateX(-${this.scrollPosition}px)`;
        
        setTimeout(() => {
            scrollContainer.style.transition = "none";
        }, 300);
    };

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
                <div className="scroll-container" ref={this.scrollContainerRef}>
                    {this.props.users.map((user) => (
                        //TODO: FIX SCROLLING
                        <div key={user.id} className="scroll-item">
                            <div className="item-content">
                                <p>{user.username}</p>
                                    <button className="add-user rounded-circle">
                                        <MdPersonAddAlt/>
                                    </button>
                            </div>
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