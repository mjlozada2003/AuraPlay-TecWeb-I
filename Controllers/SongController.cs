using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoTecWeb.Models;
using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Services;

namespace ProyectoTecWeb.Controllers
{
    public class SongController : ControllerBase
    {
        private readonly ISongService _service;
        public SongController(ISongService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs()
        {
            IEnumerable<Song> items = await _service.GetAll();
            return Ok(items);
        }
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var song = await _service.GetOne(id);
            return Ok(song);
        }
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateSong([FromBody] CreateSongDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var song = await _service.CreateSong(dto);
            return CreatedAtAction(nameof(GetOne), new { id = song.Id }, song);
        }
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateSong([FromBody] UpdateSongDto dto, Guid id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var song = await _service.UpdateSong(dto, id);
            return CreatedAtAction(nameof(GetOne), new { id = song.Id }, song);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteSong(Guid id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _service.DeleteSong(id);
            return NoContent();
        }
    }
    
}

