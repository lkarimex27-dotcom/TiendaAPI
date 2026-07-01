using LaTiendaAPI.DTOs;
using LaTiendaAPI.DTOs.Auth;
using LaTiendaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LaTiendaAPI.Helpers;
using LaTiendaAPI.Services;



namespace LaTiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LatiendaContext _context;
        private readonly JwtService _jwtService; // Servicio encargado de generar token

        public AuthController(LatiendaContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            if (!PasswordHelper.VerifyPassword(request.Password, usuario.PasswordHash, usuario.PasswordSalt))
                return Unauthorized("Contraseña incorrecta");

            var roles = usuario.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList();
            var token = _jwtService.GenerateToken(usuario, roles);

            return new LoginResponseDto
            {
                Token = token,
                Nombre = usuario.Nombre,
                Roles = roles
            };
        }


        // Agregar el endpoint en AuthController para registrar
        [HttpPost("Registrar")]
        public async Task<ActionResult> Registrar(RegisterRequestDto request)
        {
            try
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
                    return BadRequest(new { msj = "El email ya está registrado" });

                PasswordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var usuario = new Usuario
                {
                    TipoDoc = request.TipoDoc,
                    NroDoc = request.NroDoc,
                    Nombre = request.Nombre,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                foreach (var rolId in request.Roles)
                {
                    usuario.UsuarioRoles.Add(new UsuarioRole
                    {
                        RolId = rolId,
                        Usuario = usuario
                    });
                }

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Ok(new { msj = "Usuario registrado correctamente" });
            }
            catch (Exception ex)
            {
                // TEMPORAL: solo para depurar, esto expone el error real.
                // Quítalo (vuelve a {msj="Error al registrar"} sin el detalle) antes de entregar el proyecto.
                return StatusCode(500, new { msj = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }

}