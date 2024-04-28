using APIAlunoTurma.Models;
using APIAlunoTurma.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using APIAlunoTurma.Exceptions;

namespace APIAlunoTurma.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class AlunosController : ControllerBase
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
        public async Task<ActionResult<IEnumerable<Aluno>>> GetAlunosTurmas()
        {
            try
            {
                using (var connection = _connection)
                {
                    var alunos = await connection.QueryAsync<Aluno>("SELECT * FROM [Alunos]");
                    if (alunos.Count() == 0) 
                    {
                        throw new Exception();
                    }

                    return Ok(alunos);
                }
            }
            catch (Exception)
            {

                return NotFound("Sem dados na tabela [Alunos].");
            }

        }

        // Select Where --- Da para passar o id direto na URI.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Aluno>> Get(int id)
        {
            try
            {
                using (var connection = _connection)
                {
                    var alunos = await connection.QueryAsync<Aluno>($"SELECT * FROM [Alunos] WHERE AlunoId = {id}");
                    if (alunos.Count() == 0)
                    {
                        throw new Exception();
                    }
                    return Ok(alunos);
                }
            }
            catch (Exception)
            {

                return NotFound("Aluno não encontrado.");
            }

        }

        // Insert       
        [HttpPost]
        public async Task<ActionResult> Post(Aluno aluno)
        {
            try
            {
                string padraoSenhaForte = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9$*&@#a-zA-Z$*&@#]{12,}$";

                bool eSenhaForte = Regex.IsMatch(aluno.Senha, padraoSenhaForte);

                if (eSenhaForte)
                {
                    aluno.Senha = ProtegeSenha.SenhaHash(aluno.Senha);
                    var SenhaChar60 = aluno.Senha.ToString().Substring(0, 60);

                    using (var connection = _connection)
                    {
                        var existeAluno = await connection.QueryAsync<Aluno>($"SELECT * FROM [Alunos] WHERE [Usuario] = '{aluno.Usuario}'");
                        if (existeAluno.Count() == 1)
                        {
                            throw new DadosEmConflito("Já este um aluno com este usuário cadastrado");
                        }

                        await connection.QueryAsync<Aluno>($"INSERT INTO [dbo].[Alunos] ([Nome],[Usuario],[Senha]) VALUES ('{aluno.Nome}', '{aluno.Usuario}', '{SenhaChar60}')");
                        

                        var alunoReturn = await connection.QueryAsync<Aluno>($"SELECT * FROM [Alunos] " +
                            $"WHERE" +
                            $" [Nome] = '{aluno.Nome}' and" +
                            $"[Usuario] = '{aluno.Usuario}' and" +
                            $"[Senha] = '{SenhaChar60}'");

                        return Ok(alunoReturn);
                    };
                }
                else
                {
                    throw new DadosEmConflito("Senha fraca!");
                }
               
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
        public async Task<ActionResult> Put([FromQuery] int id, [FromBody]/*Aqui é o Body la do postman*/Aluno aluno)
        {
            try
            {
                if (id != aluno.AlunoId)
                {
                    throw new DadosEmConflito("AlunoId não encontrado. Verifique os Id's!");
                }
                aluno.Senha = ProtegeSenha.SenhaHash(aluno.Senha);
                using (var connection = _connection)
                {
                    var existeAluno = await connection.QueryAsync<Aluno>($"SELECT * FROM [Alunos] WHERE [Usuario] = '{aluno.Usuario}'");
                    if (existeAluno.Count() == 1)
                    {
                        throw new DadosEmConflito("Já existe um aluno com este usuário cadastrado");
                    }
                    await connection.QueryAsync<Aluno>($"UPDATE [dbo].[Alunos] SET [Nome] = '{aluno.Nome}', [Usuario] = '{aluno.Usuario}', [Senha] = '{aluno.Senha}' WHERE AlunoId = {aluno.AlunoId}");
                     
                    return Ok(aluno);
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
        public async Task<ActionResult<Aluno>> Delete([FromQuery] int id)
        {
            try
            {
                using (var connection = _connection)
                {
                    var existeAluno = await connection.QueryAsync<Aluno>($"SELECT * FROM [Alunos] WHERE alunoId = {id}");
                    if (existeAluno.Count() == 0)
                    {
                        throw new Exception();
                    }
                    await connection.QueryAsync<Aluno>($"DELETE FROM [dbo].[Alunos] WHERE AlunoId = {id}");

                    return Ok(existeAluno);
                };
            }
            catch (Exception)
            {

                return NotFound("Aluno não encontrado.");
            }

        }

    }
}
