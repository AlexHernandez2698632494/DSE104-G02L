using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductoAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace ProductoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoContext _context;
        private readonly IConnectionMultiplexer _redis;
        public ProductosController(ProductoContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productos>>> GetProductos()
        {
            //return await _context.Productos.ToListAsync();
            var db = _redis.GetDatabase();
            string cacheKey = "productoList";
            var productosCache = await db.StringGetAsync(cacheKey);
            if (!productosCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<Productos>>(productosCache);
            }
            var productos = await _context.Productos.ToListAsync();
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(productos), TimeSpan.FromMinutes(10));
            return productos;
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Productos>> GetProductos(int id)
        {
            //var productos = await _context.Productos.FindAsync(id);

            //if (productos == null)
            //{
            //    return NotFound();
            //}

            //return productos;
            var db = _redis.GetDatabase();
            string cacheKey = "producto_" + id.ToString();
            var productosCache = await db.StringGetAsync(cacheKey);
            if (!productosCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Productos>(productosCache);
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(producto), TimeSpan.FromMinutes(10));
            return producto;
        }

        // PUT: api/Productos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductos(int id, Productos producto)
        {
            //if (id != productos.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(productos).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ProductosExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            if (id != producto.Id)
            {
                return BadRequest();
            }
            _context.Entry(producto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                var db = _redis.GetDatabase();
                string cacheKeyProducto = "* producto_" + id.ToString();
                string cacheList = "* productList";
                await db.KeyDeleteAsync(cacheKeyProducto);
                await db.KeyDeleteAsync(cacheList);
            }
            catch (Exception ex)
            {
                if (!ProductosExists(id))
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

        // POST: api/Productos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Productos>> PostProductos(Productos productos)
        {
            //_context.Productos.Add(productos);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetProductos", new { id = productos.Id }, productos);
            _context.Productos.Add(productos);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyList = "productoList";
            await db.KeyDeleteAsync(cacheKeyList);
            return CreatedAtAction("GetProducto", new { id = productos.Id }, productos);
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductos(int id)
        {
            //var productos = await _context.Productos.FindAsync(id);
            //if (productos == null)
            //{
            //    return NotFound();
            //}

            //_context.Productos.Remove(productos);
            //await _context.SaveChangesAsync();

            //return NoContent();
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyProducto = "producto_" + id.ToString();
            string cacheKeyList = "productoList";
            await db.KeyDeleteAsync(cacheKeyList);
            await db.KeyDeleteAsync(cacheKeyProducto);
            return NoContent();
        }

        private bool ProductosExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
