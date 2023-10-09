import React, { Component }  from 'react'
import './NoteEditor.css'
import { TagList } from '../TagList.js';

export class NoteEditor extends Component {
    constructor (props) {
        super(props)
        this.state = {
            tag: ''
        }
    }
    
    handleTagChanged = (event) => {
        this.state.setState({
            tag: event.target.value
        })
    }

    handleDeleteTag = () => {
        const newTags = this.props.tags
        newTags.filter(tag => tag !== this.state.tag)
        this.props.changeTags(newTags)
    }

    handleAddTag = () => {
        const newTags = this.props.tags
        newTags.push(this.state.tag);
        this.props.changeTags(newTags)
    }

    render() {
        const { name } = this.props.name
        const { tags } = this.props.tags
        const { tag } = this.state.tag
        const { text } = this.props.text

        return <div className='note-editor'>
            <input type='text' width='50px' id='note-name' name='note-name' onChange={this.props.handleNameChange} value={name}>
                {name}
            </input>
            <br/>
            <button onClick={this.props.handlePost}>
                Save
            </button>
            <br/>
            <TagList tags={tags}/>
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
            <textarea name='noteText' rows='20' cols='30' onChange={this.props.handleTextChanged}>
                {text}
            </textarea>
        </div>
    }
}