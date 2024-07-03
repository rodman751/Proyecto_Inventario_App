using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Entidades;

namespace Inventario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AjusteProductoController : ControllerBase
    {
        private readonly DbContext _context;

        public AjusteProductoController(DbContext context)
        {
            _context = context;
        }

        // GET: api/AjusteProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AjusteProducto>>> GetAjusteProducto()
        {
            //return await _context.AjusteProducto.ToListAsync();
            var data = await _context.AjusteProducto
               .Include(ap => ap.DetalleAjusteProducto) 
                   .ThenInclude(dap => dap.Producto)     
               .ToListAsync();

            return data;
        }

        // GET: api/AjusteProducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AjusteProducto>> GetAjusteProducto(int id)
        {
            //var ajusteProducto = await _context.AjusteProducto.FindAsync(id);
            var ajusteProducto = await _context.AjusteProducto
                .Include(ap => ap.DetalleAjusteProducto)
                    .ThenInclude(dap => dap.Producto)
                .FirstOrDefaultAsync(ap => ap.ID_Ajuste == id);

            if (ajusteProducto == null)
            {
                return NotFound();
            }

            return ajusteProducto;
        }

        // PUT: api/AjusteProducto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAjusteProducto(int id, AjusteProducto ajusteProducto)
        {
            if (id != ajusteProducto.ID_Ajuste)
            {
                return BadRequest();
            }

            _context.Entry(ajusteProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AjusteProductoExists(id))
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

        // POST: api/AjusteProducto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AjusteProducto>> PostAjusteProducto(AjusteProducto ajusteProducto)
        {
            _context.AjusteProducto.Add(ajusteProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAjusteProducto", new { id = ajusteProducto.ID_Ajuste }, ajusteProducto);
        }

        // DELETE: api/AjusteProducto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAjusteProducto(int id)
        {
            var ajusteProducto = await _context.AjusteProducto.FindAsync(id);
            if (ajusteProducto == null)
            {
                return NotFound();
            }

            _context.AjusteProducto.Remove(ajusteProducto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("LastAjuste")]
        public async Task<ActionResult<AjusteProducto>> GetLastAjuste()
        {
            var lastAjuste = await _context.AjusteProducto
                .OrderByDescending(a => a.ID_Ajuste)
                .FirstOrDefaultAsync();

            if (lastAjuste == null)
            {
                return NotFound();
            }

            return Ok(lastAjuste);
        }
        private bool AjusteProductoExists(int id)
        {
            return _context.AjusteProducto.Any(e => e.ID_Ajuste == id);
        }

        
    }
}
