using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagementAPI.Data;
using EventManagementAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly EventManagementContext _context;
        private readonly IDistributedCache _cache;
        private readonly string cacheKey = "eventos_cache";

        public EventosController(EventManagementContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            var cachedEventos = await _cache.GetStringAsync(cacheKey);
            if (cachedEventos != null)
            {
                var eventos = JsonSerializer.Deserialize<List<Evento>>(cachedEventos);
                return Ok(eventos);
            }

            var eventosFromDb = await _context.Eventos.ToListAsync();
            var serializedEventos = JsonSerializer.Serialize(eventosFromDb);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKey, serializedEventos, cacheOptions);

            return Ok(eventosFromDb);
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")] // Add this attribute
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var cachedEvento = await _cache.GetAsync($"Evento_{id}");
            if (cachedEvento != null)
            {
                var evento = JsonSerializer.Deserialize<Evento>(cachedEvento);
                return evento;
            }

            var eventoFromDb = await _context.Eventos.FindAsync(id);
            if (eventoFromDb == null)
            {
                return NotFound();
            }

            return eventoFromDb;
        }

        // POST: api/Eventos
        [HttpPost]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync(cacheKey);

            return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
        }

        // PUT: api/Eventos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.Id)
            {
                return BadRequest();
            }

            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync(cacheKey);
                await _cache.RemoveAsync($"evento_{id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
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

        // DELETE: api/Eventos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync(cacheKey);
            await _cache.RemoveAsync($"evento_{id}");

            return NoContent();
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }
    }
}
