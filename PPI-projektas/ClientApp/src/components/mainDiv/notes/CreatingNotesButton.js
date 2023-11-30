import React, {Component} from "react";
import '../buttons.css'

export class CreatingNotesButton extends Component {
    constructor(props) {
        super(props);
    }
    
    render() {
        return (
        <div className='createNoteDiv'>
            <button className='create-button' onClick={this.props.handleCreateNote}>
                Create New Note
            </button>
        </div>
        )
    }
}