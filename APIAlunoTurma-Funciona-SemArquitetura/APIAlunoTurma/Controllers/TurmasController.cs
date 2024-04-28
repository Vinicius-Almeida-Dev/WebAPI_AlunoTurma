using APIAlunoTurma.Context;
using APIAlunoTurma.Exceptions;
using APIAlunoTurma.Models;
using APIAlunoTurma.Services;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace APITurmaTurma.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TurmasController : ControllerBase
    {        

        public readonly IDbConnection _connection = new SqlConnection(
            @"Server=localhost\SQLEXPRESS;
            Database=APIAlunoTurma;
            User Id=developer;
            Password=@lph@23010;
            Trusted_Connection=False;
            MultipleActiveResultSets=true;
            TrustServerCertificate=True");

        // Select not where
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turma>>> GetTurmasTurmas()
        {
            try
            {
                using (var connection = _connection)
                {
                    var turmas = await connection.QueryAsync<Turma>("SELECT * FROM [Turmas]");
                    if (turmas.Count() == 0)
                    {
                        throw new Exception();
                    }

                    return Ok(turmas);
                }
            }
            catch (Exception)
            {

                return NotFound("Sem dados na tabela [Turmas].");
            }

        }

        // Select Where
        [HttpGet("SelecioneUmaTrma")]
        public async Task<ActionResult<Turma>> Get([FromQuery] int id)
        {
            try
            {
                using (var connection = _connection)
                {
                    var turma = await connection.QueryAsync<Turma>($"SELECT * FROM [Turmas] WHERE TurmaId = {id}");
                    if (turma.Count() == 0)
                    {
                        throw new Exception();
                    }
                    return Ok(turma);
                }
            }
            catch (Exception)
            {

                return NotFound("Turma não encontrada.");
            }
        }

        // Insert
        [HttpPost]
        public async Task<ActionResult<Turma>> Post([FromBody] Turma turma)
        {
            try
            {
                var anoAgr = DateTime.Now.Year;

                if (turma.Ano.ToString().Length != 4 || turma.Ano < anoAgr)
                {
                    // Com mais tempo, eu poderia criar exceções específicas para cada verbo HTTP com detalhe, mencionando qual parâmetro está inconsistente.
                    throw new DadosEmConflito("O ano deve conter quatro caracteres e ser igual ou superior ao ano atual.");
                }

                using (var connection = _connection)
                {
                    var existeTurma = await connection.QueryAsync<Turma>($"SELECT * FROM [Turmas] WHERE [nomeTurma] = '{turma.nomeTurma}'");
                    if (existeTurma.Count() == 1)
                    {
                        throw new DadosEmConflito("Nome de turma já cadastrado. Verifique!");
                    }

                    await connection.QueryAsync<Turma>($"INSERT INTO [Turmas] ([cursoId],[nomeTurma],[Ano]) VALUES ({turma.cursoId}, '{turma.nomeTurma}', {turma.Ano})");


                    var turmaReturnada = await connection.QueryAsync<Turma>($"SELECT * FROM [Turmas] " +
                        $"WHERE" +
                        $" [cursoId] = '{turma.cursoId}' and" +
                        $"[nomeTurma] = '{turma.nomeTurma}' and" +
                        $"[Ano] = '{turma.Ano}'");

                    return Ok(turmaReturnada);
                };
            }
            catch (DadosEmConflito e)
            {

                return Conflict(e.Message);
            }
            catch (Exception)
            {

                return Conflict("Dados de atualização inconsistentes. Verifique!");
            }


        }

        // Update
        [HttpPut]
        public async Task<ActionResult> Put([FromQuery] int id, [FromBody] /*Aqui é o Body la do postman*/Turma turma)
        {
            try
            {
                if (id != turma.TurmaId)
                {
                    throw new DadosEmConflito("TurmaId não encontrada. Verifique os Id's!");
                }

                var anoAgr = DateTime.Now.Year;

                if (turma.Ano.ToString().Length != 4 || turma.Ano < anoAgr)
                {
                    throw new DadosEmConflito("O ano deve conter quatro caracteres e ser igual ou superior ao ano atual.");
                }

                using (var connection = _connection)
                {
                    var trataTurma = await connection.QueryAsync<Turma>($"SELECT * FROM [Turmas] WHERE [nomeTurma] = '{turma.nomeTurma}'");
                    if (trataTurma.Count() == 1)
                    {
                        throw new DadosEmConflito("Nome de turma já cadastrado. Verifique!");
                    }

                    await connection.QueryAsync<Turma>($"UPDATE [dbo].[Turmas] SET [cursoId] = '{turma.cursoId}', [nomeTurma] = '{turma.nomeTurma}', [Ano] = '{turma.Ano}' WHERE TurmaId = {turma.TurmaId}");


                    var turmaReturnada = await connection.QueryAsync<Turma>($"SELECT * FROM [Turmas] " +
                        $"WHERE" +
                        $" [cursoId] = '{turma.cursoId}' and" +
                        $"[nomeTurma] = '{turma.nomeTurma}' and" +
                        $"[Ano] = '{turma.Ano}'");

                    return Ok(turmaReturnada);
                };
            }
            catch (DadosEmConflito e)
            {

                return Conflict(e.Message);
            }
            catch (Exception)
            {

                return Conflict("Dados de atualização inconsistentes. Verifique!");
            }

        }

        // Delete
        [HttpDelete]
        public async Task<ActionResult<Turma>> Delete([FromQuery] int id)
        {
            try
            {
                using (var connection = _connection)
                {
                    var existeTurma = await connection.QueryAsync<Turma>($"SELECT * FROM [Turmas] WHERE TurmaId = {id}");
                    if (existeTurma.Count() == 0)
                    {
                        throw new Exception();
                    }
                    await connection.QueryAsync<Turma>($"DELETE FROM [dbo].[Turmas] WHERE TurmaId = {id}");

                    return Ok(existeTurma);
                };
            }
            catch (Exception)
            {

                return NotFound("Turma não encontrada.");
            }

        }

    }
}
