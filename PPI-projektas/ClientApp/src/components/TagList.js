import React, { Component } from 'react';

export class TagList extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <div>
            {this.props.tags.count > 0 ? this.props.tags.map(tag =>
                <div className="inLineTag">{tag}</div>
            ) : <br/>}
        </div>
    }
}