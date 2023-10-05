import React, { Component } from 'react';

export class TagList extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <div>
            {this.props.Tags.map((tag) => (
                <div className="inLineTag">{tag}</div>
            ))}
        </div>
    }
}