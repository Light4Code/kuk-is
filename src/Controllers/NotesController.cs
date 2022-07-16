using Microsoft.AspNetCore.Mvc;
using Notes.Models;
using Notes.Services;

namespace Notes.Controllers;

/// <summary>
/// Controller to handle notes.
/// </summary>
[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly INotesService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotesController" /> class.
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="service">Notes service</param>
    public NotesController(ILogger<NotesController> logger, INotesService service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Retrieves a single note.
    /// </summary>
    /// <param name="id">The target note id.</param>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<INote> Retrieve(Guid id)
    {
        try
        {
            return Ok(_service.ReadSingle(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }
    }

    /// <summary>
    /// Creates a new note.
    /// </summary>
    /// <param name="subject">Subject/Title for the new note.</param>
    /// <param name="text">Text for the new note.</param>
    [HttpPost("{subject}/{text}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public ActionResult<INote> Create(string subject, string text)
    {
        var result = _service.TryCreate(subject, text);
        if (result.Item1)
        {
            return Ok(result.Item2);
        }

        return new StatusCodeResult(StatusCodes.Status406NotAcceptable);
    }

    /// <summary>
    /// Edit/modifies a note.
    /// </summary>
    /// <param name="id">The target note id.</param>
    /// <param name="subject">New subject/title.</param>
    /// <param name="text">New text.</param>
    [HttpPut("{id}/{subject}/{text}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status304NotModified)]
    public IActionResult Edit(Guid id, string subject, string text)
    {
        if (_service.TryEdit(id, subject, text))
        {
            return Ok();
        }

        return new StatusCodeResult(StatusCodes.Status304NotModified);
    }

    /// <summary>
    /// Deletes a note.
    /// </summary>
    /// <param name="id">The target note id.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status304NotModified)]
    public IActionResult Delete(Guid id)
    {
        if (_service.TryDelete(id))
        {
            return Ok();
        }

        return new StatusCodeResult(StatusCodes.Status304NotModified);
    }
}