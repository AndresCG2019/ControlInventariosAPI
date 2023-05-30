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
    [Route("api/articulos")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public ArticulosController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("{clave}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Articulo>> Get(string clave)
        {
            try
            {
                Articulo articulo = await _context.Articulos.AsNoTracking().FirstOrDefaultAsync(x => x.ClaveArticulo == clave);

                if (articulo == null)
                {
                    return NotFound();
                }

                return articulo;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpGet("movimientos/{clave}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<MovimientoDTO>>> GetMovimientos(string clave)
        {
            try
            {
                Articulo articulo = await _context.Articulos
                    .Include(x => x.Ingresos)
                    .ThenInclude(x => x.Proveedor)
                    .Include(x => x.Egresos)
                    .ThenInclude(x => x.Pedido)
                    .ThenInclude(x => x.Cliente)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ClaveArticulo == clave);

                List<MovimientoDTO> movimientos = new List<MovimientoDTO>();

                foreach (var ingreso in articulo.Ingresos)
                {
                    MovimientoDTO movimiento = new MovimientoDTO() 
                    {
                        ClaveArticulo = ingreso.ClaveArticulo,
                        Fecha = ingreso.FechaIngreso.ToString("dd/MM/yyy"),
                        Monto = ingreso.PrecioCompra,
                        Cantidad = ingreso.Cantidad,
                        NombreProveedor = ingreso.Proveedor.nombre,
                        TipoMovimiento = "INGRESO"
                    };
                    movimientos.Add(movimiento);
                }

                foreach (var egreso in articulo.Egresos)
                {
                    MovimientoDTO movimiento = new MovimientoDTO()
                    {
                        ClaveArticulo = egreso.ClaveArticulo,
                        Fecha = egreso.Pedido.FechaPedido.ToString("dd/MM/yyy"),
                        Monto = egreso.PrecioVenta,
                        Cantidad = egreso.Cantidad,
                        NombreCliente = egreso.Pedido.Cliente.NombreCompleto,
                        TipoMovimiento = "EGRESO"
                    };
                    movimientos.Add(movimiento);
                }

                return movimientos;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<ArticuloDTO>>> Get()
        {
            try
            {
                List<Articulo> articulos = await _context.Articulos
                    .Include(x => x.Categoria)
                    .Include(x => x.Egresos)
                    .Include(x => x.Ingresos)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .ToListAsync();

                List<ArticuloDTO> articulosDTO = new List<ArticuloDTO>();

                foreach (var item in articulos)
                {
                    ArticuloDTO articuloDTO = _mapper.Map<ArticuloDTO>(item);
                    articuloDTO.IdCategoria = item.Categoria.Id;
                    articuloDTO.descripcionCategoria = item.Categoria.Descripcion;
                    articuloDTO.TotalComprado = item.Ingresos.Sum(x => x.Cantidad * x.PrecioCompra);
                    articuloDTO.TotalVendido = item.Egresos.Sum(x => x.Cantidad * x.PrecioVenta);

                    articulosDTO.Add(articuloDTO);
                }

                return articulosDTO;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] Articulo articulo)
        {
            try
            {
                articulo.ClaveArticulo = articulo.ClaveArticulo.ToUpper();

                await _context.Articulos.AddAsync(articulo);

                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpPost("ingresos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostIngreso([FromBody] ArticuloIngreso ingreso)
        {
            try
            {
                ingreso.FechaIngreso = DateTime.Now;

                await _context.ArticulosIngresos.AddAsync(ingreso);

                Articulo articulo = await _context.Articulos.FirstOrDefaultAsync(x => x.ClaveArticulo == ingreso.ClaveArticulo);
                articulo.Existencia = articulo.Existencia + ingreso.Cantidad;

                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpPut("{clave}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(string clave, [FromBody] Articulo articuloParam)
        {
            try
            {
                Articulo articulo = await _context.Articulos.FirstOrDefaultAsync(x => x.ClaveArticulo == clave);

                if (articulo == null)
                {
                    return NotFound();
                }

                // inicia checeo del context en almacenamiento local
                var articuloLocal = _context.Articulos.Local
                    .SingleOrDefault(x => x.ClaveArticulo == clave);

                if (articuloLocal != null)
                    _context.Entry(articuloLocal).State = EntityState.Detached;

                // termina checeo del context en almacenamiento local

                articulo = articuloParam;

                _context.Articulos.Update(articulo);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpDelete("{clave}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(string clave)
        {
            try
            {
                var existe = await _context.Articulos.AnyAsync(x => x.ClaveArticulo == clave);

                if (!existe)
                {
                    return NotFound();
                }

                _context.Remove(new Articulo() { ClaveArticulo = clave });

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
