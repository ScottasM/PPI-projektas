import React, {Component} from "react"
import {NoteViewer} from "./NoteViewer";
import {NoteEditor} from "./NoteEditor";
import "./NoteHub.css"

export class NoteHub extends Component {
    constructor(props) {
        super(props);
        this.state = {
            mounted: false,
            display: this.props.display,
            error: '',
            showDeleteMessage: true,
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

    handleDelete = async () => {
        if(this.props.noteData === undefined)
        {
            this.props.handleClose();
            return;
        }
        
        if (this.state.showDeleteMessage) {
            alert(`You're about to delete this note.`)
            this.setState({
                showDeleteMessage: false
            });
        } else fetch(`http://localhost:5268/api/note/deleteNote/${this.props.noteData.id}/${this.props.currentUserId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok)
                    throw new Error('Network response was not ok');
                this.props.handleClose();
            })
            .catch(error =>
                console.log('There was a problem with the fetch operation:', error));
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
                            changeDisplay={this.changeDisplay}
                            deleteNote={this.handleDelete}
                        />
                    </div>
                )
            case 2:
                return (
                    <div className='note-hub'>
                        <NoteEditor
                            noteData={this.props.noteData}
                            currentGroupId={this.props.currentGroupId}
                            currentUserId={this.props.currentUserId}
                            changeDisplay={(display, error) => {this.changeDisplay(display, error); this.props.fetchNotes()}}
                            deleteNote={this.handleDelete}
                        />
                    </div>
                )

        }
    }
}