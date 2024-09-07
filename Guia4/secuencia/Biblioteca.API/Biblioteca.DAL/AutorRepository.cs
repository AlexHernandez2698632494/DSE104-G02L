using Biblioteca.DAL.Interfaces;
using Biblioteca.Entities.Models;
using Dapper;
namespace Biblioteca.DAL
{
    public class AutorRepository : IAutorRepository
    {
        private readonly IDatabaseRepository databaseRepository;
        public AutorRepository(IDatabaseRepository databaseRepository)
        {
            this.databaseRepository = databaseRepository;
        }
        public async Task<List<Autor>> GetAutoresAsync()
        {
            string query = "SELECT FROM Autores";
            return await databaseRepository.GetDataByQueryAsync<Autor>(query);
        }
        public async Task<Autor> GetAutorByIdAsync(int id)
        {
            string query = "SELECT FROM Autores WHERE Id @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            return (await databaseRepository.GetDataByQueryAsync<Autor>(query, parameters)).FirstOrDefault();
        }
        public async Task<int> InsertAutorAsync(Autor autor)
        {
            string query = "INSERT INTO Autores (Nombre, Apellido) VALUES (@Nombre, @Apellido); SELECT SCOPE_IDENTITY()";
            var parameters = new DynamicParameters(); parameters.Add("@Nombre", autor.Nombre);
            parameters.Add("@Apellido", autor.Apellido);
            return await databaseRepository.InsertAsync(query, parameters);
        }
        public async Task<Autor> UpdateAutorAsync(Autor autor)
        {
            string query = "UPDATE Autores SET Nombre Nombre, Apellido @Apellido WHERE ID @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", autor.Id);
            parameters.Add("@Nombre", autor.Nombre);
            parameters.Add("@Apellido", autor.Apellido);
            await databaseRepository.Update<Autor>(query, parameters);
            return autor;
        }
        public async Task<bool> DeleteAutorAsync(int id)
        {
            string query = "DELETE FROM Autores WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            return await databaseRepository.Delete(query, parameters);
        }
    }
}