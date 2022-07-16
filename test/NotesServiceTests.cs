using Notes.Models;
using Notes.Providers;
using Notes.Services;

namespace test;

/// <summary>
/// !Just a few tests for proof of concept!
/// There should be more, like null argument checks.
/// </summary>
public class NotesServiceTests
{
    private string _expected_subject = "Test_Subject";
    private string _expected_Text = "Test_Text";

    [Fact]
    public void TryCreate_Success_Test()
    {
        // Arrange/Act
        var result = CreateSingleEntry(new NotesService(new DummyDatabaseProvider()), _expected_subject, _expected_Text);

        // Assert
        Assert.True(result.Item1);
        Assert.NotEqual(Guid.Empty, result.Item2.Header.Id);
        Assert.Equal(_expected_subject, result.Item2.Header.Subject);
        Assert.Equal(_expected_Text, result.Item2.Text);
    }

    [Fact]
    public void ReadSingle_Success_Test()
    {
        // Arrange
        var service = new NotesService(new DummyDatabaseProvider());
        var tmpResult = CreateSingleEntry(service, _expected_subject, _expected_Text);

        // Act
        var result = service.ReadSingle(tmpResult.Item2.Header.Id);

        // Assert
        Assert.Equal(tmpResult.Item2.Header.Id, result?.Header.Id);
        Assert.Equal(_expected_subject, result?.Header.Subject);
        Assert.Equal(_expected_Text, result?.Text);
    }

    [Fact]
    public void TryDelete_Success_Test()
    {
        // Arrange
        var service = new NotesService(new DummyDatabaseProvider());
        var tmpResult = CreateSingleEntry(service, _expected_subject, _expected_Text);

        // Act
        var result = service.TryDelete(tmpResult.Item2.Header.Id);

        // Assert
        var allHeaders = service.ReadAllHeaders();
        Assert.True(result);
        Assert.False(allHeaders.Any());
    }

    [Fact]
    public void ReadAllHeaders_Success_Test()
    {
        // Arrange
        var service = new NotesService(new DummyDatabaseProvider());
        var tmpResult = CreateSingleEntry(service, _expected_subject, _expected_Text);
        var tmpResult2 = CreateSingleEntry(service, "Subject2", "Text2");

        // Act
        var result = service.ReadAllHeaders();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(1, result.Count(x => x.Id == tmpResult.Item2.Header.Id));
        Assert.Equal(1, result.Count(x => x.Id == tmpResult2.Item2.Header.Id));
    }

    [Fact]
    public void TryEdit_Success_Test()
    {
        // Arrange
        var subject = "New subject";
        var text = "New text";
        var service = new NotesService(new DummyDatabaseProvider());
        var tmpResult = CreateSingleEntry(service, _expected_subject, _expected_Text);

        // Act
        var result = service.TryEdit(tmpResult.Item2.Header.Id, subject, text);

        // Assert
        var note = service.ReadSingle(tmpResult.Item2.Header.Id);
        Assert.True(result);
        Assert.Equal(subject, note.Header.Subject);
        Assert.Equal(text, note.Text);
    }

    private (bool, INote) CreateSingleEntry(INotesService service, string subject, string text)
    {
        return service.TryCreate(subject, text);
    }
}