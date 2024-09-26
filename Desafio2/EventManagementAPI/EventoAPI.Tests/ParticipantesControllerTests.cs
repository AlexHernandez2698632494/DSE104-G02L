using EventManagementAPI.Controllers;
using EventManagementAPI.Data;
using EventManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EventoAPI.Tests
{
    public class ParticipantesControllerTests
    {
        private readonly EventManagementContext _context;
        private readonly Mock<IDistributedCache> _cacheMock;

        public ParticipantesControllerTests()
        {
            _context = Setup.GetInMemoryDataBaseContext();
            _cacheMock = new Mock<IDistributedCache>();
        }

        [Fact]
        public async Task GetParticipantes_ReturnsOkResult_WithListOfParticipantes()
        {
            var controller = new ParticipantesController(_context, _cacheMock.Object);

            _cacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync((byte[])null);

            _context.Eventos.Add(new Evento { Id = 1, Nombre = "Evento 1", Lugar = "Lugar del Evento" });
            _context.Participantes.Add(new Participante
            {
                Id = 1,
                Nombre = "Participante 1",
                Email = "participante1@example.com",
                EventoId = 1
            });

            await _context.SaveChangesAsync();

            var result = await controller.GetParticipantes();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var participantes = Assert.IsAssignableFrom<IEnumerable<Participante>>(okResult.Value);
            Assert.Single(participantes);
        }

        [Fact]
        public async Task GetParticipante_ReturnsNotFound_WhenParticipanteDoesNotExist()
        {
            var controller = new ParticipantesController(_context, _cacheMock.Object);

            _cacheMock.Setup(c => c.GetAsync($"participante_{999}", It.IsAny<CancellationToken>()))
                       .ReturnsAsync((byte[])null);

            var result = await controller.GetParticipante(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostParticipante_ReturnsCreatedAtAction_WhenValid()
        {
            var controller = new ParticipantesController(_context, _cacheMock.Object);
            var participante = new Participante
            {
                Id = 1,
                Nombre = "Nuevo Participante",
                Email = "participante@example.com",
                EventoId = 1
            };

            _context.Eventos.Add(new Evento { Id = 1, Nombre = "Evento 1", Lugar = "Lugar del Evento" });
            await _context.SaveChangesAsync();

            var result = await controller.PostParticipante(participante);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.IsType<Participante>(createdAtActionResult.Value);
        }

        [Fact]
        public async Task PutParticipante_ReturnsNoContent_WhenValid()
        {
            var controller = new ParticipantesController(_context, _cacheMock.Object);

            var participante = new Participante
            {
                Id = 1,
                Nombre = "Participante 1",
                Email = "participante1@example.com",
                EventoId = 1
            };

            _context.Eventos.Add(new Evento { Id = 1, Nombre = "Evento 1", Lugar = "Lugar del Evento" });
            await _context.SaveChangesAsync();
            await _context.Participantes.AddAsync(participante);
            await _context.SaveChangesAsync();

            participante.Nombre = "Participante Modificado";
            var result = await controller.PutParticipante(1, participante);

            Assert.IsType<NoContentResult>(result);
            var updatedParticipante = await _context.Participantes.FindAsync(1);
            Assert.Equal("Participante Modificado", updatedParticipante.Nombre);
        }

        [Fact]
        public async Task DeleteParticipante_ReturnsNoContent_WhenParticipanteExists()
        {
            var controller = new ParticipantesController(_context, _cacheMock.Object);
            var participante = new Participante
            {
                Id = 1,
                Nombre = "Participante a Eliminar",
                Email = "participante@example.com",
                EventoId = 1
            };

            _context.Eventos.Add(new Evento { Id = 1, Nombre = "Evento 1", Lugar = "Lugar del Evento" });
            await _context.Participantes.AddAsync(participante);
            await _context.SaveChangesAsync();

            var result = await controller.DeleteParticipante(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Participantes.FindAsync(1));
        }
    }
}
