using apiSolicitudes.Data;
using apiSolicitudes.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiSolicitudes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolicitudController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SolicitudController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Listar")]
        public async Task<IActionResult> ListarSolicitudes([FromBody] ListaRequest request)
        {
            var solicitudes = new List<ListaResponse>();
            if (request.Rol == "cliente")
            {

                solicitudes = await (from sol in _context.Solicitudes
                                     join user in _context.Usuarios
                                     on sol.ClienteId equals user.UsuarioId
                                     where user.NombreUsuario.Equals(request.Usuario)
                                     && (sol.Estado == request.Estado || string.IsNullOrEmpty(request.Estado))
                                     && (sol.FechaIngreso == request.FechaIngreso || string.IsNullOrEmpty(request.FechaIngreso))
                                     select new ListaResponse()
                                     {
                                         SolicitudId = sol.SolicitudId,
                                         ClienteId = sol.ClienteId,
                                         Cliente = user.NombreUsuario,
                                         Tipo = sol.Tipo,
                                         Estado = sol.Estado,
                                         FechaIngreso = sol.FechaIngreso
                                     }).ToListAsync();
            }
            else
            {
                solicitudes = await (from sol in _context.Solicitudes
                                     join user in _context.Usuarios
                                     on sol.ClienteId equals user.UsuarioId
                                     where (sol.ClienteId == request.ClienteId || request.ClienteId.Equals(0))
                                     && (sol.FechaIngreso == request.FechaIngreso || string.IsNullOrEmpty(request.FechaIngreso))
                                     select new ListaResponse()
                                     {
                                         SolicitudId = sol.SolicitudId,
                                         ClienteId = sol.ClienteId,
                                         Cliente = user.NombreUsuario,
                                         Tipo = sol.Tipo,
                                         Estado = sol.Estado,
                                         FechaIngreso = sol.FechaIngreso
                                     }).ToListAsync();
            }
            return Ok(new { CodError = "0000", MensajeError = "Solicitudes listadas", Data = solicitudes });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ConsultarSolicitud(int id)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound(new { CodError = "0001", MensajeError = "Solicitud no encontrada", Data = "" });
            }
            return Ok(new { CodError = "0000", MensajeError = "Solicitud encontrada", Data = solicitud });
        }

        [HttpPost("Ingresar")]
        public async Task<IActionResult> InsertarSolicitud([FromBody] SolicitudRequest request)
        {
            var solicitud = new Solicitud();
            solicitud.Tipo = request.Tipo;
            solicitud.Descripcion = request.Descripcion;
            solicitud.Justificativo = request.Justificativo;
            solicitud.FechaIngreso = DateTime.Now.ToString();
            solicitud.Estado = "INGRESADO";

            _context.Solicitudes.Add(solicitud);
            await _context.SaveChangesAsync();

            return Ok(new { CodError = "0000", MensajeError = "Solicitud Ingresada Correctamente", Data = "" });
        }

        [HttpPost("Actualizar/{id}")]
        public async Task<IActionResult> ActualizarSolicitud(int id, [FromBody] SolicitudRequest request)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);

            solicitud.Tipo = request.Tipo;
            solicitud.Descripcion = request.Descripcion;
            solicitud.Justificativo = request.Justificativo;
            solicitud.FechaActualizacion = DateTime.Now.ToString();
            _context.Update(solicitud);
            await _context.SaveChangesAsync();

            return Ok(new { CodError = "0000", MensajeError = "Solicitud Actualizada Correctamente", Data = "" });
        }

        [HttpPost("Enviar/{id}")]
        public async Task<IActionResult> EnviarSolicitud(int id)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);

            solicitud.Estado = "EN CURSO";
            _context.Update(solicitud);
            await _context.SaveChangesAsync();

            return Ok(new { CodError = "0000", MensajeError = "Solicitud Enviada Correctamente", Data = "" });
        }

        [HttpPost("Cerrar/{id}")]
        public async Task<IActionResult> CerrarSolicitud(int id, [FromBody] CerrarRequest request)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);

            solicitud.Estado = request.Estado;
            solicitud.DetalleGestion = request.DetalleGestion;
            solicitud.FechaGestion = DateTime.Now.ToString();
            _context.Update(solicitud);
            await _context.SaveChangesAsync();

            return Ok(new { CodError = "0000", MensajeError = "Solicitud Cerrada Correctamente", Data = "" });
        }
    }

    public class ListaRequest
    {
        public string Usuario { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
        public string FechaIngreso { get; set; }
        public int ClienteId { get; set; }
    }

    public class SolicitudRequest
    {
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Justificativo { get; set; }
    }

    public class ListaResponse
    {
        public int SolicitudId { get; set; }
        public int ClienteId { get; set; }
        public string Cliente { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public string FechaIngreso { get; set; }
    }

    public class CerrarRequest
    {
        public string Estado { get; set; }
        public string DetalleGestion { get; set; }
    }
}
