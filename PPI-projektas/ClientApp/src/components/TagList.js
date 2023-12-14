import React, { Component } from 'react';

export class TagList extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="tag-container">
                {this.props.noteTags.length > 0 ? this.props.noteTags.map(tag =>
                    <div className="tag scroll-item">
                        <div className="item-content">
                            <p>{tag}</p>
                            <button type="button" className="remove-user rounded-circle" onClick={() => this.props.deleteTag(tag)}></button>
                        </div>
                    </div>
                    ) : <p>Note contains no tags.</p>}
            </div>
        )
    }
}