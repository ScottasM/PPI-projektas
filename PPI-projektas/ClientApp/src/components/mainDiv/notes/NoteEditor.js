import React, { Component }  from 'react';
import './NoteHub.css';
import {TagList} from "../../TagList";

export class NoteEditor extends Component {
    constructor (props) {
        super(props)
        this.state = {
            noteName: this.props.noteName,
            noteTags: this.props.noteTags,
            noteText: this.props.noteText,
            saved: true,
            showNotSavedMessage: true,
            showDeleteMessage: true,
            tag: ''
        }
    }

    handlePost = async () => {
        if (this.state.noteName === '')
            await this.setState({
                noteName: 'Untitled Note'
            });
        
        const noteData = {
            AuthorId: '0f8fad5b-d9cb-469f-a165-70867728950e', // temporary static user id
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
        } else fetch(`http://localhost:5268/api/note/deleteNote/${this.props.noteId}/0f8fad5b-d9cb-469f-a165-70867728950e`, {
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
    
    handleNameChange = (event) => {
        this.setState({
            noteName: event.target.value,
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }

    handleTextChanged = (event) => {
        this.setState({
            noteText: event.target.value,
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }
    
    handleTagChanged = (event) => {
        this.setState({
            tag: event.target.value
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
        return (
            <div className='note-editor'>
                <div className='editor prop-side'>
                    <label>Note Name:</label>
                    <br />
                    <input type='text' id='note-name' name='note-name' onChange={this.handleNameChange} value={this.state.noteName} />
                    <br />
                    <button className='submitButton' onClick={this.handlePost}> Save </button>
                    <button className='submitButton' onClick={this.handleExit}> Exit </button>
                    <button className='submitButton' onClick={this.handleDelete}> Delete </button>
                    <br />
                    <label>Tags:</label>
                    <br />
                    <input type='text' id='tag-name' name='tag-name' value={this.state.tag} onChange={this.handleTagChanged} />
                    <br />
                    <button className='submitButton' onClick={this.handleDeleteTag}> Delete tag </button>
                    <button className='submitButton' onClick={this.handleAddTag}> Add tag </button>
                    <br />
                    <TagList deleteTag={this.handleDeleteTag}  noteTags={this.state.noteTags} />
                </div>
                <div className='editor text-side'>
                    <textarea name='noteText' rows='9' cols='50' placeholder='Note text...'value={this.state.noteText} onChange={this.handleTextChanged} />
                </div>
            </div>
        )
    }
}
