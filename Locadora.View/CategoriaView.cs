using Locadora.Controller;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Inputs;

namespace Locadora.View
{
    public class CategoriaView
    {
        public static void ExibirMenuCategorias()
        {
            int op;
            bool convertido;

            CategoriaController categoriaController = new CategoriaController();
            do
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("===== Menu de Categorias =====");
                    Console.WriteLine("1. Adicionar Categoria");
                    Console.WriteLine("2. Listar Categorias");
                    Console.WriteLine("3. Buscar Categoria Individualmente");
                    Console.WriteLine("4. Atualizar Diária da Categoria");
                    Console.WriteLine("0. Voltar ao Menu Principal");
                    Console.WriteLine("=============================\n");
                    Console.Write("Selecione uma opção: ");
                    int.TryParse(Console.ReadLine(), out op);

                    if (op >= 0 && op <= 4)
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
                        Console.WriteLine("===== Adicionar Categoria =====\n");

                        string nome = InputHelper.LerString("Digite o nome da categoria: ", "Nome inválido.");
                        decimal diaria = InputHelper.LerDecimal("Digite o valor da diária da categoria: ", "Valor da diária inválida.");
                        string? descricao = InputHelper.LerStringNulo("Digite a descrição da categoria (opcional): ", "Descrição inválida", true);

                        Categoria categoria = new Categoria(nome, descricao, diaria);
                        try
                        {
                            categoriaController.AdicionarCategoria(categoria);

                            Console.WriteLine("\nCategoria adicionada com sucesso!");
                            Console.ReadKey();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Erro inesperado ao adicionar categoria. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao adicionar categoria. " + ex.Message);
                        }
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("===== Listar Categorias =====\n");

                        try
                        {
                            var listaCategorias = new List<Categoria>();

                            listaCategorias = categoriaController.ListarTodasCategorias();

                            foreach (var item in listaCategorias)
                            {
                                Console.WriteLine("=============================");
                                Console.WriteLine(item);
                                Console.WriteLine("=============================\n");
                            }
                            Console.ReadKey();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Erro inesperado ao listar categoria. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao listar categorias. " + ex.Message);
                        }
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("===== Buscar Categoria =====\n");

                        string nomeLido = InputHelper.LerString("Digite o nome da categoria: ", "Nome inválido.");
                        try
                        {
                            Categoria categoriaLida = categoriaController.BuscarCategoriaPorNome(nomeLido);

                            Console.WriteLine("\n===== Categoria Encontrada =====\n");
                            Console.WriteLine(categoriaLida);
                            Console.ReadKey();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Erro inesperado ao buscar categoria. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao listar categorias. " + ex.Message);
                        }
                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("===== Alterar Diária Categoria =====");

                        string buscaNome = InputHelper.LerString("Digite o nome da categoria: ", "Nome inválido.");

                        string? alterarDescricao = InputHelper.LerString("Digite a descrição atualizada: ", "Descrição inválida");

                        decimal alterarDiaria = InputHelper.LerDecimal("Digite o valor da diária atualizada: ", "Valor da diária inválido");

                        Categoria atualizarCategoria = new Categoria(buscaNome, alterarDescricao, alterarDiaria);

                        try
                        {
                            categoriaController.AtualizarCategoria(buscaNome, atualizarCategoria);

                            Console.WriteLine("Diária da categoria alterado com sucesso");

                            Console.ReadKey();
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception("Erro inesperado ao alterar categoria. " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao alterar categoria. " + ex.Message);
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
