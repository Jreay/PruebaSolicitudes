using apiSolicitudes.Data;
using Microsoft.AspNetCore.Mvc;

namespace apiSolicitudes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.NombreUsuario == request.Usuario && u.Clave == request.Clave);

            if (usuario == null)
            {
                return Unauthorized(new { IsValid = false, rol = string.Empty });
            }

            var rol = usuario.EsAdministrador ? "administrador" : "cliente";
            return Ok(new { IsValid = true, rol });
        }
    }

    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }

    }


}
