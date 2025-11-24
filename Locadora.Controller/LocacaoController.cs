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
        var locacaoFuncionarioController = new LocacaoFuncionarioController();
        var locacoes = new List<Locacao>();

        try
        {
            var command = new SqlCommand(Locacao.SELECTLOCACOES, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var diasLocacao = (int)(reader.GetDateTime(8) - reader.GetDateTime(7)).TotalDays;
                var locacao = new Locacao(
                    reader.GetInt32(4),
                    reader.GetInt32(5),
                    reader.GetDateTime(7),
                    diasLocacao
                );

                var listaFuncionario = locacaoFuncionarioController.BuscarFuncionariosPorLocacao(reader.GetInt32(6));
                string funcionario1 = listaFuncionario[0];
                string funcionario2 = listaFuncionario.Count > 1 ? listaFuncionario[1] :  null;

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

    public void CancelarLocacao(int locacaoID)
    {
        var veiculoController = new VeiculoController();
        var clienteController = new ClienteController();
        var locacaoFuncionarioController = new LocacaoFuncionarioController();

        var connection = new SqlConnection(ConnectionDB.GetConnectionString());

        connection.Open();

        using (var transaction = connection.BeginTransaction())
        {
            try
            {

                var command = new SqlCommand(Locacao.SELECTLOCAOPORID, connection, transaction);
                command.Parameters.AddWithValue("@LocacaoId", locacaoID);

                Locacao locacao = null;
                using (var reader = command.ExecuteReader())
                {
                    int diasLocacao = 0;

                    if (reader.Read())
                    {
                        diasLocacao = (int)(reader.GetDateTime(4) - reader.GetDateTime(3)).TotalDays;
                        locacao = new Locacao(
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDateTime(3),
                            diasLocacao
                        );

                        locacao.SetLocacaoID(reader.GetInt32(0));
                        locacao.SetDataDevolucaoPrevista(reader.GetDateTime(4));
                        locacao.SetDataDevolucaoReal(DateTime.Now);
                        locacao.SetValorDiaria(reader.GetDecimal(6));
                        locacao.SetValorTotal(reader.GetDecimal(7));
                        locacao.SetMulta(locacao.ValorDiaria);
                        locacao.SetStatus(EStatusLocacao.Cancelada);
                    }
                }

                var commandAtualizarLocacao = new SqlCommand(Locacao.UPDATELOCACAO, connection, transaction);
                commandAtualizarLocacao.Parameters.AddWithValue("@IdLocacao", locacao.LocacaoID);
                commandAtualizarLocacao.Parameters.AddWithValue("@Status", locacao.Status.ToString());
                commandAtualizarLocacao.Parameters.AddWithValue("@Multa", locacao.Multa);
                commandAtualizarLocacao.Parameters.AddWithValue("@DataDevolucaoReal", locacao.DataDevolucaoReal);
                commandAtualizarLocacao.Parameters.AddWithValue("@ValorTotal", locacao.Multa);

                var (marca, modelo, placaVeiculo) = veiculoController.BuscarMarcaModeloPorVeiculoID(locacao.VeiculoID);
                veiculoController.AtualizarStatusVeiculo(EStatusVeiculo.Disponivel.ToString(), placaVeiculo);

                commandAtualizarLocacao.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw new Exception("Erro inesperado ao cancelar locação: " + ex.Message);
            }
        }
    }

    public void EncerrarLocacao(int locacaoID)
    {
        var veiculoController = new VeiculoController();
        var clienteController = new ClienteController();
        var locacaoFuncionarioController = new LocacaoFuncionarioController();

        Locacao locacaoEncontrada = null;

        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                var command = new SqlCommand(Locacao.SELECTLOCAOPORID, connection, transaction);
                command.Parameters.AddWithValue("@locacaoID", locacaoID);

                using (var reader = command.ExecuteReader())
                {
                    int diasPrevistos = 0;
                    int diasLocados = 0;

                    if (reader.Read())
                    {
                        diasPrevistos = (int)(reader.GetDateTime(4) - reader.GetDateTime(3)).TotalDays;
                        locacaoEncontrada = new Locacao(
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDateTime(3),
                            diasPrevistos
                        );


                        locacaoEncontrada.SetLocacaoID(reader.GetInt32(0));
                        locacaoEncontrada.SetDataDevolucaoPrevista(reader.GetDateTime(4));
                        locacaoEncontrada.SetDataDevolucaoReal(DateTime.Now);
                        locacaoEncontrada.SetValorDiaria(reader.GetDecimal(6));
                        diasLocados = (int)Math.Ceiling((locacaoEncontrada.DataDevolucaoReal.Value - locacaoEncontrada.DataLocacao).TotalDays);
                        locacaoEncontrada.SetValorTotal(locacaoEncontrada.ValorDiaria * diasLocados);
                        locacaoEncontrada.SetMulta(reader.GetDecimal(8));
                        locacaoEncontrada.SetStatus(EStatusLocacao.Concluida);
                    }
                }


                var commandAtualizarLocacao = new SqlCommand(Locacao.UPDATELOCACAO, connection, transaction);
                commandAtualizarLocacao.Parameters.AddWithValue("@IdLocacao", locacaoEncontrada.LocacaoID);
                commandAtualizarLocacao.Parameters.AddWithValue("@Status", locacaoEncontrada.Status.ToString());
                commandAtualizarLocacao.Parameters.AddWithValue("@Multa", locacaoEncontrada.Multa);
                commandAtualizarLocacao.Parameters.AddWithValue("@DataDevolucaoReal", locacaoEncontrada.DataDevolucaoReal);
                commandAtualizarLocacao.Parameters.AddWithValue("@ValorTotal", locacaoEncontrada.ValorTotal);


                var (marca, modelo, placaVeiculo) = veiculoController.BuscarMarcaModeloPorVeiculoID(locacaoEncontrada.VeiculoID);
                veiculoController.AtualizarStatusVeiculo(EStatusVeiculo.Disponivel.ToString(), placaVeiculo);

                commandAtualizarLocacao.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw new Exception("Erro inesperado ao encerrar locação: " + ex.Message);
            }
        }
    }
}