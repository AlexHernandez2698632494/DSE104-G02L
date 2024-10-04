using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using VehiculosAPI.Models;

namespace VehiculosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MarcasController : ControllerBase
    {
        private readonly VehiculoDBContext _context;

        public MarcasController(VehiculoDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marcas>>> GetMarcas()
        {
            return await _context.Marcas.Include(m => m.Modelos).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Marcas>> GetMarca(int id)
        {
            var marca = await _context.Marcas.Include(m => m.Modelos).FirstOrDefaultAsync(m => m.ID == id);

            if (marca == null)
            {
                return NotFound();
            }
            return marca;
        }

        [HttpPost]
        public async Task<ActionResult<Marcas>> PostMarca(Marcas marca)
        {
            _context.Marcas.Add(marca);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMarca), new { id = marca.ID }, marca);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarca(int id, Marcas marca)
        {
            if (id != marca.ID)
            {
                return BadRequest();
            }

            _context.Entry(marca).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarca(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }

            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
