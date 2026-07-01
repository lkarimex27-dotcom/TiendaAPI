using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaTiendaAPI.Models;
using LaTiendaAPI.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaTiendaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly LatiendaContext dbContext;
        private readonly IMapper _mapper;

        // Constructor con inyección del Contexto y AutoMapper
        public ProductoController(LatiendaContext _dbContext, IMapper mapper)
        {
            dbContext = _dbContext;
            _mapper = mapper;
        }

        // GET: api/producto/lista
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                lista = dbContext.Productos.Include(c => c.objCategoria).ToList();
                var listaDto = _mapper.Map<List<ProductoDto>>(lista);

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok", response = listaDto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        // GET: api/producto/obtener/5
        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            try
            {
                Producto oProducto = dbContext.Productos.Include(c => c.objCategoria)
                                                       .FirstOrDefault(p => p.idProducto == idProducto);

                if (oProducto == null)
                {
                    return NotFound(new { msj = "Producto No encontrado" });
                }

                var dto = _mapper.Map<ProductoDto>(oProducto);
                return StatusCode(StatusCodes.Status200OK, new { msj = "ok", response = dto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        // POST: api/producto/guardar
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] ProductoCreateDto objetoDto)
        {
            try
            {
                var objeto = _mapper.Map<Producto>(objetoDto);
                dbContext.Productos.Add(objeto);
                dbContext.SaveChanges();

                // Mapeamos el resultado creado para devolverlo como DTO
                var resultadoDto = _mapper.Map<ProductoDto>(objeto);
                return StatusCode(StatusCodes.Status201Created, new { msj = "ok", response = resultadoDto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = $"Error al guardar producto: {ex.Message}" });
            }
        }

        // PUT: api/producto/editar/5
        [HttpPut]
        [Route("Editar/{idProducto:int}")]
        public IActionResult Editar(int idProducto, [FromBody] ProductoCreateDto objetoDto)
        {
            Producto oProducto = dbContext.Productos.Find(idProducto);

            if (oProducto == null)
            {
                return NotFound(new { msj = "Producto no encontrado" });
            }

            try
            {
                // AutoMapper mapea los cambios del DTO directamente sobre el producto existente
                _mapper.Map(objetoDto, oProducto);

                dbContext.Productos.Update(oProducto);
                dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = $"Error al editar producto: {ex.Message}" });
            }
        }

        // DELETE: api/producto/eliminar/5
        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            Producto oProducto = dbContext.Productos.Find(idProducto);

            if (oProducto == null)
            {
                return NotFound(new { msj = "Producto no encontrado" });
            }

            try
            {
                dbContext.Productos.Remove(oProducto);
                dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = $"Error al eliminar producto: {ex.Message}" });
            }
        }
    }
}