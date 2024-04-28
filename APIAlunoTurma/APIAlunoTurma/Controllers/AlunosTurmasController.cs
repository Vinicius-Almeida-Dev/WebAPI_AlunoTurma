using APIAlunoTurma.Models;
using Microsoft.AspNetCore.Mvc;
using APIAlunoTurma.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace APIAluno_TurmaAluno_Turma.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AlunosTurmasController : ControllerBase
    {
        // ATENÇÃO!! Não estou tratando exeções nesse controller com a estrutura TRY.. aqui podemos testar o Middleware implementado com o  Extension Method.

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
        public async Task<ActionResult<IEnumerable<AlunoTurma>>> GetAluno_TurmasAluno_Turmas()
        {

            using (var connection = _connection)
            {
                string[] teste = null;
                if (teste.Length > 0)
                {

                }

                var alunoTurmas = await connection.QueryAsync<AlunoTurma>("SELECT * FROM [AlunosTurmas]");
                if (alunoTurmas.Count() == 0)
                {
                    throw new Exception();
                }

                return Ok(alunoTurmas);
            }


        }

        // Select Where
        [HttpGet("SelecionaAlunoTurma")]
        public async Task<ActionResult<AlunoTurma>> Get([FromQuery] int id)
        {

            using (var connection = _connection)
            {
                var alunoTurma = await connection.QueryAsync<AlunoTurma>($"SELECT * FROM [AlunosTurmas] WHERE AlunoTurmaId = {id}");
                if (alunoTurma.Count() == 0)
                {
                    throw new Exception();
                }
                return Ok(alunoTurma);
            }


        }

        // Insert
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AlunoTurma alunoTurma)
        {


            using (var connection = _connection)
            {
                var trataAlunoTurma = await connection.QueryAsync<AlunoTurma>($"SELECT * FROM [AlunosTurmas] WHERE [alunoId] = {alunoTurma.alunoId} and [turmaId] = {alunoTurma.turmaId}");
                if (trataAlunoTurma.Count() == 1)
                {
                    throw new DadosEmConflito("Aluno já cadastrado a uma turma. Verifique!");
                }

                var existeAluno = await connection.QueryAsync<Aluno>($"SELECT * FROM [Alunos] WHERE [AlunoId] = {alunoTurma.alunoId}");
                var existeTurma = await connection.QueryAsync<Turma>($"SELECT * FROM [Turmas] WHERE [TurmaId] = {alunoTurma.turmaId}");
                if (existeAluno.Count() == 1 && existeTurma.Count() == 1)
                {
                    await connection.QueryAsync<AlunoTurma>($"INSERT INTO [AlunosTurmas] ([alunoId], [turmaId]) VALUES ({alunoTurma.alunoId}, {alunoTurma.turmaId})");


                    var turmaReturnada = await connection.QueryAsync<AlunoTurma>($"SELECT * FROM [AlunosTurmas] " +
                        $"WHERE" +
                        $" [alunoId] = '{alunoTurma.alunoId}' and" +
                        $"[turmaId] = '{alunoTurma.turmaId}'");

                    return Ok(turmaReturnada);

                }
                else
                {
                    throw new DadosEmConflito("Aluno ou turma, não encontrado em nossa base de dados. Verifique!");
                }


            };

        }

        // Update
        [HttpPut]
        public async Task<ActionResult> Put([FromQuery] int id, [FromBody] /*Aqui é o Body la do postman*/AlunoTurma alunoTurma)
        {



            using (var connection = _connection)
            {
                var trataAlunoTurma = await connection.QueryAsync<AlunoTurma>($"SELECT * FROM [AlunosTurmas] WHERE [alunoId] = {alunoTurma.alunoId} and [turmaId] = {alunoTurma.turmaId}");
                if (trataAlunoTurma.Count() == 1)
                {
                    throw new DadosEmConflito("Aluno já cadastrado a uma turma. Verifique!");
                }
                var existeAluno = await connection.QueryAsync<Aluno>($"SELECT * FROM [Alunos] WHERE [Usuario] = '{alunoTurma.alunoId}'");
                var existeTurma = await connection.QueryAsync<Turma>($"SELECT * FROM [Turmas] WHERE [nomeTurma] = '{alunoTurma.turmaId}'");
                if (existeAluno.Count() == 1 && existeTurma.Count() == 1)
                {
                    await connection.QueryAsync<AlunoTurma>($"UPDATE [dbo].[AlunosTurmas] SET [alunoId] = '{alunoTurma.alunoId}', [turmaId] = '{alunoTurma.turmaId}' WHERE AlunoTurmaId = {alunoTurma.AlunoTurmaId}");


                    var turmaReturnada = await connection.QueryAsync<Turma>($"SELECT * FROM [AlunosTurmas] " +
                        $"WHERE" +
                        $" [alunoId] = '{alunoTurma.alunoId}' and" +
                        $"[turmaId] = '{alunoTurma.turmaId}'");

                    return Ok(turmaReturnada);
                }
                else
                {
                    throw new DadosEmConflito("Aluno ou turma, não encontrado em nossa base de dados. Verifique!");
                }
            };


        }

        // Delete
        [HttpDelete]
        public async Task<ActionResult<AlunoTurma>> Delete([FromQuery] int id)
        {

            using (var connection = _connection)
            {
                var existeAlunoTurma = await connection.QueryAsync<AlunoTurma>($"SELECT * FROM [AlunosTurmas] WHERE [AlunoTurmaId] = {id}");
                if (existeAlunoTurma.Count() == 0)
                {
                    throw new Exception();
                }
                await connection.QueryAsync<Aluno>($"DELETE FROM [dbo].[AlunosTurmas] WHERE AlunoId = {id}");

                return Ok(existeAlunoTurma);
            };


        }

    }
}
