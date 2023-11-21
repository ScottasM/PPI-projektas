import React, {Component} from 'react'
import {TagList} from "../../TagList";
import {MdDelete, MdEditDocument} from "react-icons/md";

export class NoteViewer extends Component {
    constructor (props) {
        super(props)
    }
    
    render() {
        const {noteData} = this.props;

        return (
            <div className={"note-card selected"}>
                <div className="note-title">
                    <p>{noteData.name}</p>
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
                    <button className="button button-hover delete-button delete-button-hover no-close-button">
                        <MdDelete />
                    </button>
                    <button className="button button-hover edit-button edit-button-hover no-close-button" onClick={() => this.props.changeDisplay(2, '')}>
                        <MdEditDocument />
                    </button>
                </div>
            </div>
        )
    }
}