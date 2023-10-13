import React, {Component} from "react"
import {NoteViewer} from "./NoteViewer";
import {NoteEditor} from "./NoteEditor";
import "./NoteHub.css"

export class NoteHub extends Component {
    constructor(props) {
        super(props);
    }
    
    state = {
        name: '',
        tags: [],
        text: '',
        mounted: false,
        showEditor: false
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
            await fetch(`http://localhost:5268/api/note/open?id=${this.props.noteId}`,)
                .then(async response => {
                    if (!response.ok)
                        throw new Error('Network response was not ok');
                    return await response.json();
                })
                .then(note =>
                    this.setState({
                        name: note.name,
                        tags: note.tags,
                        text: note.text
                    }));
        }
    catch (error) {
            console.error('There was a problem with the fetch operation:', error);
        }
    }
    
    transferChanges = (name, tags, text) => {
        this.setState({
            name: name,
            tags: tags,
            text: text
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
                {this.state.showEditor ? <NoteEditor
                    name={this.state.name}
                    tags={this.state.tags}
                    text={this.state.text}
                    noteId={this.props.noteId}
                    transferChanges={this.transferChanges}
                    closeEditor={this.toggleEditor}
                /> : <NoteViewer
                    name={this.state.name}
                    tags={this.state.tags}
                    text={this.state.text}
                    exitNote={this.props.exitNote}
                    openEditor={this.state.toggleEditor}
                />}
            </div>
        )
    }
}