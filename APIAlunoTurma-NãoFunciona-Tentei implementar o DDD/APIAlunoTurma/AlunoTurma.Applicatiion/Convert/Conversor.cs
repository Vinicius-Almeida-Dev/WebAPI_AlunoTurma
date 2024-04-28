using AlunoTurma.Application.Models;
using AlunoTurma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlunoTurma.Application.Convert
{
	public class Conversor
	{
        public List<AlunoModel> ConvertAlunos(IEnumerable<Domain.Entities.Aluno> alunos)
		{
			var listAlunos = new List<AlunoModel>();

			foreach (var aluno in alunos)
			{
				var alunoModel = new AlunoModel
				{
					AlunoId = aluno.AlunoId,
					Nome = aluno.Nome,
					Usuario = aluno.Usuario,
					Senha = aluno.Senha,
				};

				listAlunos.Add(alunoModel);
            }

			return listAlunos;
		} 
	}
}
