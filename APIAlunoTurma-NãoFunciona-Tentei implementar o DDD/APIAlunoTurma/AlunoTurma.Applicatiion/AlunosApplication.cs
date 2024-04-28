using AlunoTurma.Application.Convert;
using AlunoTurma.Application.Interfaces;
using AlunoTurma.Application.Models;
using AlunoTurma.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlunoTurma.Application
{
	public class AlunosApplication : IAlunosApplication
    {
        IAlunosServices _alunosServices;

        public AlunosApplication(IAlunosServices alunosServices)
        {
            _alunosServices = alunosServices;
        }

        public async Task<List<AlunoModel>> ObtemAlunosAsync()
		{
            var conversor = new Conversor();

            var alunos = await _alunosServices.ObtemListaAlunosAsync();

            return conversor.ConvertAlunos(alunos);
        }
	}
}
