using Medilink.Interfaces;
using Medilink.Models;
using Medilink.Context;
using Microsoft.EntityFrameworkCore;
namespace Medilink.Services
{
    public class InsumoService : IInsumoService
    {
        private readonly MedilinkDbContext _context;
        public InsumoService(MedilinkDbContext context)
        {
            _context = context;
        }
        public async Task<Insumo> AddInsumo(Insumo insumo)
        {
            var insumoInventario = await GetInsumo(insumo.Id);
            if (insumoInventario != null)
            {
                insumoInventario.cantidadInventario +=insumo.cantidadInventario;
                await _context.SaveChangesAsync();
                return insumoInventario;
            }
            await _context.Insumos.AddAsync(insumo);
            await _context.SaveChangesAsync();
            return insumo;
        }

        public async Task<bool> RestarCantidad(int id, int cantidad)
        {
            var insumo = await GetInsumo(id);
            if (insumo == null || cantidad <= 0 || insumo.cantidadInventario < cantidad)
            return false;
            insumo.cantidadInventario -= cantidad;
            await UpdateInsumo(insumo);
            return true;
        }
               public async Task<Insumo> GetInsumo(int id)
        {
            return await _context.Insumos.FindAsync(id);
        }

        public async Task<bool> DeleteInsumo(int id)
        {
            var insumo = await GetInsumo(id);
            if (insumo == null) return false;
            _context.Insumos.Remove(insumo);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Insumo>> GetInsumos()
        {
            return await _context.Insumos.ToListAsync();
        }

        public async Task<bool> UpdateInsumo(Insumo insumo)
        {
            var InsumoExiste = await GetInsumo(insumo.Id);
            if (InsumoExiste == null) return false;
            InsumoExiste.Nombre = insumo.Nombre;
            InsumoExiste.Descripcion = insumo.Descripcion;
            InsumoExiste.cantidadInventario = insumo.cantidadInventario;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Insumos.AnyAsync(m => m.Id == insumo.Id))
                    return false;
                else
                    throw;
            }
        }

        public void PedidoInsumos(){}
    }
}