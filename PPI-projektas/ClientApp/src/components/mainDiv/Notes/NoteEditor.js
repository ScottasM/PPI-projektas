import React, { Component }  from 'react';
import './NoteHub.css';
import {TagList} from "../../TagList";

export class NoteEditor extends Component {
    constructor (props) {
        super(props)
        this.state = {
            name: this.props.name,
            tags: this.props.tags,
            text: this.props.text,
            saved: true,
            showNotSavedMessage: false,
            tag: ''
        }
    }

    handlePost = async () => {
        const noteData = {
            Name: this.state.name,
            AuthorGuid: '0f8fad5b-d9cb-469f-a165-70867728950e', // temporary static user id
            Tags: this.state.tags,
            Text: this.state.text,
            Id: this.props.noteId
        };

        await fetch('http://localhost:5268/api/note/updatenote', { // temporary localhost api url
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
            .catch((error) => 
                console.error('There was a problem with the fetch operation:', error));
    }
    
    handleExit = () => {
        if (this.state.saved)
            this.props.transferChanges(this.state.name, this.state.tags, this.state.text);
        else if (this.state.showNotSavedMessage)
            this.props.closeEditor();
        else
            this.setState({
                showNotSavedMessage: true
            });
    }
    
    handleNameChange = (event) => {
        this.setState({
            saved: false,
            name: event.target.value
        })
    }

    handleTextChanged = (event) => {
        this.setState({
            saved: false,
            text: event.target.value
        })
    }
    
    handleTagChanged = (event) => {
        this.state.setState({
            saved: false,
            tag: event.target.value
        })
    }

    handleDeleteTag = () => {
        const newTags = this.props.tags
        newTags.filter(tag => tag !== this.state.tag)
        this.setState({
            tags: newTags, 
            saved: false,
            tag: ''
        })
    }

    handleAddTag = () => {
        const newTags = this.props.tags
        newTags.push(this.state.tag);
        this.setState({
            tags: newTags,
            saved: false,
            tag: ''
        })
    }

    render() {
        const { name } = this.state.name;
        const { tag } = this.state.tag;
        const { text } = this.state.text;

        return <div className='note-editor'>
            <input type='text' width='50px' id='note-name' name='note-name' onChange={this.handleNameChange} value={name}>
                {name}
            </input>
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
            <TagList tags={this.state.tags}/>
            <input type='text' width='50px' id='tag-name' name='tag-name' value={tag} onChange={this.handleTagChanged}>
                {tag}
            </input>
            <br/>
            <button onClick={this.handleDeleteTag}>
                Delete tag
            </button>
            <button onClick={this.handleAddTag}>
                Add tag
            </button>
            <br/>
            <textarea name='noteText' rows='20' cols='30' onChange={this.handleTextChanged}>
                {text}
            </textarea>
        </div>
    }
}
