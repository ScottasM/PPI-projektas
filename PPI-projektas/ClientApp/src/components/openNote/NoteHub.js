import react, {Component, useEffect} from "react"
import {NoteViewer} from "./NoteViewer";
import {NoteEditor} from "./NoteEditor";

export class NoteHub extends Component {
    constructor(props) {
        super(props)
        this.setState({
            toggleEditor: false
            })
    }
    
    componentDidMount() {
        this.fetchNote()
    }

    fetchNote = async () => {
        try {
            const response = fetch('http://localhost:5268/api/notes/${this.props.noteId}')
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json()
        this.setState({
            name: data.name,
            tags: data.tags,
            text: data.text
        })
        console.log('Got')
        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
        }
    }

    handleNameChange = (event) => {
        this.setState({
            name: event.target.value
        })
    }
    
    changeTags = (tag) => {
        this.setState({
            tagChange: tag
        })
    }

    handleTextChanged = (event) => {
        this.setState({
            text: event.target.value
        })
    }
    
    render() {
        return (
            <div>
                {!this.state.toggleEditor && <NoteViewer
                    name={this.state.name}
                    tags={this.state.tags}
                    text={this.state.name}
                    toggleEditor={this.state.toggleEditor}
                />}
                {this.state.toggleEditor && <NoteEditor
                    name={this.state.name}
                    tags={this.state.tags}
                    text={this.state.name}
                    handleNameChange={this.handleNameChange}
                    changeTagsChange={this.changeTags}
                    handleTextChange={this.handleTextChanged}
                />}
            </div>
        )
    }
}