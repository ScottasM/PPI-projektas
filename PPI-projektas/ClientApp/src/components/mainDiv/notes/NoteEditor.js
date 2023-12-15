import React, { Component }  from 'react';
import './NoteHub.css';
import {TagList} from "../../TagList";
import {MdDelete, MdEditDocument, MdSave} from "react-icons/md";
import {Tag} from "reactstrap";

export class NoteEditor extends Component {
    constructor (props) {
        super(props)
        this.state = {
            id: this.props.noteData === undefined ? 0 : this.props.noteData.id,
            name: this.props.noteData === undefined ? '' : this.props.noteData.name,
            text: this.props.noteData === undefined ? '' : this.props.noteData.text,
            tags: this.props.noteData === undefined ? [] : (this.props.noteData.tags == null ? [] : this.props.noteData.tags),
            saved: true,
            showNotSavedMessage: false,
            showTagSearch: false,
            tagSearch: '',
            tagResults: [],
        }
    }

    handleCreateNote = async () => {
        try {
            const response = await fetch(`http://localhost:5268/api/note/createNote/${this.props.currentGroupId}/${this.props.currentUserId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const noteId = await response.json();
            
            this.setState({
                id: noteId,
            }, () => {
                this.handleSave();
            });

        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
        }
    }

    handleSave = async () => {
        if(this.state.id === 0)
        {
            await this.handleCreateNote();
            return;
        }
        
        if (this.state.noteName === '')
            alert('Note name cannot be empty!');
        
        if (this.state.noteText === '')
            alert('Note text cannot be empty!');
        
        const noteData = {
            Id: this.props.currentUserId,
            Name: this.state.name,
            Tags: this.state.tags == null ? [] : this.state.tags,
            Text: this.state.text
        };

        try {
            const response = await fetch(`http://localhost:5268/api/note/updateNote/${this.state.id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(noteData),
            });

            if (!response.ok) {
                alert(`Changes weren't saved!`);
                this.setState({
                    showNotSavedMessage: false,
                });
                throw new Error('Network response was not ok');
            }

            this.setState({
                saved: true,
                showNotSavedMessage: false,
            }, () => {
                this.handleExit();
            });
        } catch (error) {
            console.error('There was a problem with the fetch operation:', error);
        }
    }
    
    handleExit = () => {
        if (this.state.saved)
        {
            this.props.changeDisplay(1, '');
        }
        else if (this.state.showNotSavedMessage) {
            alert(`Changes weren't saved!`);
            this.setState({
                showNotSavedMessage: false
            });
        }
        else
            this.props.changeDisplay(1, '');
    }
    
    handleTitleChange = (event) => {
        this.setState({
            name: event.target.value,
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }

    handleTextChange = (event) => {
        this.setState({
            text: event.target.value,
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        })
    }

    handleDeleteTag = (tag) => {
        const index = this.state.tags.indexOf(tag);
        if (index === -1)
            return;
        this.setState((prevState) => ({
            tags: prevState.tags.splice(index, 1),
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        }));
    }

    handleAddTag = (tag) => {
        this.setState((prevState) => ({
            tags: [...prevState.tags, tag],
            saved: false,
            showNotSavedMessage: true,
            showDeleteMessage: true
        }));
    }
    
    toggleTagSearch = () => {
        this.setState((prevState) => ({
            showTagSearch: !prevState.showTagSearch
        }))
    }

    handleTagSearch = (event) => {
        this.setState({tagSearch: event.target.value }, () => {
            if(this.state.tagSearch){
                this.handleTagGet();
            }
        });
    }

    handleTagGet = async () => {
        try {
            const response = await fetch(`http://localhost:5268/api/note/searchTags/${this.props.currentGroupId}/${this.state.tagSearch}`);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            
            let tags = await response.json();
            tags = tags.filter(el =>
                !this.state.tags.some(tag => tag === el)
            );
            if(tags === null) tags = [];
            this.setState({ tagResults: tags});
            
        } catch (error) {
            console.error('There was a problem with the get operation:', error);
        }
    }

    render() {
        const {name, text, tags, showTagSearch, tagSearch, tagResults} = this.state;
        const maxVisibleTags = 3;

        return (
            <div className="note-card selected">
                <div className="note-title">
                    <input 
                        className="note-title-edit" 
                        type="text" 
                        value={name}
                        placeholder="Enter title..."
                        onChange={(e) => this.handleTitleChange(e)} 
                    />
                </div>
                <div className="note-tags">
                    {tags != null && tags.slice(0, maxVisibleTags).map(tag => (
                        <span className="cursor-pointer" key={tag} onClick={() => this.handleDeleteTag(tag)}>{tag}</span>
                        )
                    )}
                    {tags != null && tags.length > maxVisibleTags && (
                        <span key="ellipsis">...</span>
                    )}
                    <span className="cursor-pointer" onClick={this.toggleTagSearch}>+</span>
                    { showTagSearch &&
                        <div className="tag-select">
                            <div className="tag-search">
                                <input 
                                    type="text" 
                                    value={tagSearch}
                                    placeholder="Search tags..."
                                    onChange={(e) => this.handleTagSearch(e)}/>
                            </div>
                            <div className="tags">
                                {tagResults.map(tag => (
                                        <span onClick={() => this.handleAddTag(tag)}>{tag}</span>
                                    )
                                )}
                                {tagResults.length <= 3 && tagSearch !== '' && !tagResults.includes(tagSearch) &&
                                    <span onClick={() => this.handleAddTag(tagSearch)}>{tagSearch}</span>
                                }
                            </div>
                        </div>
                    }
                </div>
                <div className="note-text">
                    <textarea 
                        className="note-text-edit" 
                        value={text}
                        placeholder="Enter text..."
                        onChange={(e) => this.handleTextChange(e)} />
                </div>
                <div className="note-misc">
                    <button className="button save-button" onClick={this.handleSave}>
                        <MdSave /> Save
                    </button>
                </div>
                <div className="note-buttons">
                    <button className="button button-hover delete-button delete-button-hover" onClick={this.props.deleteNote}>
                        <MdDelete />
                    </button>
                    <button className="button button-hover edit-button edit-button-hover" onClick={() => this.props.changeDisplay(2, '')}>
                        <MdEditDocument />
                    </button>
                </div>
            </div>
        )
    }
}
