import React, { Component }  from 'react';
import './NoteHub.css';
import {TagList} from "../../TagList";
import {MdDelete, MdEditDocument, MdSave} from "react-icons/md";

export class NoteEditor extends Component {
    constructor (props) {
        super(props)
        this.state = {
            id: this.props.noteData === undefined ? 0 : this.props.noteData.id,
            name: this.props.noteData === undefined ? '' : this.props.noteData.name,
            text: this.props.noteData === undefined ? '' : this.props.noteData.text,
            tags: this.props.noteData === undefined ? 0 : this.props.noteData.tags,
            saved: true,
            showNotSavedMessage: false,
        }
    }

    handleCreateNote = async () => {
        try {
            const response = await fetch(`http://localhost:5268/api/note/createNote/${this.props.currentGroupId}/${this.props.currentUserId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const noteId = await response.json();
            
            this.setState({
                id: noteId,
            }, () => {
                this.handleSave();
            });

        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
        }
    }

    handleSave = async () => {
        if(this.state.id === 0)
        {
            await this.handleCreateNote();
            return;
        }
        
        if (this.state.noteName === '')
            alert('Note name cannot be empty!');
        
        if (this.state.noteText === '')
            alert('Note text cannot be empty!');
        
        const noteData = {
            Id: this.props.currentUserId,
            Name: this.state.name,
            Tags: this.state.tags === 0 ? [] : this.state.tags,
            Text: this.state.text
        };

        try {
            const response = await fetch(`http://localhost:5268/api/note/updateNote/${this.state.id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(noteData),
            });

            if (!response.ok) {
                alert(`Changes weren't saved!`);
                this.setState({
                    showNotSavedMessage: false,
                });
                throw new Error('Network response was not ok');
            }

            this.setState({
                saved: true,
                showNotSavedMessage: false,
            }, () => {
                this.handleExit();
            });
        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
        }
    }
    
    handleExit = () => {
        if (this.state.saved)
        {
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
            name: event.target.value,
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }

    handleTextChange = (event) => {
        this.setState({
            text: event.target.value,
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }
    
    handleTagChanged = (event) => { //TODO: Add tag selection
        this.setState({
            tags: event.target.value
        })
    }

    handleDeleteTag = (tag) => {
        const index = this.state.noteTags.indexOf(tag);
        if (index === -1)
            return;
        const newTags = this.props.tags;
        newTags.splice(index, 1)
        this.setState({
            tags: newTags,
            tag: '',
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }

    handleAddTag = () => {
        if (this.state.tag === '') return;

        const newTags = this.props.tags
        newTags.push(this.state.tag);
        this.setState({
            tags: newTags,
            tag: '',
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }

    render() {
        const {name, text, tags} = this.state;

        return (
            <div className="note-card selected">
                <div className="note-title">
                    <input className="note-title-edit" type="text" value={name} onChange={(e) => this.handleTitleChange(e)} />
                </div>
                <div className="note-tags">
                    {tags !== 0 && tags.map(tag => (
                            <span>{tag}</span>
                        )
                    )}
                </div>
                <div className="note-text">
                    <textarea className="note-text-edit" value={text} onChange={(e) => this.handleTextChange(e)} />
                </div>
                <div className="note-misc">
                    <button className="button save-button" onClick={this.handleSave}>
                        <MdSave /> Save
                    </button>
                </div>
                <div className="note-buttons">
                    <button className="button button-hover delete-button delete-button-hover" onClick={this.props.deleteNote}>
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
