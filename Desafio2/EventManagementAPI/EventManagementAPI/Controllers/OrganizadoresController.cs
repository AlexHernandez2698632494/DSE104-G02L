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
    public class OrganizadoresController : ControllerBase
    {
        private readonly EventManagementContext _context;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "organizadores_cache";

        public OrganizadoresController(EventManagementContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organizador>>> GetOrganizadores()
        {
            var cachedOrganizadores = await _cache.GetAsync(CacheKey);
            if (cachedOrganizadores != null)
            {
                var organizadores = JsonSerializer.Deserialize<List<Organizador>>(cachedOrganizadores);
                return Ok(organizadores);
            }

            var organizadoresFromDb = await _context.Organizadores.ToListAsync();
            var serializedOrganizadores = JsonSerializer.Serialize(organizadoresFromDb);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(CacheKey, serializedOrganizadores, cacheOptions);

            return Ok(organizadoresFromDb);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Organizador>> GetOrganizador(int id)
        {
            var cacheKeyById = $"organizador_{id}";
            var cachedOrganizador = await _cache.GetAsync(cacheKeyById);
            if (cachedOrganizador != null)
            {
                var organizador = JsonSerializer.Deserialize<Organizador>(cachedOrganizador);
                return Ok(organizador);
            }

            var organizadorFromDb = await _context.Organizadores.FindAsync(id);
            if (organizadorFromDb == null)
            {
                return NotFound();
            }

            var serializedOrganizador = JsonSerializer.Serialize(organizadorFromDb);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKeyById, serializedOrganizador, cacheOptions);

            return Ok(organizadorFromDb);
        }

        [HttpPost]
        public async Task<ActionResult<Organizador>> PostOrganizador(Organizador organizador)
        {
            var evento = await _context.Eventos.FindAsync(organizador.EventoId);
            if (evento == null)
            {
                return BadRequest("Evento no encontrado.");
            }

            _context.Organizadores.Add(organizador);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync(CacheKey);

            return CreatedAtAction(nameof(GetOrganizador), new { id = organizador.Id }, organizador);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganizador(int id, Organizador organizador)
        {
            if (id != organizador.Id)
            {
                return BadRequest();
            }

            var evento = await _context.Eventos.FindAsync(organizador.EventoId);
            if (evento == null)
            {
                return BadRequest("Evento no encontrado.");
            }

            _context.Entry(organizador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _cache.RemoveAsync(CacheKey);
                await _cache.RemoveAsync($"organizador_{id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Organizadores.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganizador(int id)
        {
            var organizador = await _context.Organizadores.FindAsync(id);
            if (organizador == null)
            {
                return NotFound();
            }

            _context.Organizadores.Remove(organizador);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync(CacheKey);
            await _cache.RemoveAsync($"organizador_{id}");

            return NoContent();
        }
    }
}
