using System;
using System.Collections.Generic;
using System.IO;

using PPI_projektas.objects.abstractions;

public class Note : Entity
{
	//all pragma warnings are to be removed when User class is implemented
	#pragma warning disable CS0246
	public readonly User author;
	#pragma warning restore CS0246

	public List<string> tags { get; set; }
	string location;

	#pragma warning disable CS0246, CS0176
	public Note(User author) : base()
	{
		this.author = author;
		tags = new List<string>();
		location = "";	//Temporary, will be updated when database becomes available
		//File will be created here
	}
	#pragma warning restore CS0246, CS0176

	//public void updateNote(string text) => File.WriteAllText(location, text);
}
