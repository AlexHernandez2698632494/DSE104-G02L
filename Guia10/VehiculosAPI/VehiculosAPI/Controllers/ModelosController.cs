using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using VehiculosAPI.Models;

namespace VehiculosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ModelosController : ControllerBase
    {
        private readonly VehiculoDBContext _context;

        public ModelosController(VehiculoDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Modelo>>> GetModelos()
        {
            return await _context.Modelos.Include(m => m.Vehiculos).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Modelo>> GetModelo(int id)
        {
            var modelo = await _context.Modelos.Include(m => m.Vehiculos).FirstOrDefaultAsync(m => m.Id == id);

            if (modelo == null)
            {
                return NotFound();
            }

            return modelo;
        }

        [HttpPost]
        public async Task<ActionResult<Modelo>> PostModelo(Modelo modelo)
        {
            _context.Modelos.Add(modelo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetModelo), new { id = modelo.Id }, modelo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutModelo(int id, Modelo modelo)
        {
            if (id != modelo.Id)
            {
                return BadRequest();
            }

            _context.Entry(modelo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModelo(int id)
        {
            var modelo = await _context.Modelos.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            _context.Modelos.Remove(modelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
