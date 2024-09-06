using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibroAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace LibroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly LibroContext _context;
        private readonly IConnectionMultiplexer _redis;


        public LibrosController(LibroContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> Getlibros()
        {
            //return await _context.libros.ToListAsync();
            var db = _redis.GetDatabase();
            string cacheKey = "librosList";
            var librosCache = await db.StringGetAsync(cacheKey);
            if (!librosCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<Libro>>(librosCache);
            }
            var libros = await _context.libros.ToListAsync();
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(libros), TimeSpan.FromMinutes(10));
            return libros;
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            //var libro = await _context.libros.FindAsync(id);

            //if (libro == null)
            //{
            //    return NotFound();
            //}

            //return libro;
            var db = _redis.GetDatabase();
            string cacheKey = "libro_" + id.ToString();
            var librosCache = await db.StringGetAsync(cacheKey);
            if (!librosCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Libro>(librosCache);
            }
            var libro = await _context.libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(libro), TimeSpan.FromMinutes(10));
            return libro;
        }

        // PUT: api/Libros/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            //if (id != libro.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(libro).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!LibroExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            if (id != libro.Id)
            {
                return BadRequest();
            }
            _context.Entry(libro).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                var db = _redis.GetDatabase();
                string cacheKeyProducto = "* libro_" + id.ToString();
                string cacheList = "* libroList";
                await db.KeyDeleteAsync(cacheKeyProducto);
                await db.KeyDeleteAsync(cacheList);
            }
            catch (Exception ex)
            {
                if (!LibroExists(id))
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

        // POST: api/Libros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(Libro libro)
        {
            //_context.libros.Add(libro);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetLibro", new { id = libro.Id }, libro);
            _context.libros.Add(libro);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyList = "libroList";
            await db.KeyDeleteAsync(cacheKeyList);
            return CreatedAtAction("GetLibro", new { id = libro.Id }, libro);
        }

        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            //var libro = await _context.libros.FindAsync(id);
            //if (libro == null)
            //{
            //    return NotFound();
            //}

            //_context.libros.Remove(libro);
            //await _context.SaveChangesAsync();

            //return NoContent();
            var libro = await _context.libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            _context.libros.Remove(libro);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyLibro = "libros_" + id.ToString();
            string cacheKeyList = "libroList";
            await db.KeyDeleteAsync(cacheKeyList);
            await db.KeyDeleteAsync(cacheKeyLibro);
            return NoContent();
        }

        private bool LibroExists(int id)
        {
            return _context.libros.Any(e => e.Id == id);
        }
    }
}
