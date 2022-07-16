namespace Notes.Models;

public interface INote
{
    INoteHeader Header { get; set; }
    string Text { get; set; }
}