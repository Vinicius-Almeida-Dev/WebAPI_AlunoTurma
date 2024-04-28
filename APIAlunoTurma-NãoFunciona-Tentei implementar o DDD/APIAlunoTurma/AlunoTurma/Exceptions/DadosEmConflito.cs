namespace APIAlunoTurma.Exceptions
{
    public class DadosEmConflito : ApplicationException
    {
        public DadosEmConflito(string? message) : base(message)
        {
        }
    }
}
