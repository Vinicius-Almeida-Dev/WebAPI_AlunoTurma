using AlunoTurma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlunoTurma.Domain.Services
{
	public interface IAlunosServices
	{
        Task<IEnumerable<Aluno>> ObtemListaAlunosAsync();


    }
}
