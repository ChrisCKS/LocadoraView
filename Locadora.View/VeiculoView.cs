using Locadora.Controller;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Inputs;

namespace Locadora.View
{
    public class VeiculoView
    {
        public static void ExibirMenuVeiculos()
        {
            int op;
            bool convertido;

            VeiculoController veiculoController = new VeiculoController();
            do
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("===== Menu de Veículos =====");
                    Console.WriteLine("1. Adicionar Veículo");
                    Console.WriteLine("2. Listar Veículos");
                    Console.WriteLine("3. Buscar Veículo Por Placa");
                    Console.WriteLine("4. Atualizar Status do Veículo");
                    Console.WriteLine("5. Excluir Veículo");
                    Console.WriteLine("0. Voltar ao Menu Principal");
                    Console.WriteLine("=============================\n");
                    Console.Write("Selecione uma opção: ");
                    int.TryParse(Console.ReadLine(), out op);

                    if (op >= 0 && op <= 5)
                    {
                        convertido = true;
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        convertido = false;
                    }
                } while (convertido == false);

                switch (op)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("===== Adicionar Veiculo =====\n");

                        int categoriaID = InputHelper.LerInt("Digite o ID da categoria: ", "ID da categoria inválido");
                        string placa = InputHelper.LerString("Digite a placa do veículo: ", "Placa inválida.");
                        string marca = InputHelper.LerString("Digite a marca do veículo: ", "Marca inválida.");
                        string modelo = InputHelper.LerString("Digite o modelo do veículo: ", "Modelo inválido");
                        int ano = InputHelper.LerInt("Digite o ano do veículo: ", "Ano inválido");
                        string statusVeiculo = InputHelper.LerString("Digite o status do veículo: ", "Status do veículo inválido");

                        Veiculo veiculo = new Veiculo(categoriaID, placa, marca, modelo, ano, statusVeiculo);

                        try
                        {
                            veiculoController.AdicionarVeiculo(veiculo);
                            Console.WriteLine("\nVeiculo adicionado com sucesso!");

                            Console.ReadKey();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Erro ao adicionar veículo. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao cadastrar veículo. " + ex.Message);
                        }

                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("===== Listar Veículos =====\n");

                        try
                        {
                            var listaVeiculos = new List<Veiculo>();

                            listaVeiculos = veiculoController.ListarTodosVeiculos();

                            foreach (var item in listaVeiculos)
                            {
                                Console.WriteLine("=============================");
                                Console.WriteLine(item);
                                Console.WriteLine("=============================\n");
                            }

                            Console.ReadKey();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Erro ao listar veículo. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao listar veículos. " + ex.Message);
                        }

                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("===== Buscar Veículo Por Placa =====\n");
                        try
                        {
                            string placaLida = InputHelper.LerString("Digite a placa do veículo: ", "Placa inválida");

                            Veiculo veiculoLido = veiculoController.BuscarVeiculoPlaca(placaLida);

                            Console.WriteLine("\n===== Veículo Encontrado =====\n");

                            Console.WriteLine(veiculoLido);

                            Console.ReadKey();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Erro ao buscar veículo. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao buscar veículo. " + ex.Message);
                        }
                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("===== Alterar Status Veículo =====");

                        string buscaPlaca = InputHelper.LerString("Digite a placa do veículo: ", "Placa inválida");
                        string statusNovo = InputHelper.LerString("Digite o status do veículo", "Status inválido");

                        try
                        {
                            veiculoController.AtualizarStatusVeiculo(statusNovo, buscaPlaca);

                            Console.WriteLine("Status do veículo alterado com sucesso");

                            Console.ReadKey();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Erro ao atualizar veículo. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao atualizar veículo. " + ex.Message);
                        }
                        break;

                    case 5:
                        Console.Clear();
                        Console.WriteLine("===== Remover Veículo =====");

                        try
                        {
                            string excluirVeiculo = InputHelper.LerString("Digite a placa do veículo", "Placa inválido");

                            veiculoController.DeletarVeiculo(excluirVeiculo);

                            Console.WriteLine("Veículo removido com sucesso!");

                            Console.ReadKey();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Erro ao remover veículo. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao remover veículo. " + ex.Message);
                        }
                        break;

                    case 0:
                        break;

                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }

            } while (op != 0);
        }
    }
}
