using Locadora.Models;


namespace Locadora.Controller.Interface
{
    public interface IClienteController
    {
        public void AdicionarCliente(Cliente cliente, Documento documento);

        public List<Cliente> ListarTodosCliente();

        public Cliente BuscaClientePorEmail(string email);

        public string BuscarNomeClientePorID(int clienteID);

        public void AtualizarTelefoneCliente(string telefone, string email);

        public void AtualizarDocumentoCliente(Documento documento, string email);

        public void DeletarCliente(string email);
    }
}
