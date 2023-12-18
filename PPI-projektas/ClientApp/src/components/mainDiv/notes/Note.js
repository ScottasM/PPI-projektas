import React, {Component} from 'react'
import './Note.css'
import { MdDelete, MdEditDocument } from "react-icons/md";

export class Note extends Component {
    constructor(props) {
        super(props);
    }
    render() {
        const { noteData, handleSelect} = this.props;
        const maxVisibleTags = 3;
        
        return (
            <div className="note-card unselected" onClick={(event) => handleSelect(event, noteData.id)}>
                <div className="note-title">
                    <p>{noteData.name}</p>
                </div>
                <div className="note-tags">
                    {noteData.tags != null && noteData.tags.length > 0 && noteData.tags.slice(0, maxVisibleTags).map(tag => (
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
                    <button className="button delete-button">
                        <MdDelete />
                    </button>
                    <button className="button edit-button">
                        <MdEditDocument />
                    </button>
                </div>
            </div>
        )
    }
}