using Moq;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using EventManagementAPI.Models;
using EventManagementAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EventoAPI.Tests
{
    public class EventoControllerTests
    {
        private Mock<IDistributedCache> mockCache;

        public EventoControllerTests()
        {
            mockCache = new Mock<IDistributedCache>(); // Crear el mock de IDistributedCache
        }
        
        [Fact]
        public async Task PostEvento_AgregaEvento_CuandoEventoEsValido()
        {
            var context = Setup.GetInMemoryDataBaseContext();
            var controller = new EventosController(context, mockCache.Object); // Pasar el mock de IDistributedCache
            var nuevoEvento = new Evento { Nombre = "Fiestas Julias", Lugar = "Santa Ana", Fecha = new DateTime(2024, 7, 25) };
            var result = await controller.PostEvento(nuevoEvento);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var evento = Assert.IsType<Evento>(createdResult.Value);
            Assert.Equal("Fiestas Julias", evento.Nombre);
        }

        [Fact]
        public async Task GetEvento_RetornaEvento_CuandoIdEsValido()
        {
            // Arrange
            var context = Setup.GetInMemoryDataBaseContext();
            var evento = new Evento
            {
                Id = 1,
                Nombre = "cumpleaños",
                Lugar = "mi casa",
                Fecha = new DateTime(2024, 4, 7)
            };

            // Add the event to the in-memory database
            context.Eventos.Add(evento);
            await context.SaveChangesAsync();

            // Serialize the event to a byte array for the cache
            var serializedEvento = JsonSerializer.SerializeToUtf8Bytes(evento);

            // Mock the cache to return the serialized event when GetAsync is called
            mockCache.Setup(cache => cache.GetAsync("Evento_1", default)) // Ensure correct cache key
                     .ReturnsAsync(serializedEvento);

            var controller = new EventosController(context, mockCache.Object);

            // Act
            var result = await controller.GetEvento(evento.Id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Evento>>(result);
            var returnValue = Assert.IsType<Evento>(actionResult.Value);
            Assert.Equal("cumpleaños", returnValue.Nombre);
        }



        [Fact]
        public async Task GetEvento_RetornaNotFound_CuandoIdNoExiste()
        {
            // Arrange
            var context = Setup.GetInMemoryDataBaseContext();
            var mockCache = new Mock<IDistributedCache>();

            mockCache.Setup(cache => cache.GetAsync(It.IsAny<string>(), default))
                     .ReturnsAsync((byte[])null); // Simulate no event in the cache

            var controller = new EventosController(context, mockCache.Object);

            // Act
            var result = await controller.GetEvento(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task PostEvento_NoAgregaEvento_CuandoNombreEsNulo()
        {
            // Arrange
            var context = Setup.GetInMemoryDataBaseContext();
            var mockCache = new Mock<IDistributedCache>();
            var controller = new EventosController(context, mockCache.Object);

            var nuevoEvento = new Evento
            {
                Nombre = null,  // Nombre nulo para probar la validación
                Lugar = "lugar",
                Fecha = DateTime.Now
            };

            // Simular un error de modelo (nombre nulo)
            controller.ModelState.AddModelError("Nombre", "El nombre es requerido");

            // Act
            var result = await controller.PostEvento(nuevoEvento);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);  // Verifica que retorna BadRequest
        }

        [Fact]
        public async Task PostEvento_IncrementaConteo_CuandoSeAgregaNuevoEvento()
        {
            var context = Setup.GetInMemoryDataBaseContext();
            var controller = new EventosController(context, mockCache.Object); // Pasar el mock de IDistributedCache
            var eventoInicial = new Evento
            {
                Nombre = "Evento 1",
                Lugar = "Lugar 1",
                Fecha = DateTime.Now
            };
            await controller.PostEvento(eventoInicial);
            var nuevoEvento = new Evento
            {
                Nombre = "Evento 2",
                Lugar = "Lugar 2",
                Fecha = DateTime.Now
            };
            await controller.PostEvento(nuevoEvento);
            var eventos = await context.Eventos.ToListAsync();
            Assert.Equal(2, eventos.Count);
        }
    }
}
