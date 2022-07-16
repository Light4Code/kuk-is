using Notes.Models;

namespace Notes.Providers;

/// <summary>
/// !Just an dummy for proof of concept!
/// The dummy will not be tested! (indirectly of course, because it's the only provider we have)
/// </summary>
public class DummyDatabaseProvider : IDatabaseProvider
{
    private IList<INote> notes = new List<INote>();

    public IEnumerable<INoteHeader> ReadAllHeaders()
    {
        foreach (var note in notes)
        {
            yield return note.Header;
        }
    }

    public INote ReadSingle(Guid id)
    {
        // Throws if no matching id found!
        return notes.First(x => x.Header.Id == id);
    }

    public (bool, INote) TryCreate(string subject, string text)
    {
        var note = new Note(subject, text);
        notes.Add(note);
        return (true, note);
    }

    public bool TryDelete(Guid id)
    {
        try
        {
            var note = ReadSingle(id);
            notes.Remove(note);
            return true;
        }
        catch (Exception ex)
        {
            // Should be replaced with some real log!
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool TryEdit(Guid id, string subject, string text)
    {
        try
        {
            var note = ReadSingle(id);
            note.Header.Subject = subject;
            note.Text = text;
            return true;

        }
        catch (Exception ex)
        {
            // Should be replaced with some real log!
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}