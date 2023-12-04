import React, {Component} from 'react'
import './Note.css'
import { MdDelete, MdEditDocument } from "react-icons/md";

export class Note extends Component {
    constructor(props) {
        super(props);
    }
    render() {
        const { noteData, handleSelect} = this.props;
        
        return (
            <div className="note-card unselected" onClick={(event) => handleSelect(event, noteData.id)}>
                <div className="note-title">
                    <p>{noteData.name}</p>
                </div>
                <div className="note-tags">
                    {noteData.tags.map(tag => (
                        <span>{tag}</span>
                        )
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