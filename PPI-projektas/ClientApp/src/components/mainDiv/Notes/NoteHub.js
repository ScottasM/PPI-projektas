import react, {Component} from "react"
import {NoteViewer} from "./NoteViewer";
import {NoteEditor} from "./NoteEditor";

export class NoteHub extends Component {
    constructor(props) {
        super(props);
        this.setState({
            mounted: false,
            toggleEditor: false
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
    
    handlePost = async () => {
        const noteData = {
            Name: this.state.name,
            AuthorGuid: '0f8fad5b-d9cb-469f-a165-70867728950e', // temporary static user id
            Tags: this.state.tags,
            Text: this.state.text,
        };

        await fetch('http://localhost:5268/api/note/updatenote', { // temporary localhost api url
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(noteData)
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
            })
            .catch((error) => {
                console.error('There was a problem with the fetch operation:', error);
            });
    }

    handleNameChange = (event) => {
        this.setState({
            name: event.target.value
        })
    }

    handleTextChanged = (event) => {
        this.setState({
            text: event.target.value
        })
    }

    changeTags = (tag) => {
        this.setState({
            tagChange: tag
        })
    }
    
    render() {
        return (
            <div>
                {!this.state.toggleEditor && <NoteViewer
                    name={this.state.name}
                    tags={this.state.tags}
                    text={this.state.text}
                    toggleEditor={this.state.toggleEditor}
                />}
                {this.state.toggleEditor && <NoteEditor
                    name={this.state.name}
                    tags={this.state.tags}
                    text={this.state.text}
                    handleNameChange={this.handleNameChange}
                    handleTextChange={this.handleTextChanged}
                    changeTags={this.changeTags}
                    handlePost={this.handlePost}
                />}
            </div>
        )
    }
}