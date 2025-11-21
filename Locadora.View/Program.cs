using Locadora.Models;
using Locadora.Controller;
using Microsoft.Data.SqlClient;
using Utils.Database;
using Locadora.Models.Enums;

var veiculoController = new VeiculoController();
try 
{
    //var veiculo = new Veiculo(1, "XYZ-1234", "Toyota", "Corolla", 2020, EStatusVeiculo.Disponivel.ToString());
    //veiculoController.AdicionarVeiculo(veiculo);
    var veiculos = veiculoController.ListarTodosVeiculos();

    foreach (var item in veiculos)
    {
        Console.WriteLine(item);
    }
}
catch (Exception ex)
{
    Console.WriteLine("Erro" + ex.Message);
}

//Console.WriteLine(veiculoController.BuscarVeiculoPlaca("XYZ-1234"));

//var veiculo = veiculoController.BuscarVeiculoPlaca("XYZ-1234");

//veiculoController.DeletarVeiculo(veiculo.VeiculoId);

veiculoController.AtualizarStatusVeiculo(EStatusVeiculo.Manutencao.ToString(), "XYZ-1234");
Console.WriteLine(veiculoController.BuscarVeiculoPlaca("XYZ-1234"));
























/*CATEGORIA*/

//Categoria categoria = new Categoria("Grupo H", 89.00m );

//var categoriaController = new CategoriaController();

#region AdicionarCategoria
/*try 
{
    categoriaController.AdicionarCategoria(categoria);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}*/
#endregion

#region SELECTALLCATEGORIAS
/*try 
{
    var listaCategorias = categoriaController.ListarTodasCategorias();

    foreach (var categoriaDaLista in listaCategorias)
    {
        Console.WriteLine(categoriaDaLista);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}*/
#endregion

#region UPDATECATEGORIA
/*try 
{
    categoriaController.AtualizarCategoria("Grupoc C", 95.00m, "Carro Economico Atualizado");
    Console.WriteLine(categoriaController.BuscarCategoriaPorNome("Grupoc C"));
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}*/
#endregion

#region DELETECATEGORIA
/*try 
{
    categoriaController.DeletarCategoria("Grupo H");
    Console.WriteLine("Categoria deletada com sucesso.");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}*/
#endregion






/*CLIENTE*/


/*Cliente cliente = new Cliente("Novo Cliete documento", "x@email.com", "123456");
Documento documento = new Documento("CPF", "987654321", new DateOnly(2023, 01, 01), new DateOnly(2035, 01, 01));*/

//Console.WriteLine(cliente);

/*var clienteController = new ClienteController();


try
{
    clienteController.AtualizarDocumentoCliente(documento, "x@email.com");

    Console.WriteLine(clienteController.BuscaClientePorEmail("novoemail@2.com"));

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}*/

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
