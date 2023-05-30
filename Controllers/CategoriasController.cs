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
    [Route("api/categorias")]
    [ApiController]
    public class CategoriasController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public CategoriasController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            try
            {
                Categoria categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (categoria == null)
                {
                    return NotFound();
                }

                return categoria;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            try
            {
                List<Categoria> categorias = await _context.Categorias
                    .AsNoTracking()
                    .ToListAsync();

                return categorias;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] Categoria categoria)
        {
            try
            {
                _context.Categorias.Add(categoria);
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
        public async Task<ActionResult> Put(int id, [FromBody] Categoria categoriaParam)
        {
            try
            {
                Categoria categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

                if (categoria == null)
                {
                    return NotFound();
                }

                // inicia checeo del context en almacenamiento local
                var categoriaLocal = _context.Categorias.Local
                    .SingleOrDefault(x => x.Id == id);

                if (categoriaLocal != null)
                    _context.Entry(categoriaLocal).State = EntityState.Detached;

                // termina checeo del context en almacenamiento local

                categoria = categoriaParam;

                _context.Categorias.Update(categoria);

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
                var existe = await _context.Categorias.AnyAsync(x => x.Id == id);

                if (!existe)
                {
                    return NotFound();
                }

                _context.Remove(new Categoria() {Id = id});

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
