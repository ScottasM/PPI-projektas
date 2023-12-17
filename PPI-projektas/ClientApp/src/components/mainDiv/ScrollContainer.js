import React, {Component} from 'react';
import {MdPersonAddAlt} from "react-icons/md";

export class ScrollContainer extends Component {
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
    
    render () {
        return (
            <div className="scroll-container" ref={this.addMemberScrollContainerRef}>
                {this.props.elements.map((element) => (
                    <div key={element.id} className="scroll-item">
                        <div className="item-content">
                            <p>{element.name}</p>
                            <button type="button" className={this.props.buttonClassName} onClick={() => this.props.behaviour(element.id)}>
                                {this.props.iconType}
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        )
    }
}