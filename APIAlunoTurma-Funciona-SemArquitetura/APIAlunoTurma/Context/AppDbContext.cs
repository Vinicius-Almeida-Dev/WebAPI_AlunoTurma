using APIAlunoTurma.Models;
using Microsoft.EntityFrameworkCore;

namespace APIAlunoTurma.Context;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Aluno>? Alunos { get; set; }
    public DbSet<AlunoTurma>? AlunosTurmas { get; set; }
    public DbSet<Turma>? Turmas { get; set; }

}
