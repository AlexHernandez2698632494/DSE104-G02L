using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DUI.Controllers;
using DUI.Models;

namespace DUI.Controllers
{
    public class PersonasAPI : Controller
    {
        [Route("api/[controller]")]
        [ApiController]
        public class PersonasController : ControllerBase
        {
            private readonly PersonasContext _context;

            public PersonasController(PersonasContext context)
            {
                _context = context;
            }

            // POST: api/Personas
            [HttpPost]
            public async Task<ActionResult<Persona>> PostPersona(Persona persona)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Personas.Add(persona);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPersona), new { id = persona.Id }, persona);
            }

            // GET: api/Personas/{id}
            [HttpGet("{id}")]
            public async Task<ActionResult<Persona>> GetPersona(int id)
            {
                var persona = await _context.Personas.FindAsync(id);

                if (persona == null)
                {
                    return NotFound();
                }

                return persona;
            }
        }
    }
}
