import React, {Component} from 'react'
import './Note.css'
import { MdDelete, MdEditDocument } from "react-icons/md";

export class Note extends Component {
    constructor(props) {
        super(props);
    }
    render() {
        const { title, selected, handleSelect, id } = this.props;
        
        return (
            <div className={`note-card ${selected ? 'selected' : 'unselected'}`} onClick={() => handleSelect(id)}>
                <div className="note-title">
                    <p>{title}</p>
                </div>
                <div className="note-tags">
                    <span>Math</span>
                    <span>Formula</span>
                    <span>1 semester</span>
                </div>
                <div className="note-text">
                    <p>sample note text</p>
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