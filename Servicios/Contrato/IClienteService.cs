using Microsoft.EntityFrameworkCore;
using MiradorB.Models;
using MiradorB.Models;

namespace MiradorB.Servicios.Contrato
{
    public interface IClienteService
    {
        Task<Cliente> GetCliente(string correo, string contrasena);
        Task<Cliente> GetClienteByEmail(string correo);
        Task<Cliente> SaveCliente(Cliente modelo);
        Task<bool> UpdateCliente(Cliente cliente);
    }
}