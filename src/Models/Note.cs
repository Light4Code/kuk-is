namespace Notes.Models;

public class Note : INote
{
    public INoteHeader Header { get; set; } = new NoteHeader();
    
    public string Text { get; set; } = string.Empty;

    public Note() { }

    public Note(string subject, string text)
    {
        Header = new NoteHeader { Subject = subject };
        Text = text;
    }
}