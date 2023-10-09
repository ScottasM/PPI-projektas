import React, {Component} from "react"
import {NoteViewer} from "./NoteViewer";
import {NoteEditor} from "./NoteEditor";

export class NoteHub extends Component {
    constructor(props) {
        super(props);
        this.setState({
            mounted: false,
            showEditor: false
            });
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
            const response = fetch('http://localhost:5268/api/notes?id=${this.props.noteId}');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json()
        this.setState({
            name: data.name,
            tags: data.tags,
            text: data.text
        });
        } catch (error) {
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
        this.setState(prevState => {
            showEditor: !prevState.showEditor
        });
    }
    
    render() {
        return (
            <div>
                {!this.state.showEditor && <NoteViewer
                    name={this.state.name}
                    tags={this.state.tags}
                    text={this.state.text}
                    toggleNote={this.props.toggleNote}
                    toggleEditor={this.state.toggleEditor}
                />}
                {this.state.showEditor && <NoteEditor
                    name={this.state.name}
                    tags={this.state.tags}
                    text={this.state.text}
                    transferChanges={this.transferChanges}
                    toggleEditor={this.toggleEditor}
                />}
            </div>
        )
    }
}