import React, { Component }  from 'react';
import './NoteHub.css';
import {TagList} from "../../TagList";
import {MdDelete, MdEditDocument, MdSave} from "react-icons/md";

export class NoteEditor extends Component {
    constructor (props) {
        super(props)
        this.state = {
            noteName: this.props.noteData.name,
            noteText: this.props.noteData.text,
            noteTags: this.props.noteData.tags,
            saved: true,
            showNotSavedMessage: true,
            showDeleteMessage: true,
        }
    }

    handleSave = async () => {
        if (this.state.noteName === '')
            alert('Note name cannot be empty!');
        
        if (this.state.noteText === '')
            alert('Note text cannot be empty!');
        
        const noteData = {
            AuthorId: this.props.currentUserId,
            Name: this.state.noteName,
            Tags: this.state.noteTags,
            Text: this.state.noteText
        };

        fetch(`http://localhost:5268/api/note/updateNote/${this.props.noteId}`, { // temporary localhost api url
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(noteData)
        })
            .then(async response => {
                if (!response.ok) {
                    alert(`Changes weren't saved!`);
                    this.setState({
                        showNotSavedMessage: false
                    });
                    throw new Error('Network response was not ok');
                }
                await this.setState({
                    saved: true
                });
                this.handleExit();
            })
            .catch((error) =>
                console.error('There was a problem with the fetch operation:', error));
    }
    
    handleDelete = async () => {
        if (this.state.showDeleteMessage) {
            alert(`You're about to delete this note.`)
            this.setState({
                showDeleteMessage: false
            });
        } else fetch(`http://localhost:5268/api/note/deleteNote/${this.props.noteId}/${this.props.currentUserId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok)
                    throw new Error('Network response was not ok');
                this.props.exitNote();
            })
            .catch(error =>
                console.log('There was a problem with the fetch operation:', error));
    }
    
    handleExit = () => {
        if (this.state.saved)
        {
            this.props.transferChanges(this.state.noteName, this.state.noteTags, this.state.noteText);
            this.props.changeDisplay(1, '');
        }
        else if (this.state.showNotSavedMessage) {
            alert(`Changes weren't saved!`);
            this.setState({
                showNotSavedMessage: false
            });
        }
        else
            this.props.changeDisplay(1, '');
    }
    
    handleTitleChange = (event) => {
        this.setState({
            noteName: event.target.value,
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }

    handleTextChange = (event) => {
        this.setState({
            noteText: event.target.value,
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }
    
    handleTagChanged = (event) => {
        this.setState({
            noteTags: event.target.value
        })
    }

    handleDeleteTag = (tag) => {
        const index = this.state.noteTags.indexOf(tag);
        if (index === -1)
            return;
        const newTags = this.props.noteTags;
        newTags.splice(index, 1)
        this.setState({
            noteTags: newTags,
            tag: '',
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }

    handleAddTag = () => {
        if (this.state.tag === '') return;

        const newTags = this.props.noteTags
        newTags.push(this.state.tag);
        this.setState({
            noteTags: newTags,
            tag: '',
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }

    render() {
        const {noteData} = this.props;

        return (
            <div className="note-card selected">
                <div className="note-title">
                    <input className="note-title-edit" type="text" value={noteData.name} onChange={(e) => this.handleTitleChange(e)} />
                </div>
                <div className="note-tags">
                    <span>Math</span>
                    <span>Formula</span>
                    <span>1 semester</span>
                </div>
                <div className="note-text">
                    <textarea className="note-text-edit" value={"testing"} onChange={(e) => this.handleTextChange(e)} />
                </div>
                <div className="note-misc">
                    <button className="button save-button" onClick={this.handleSave}>
                        <MdSave /> Save
                    </button>
                </div>
                <div className="note-buttons">
                    <button className="button button-hover delete-button delete-button-hover">
                        <MdDelete />
                    </button>
                    <button className="button button-hover edit-button edit-button-hover" onClick={() => this.props.changeDisplay(2, '')}>
                        <MdEditDocument />
                    </button>
                </div>
            </div>
        )
    }
}
