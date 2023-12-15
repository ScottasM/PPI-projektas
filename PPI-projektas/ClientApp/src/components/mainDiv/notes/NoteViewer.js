import React, {Component} from 'react'
import {TagList} from "../../TagList";
import {MdDelete, MdEditDocument} from "react-icons/md";

export class NoteViewer extends Component {
    constructor (props) {
        super(props)
    }
    
    render() {
        const {noteData} = this.props;
        const maxVisibleTags = 3;

        return (
            <div className={"note-card selected"}>
                <div className="note-title">
                    <p>{noteData.name}</p>
                </div>
                <div className="note-tags">
                    {noteData !== 0 && noteData.tags != null && noteData.tags.slice(0, maxVisibleTags).map(tag => (
                            <span>{tag}</span>
                        )
                    )}
                    {noteData.tags != null && noteData.tags.length > maxVisibleTags && (
                        <span key="ellipsis">...</span>
                    )}
                </div>
                <div className="note-text">
                    <p>{noteData.text}</p>
                </div>
                <div className="note-buttons">
                    <button className="button button-hover delete-button delete-button-hover no-close-button" onClick={this.props.deleteNote}>
                        <MdDelete />
                    </button>
                    <button className="button button-hover edit-button edit-button-hover no-close-button" onClick={() => this.props.changeDisplay(2, '')}>
                        <MdEditDocument />
                    </button>
                </div>
            </div>
        )
    }

    static defaultProps = {
        noteData: 0,
    };
}