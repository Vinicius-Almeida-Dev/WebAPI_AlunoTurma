using AlunoTurma.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlunoTurma.Infrastructure.Context;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Aluno>? Alunos { get; set; }
    public DbSet<AlunoTurma.Domain.Entities.AlunoTurma>? AlunosTurmas { get; set; }
    public DbSet<Turma>? Turmas { get; set; }

}
