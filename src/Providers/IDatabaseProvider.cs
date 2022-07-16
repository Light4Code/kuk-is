using Notes.Models;

namespace Notes.Providers;

public interface IDatabaseProvider
{
    (bool, INote) TryCreate(string subject, string text);
    INote ReadSingle(Guid id);
    IEnumerable<INoteHeader> ReadAllHeaders();
    bool TryEdit(Guid id, string subject, string text);
    bool TryDelete(Guid id);
}