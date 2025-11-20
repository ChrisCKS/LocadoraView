using Locadora.Models;
using Locadora.Controller;
using Microsoft.Data.SqlClient;
using Utils.Database;

Cliente cliente = new Cliente("Novo Cliete documento", "x@email.com", "123456");
Documento documento = new Documento("CPF", "987654321", new DateOnly(2023, 01, 01), new DateOnly(2035, 01, 01));

//Console.WriteLine(cliente);

var clienteController = new ClienteController();


try
{
    clienteController.AtualizarDocumentoCliente(documento, "x@email.com");

    Console.WriteLine(clienteController.BuscaClientePorEmail("novoemail@2.com"));

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

/*try
{
    clienteController.AdicionarCliente(cliente, documento);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}*/



/*try
{
    var listaClientes = clienteController.ListarTodosCliente();

    foreach (var clientedaLista in listaClientes) 
    {
        Console.WriteLine(clientedaLista);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}*/

/*clienteController.AdicionarCliente(cliente);

var listadeClientes = clienteController.ListarTodosCliente();

foreach (var clientedaLista in listadeClientes)
{
    Console.WriteLine(clientedaLista);
}*/


//clienteController.DeletarCliente(" tr@tr.com");

//clienteController.AtualizarTelefoneCliente("9998888", "x@x.com");
//Console.WriteLine(clienteController.BuscaClientePorEmail("x@x.com"));


//fazendo uma tratativa de erro para o try em AdicionarCliente
/*try
{
    clienteController.AdicionarCliente(cliente);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}*/



/*Cliente cliente = new Cliente("Christian" , "c@c.com");
//Documento documento = new Documento(1, "RG", "123456789", new DateOnly(2020, 01, 01), new DateOnly(2030, 01, 01));

Console.WriteLine(cliente);
//Console.WriteLine(documento);

var connection = new SqlConnection(ConnectionDB.GetConnectionString());

connection.Open();

SqlCommand command = new SqlCommand(Cliente.INSERTCLIENTE, connection);

command.Parameters.AddWithValue("@Nome", cliente.Nome);
command.Parameters.AddWithValue("@Email", cliente.Email); //if     //else
command.Parameters.AddWithValue("@Telefone", cliente.Telefone ?? (object)DBNull.Value);

cliente.setClienteID(Convert.ToInt32(command.ExecuteScalar())); 

Console.ReadLine();

connection.Close();*/

/*outra opção*/
//int clienteId = (Convert.ToInt32(command.ExecuteScalar());
//cliente.SetClienteID(clienteId)