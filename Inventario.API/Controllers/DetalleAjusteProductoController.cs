using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Entidades;
using Inventario.Entidades.DTO;

namespace Inventario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleAjusteProductoController : ControllerBase
    {
        private readonly DbContext _context;

        public DetalleAjusteProductoController(DbContext context)
        {
            _context = context;
        }

        // GET: api/DetalleAjusteProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleAjusteProducto>>> GetDetalleAjusteProducto()
        {
            //return await _context.DetalleAjusteProducto.ToListAsync();
            var data = await _context.DetalleAjusteProducto
                .Include(x => x.AjusteProducto)
                .Include(x => x.Producto)
                .ToListAsync();
            return data;
        }

        // GET: api/DetalleAjusteProducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleAjusteProducto>> GetDetalleAjusteProducto(int id)
        {
            //var detalleAjusteProducto = await _context.DetalleAjusteProducto.FindAsync(id);
            var detalleAjusteProducto = await _context.DetalleAjusteProducto
            .Include(dap => dap.AjusteProducto) 
            .Include(dap => dap.Producto)       
            .FirstOrDefaultAsync(dap => dap.ID_DetalleAjuste == id);

            if (detalleAjusteProducto == null)
            {
                return NotFound();
            }

            return detalleAjusteProducto;
        }

        // PUT: api/DetalleAjusteProducto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleAjusteProducto(int id, DetalleAjusteProducto detalleAjusteProducto)
        {
            if (id != detalleAjusteProducto.ID_DetalleAjuste)
            {
                return BadRequest();
            }

            _context.Entry(detalleAjusteProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleAjusteProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DetalleAjusteProducto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetalleAjusteProducto>> PostDetalleAjusteProducto(DetalleAjusteProducto detalleAjusteProducto)
        {
            _context.DetalleAjusteProducto.Add(detalleAjusteProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleAjusteProducto", new { id = detalleAjusteProducto.ID_DetalleAjuste }, detalleAjusteProducto);
        }

        // DELETE: api/DetalleAjusteProducto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleAjusteProducto(int id)
        {
            var detalleAjusteProducto = await _context.DetalleAjusteProducto.FindAsync(id);
            if (detalleAjusteProducto == null)
            {
                return NotFound();
            }

            _context.DetalleAjusteProducto.Remove(detalleAjusteProducto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetalleAjusteProductoExists(int id)
        {
            return _context.DetalleAjusteProducto.Any(e => e.ID_DetalleAjuste == id);
        }
        [HttpGet("GetAjusteDetalles/{id}")]
        public async Task<ActionResult<IEnumerable<DetalleAjusteProductoDTO>>> GetAjusteDetalles(int id)
        {
            var result = await (from a in _context.AjusteProducto
                                join d in _context.DetalleAjusteProducto on a.ID_Ajuste equals d.ID_Ajuste
                                join p in _context.Producto on d.ID_Producto equals p.ID_Producto
                                where a.ID_Ajuste == id
                                select new DetalleAjusteProductoDTO
                                {
                                    Fecha = a.Fecha,
                                    ID_DetalleAjuste = d.ID_DetalleAjuste,
                                    ID_Producto = d.ID_Producto,
                                    NombreProducto = p.Nombre,
                                    CodigoProducto = p.Codigo,
                                    CantidadAjustada = d.CantidadAjustada,
                                    RazonAjuste = d.RazonAjuste
                                }).ToListAsync();

            if (!result.Any())
            {
                return NotFound();
            }

            return result;
        }

    }
}
