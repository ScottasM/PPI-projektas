﻿import React, {Component} from "react"
import {NoteViewer} from "./NoteViewer";
import {NoteEditor} from "./NoteEditor";
import "./NoteHub.css"
import {NoteDisplay} from "./NoteDisplay";

export class NoteHub extends Component {
    constructor(props) {
        super(props);
    }
    
    state = {
        mounted: false,
        display: this.props.display,
        noteName: '',
        noteTags: [],
        noteText: ''
    }
    
    componentDidMount() {
        if (!this.state.mounted) {
            this.fetchNote();
            this.setState({
                mounted: true
            });
        }
    }

    fetchNote = async () => {
        try {
            fetch(`http://localhost:5268/api/note/open/${this.props.noteId}`,)
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
                    <div className='noteHub'>
                        <p>{this.state.error}</p>
                    </div>
                )
            case 1:
                return (
                    <div className='noteHub'>
                        <NoteViewer
                            noteName={this.state.noteName}
                            noteTags={this.state.noteTags}
                            noteText={this.state.noteText}
                            exitNote={this.props.exitNote}
                            changeDisplay={this.changeDisplay}
                        />
                    </div>
                )
            case 2:
                return (
                    <div className='noteHub'>
                        <NoteEditor
                            noteName={this.state.noteName}
                            noteTags={this.state.noteTags}
                            noteText={this.state.noteText}
                            noteId={this.props.noteId}
                            transferChanges={this.transferChanges}
                            changeDisplay={this.changeDisplay}
                        />
                    </div>
                )

        }
    }
}