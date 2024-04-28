using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper;

namespace AlunoTurma.Domain.Entities;
[Table("Alunos")]
public class Aluno
{
    [Key]
    public int AlunoId { get; set; } // Quando se utiliza o tipo int em uma entidade que será enterada com o Entity FrameWork, ele interpretara o Id como uma chave primaria.
                                     // Tendo duas formas de interpretação, utilizando a nomenclatura da propria classe como no formato acima,
                                     // ou indicando apenas o nome "Id".   
    public string? Nome { get; set; }
    public string? Usuario { get; set; }

    public string? Senha { get; set; }
}
