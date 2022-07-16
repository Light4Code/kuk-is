using Notes.Models;
using Notes.Providers;

namespace Notes.Services;

public class NotesService : INotesService
{
    private IDatabaseProvider _databaseProvider;

    public NotesService(IDatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
    }

    public IEnumerable<INoteHeader> ReadAllHeaders() => _databaseProvider.ReadAllHeaders();

    public INote ReadSingle(Guid id) => _databaseProvider.ReadSingle(id);

    public (bool, INote) TryCreate(string subject, string text)
    {
        if (string.IsNullOrWhiteSpace(subject))
        {
            Console.WriteLine("Subject can't be empty!");
            return (false, new Note());
        }

        if(string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Text can't be empty!");
            return (false, new Note());
        }

        return _databaseProvider.TryCreate(subject, text);
    }

    public bool TryDelete(Guid id) => _databaseProvider.TryDelete(id);

    public bool TryEdit(Guid id, string subject, string text)
    {
        if (string.IsNullOrWhiteSpace(subject))
        {
            Console.WriteLine("Subject can't be empty!");
            return false;
        }

        if(string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Text can't be empty!");
            return false;
        }

        return _databaseProvider.TryEdit(id, subject, text);
    }
}