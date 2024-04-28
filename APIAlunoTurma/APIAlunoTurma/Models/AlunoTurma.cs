using System.ComponentModel.DataAnnotations;

namespace APIAlunoTurma.Models
{
    public class AlunoTurma
    {
        [Key]
        public int AlunoTurmaId { get; set; }
        public int alunoId { get; set; }
        public int turmaId { get; set; }
    }
}
