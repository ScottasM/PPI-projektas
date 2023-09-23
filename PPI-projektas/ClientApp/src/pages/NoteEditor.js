import React from 'react'
import "..\\NoteEditor.css"

export const NoteEditor = (props) => {
    const saveNote = () => {
        //Note is saved to server
    }

    return (
        <div className="note-editor">
            <h1>{props.name}</h1>
            <button onClick={saveNote}>Save</button>
            <textarea name="noteText" rows="20" cols="30">{props.text}</textarea>
        </div>
    )
}