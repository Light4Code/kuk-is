namespace Notes.Models;

public class NoteHeader : INoteHeader
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Subject { get; set; } = string.Empty;
}