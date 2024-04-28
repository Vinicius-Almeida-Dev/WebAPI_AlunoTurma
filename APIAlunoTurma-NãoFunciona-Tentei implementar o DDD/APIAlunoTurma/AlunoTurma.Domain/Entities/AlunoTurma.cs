using System.ComponentModel.DataAnnotations;

namespace AlunoTurma.Domain.Entities;

public class AlunoTurma
{
    [Key]
    public int AlunoTurmaId { get; set; }
    public int alunoId { get; set; }
    public int turmaId { get; set; }
}
