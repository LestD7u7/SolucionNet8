using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;
using System;

namespace BancaLafise.Infrastructure.Repository
{
    public class ClienteRepository : BaseRepository<Cliente> ,IClienteRepository
    {
        public ClienteRepository(LafiseDbContext context) : base(context)
        {
        }
    }
}
