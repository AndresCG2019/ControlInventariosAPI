using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using ControlInventariosAPI.Entidades;
using Microsoft.EntityFrameworkCore;
using ControlInventariosAPI.DTOs;

namespace ControlInventariosAPI.Controllers
{
    [Route("api/pedidos")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public PedidosController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Pedido>> Get(int id)
        {
            try
            {
                Pedido pedido = await _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(x => x.IdPedido == id);

                if (pedido == null)
                {
                    return NotFound();
                }

                return pedido;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpGet("detalles/{idPedido:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<ArticuloEgresoDTO>>> GetDetalles(int idPedido)
        {
            try
            {
                Pedido pedido = await _context.Pedidos
                    .Include(x => x.Egresos)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.IdPedido == idPedido);

                List<ArticuloEgresoDTO> result = new List<ArticuloEgresoDTO>();

                foreach (var item in pedido.Egresos)
                {
                    ArticuloEgresoDTO dto = new ArticuloEgresoDTO() 
                    {
                        IdEgreso = item.IdEgreso,
                        PrecioVenta = item.PrecioVenta,
                        Cantidad = item.Cantidad,
                        ClaveArticulo = item.ClaveArticulo,
                        IdPedido = item.IdPedido
                    };
                    result.Add(dto);
                }
                return result;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<PedidoDTO>>> Get()
        {
            try
            {
                List<Pedido> pedidos = await _context.Pedidos
                    .Include(x => x.Cliente)
                    .Include(x => x.Egresos)
                    .AsNoTracking()
                    .ToListAsync();

                List<PedidoDTO> pedidoDTOs = new List<PedidoDTO>();

                foreach (var item in pedidos)
                {
                    PedidoDTO dto = _mapper.Map<PedidoDTO>(item);
                    dto.NombreCliente = item.Cliente.NombreCompleto;
                    dto.total = item.Egresos.Sum(x => x.PrecioVenta * x.Cantidad);

                    pedidoDTOs.Add(dto);
                }

                return pedidoDTOs;
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] Pedido pedido)
        {
            try
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpPost("detalles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostDetalle([FromBody] ArticuloEgreso detalle)
        {
            try
            {
                Articulo articulo = await _context.Articulos.FirstOrDefaultAsync(x => x.ClaveArticulo == detalle.ClaveArticulo);

                bool articuloYPedidoDuplicado = await _context.ArticuloEgresos
                    .Where(x => x.ClaveArticulo == detalle.ClaveArticulo && x.IdPedido == detalle.IdPedido)
                    .AnyAsync();

                if (detalle.Cantidad > articulo.Existencia) return BadRequest("Se excedió la existencia del artículo");
                if (articuloYPedidoDuplicado) return BadRequest("No se puede agregar el mismo artículo dos veces en un mismo pedido");
                
                await _context.ArticuloEgresos.AddAsync(detalle);

                articulo.Existencia = articulo.Existencia - detalle.Cantidad;

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
        public async Task<ActionResult> Put(int id, [FromBody] Pedido pedidoParam)
        {
            try
            {
                Pedido pedido = await _context.Pedidos.FirstOrDefaultAsync(x => x.IdPedido == id);

                if (pedido == null)
                {
                    return NotFound();
                }

                // inicia checeo del context en almacenamiento local
                var pedidoLocal = _context.Pedidos.Local
                    .SingleOrDefault(x => x.IdPedido == id);

                if (pedidoLocal != null)
                    _context.Entry(pedidoLocal).State = EntityState.Detached;

                // termina checeo del context en almacenamiento local

                pedido = pedidoParam;

                _context.Pedidos.Update(pedido);

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
                var existe = await _context.Pedidos.AnyAsync(x => x.IdPedido == id);

                if (!existe)
                {
                    return NotFound();
                }

                bool tieneDetalles = _context.ArticuloEgresos.Any(x => x.IdPedido == id);

                if (tieneDetalles) return BadRequest("No se pueden borrar pedidos que aun tengan detalles");

                _context.Remove(new Pedido() { IdPedido = id });

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Algo salio mal...");
            }
        }

        [HttpDelete("detalles/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteDetalle(int id)
        {
            try
            {
                ArticuloEgreso detalle = await _context.ArticuloEgresos.FirstOrDefaultAsync(x => x.IdEgreso == id);

                if (detalle == null)
                {
                    return NotFound();
                }

                Articulo articulo = await _context.Articulos.FirstOrDefaultAsync(x => x.ClaveArticulo == detalle.ClaveArticulo);

                articulo.Existencia = articulo.Existencia + detalle.Cantidad;

                _context.Remove(detalle);

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
