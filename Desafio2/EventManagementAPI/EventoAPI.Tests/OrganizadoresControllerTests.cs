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
    public class OrganizadoresControllerTests
    {
        private readonly EventManagementContext _context;
        private readonly Mock<IDistributedCache> _cacheMock;

        public OrganizadoresControllerTests()
        {
            _context = Setup.GetInMemoryDataBaseContext();
            _cacheMock = new Mock<IDistributedCache>();
        }

        [Fact]
        public async Task GetOrganizadores_ReturnsOkResult_WithListOfOrganizadores()
        {
            // Arrange
            var controller = new OrganizadoresController(_context, _cacheMock.Object);

            // Simulate that the cache does not have data
            _cacheMock.Setup(c => c.GetAsync("organizadores_cache", It.IsAny<CancellationToken>()))
                       .ReturnsAsync((byte[])null); // No data in cache

            // Add necessary event
            _context.Eventos.Add(new Evento { Id = 1, Nombre = "Evento 1", Lugar = "Lugar del Evento" });

            // Add an organizer with all required properties
            _context.Organizadores.Add(new Organizador
            {
                Id = 1,
                Nombre = "Organizador 1",
                Cargo = "Cargo del Organizador",
                EventoId = 1
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await controller.GetOrganizadores();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var organizadores = Assert.IsAssignableFrom<IEnumerable<Organizador>>(okResult.Value);
            Assert.Single(organizadores);
        }

        [Fact]
        public async Task GetOrganizador_ReturnsNotFoundResult_WhenOrganizadorDoesNotExist()
        {
            // Arrange
            var controller = new OrganizadoresController(_context, _cacheMock.Object);

            // Simulate that no organizador is in the cache
            _cacheMock.Setup(c => c.GetAsync($"organizador_{999}", It.IsAny<CancellationToken>()))
                       .ReturnsAsync((byte[])null); // Simulate cache miss

            // Act
            var result = await controller.GetOrganizador(999); // ID that does not exist

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostOrganizador_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var controller = new OrganizadoresController(_context, _cacheMock.Object);

            // Ensure to set all required properties including 'Cargo'
            var organizador = new Organizador
            {
                Id = 1,
                Nombre = "Nuevo Organizador",
                Cargo = "Cargo del Nuevo Organizador", // Add this line
                EventoId = 1
            };

            // Simulate that the Evento exists with all required properties
            _context.Eventos.Add(new Evento
            {
                Id = 1,
                Nombre = "Evento 1",
                Lugar = "Lugar del Evento"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await controller.PostOrganizador(organizador);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.IsType<Organizador>(createdAtActionResult.Value);
        }

        [Fact]
        public async Task PutOrganizador_ReturnsNoContent_WhenValid()
        {
            // Arrange
            var controller = new OrganizadoresController(_context, _cacheMock.Object);

            // Add an organizador with all required properties
            var organizador = new Organizador
            {
                Id = 1,
                Nombre = "Organizador 1",
                Cargo = "Cargo del Organizador", // Ensure this is set
                EventoId = 1
            };

            // Simulate that the Evento exists
            _context.Eventos.Add(new Evento
            {
                Id = 1,
                Nombre = "Evento 1",
                Lugar = "Lugar del Evento"
            });
            await _context.SaveChangesAsync();

            // Add the organizador to the context
            await _context.Organizadores.AddAsync(organizador);
            await _context.SaveChangesAsync();

            // Act
            organizador.Nombre = "Organizador Modificado"; // Update the name
            organizador.Cargo = "Cargo Modificado"; // Update the cargo to ensure it's set
            var result = await controller.PutOrganizador(1, organizador);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedOrganizador = await _context.Organizadores.FindAsync(1);
            Assert.Equal("Organizador Modificado", updatedOrganizador.Nombre);
            Assert.Equal("Cargo Modificado", updatedOrganizador.Cargo); // Ensure cargo is updated
        }


        [Fact]
        public async Task DeleteOrganizador_ReturnsNoContent_WhenOrganizadorExists()
        {
            // Arrange
            var controller = new OrganizadoresController(_context, _cacheMock.Object);
            var organizador = new Organizador
            {
                Id = 1,
                Nombre = "Organizador 1",
                Cargo = "Cargo del Organizador",
                EventoId = 1
            };

            // Simulate that the Evento exists
            _context.Eventos.Add(new Evento { Id = 1, Nombre = "Evento 1", Lugar = "Lugar del Evento" });
            await _context.Organizadores.AddAsync(organizador);
            await _context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteOrganizador(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Organizadores.FindAsync(1));
        }
    }
}
