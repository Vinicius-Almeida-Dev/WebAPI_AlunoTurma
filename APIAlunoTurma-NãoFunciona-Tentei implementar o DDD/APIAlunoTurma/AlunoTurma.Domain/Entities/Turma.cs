using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AlunoTurma.Domain.Entities;
[Table("Turmas")]
public class Turma
{
    [Key]
    public int TurmaId { get; set; }
    public int cursoId { get; set; }
    public string? nomeTurma { get; set; }
    public int Ano { get; set; }

}
