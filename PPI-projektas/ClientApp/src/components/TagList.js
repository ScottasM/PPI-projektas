import React, { Component } from 'react';

export class TagList extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <div>
            {this.props.noteTags.length > 0 ? this.props.noteTags.map(tag =>
                <div className="inLineTag">{tag}</div>
            ) : <p>Note contains no tags.</p>}
        </div>
    }
}