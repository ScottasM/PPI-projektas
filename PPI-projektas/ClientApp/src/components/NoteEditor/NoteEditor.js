import React,{ Component }  from 'react'
import "..\\NoteEditor.css"

export class NoteEditor extends Component {
    constructor (props) {
        super(props);
    }

    handleTextChange = () => {
        //Note is saved to server
    }

    handleNameChange = () => {
        //Name is saved to server
    }

    handleTagsChanged = () => {
        //Tags are saved to server
    }

    render() {
        <div className="note-editor">
            <input type="text" onChange={handleNameChange}>{props.name}</input>
            <button onClick={handleTextChange}>Save</button>
            <textarea name="noteText" rows="20" cols="30">{props.text}</textarea>
        </div>
    }
}