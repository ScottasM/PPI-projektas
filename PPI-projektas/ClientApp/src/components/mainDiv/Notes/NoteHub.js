import React, {Component} from "react"
import {NoteViewer} from "./NoteViewer";
import {NoteEditor} from "./NoteEditor";
import "./NoteHub.css"

export class NoteHub extends Component {
    constructor(props) {
        super(props);
        this.state = {
            noteName: '',
            noteTags: [],
            noteText: '',
            mounted: false,
            showEditor: false
        };
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
            await fetch(`http://localhost:5268/api/note/open/${this.props.noteId}`,)
                .then(async response => {
                    if (!response.ok)
                        throw new Error('Network response was not ok');
                    return await response.json();
                })
                .then(note => {
                    this.setState({
                        noteName: note.name,
                        noteTags: note.tags,
                        noteText: note.text
                    });
                });
        }
    catch (error) {
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
    
    toggleEditor = () => {
        this.setState(prevState => ({
            showEditor: !prevState.showEditor
        }));
    }
    
    render() {
        
        return (
            <div className='noteHub'>
                {this.state.showEditor ?
                    <NoteEditor
                        noteName={this.state.noteName}
                        noteTags={this.state.noteTags}
                        noteText={this.state.noteText}
                        noteId={this.props.noteId}
                        transferChanges={this.transferChanges}
                        closeEditor={this.toggleEditor}
                    /> 
                    :
                        <NoteViewer
                            noteName={this.state.noteName}
                            noteTags={this.state.noteTags}
                            noteText={this.state.noteText}
                            exitNote={this.props.exitNote}
                            openEditor={this.toggleEditor}
                        />
                }
            </div>
        )
    }
}