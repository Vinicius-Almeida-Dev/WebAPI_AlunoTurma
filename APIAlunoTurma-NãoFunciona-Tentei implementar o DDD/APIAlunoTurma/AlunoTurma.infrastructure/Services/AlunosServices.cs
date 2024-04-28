using AlunoTurma.Domain.Entities;
using AlunoTurma.Domain.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AlunoTurma.Infrastructure.Services
{
	public class AlunosServices : IAlunosServices
    {
        public readonly IDbConnection _connection = new SqlConnection(
        @"Server=localhost\SQLEXPRESS;
                Database=APIAlunoTurma;
                User Id=developer;
                Password=@lph@23010;
                Trusted_Connection=False;
                MultipleActiveResultSets=true;
                TrustServerCertificate=True");

        public async Task<IEnumerable<Aluno>> ObtemListaAlunosAsync()
        {
            using (var connection = _connection)
            {
                var alunos = await connection.QueryAsync<Aluno>("SELECT * FROM [Alunos]");
                if (alunos.Count() == 0)
                {
                    throw new Exception();
                }

                return alunos;
            }
        }
    }
}
