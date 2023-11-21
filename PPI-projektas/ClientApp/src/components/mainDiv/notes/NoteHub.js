import React, {Component} from "react"
import {NoteViewer} from "./NoteViewer";
import {NoteEditor} from "./NoteEditor";
import "./NoteHub.css"
import {NoteDisplay} from "./NoteDisplay";

export class NoteHub extends Component {
    constructor(props) {
        super(props);
        this.state = {
            mounted: false,
            display: 1,
            error: '',
        }
    }
    
    componentDidMount() {
        if (!this.state.mounted) {
            // this.fetchNote();
            this.setState({
                mounted: true
            });
        }
    }

    fetchNote = async () => {
        try {
            fetch(`http://localhost:5268/api/note/openNote/${this.props.noteId}`,)
                .then(async response => {
                    if (!response.ok)
                        throw new Error('Network response was not ok');
                    return await response.json();
                })
                .then(note => {
                    this.setState({
                        noteName: note.name,
                        noteTags: note.tags,
                        noteText: note.text,
                    });
                });
        }
    catch (error) {
            this.setState({
               error: error 
            });
            console.error('There was a problem with the fetch operation:', error);
        }
    }
    
    transferChanges = (name, tags, text) => {
        this.setState({
            noteName: name,
            noteTags: tags,
            noteText: text
        })
    }   
    
    changeDisplay = (display, error) => {
        this.setState({
            display: display,
            error: error
        });
    }
    
    render() {
        switch (this.state.display) {
            case 0:
                return (
                    <div className='note-hub'>
                        <p>{this.state.error}</p>
                    </div>
                )
            case 1:
                return (
                    <div className='note-hub'>
                        <NoteViewer
                            noteData={this.props.noteData}
                            exitNote={this.props.exitNote}
                            changeDisplay={this.changeDisplay}
                            // deleteNote={}
                        />
                    </div>
                )
            case 2:
                return (
                    <div className='note-hub'>
                        <NoteEditor
                            noteData={this.props.noteData}
                            exitNote={this.props.exitNote}
                            currentGroupId={this.props.currentGroupId}
                            currentUserId={this.props.currentUserId}
                            changeDisplay={this.changeDisplay}
                        />
                    </div>
                )

        }
    }
}