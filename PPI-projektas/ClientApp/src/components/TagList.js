import React, { Component } from 'react';

export const TagList = (props) => {
    return <div>
        {this.props.Tags.map((tag) => (
            <div className="inline-tag">{tag}</div>
        ))}
    </div>
}
/*export class Tag extends Component {
    constructor(props) {
        super(props);
    }

    renderTagsList() {
        return <div>
            {this.props.Tags.map((tag) => (
                <div className="inline-tag">{tag}</div>
            ))}
        </div>
    }
}*/