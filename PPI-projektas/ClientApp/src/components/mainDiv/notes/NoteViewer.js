import React, {Component} from 'react'
import {TagList} from "../../TagList";
import {MdDelete, MdEditDocument} from "react-icons/md";
import { AiFillStar } from "react-icons/ai";
import { PiAddressBook } from "react-icons/pi";

export class NoteViewer extends Component {
    constructor (props) {
        super(props)
        this.state = {
            favorited: false
        }
    }

    handleFavoriteNote = () => {

    }

    render() {
        const {noteData} = this.props;
        const maxVisibleTags = 3;

        return (
            <div className={"note-card selected"}>
                <div className="note-title">
                    <p>{noteData.name}</p>
                    <div className="fav-button">
                        <AiFillStar
                            className="star"
                            color={this.state.favorited ? "#ffc107" : "#e4e5e9"}
                            size={25}
                            onClick={() => this.handleFavoriteNote() & this.setState({ favorited: (this.state.favorited ? false : true) })}
                        />
                    </div>
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
                    {noteData.canEditPrivileges &&
                        <button className="button button-hover delete-button delete-button-hover no-close-button" onClick={this.props.deleteNote}>
                            <MdDelete />
                        </button>
                    }
                    {noteData.canEditNote &&
                        <button className="button button-hover edit-button edit-button-hover no-close-button" onClick={() => this.props.changeDisplay(2, '')}>
                            <MdEditDocument />
                        </button>
                    }
                    {noteData.canEditPrivileges &&
                        <button className="button button-hover privileges-button privileges-button-hover" onClick={this.props.toggleNotePrivilegeMenu}>
                            <PiAddressBook />
                        </button>
                    }
                </div>
            </div>
        )
    }

    static defaultProps = {
        noteData: 0,
    };
}