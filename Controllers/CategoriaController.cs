using AutoMapper;
using LaTiendaAPI.Models;
using LaTiendaAPI.DTOs;
using LaTiendaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaTiendaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly LatiendaContext dbContext;
        private readonly IMapper _mapper;

        public CategoriaController(LatiendaContext _dbContext, IMapper mapper)
        {
            dbContext = _dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                List<Categoria> lista = dbContext.Categorias.ToList();
                return StatusCode(StatusCodes.Status200OK, new { msj = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Categoria objeto)
        {
            try
            {
                dbContext.Categorias.Add(objeto);
                dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, new { msj = "ok", response = objeto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar/{idCategoria:int}")]
        public IActionResult Editar(int idCategoria, [FromBody] Categoria objeto)
        {
            try
            {
                Categoria? oCategoria = dbContext.Categorias.Find(idCategoria);
                if (oCategoria == null)
                    return NotFound(new { msj = "Categoría no encontrada" });

                oCategoria.nombre = objeto.nombre;
                oCategoria.estado = objeto.estado;
                dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idCategoria:int}")]
        public IActionResult Eliminar(int idCategoria)
        {
            try
            {
                Categoria? oCategoria = dbContext.Categorias.Find(idCategoria);
                if (oCategoria == null)
                    return NotFound(new { msj = "Categoría no encontrada" });

                dbContext.Categorias.Remove(oCategoria);
                dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { msj = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msj = ex.Message });
            }
        }
    }
}