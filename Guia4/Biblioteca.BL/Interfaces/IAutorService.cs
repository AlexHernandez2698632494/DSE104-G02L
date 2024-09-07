using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.Entites.DTO;

namespace Biblioteca.BL.Interfaces
{
    public interface IAutorService
    {
        public Task<List<AutorDto>> GetAutoresAsync();
        public Task<AutorDto>GetAutorByIdAsync(int id);
        public Task<int>InsertAutorAsync(AutorDto autor);
        public Task<AutorDto> UpdateAutorAsync(AutorDto autor);
        public Task<bool> DeleteAutorAsync(int id);
    }
}
