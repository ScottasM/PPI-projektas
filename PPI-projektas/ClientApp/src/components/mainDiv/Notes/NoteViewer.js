import react, {Component} from 'react'
import '../TagList.js'
import {TagList} from "../TagList";

export class NoteViewer extends Component {
    constructor (props) {
        super(props)
    }
    
    render() {
        return <div>
            <h2>{this.props.name}</h2>
            <input type="button" onClick={this.props.toggleEditor}>Edit</input>
            <br/>
            <TagList tags={this.props.tags}/>
            <br/>
            <p>{this.props.text}</p>
        </div>
    }
}