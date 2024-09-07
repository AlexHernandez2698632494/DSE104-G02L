using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.DAL.Interface
{
    public interface IDatabaseRepository
    {
        public Task<List<T>> GetDataByQueryAsync<T>(string query, DynamicParameters? parameters = null);
        public Task<int> InsertAsync(string storeProcedure, DynamicParameters? parameters = null);
        public Task<T> Update<T>(string storeProcedure, DynamicParameters? parameters = null);
        public Task<bool> Delete(string storeProcedure, DynamicParameters? parameters= null);
    }
}
