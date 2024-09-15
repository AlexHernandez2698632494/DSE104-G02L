using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using DUI.Controllers;
using DUI.Models;
using System;
using System.Threading.Tasks;
using Xunit;
using static DUI.Controllers.PersonasAPI;

namespace PersonasAPI.Tests
{
    public class PersonasControllerTests
    {
        [Fact]
        public async Task PostPersona_ReturnsCreatedAtAction_WhenModelIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PersonasContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new PersonasContext(options);
            var controller = new PersonasController(context);

            var persona = new Persona
            {
                PrimerNombre = "Juan",
                PrimerApellido = "Perez",
                DUI = "12345678-9",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };

            // Act
            var result = await controller.PostPersona(persona);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetPersona", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task PostPersona_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PersonasContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new PersonasContext(options);
            var controller = new PersonasController(context);

            var persona = new Persona
            {
                PrimerApellido = "Perez",  // PrimerNombre está faltando
                DUI = "12345678-9",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };

            // Act
            var result = await controller.PostPersona(persona);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}