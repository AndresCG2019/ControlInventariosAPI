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
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public ClientesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            try
            {
                Cliente cliente = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.IdCliente == id);

                if (cliente == null)
                {
                    return NotFound();
                }

                return cliente;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Cliente>>> Get()
        {
            try
            {
                List<Cliente> clientes = await _context.Clientes
                    .AsNoTracking()
                    .ToListAsync();

                return clientes;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] Cliente cliente)
        {
            try
            {
                _context.Clientes.Add(cliente);
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
        public async Task<ActionResult> Put(int id, [FromBody] Cliente clienteParam)
        {
            try
            {
                Cliente cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);

                if (cliente == null)
                {
                    return NotFound();
                }

                // inicia checeo del context en almacenamiento local
                var clienteLocal = _context.Clientes.Local
                    .SingleOrDefault(x => x.IdCliente == id);

                if (clienteLocal != null)
                    _context.Entry(clienteLocal).State = EntityState.Detached;

                // termina checeo del context en almacenamiento local

                cliente = clienteParam;

                _context.Clientes.Update(cliente);

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
                var existe = await _context.Clientes.AnyAsync(x => x.IdCliente == id);

                if (!existe)
                {
                    return NotFound();
                }

                _context.Remove(new Cliente() { IdCliente = id });

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
