import React,{ Component }  from 'react'
import "../TagList.js"
import "../NoteEditor.css"
import { TagList } from '../TagList.js';

export class NoteEditor extends Component {
    constructor (props) {
        super(props);
    }

    handleSave = () => {
        //Note is saved to server
    }

    handleNameChange = () => {
        //Name is saved to server
    }

    handleTagsChanged = () => {
        //Tags are saved to server
    }

    handleDeleteTag = () => {
        if (this.props.tags.indexOf());
    }

    render() {
        return <div className="note-editor">
            <input type="text" onChange={handleNameChange}>{this.props.name}</input>
            <button onClick={handleSave}>Save</button>
            <TagList tags={this.props.tags}/>
            <input type="text" width="50px" id="deleteTag"></input>
            <button onClick={}>Delete tag</button>
            <textarea name="noteText" rows="20" cols="30">{props.text}</textarea>
        </div>
    }
}