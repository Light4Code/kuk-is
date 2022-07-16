namespace Notes.Models;

public interface INoteHeader
{
    Guid Id { get; set; }
    string Subject { get; set; }
}