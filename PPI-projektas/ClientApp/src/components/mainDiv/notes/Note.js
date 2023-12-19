import React, {Component} from 'react'
import './Note.css'
import { MdDelete, MdEditDocument } from "react-icons/md";
import { PiAddressBook } from "react-icons/pi";

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
                    {noteData.text}
                </div>
                <div className="note-buttons">
                    {noteData.canEditPrivileges &&
                        <button className="button delete-button">
                            <MdDelete />
                        </button>
                    }
                    {noteData.canEditNote &&
                        <button className="button edit-button">
                            <MdEditDocument />
                        </button>
                    }
                    {noteData.canEditPrivileges &&
                        <button className="button privileges-button">
                            <PiAddressBook />
                        </button>
                    }
                </div>
            </div>
        )
    }
}