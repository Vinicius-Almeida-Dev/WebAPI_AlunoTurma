using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using APIAlunoTurma.Exceptions;
using AlunoTurma.Application.Interfaces;
using AlunoTurma.Application.Models;

namespace APIAlunoTurma.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class AlunosController : ControllerBase
    {
        IAlunosApplication _alunosApplication;

        public AlunosController(IAlunosApplication alunosApplication)
        {
            _alunosApplication = alunosApplication;
        }


        // Select not where
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoModel>>> GetAlunosTurmas()
        {
            try
            {
                var response = await _alunosApplication.ObtemAlunosAsync();

                return Ok(response);

            }
            catch (Exception)
            {

                return NotFound("Sem dados na tabela [Alunos].");
            }

        }

    }
}
