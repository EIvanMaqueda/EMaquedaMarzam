using System.Web.Http;

namespace SL.Controllers
{
    public class UsuarioController : ApiController
    {
        // GET: Usuario
        [HttpGet]
        [Route("Api/Usuario/Login/{Nombre}/{Password}")]
        public IHttpActionResult Login(string Nombre, string Password)
        {
            ML.Result result=BL.Usuario.Login(Nombre, Password);
            if (result.Correct)
            {
                return Ok(result);
            }
            else { 
            
                return NotFound();
            }
            
        }
    }
}