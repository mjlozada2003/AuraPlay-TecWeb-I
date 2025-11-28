using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoTecWeb.Models;
using ProyectoTecWeb.Models.DTOS;
using ProyectoTecWeb.Services;

namespace ProyectoTecWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _service;
        public PlaylistController(IPlaylistService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPlaylists()
        {
            IEnumerable<Playlist> items = await _service.GetAll();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var playlist = await _service.GetOne(id);
            return Ok(playlist);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // ✅ EXTRAER ID DEL TOKEN
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (userIdClaim == null) return Unauthorized();

            // Pasamos el ID al servicio
            var playlist = await _service.CreatePlaylist(dto, Guid.Parse(userIdClaim));

            return CreatedAtAction(nameof(GetOne), new { id = playlist.Id }, playlist);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdatePlaylist([FromBody] UpdatePlaylistDto dto, Guid id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var playlist = await _service.UpdatePlaylist(dto, id);
            return CreatedAtAction(nameof(GetOne), new { id = playlist.Id }, playlist);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _service.DeletePlaylist(id);
            return NoContent();
        }

        [HttpPost("{id}/songs")]
        [Authorize]
        public async Task<IActionResult> AddSongsToPlaylist(Guid id, [FromBody] AddSongToPlaylistDto dto)
        {
            if (dto == null || dto.Songs == null || !dto.Songs.Any())
                return BadRequest("Debe enviar al menos un id de canción.");

            await _service.AddSongToPlaylist(id, dto.Songs);

            return NoContent(); // o Ok(...)
        }

        [HttpDelete("{playlistId:guid}/songs/{songId:guid}")]
        [Authorize]
        public async Task<IActionResult> RemoveSongFromPlaylist(Guid playlistId, Guid songId)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            await _service.RemoveSongFromPlaylist(playlistId, songId);
            return NoContent();
        }
    }
}
