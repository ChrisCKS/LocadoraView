using Locadora.View;

namespace Locadora.View;

public class MenuView
{
    public static void ExibirMenuPrincipal()
    {
        int op;
        bool convertido;
        do
        {
            Console.Clear();
            Console.WriteLine("===== Menu Principal =====");
            Console.WriteLine("1. Gerenciar Locações");
            Console.WriteLine("2. Gerenciar Clientes");
            Console.WriteLine("3. Gerenciar Veículos");
            Console.WriteLine("4. Gerenciar Categorias");
            Console.WriteLine("5. Gerenciar Funcionários");
            Console.WriteLine("0. Sair");
            Console.WriteLine("===========================\n");
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
                ExibirMenuLocacao();
                break;

            case 2:
                ClienteView.ExibirMenuClientes();
                break;

            case 3:
                VeiculoView.ExibirMenuVeiculos();
                break;

            case 4:
                CategoriaView.ExibirMenuCategorias();
                break;

            case 5:
                ExibirMenuFuncionarios();
                break;

            case 0:
                Console.WriteLine("Saindo do sistema...");
                break;

            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
}