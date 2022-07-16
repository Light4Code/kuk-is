using Microsoft.AspNetCore.Mvc;
using Notes.Models;
using Notes.Services;

namespace Notes.Controllers;

/// <summary>
/// Controller to handle note headers.
/// </summary>
[ApiController]
[Route("[controller]")]
public class NoteHeadersController : ControllerBase
{
    private readonly INotesService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="NoteHeadersController" /> class.
    /// </summary>
    /// <param name="service">Notes service</param>
    public NoteHeadersController(INotesService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Retrieves all note headers.
    /// </summary>
    [HttpGet]
    public IEnumerable<INoteHeader> Retrieve()
    {
        return _service.ReadAllHeaders();
    }
}
