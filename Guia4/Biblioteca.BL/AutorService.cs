using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.BL.Interfaces;
using Biblioteca.DAL.Interface;
using Biblioteca.Entites.DTO;
using Biblioteca.Entites.Models;
using System.Reflection;

namespace Biblioteca.BL
{
    public class AutorService : IAutorService
    {
        private readonly IAutorRepository repository;
        private readonly IMapper mapper;
        public AutorService(IAutorRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<List<AutorDto>> GetAutoresAsync()
        {
            try
            {
                var result = await repository.GetAutoresAsync();
                return mapper.Map<List<Autor>, List<AutorDto>>(result);
            }
            catch (Exception ex)
            {
                return new List<AutorDto>();
            }
        }
        public async Task<AutorDto> GetAutorByIdAsync(int id)
        {
            try
            {
                var result = await repository.GetAutorByIdAsync(id);
                return mapper.Map<Autor, AutorDto>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<int> InsertAutorAsync(AutorDto model)
        {
            try
            {
                var entity = mapper.Map<AutorDto, Autor>(model);
                return await repository.InsertAutorAsync(entity);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<AutorDto> UpdateAutorAsync(AutorDto model)
        {
            try
            {
                var entity = mapper.Map<AutorDto, Autor>(model);
                var result = await repository.UpdateAutorAsync(entity);
                return mapper.Map<Autor, AutorDto>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> DeleteAutorAsync(int id)
        {
            try
            {
                return await repository.DeleteAutorAsync(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}