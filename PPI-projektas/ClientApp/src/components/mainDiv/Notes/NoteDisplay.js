import React, {Component} from 'react'

class NoteDisplay extends Component {
    constructor(props) {
        super(props)
    }
    
    handleOpenNote = () => {
        this.props.openNote(this.props.id);
    }
    
    render() {
        return (
            <li>
                <h2 onClick={this.handleOpenNote}>{this.state.name}</h2>
            </li>
        )
    }
}

export class NoteList extends Component {
    constructor(props) {
        super(props)
    }
    
    render() {
        return (
            <div>
                <ul>
                    {this.props.notes.map(note => (
                        <NoteDisplay name={note.name} id={note.id} openNote={this.props.openNote}/>
                    ))}
                </ul>
            </div>
        )
    }

}