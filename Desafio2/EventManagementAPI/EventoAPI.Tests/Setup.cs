using EventManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventoAPI.Tests
{
    public static class Setup
    {
        public static EventManagementContext GetInMemoryDataBaseContext()
        {
            var options = new DbContextOptionsBuilder<EventManagementContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var context = new EventManagementContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
