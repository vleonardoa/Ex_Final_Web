using Dapper;
    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication2.Entity;
using WebApplication2.Helper;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class paciente : ControllerBase
    {
        private readonly IConfiguration _config;

        public paciente(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        public ActionResult<object> login([FromBody] Persona persona)
        {
            JWTHelper tks = new JWTHelper();
            Persona use = new Persona();
            string Token = tks.CrearToken(persona.Usuario);
            use.Usuario = persona.Usuario;
            use.token = Token;
            return Ok(use);
        }
        [HttpGet]
        [Authorize]
        [Route("GetAllPacientes")]
        public async Task<ActionResult<List<Cliente>>> GetAllPacientes()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Cliente> heroes = await SelectAllPacientes(connection);
            return Ok(heroes);
        }

        [HttpPost]
        [Authorize]
        [Route("SetPaciente")]
        public async Task<ActionResult<List<Cliente>>> SetPaciente(Cliente cliente)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO Pacientes (Temperatura,Pulso,Respiracion,Presion_sanguinea,fecha) Values  (@Temperatura, @Pulso,@Respiracion,@Presion_sanguinea,@fecha) ", cliente);
            return Ok(await SelectAllPacientes(connection));
        }
        private static async Task<IEnumerable<Cliente>> SelectAllPacientes(SqlConnection connection)
        {
            return await connection.QueryAsync<Cliente>("select * from Pacientes");
        }
    }

}

