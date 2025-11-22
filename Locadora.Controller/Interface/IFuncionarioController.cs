using Locadora.Models;

namespace Locadora.Controller.Interfaces;

public interface IFuncionarioController
{
    public void AdicionarFuncionario(Funcionario funcionario);
    public List<Funcionario> ListarTodosFuncionarios();
    public Funcionario BuscarFuncionarioPorCPF(string cpf);
    public void AtualizarSalario(string cpf, decimal salario);
    public void DeletarFuncionario(string cpf);
}