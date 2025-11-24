using Locadora.Controller;
using Locadora.Models;
using Microsoft.Data.SqlClient;

using Utils.Inputs;

namespace Locadora.View
{
    public class FuncionarioView
    {
        FuncionarioController funcionarioController = new FuncionarioController();

        public static void ExibirMenuFuncionarios()
        {
            FuncionarioController funcionarioController = new FuncionarioController();

            int op;
            bool convertido;

            do
            {
                Console.Clear();
                Console.WriteLine("===== Menu de Funcionários =====");
                Console.WriteLine("1. Adicionar Funcionário");
                Console.WriteLine("2. Listar Funcionários");
                Console.WriteLine("3. Buscar Funcionário Por CPF");
                Console.WriteLine("4. Atualizar Salário de Funcionário");
                Console.WriteLine("5. Excluir Funcionário");

                int.TryParse(Console.ReadLine(), out op);

                convertido = (op >= 0 && op <= 5);

                if (!convertido)
                {
                    Console.WriteLine("Opção inválida. Tente novamente.");
                }

            } while (!convertido);

            switch (op)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("===== Adicionar Funcionário =====\n");

                    string nome = InputHelper.LerString("Digite o nome do funcionário: ", "Nome inválido.");
                    string cpf = InputHelper.LerString("Digite o CPF do funcionário: ", "CPF inválido.");
                    string cargo = InputHelper.LerString("Digite o cargo do funcionário: ", "Cargo inválido.");
                    decimal salario = InputHelper.LerDecimal("Digite o salário do funcionário: ", "Salário inválido.");

                    Funcionario novoFuncionario = new Funcionario(nome, cpf, cargo, salario);

                    try
                    {
                        funcionarioController.AdicionarFuncionario(novoFuncionario);
                        Console.WriteLine("\nFuncionário adicionado com sucesso!");
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Erro ao adicionar funcionário. " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro inesperado ao adicionar funcionário. " + ex.Message);
                    }
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("===== Listar Funcionários =====\n");

                    try
                    {
                        var listaFuncionarios = funcionarioController.ListarTodosFuncionarios();

                        foreach (var f in listaFuncionarios)
                        {
                            Console.WriteLine(f);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao listar funcionários. " + ex.Message);
                    }
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("===== Buscar Funcionário Por CPF =====\n");

                    try
                    {
                        string buscaCpf = InputHelper.LerString("Digite o CPF: ", "CPF inválido.");

                        Funcionario funcionario = funcionarioController.BuscarFuncionarioPorCPF(buscaCpf);

                        Console.WriteLine("\n===== Funcionário Encontrado =====\n");
                        Console.WriteLine(funcionario);
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Erro ao buscar funcionário. " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro inesperado ao buscar funcionário. " + ex.Message);
                    }
                    break;

                case 4:
                    Console.Clear();
                    Console.WriteLine("===== Alterar Salário Funcionário =====\n");

                    string cpfFuncionario = InputHelper.LerString("Digite o CPF do funcionário: ", "CPF inválido.");
                    decimal novoSalario = InputHelper.LerDecimal("Digite o novo salário: ", "Valor inválido.");

                    try
                    {
                        funcionarioController.AtualizarSalario(cpfFuncionario, novoSalario);
                        Console.WriteLine("Salário do funcionário alterado com sucesso!");
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Erro ao atualizar salário. " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao atualizar salário. " + ex.Message);
                    }
                    break;

                case 5:
                    Console.Clear();
                    Console.WriteLine("===== Remover Funcionário =====\n");

                    try
                    {
                        string deletaCpf = InputHelper.LerString("Digite o CPF do funcionário: ", "CPF inválido.");

                        funcionarioController.DeletarFuncionario(deletaCpf);

                        Console.WriteLine("Funcionário removido com sucesso!");
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Erro ao excluir funcionário. " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao excluir funcionário. " + ex.Message);
                    }
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }
}
