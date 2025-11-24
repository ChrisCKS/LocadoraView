using Locadora.Controller.Interfaces;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using Utils.Database;

namespace Locadora.Controller;

public class LocacaoController : ILocacaoController
{
    public void AdicionarLocacao(Locacao locacao, string cpf)
    {
        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();

        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            LocacaoFuncionarioController locacaoFuncionarioController = new LocacaoFuncionarioController();
            VeiculoController veiculoController = new VeiculoController();
            FuncionarioController funcionarioController = new FuncionarioController();

            var diaria = veiculoController.BuscarDiariaPorVeiculoID(locacao.VeiculoID);
            if (diaria == 0)
                throw new Exception("Diária do veículo não encontrada.");

            var statusVeiculo = veiculoController.BuscarStatusPorVeiculoID(locacao.VeiculoID);
            if (statusVeiculo != "Disponível")
                throw new Exception("Veículo não está disponível para locação.");


            var dias = (locacao.DataDevolucaoPrevista - locacao.DataLocacao).TotalDays;
            var total = diaria * (decimal)dias;

            try
            {
                var command = new SqlCommand(Locacao.INSERTLOCACAO, connection, transaction);

                command.Parameters.AddWithValue("@ClienteID", locacao.ClienteID);
                command.Parameters.AddWithValue("@VeiculoID", locacao.VeiculoID);
                command.Parameters.AddWithValue("@DataLocacao", locacao.DataLocacao);
                command.Parameters.AddWithValue("@DataDevolucaoPrevista", locacao.DataDevolucaoPrevista);
                command.Parameters.AddWithValue("@DataDevolucaoReal",
                    (object?)locacao.DataDevolucaoReal ?? DBNull.Value);

                command.Parameters.AddWithValue("@ValorDiaria", diaria);
                command.Parameters.AddWithValue("@ValorTotal", total);
                command.Parameters.AddWithValue("@Multa", (object?)locacao.Multa ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", locacao.Status.ToString());

                int locacaoID = Convert.ToInt32(command.ExecuteScalar());
                transaction.Commit();

                var (marca, modelo, placaVeiculo) = veiculoController.BuscarMarcaModeloPorVeiculoID(locacao.VeiculoID);

                veiculoController.AtualizarStatusVeiculo(EStatusVeiculo.Alugado.ToString(), placaVeiculo);

                var funcionario = funcionarioController.BuscarFuncionarioPorCPF(cpf);
                int funcionarioID = funcionario.FuncionarioID;
                locacaoFuncionarioController.Adicionar(locacaoID, funcionarioID);
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw new Exception("Erro inesperado ao adicionar veículo: " + ex.Message);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro inesperado ao adicionar veículo: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public List<Locacao> ListarLocacoes()
    {
        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();


        var veiculoController = new VeiculoController();
        var clienteController = new ClienteController();
        var funcionarioController = new FuncionarioController();
        var locacoes = new List<Locacao>();


        try
        {
            var command = new SqlCommand(Locacao.SELECTLOCACOES, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var diasLocacao = (int)(reader.GetDateTime(7) - reader.GetDateTime(6)).TotalDays;
                var locacao = new Locacao(
                    reader.GetInt32(4),
                    reader.GetInt32(5),
                    reader.GetDateTime(6),
                    diasLocacao
                );

                //TODO:trazer funcionario (posição no select = 0)

                var nomeCliente = clienteController.BuscarNomeClientePorID(reader.GetInt32(4));
                var (marca, modelo, placa) = veiculoController.BuscarMarcaModeloPorVeiculoID(reader.GetInt32(5));

                locacao.SetFuncionario("Teste");
                locacao.SetNomeCliente(nomeCliente);
                locacao.SetMarca(marca);
                locacao.SetModelo(modelo);
                locacao.SetDataDevolucaoPrevista(reader.GetDateTime(7));
                locacao.SetDataDevolucaoReal(reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8));
                locacao.SetValorDiaria(reader.GetDecimal(9));
                locacao.SetValorTotal(reader.GetDecimal(10));
                locacao.SetMulta(reader.GetDecimal(11));
                locacao.SetStatus((EStatusLocacao)Enum.Parse(typeof(EStatusLocacao), reader.GetString(12)));


                locacoes.Add(locacao);
            }

            return locacoes;
        }
        catch (SqlException ex)
        {
            throw new Exception("Erro inesperado ao listar veículos: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao listar veículos: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }


    public Locacao BuscarLocacaoPorID(int locacaoID)
    {
        return null;
    }

    public void AtualizarStatusLocacao(string placa)
    {
        //status cancelado
        /// atualizar valor total real(valor real - valor previsto == multa)
        var veiculoController = new VeiculoController();
        var clienteController = new ClienteController();
        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                var veiculo = veiculoController.BuscarVeiculoPlaca(placa);

                if (veiculo is null)
                    throw new Exception("Veículo não encontrado!");
                var command = new SqlCommand(Locacao.SELECTLOCAOPORVEICULOID, connection, transaction);
                command.Parameters.AddWithValue("@VeiculoID", veiculo.VeiculoID);
                var reader = command.ExecuteReader();
                Locacao locacao = null;
                int diasLocacao = 0;
                while (reader.Read())
                {
                    diasLocacao = (int)(reader.GetDateTime(7) - reader.GetDateTime(6)).TotalDays;
                    locacao = new Locacao(
                        reader.GetInt32(4),
                        reader.GetInt32(5),
                        reader.GetDateTime(6),
                        diasLocacao
                    );
                }

                var nomeCliente = clienteController.BuscarNomeClientePorID(reader.GetInt32(4));
                var (marca, modelo, placaVeiculo) = veiculoController.BuscarMarcaModeloPorVeiculoID(reader.GetInt32(5));

                locacao.SetFuncionario("Teste");
                locacao.SetNomeCliente(nomeCliente);
                locacao.SetMarca(marca);
                locacao.SetModelo(modelo);
                locacao.SetDataDevolucaoPrevista(reader.GetDateTime(7));
                locacao.SetDataDevolucaoReal(reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8));
                locacao.SetValorDiaria(reader.GetDecimal(9));
                locacao.SetValorTotal(reader.GetDecimal(10));
                locacao.SetMulta(locacao.ValorDiaria);

                locacao.SetStatus(EStatusLocacao.Cancelada);

                using (transaction)
                {
                    var commandAtualizarLocacao = new SqlCommand(Locacao.UPDATELOCACAO, connection, transaction);
                    commandAtualizarLocacao.Parameters.AddWithValue("@IdLocacao", locacao.LocacaoID);
                    commandAtualizarLocacao.Parameters.AddWithValue("@Status", locacao.Status);
                    commandAtualizarLocacao.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw new Exception("Erro inesperado ao cancelar locação: " + ex.Message);
            }
        }
    }

    public void EncerrarLocacao(Locacao locacao, string placa)
    {
        //buscar a locacao -> placa do veiculo

        //status cancelado
        /// atualizar valor total real(valor real - valor previsto == multa)
        var veiculoController = new VeiculoController();
        var clienteController = new ClienteController();
        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                var veiculo = veiculoController.BuscarVeiculoPlaca(placa);
                if (veiculo is null)
                    throw new Exception("Veículo não encontrado!");
                var command = new SqlCommand(Locacao.SELECTLOCAOPORVEICULOID, connection, transaction);
                command.Parameters.AddWithValue("@VeiculoID", veiculo.VeiculoID);
                var reader = command.ExecuteReader();
                Locacao locacaoEncontrada = null;
                int diasLocacao = 0;
                while (reader.Read())
                {
                    diasLocacao = (int)(reader.GetDateTime(7) - reader.GetDateTime(6)).TotalDays;
                    locacaoEncontrada = new Locacao(
                        reader.GetInt32(4),
                        reader.GetInt32(5),
                        reader.GetDateTime(6),
                        diasLocacao
                    );
                }

                var nomeCliente = clienteController.BuscarNomeClientePorID(reader.GetInt32(4));
                var (marca, modelo, placaVeiculo) = veiculoController.BuscarMarcaModeloPorVeiculoID(reader.GetInt32(5));

                locacaoEncontrada.SetFuncionario("Teste");
                locacaoEncontrada.SetNomeCliente(nomeCliente);
                locacaoEncontrada.SetMarca(marca);
                locacaoEncontrada.SetModelo(modelo);
                locacaoEncontrada.SetDataDevolucaoPrevista(reader.GetDateTime(7));
                locacaoEncontrada.SetDataDevolucaoReal(DateTime.Now);
                locacaoEncontrada.SetValorDiaria(reader.GetDecimal(9));
                locacaoEncontrada.SetValorTotal(locacaoEncontrada.ValorDiaria * (locacaoEncontrada.DataDevolucaoReal.Value.DayOfYear - locacaoEncontrada.DataDevolucaoPrevista.DayOfYear));
                locacao.SetMulta(reader.GetDecimal(11));

                locacaoEncontrada.SetStatus(EStatusLocacao.Concluida);

                using (transaction)
                {
                    var commandAtualizarLocacao = new SqlCommand(Locacao.UPDATELOCACAO, connection, transaction);
                    commandAtualizarLocacao.Parameters.AddWithValue("@IdLocacao", locacaoEncontrada.LocacaoID);
                    commandAtualizarLocacao.Parameters.AddWithValue("@Status", locacaoEncontrada.Status);
                    commandAtualizarLocacao.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw new Exception("Erro inesperado ao encerrar locação: " + ex.Message);
            }
        }
    }
}