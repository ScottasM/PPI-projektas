﻿using System;
using System.Collections.Generic;
using System.IO;

public class Note
{
	//all pragma warnings are to be removed when User class is implemented
	#pragma warning disable CS0246
	const User author = null;
	#pragma warning restore CS0246

	public List<string> tags { get; set; }
	string location;

	#pragma warning disable CS0246, CS0176
	public Note(User author)
	{
		this.author = author;
		tags = new List<string>();
		location = "";	//Temporary, will be updated when database becomes available
		//File will be created here
	}
	#pragma warning restore CS0246, CS0176

	public void updateNote(string text) => File.WriteAllText(location, text);
}