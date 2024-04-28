using AlunoTurma.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlunoTurma.Application.Interfaces
{
	public interface IAlunosApplication
	{
        Task<List<AlunoModel>> ObtemAlunosAsync();

    }
}
