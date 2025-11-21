using Locadora.Controller;
using Locadora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Inputs;

namespace Locadora.View
{
    public class ClienteView
    {
        public readonly ClienteController controller = new ClienteController();
        public void AdicionarCliente() 
        {
            Console.WriteLine("=== Adicionar Cliente ===");

            string nome = InputHelper.LerString("Digite o nome do cliente: ", "Nome inválido.");
            string email = InputHelper.LerString("Digite o email do cliente: ", "Email inválido.");
            string telefone = InputHelper.LerString("Digite o telefone do cliente (opcional): ", "Telefone Invalido", "");
            string tipoDoc = InputHelper.LerString("Digite o tipo de documento: ", "Informe um tipo válido!");
            string numero = InputHelper.LerString("Digite o numero do Documento: ", "Número inválido!");
            DateOnly emissao = InputHelper.LerData("Digite a Data de Emissão (dd/MM/yyyy): ", "Data inválida!");
            DateOnly validade = InputHelper.LerData("Digite a data de Validade (dd/MM/yyyy): ", "Data inválida!");


            var cliente = new Cliente(nome, email, telefone);
            var documento = new Documento(tipoDoc, numero, emissao, validade);
            controller.AdicionarCliente(cliente, documento);
        }

        public void ListarClientes()
        {
            Console.WriteLine("=== Lista de Clientes ===\n");

            var lista = controller.ListarTodosCliente();

            foreach (var c in lista)
            {
                Console.WriteLine($"Nome: {c.Nome}");
                Console.WriteLine($"Email: {c.Email}");
                Console.WriteLine($"Telefone: {c.Telefone}");
                Console.WriteLine($"Documento: {c.Documento.TipoDocumento} - {c.Documento.Numero}");
            }
        }
        public void BuscarPorEmail()
        {
            Console.WriteLine("=== Buscar Cliente por Email ===");

            string email = InputHelper.LerString("Email: ", "Email inválido!");

            var cliente = controller.BuscaClientePorEmail(email);

            if (cliente is null)
                Console.WriteLine("Cliente não encontrado!");
            else
                Console.WriteLine(cliente);

        }

        public void AtualizarDocumento()
        {
            Console.WriteLine("=== Atualizar Documento ===");

            string email = InputHelper.LerString("Email do Cliente: ", "Email inválido!");
            string tipo = InputHelper.LerString("Novo Tipo Documento: ", "Tipo inválido!");
            string numero = InputHelper.LerString("Número: ", "Número inválido!");
            DateOnly emissao = InputHelper.LerData("Emissão: ", "Data inválida!");
            DateOnly validade = InputHelper.LerData("Validade: ", "Data inválida!");

            var documento = new Documento(tipo, numero, emissao, validade);

            controller.AtualizarDocumentoCliente(documento, email);

            Console.WriteLine("Documento atualizado!");
        }

        public void DeletarCliente()
        {
            Console.Clear();
            Console.WriteLine("=== Deletar Cliente ===");

            string email = InputHelper.LerString("Email do Cliente: ", "Email inválido!");

            controller.DeletarCliente(email);

            Console.WriteLine("Cliente deletado com sucesso!");
            Console.ReadKey();
        }
    }
}
