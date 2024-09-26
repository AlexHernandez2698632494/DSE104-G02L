using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EventManagementAPI.Data;
using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantesController : ControllerBase
    {
        private readonly EventManagementContext _context;
        private readonly IDistributedCache _cache;
        private readonly string cacheKey = "participantes_cache";

        public ParticipantesController(EventManagementContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participante>>> GetParticipantes()
        {
            var cachedParticipantes = await _cache.GetStringAsync(cacheKey);
            if (cachedParticipantes != null)
            {
                var participantes = JsonSerializer.Deserialize<List<Participante>>(cachedParticipantes);
                return Ok(participantes);
            }

            var participantesFromDb = await _context.Participantes.ToListAsync();
            if (participantesFromDb == null || !participantesFromDb.Any())
            {
                return NotFound();
            }

            var serializedParticipantes = JsonSerializer.Serialize(participantesFromDb);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKey, serializedParticipantes, cacheOptions);

            return Ok(participantesFromDb);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Participante>> GetParticipante(int id)
        {
            var cacheKeyById = $"participante_{id}";
            var cachedParticipante = await _cache.GetStringAsync(cacheKeyById);

            if (cachedParticipante != null)
            {
                var participante = JsonSerializer.Deserialize<Participante>(cachedParticipante);
                return Ok(participante);
            }

            var participanteFromDb = await _context.Participantes.FindAsync(id);

            if (participanteFromDb == null)
            {
                return NotFound();
            }

            var serializedParticipante = JsonSerializer.Serialize(participanteFromDb);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKeyById, serializedParticipante, cacheOptions);

            return Ok(participanteFromDb);
        }

        [HttpPost]
        public async Task<ActionResult<Participante>> PostParticipante(Participante participante)
        {
            var evento = await _context.Eventos.FindAsync(participante.EventoId);
            if (evento == null)
            {
                return BadRequest("El evento asociado no fue encontrado.");
            }

            _context.Participantes.Add(participante);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync(cacheKey);

            return CreatedAtAction(nameof(GetParticipante), new { id = participante.Id }, participante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipante(int id, Participante participante)
        {
            if (id != participante.Id)
            {
                return BadRequest("Los IDs no coinciden.");
            }

            var evento = await _context.Eventos.FindAsync(participante.EventoId);
            if (evento == null)
            {
                return BadRequest("El evento asociado no fue encontrado.");
            }

            _context.Entry(participante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync(cacheKey);
                await _cache.RemoveAsync($"participante_{id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Participantes.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipante(int id)
        {
            var participante = await _context.Participantes.FindAsync(id);
            if (participante == null)
            {
                return NotFound();
            }

            _context.Participantes.Remove(participante);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync(cacheKey);
            await _cache.RemoveAsync($"participante_{id}");

            return NoContent();
        }
    }
}
