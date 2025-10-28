using KanbanAPI.Dtos;
using KanbanAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanAPI.Controllers;

[ApiController]
[Route("api/board")]

public class BoardController : ControllerBase
{
    private readonly KanbanDbContext _db;
    public BoardController(KanbanDbContext db) => _db = db;

    // GET: api/board/board -> returns the single board with columns and tasks

    [HttpGet]
    public async Task<ActionResult<BoardDto>> Get()
    {
        var board = await _db.Boards
            .Include(b => b.Columns)
                .ThenInclude(c => c.Tasks)
            .FirstOrDefaultAsync();
            if (board is null)
            {
                return NotFound(new { Message = "Board not found" });
            }
        return Ok(board.ToDto());
    }
}