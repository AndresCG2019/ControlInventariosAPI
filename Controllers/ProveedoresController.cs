using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using ControlInventariosAPI.DTOs;
using ControlInventariosAPI.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ControlInventariosAPI.Controllers
{
    [Route("api/proveedores")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public ProveedoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Proveedor>> Get(int id)
        {
            try
            {
                Proveedor proveedor = await _context.Proveedores.AsNoTracking().FirstOrDefaultAsync(x => x.IdProveedor == id);

                if (proveedor == null)
                {
                    return NotFound();
                }

                return proveedor;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Proveedor>>> Get()
        {
            try
            {
                List<Proveedor> proveedores = await _context.Proveedores
                    .AsNoTracking()
                    .ToListAsync();

                return proveedores;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] Proveedor proveedor)
        {
            try
            {
                _context.Proveedores.Add(proveedor);
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int id, [FromBody] Proveedor proveedorParam)
        {
            try
            {
                Proveedor proveedor = await _context.Proveedores.FirstOrDefaultAsync(x => x.IdProveedor == id);

                if (proveedor == null)
                {
                    return NotFound();
                }

                // inicia checeo del context en almacenamiento local
                var proveedorLocal = _context.Proveedores.Local
                    .SingleOrDefault(x => x.IdProveedor == id);

                if (proveedorLocal != null)
                    _context.Entry(proveedorLocal).State = EntityState.Detached;

                // termina checeo del context en almacenamiento local

                proveedor = proveedorParam;

                _context.Proveedores.Update(proveedor);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existe = await _context.Proveedores.AnyAsync(x => x.IdProveedor == id);

                if (!existe)
                {
                    return NotFound();
                }

                _context.Remove(new Proveedor() { IdProveedor = id });

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }
    }
}
