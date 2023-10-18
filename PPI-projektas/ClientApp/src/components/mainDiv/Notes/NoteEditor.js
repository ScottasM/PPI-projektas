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
            showNotSavedMessage: false,
            tag: ''
        }
    }

    handlePost = async () => {
        const noteData = {
            AuthorGuid: '0f8fad5b-d9cb-469f-a165-70867728950e', // temporary static user id
            Name: this.state.noteName,
            Tags: this.state.noteTags,
            Text: this.state.noteText
        };

        await fetch(`http://localhost:5268/api/note/updatenote/${this.props.noteId}`, { // temporary localhost api url
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(noteData)
        })
            .then((response) => {
                if (!response.ok) {
                    this.setState({
                        showNotSavedMessage: true
                    });
                    throw new Error('Network response was not ok');
                }
                else
                    this.setState({
                        saved: true,
                        showNotSavedMessage: false
                    });
            })
            .catch((error) => {
                this.props.changeDisplay(0, error);
                console.error('There was a problem with the fetch operation:', error);
            });
    }
    
    handleExit = () => {
        if (this.state.saved)
        {
            this.props.transferChanges(this.state.noteName, this.state.noteTags, this.state.noteText);
            this.props.changeDisplay(1, '');
        }
        else if (this.state.showNotSavedMessage)
            this.props.changeDisplay(1, '');
        else
            this.setState({
                showNotSavedMessage: true
            });
    }
    
    handleNameChange = (event) => {
        this.setState({
            saved: false,
            noteName: event.target.value
        })
    }

    handleTextChanged = (event) => {
        this.setState({
            saved: false,
            noteText: event.target.value
        })
    }
    
    handleTagChanged = (event) => {
        this.setState({
            tag: event.target.value
        })
    }

    handleDeleteTag = () => {
        const index = this.state.noteTags.indexOf(this.state.tag);
        if (index === -1)
            return;
        const newTags = this.props.noteTags;
        newTags.splice(index, 1)
        this.setState({
            noteTags: newTags, 
            saved: false,
            tag: ''
        })
    }

    handleAddTag = () => {
        const newTags = this.props.noteTags
        newTags.push(this.state.tag);
        this.setState({
            noteTags: newTags,
            saved: false,
            tag: ''
        })
    }

    render() {
        return <div className='note-editor'>
            <input type='text' width='50px' id='note-name' name='note-name' onChange={this.handleNameChange} value={this.state.noteName}/>
            <br/>
            <button onClick={this.handlePost}>
                Save
            </button>
            <button onClick={this.handleExit}>
                Exit
            </button>
            {this.state.showNotSavedMessage && <h2 color="red">
                Changes weren't saved!
            </h2>}
            <br/>
            <TagList noteTags={this.state.noteTags}/>
            <input type='text' width='50px' id='tag-name' name='tag-name' value={this.state.tag} onChange={this.handleTagChanged}/>
            <br/>
            <button onClick={this.handleDeleteTag}>
                Delete tag
            </button>
            <button onClick={this.handleAddTag}>
                Add tag
            </button>
            <br/>
            <textarea name='noteText' rows='20' cols='30' value={this.state.noteText} onChange={this.handleTextChanged}/>
        </div>
    }
}
