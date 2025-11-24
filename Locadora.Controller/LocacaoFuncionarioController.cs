using Locadora.Controller.Interface;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Database;

namespace Locadora.Controller;

public class LocacaoFuncionarioController : ILocacaoFuncionarioController
{
    public void Adicionar(int locacaoID, int funcionarioID)
    {
        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();

        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            try
            {
                SqlCommand command = new SqlCommand(LocacaoFuncionario.INSERTRELACAO, connection, transaction);
                command.Parameters.AddWithValue("@LocacaoID", locacaoID);
                command.Parameters.AddWithValue("@FuncionarioID", funcionarioID);

                command.ExecuteNonQuery();
                transaction.Commit();

            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao adiconar funcionario e locação a tabela relacional: " + ex.Message);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro inesperado ao adiconar funcionario e locação a tabela relacional: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public List<string> BuscarFuncionariosPorLocacao(int locacaoID)
    {
        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();

        try
        {
            List<string> funcionarios = new List<string>();
            FuncionarioController funcionarioController = new FuncionarioController();

            SqlCommand command = new SqlCommand(LocacaoFuncionario.SELECTFUNCIONARIOSPORLOCACAO, connection);
            command.Parameters.AddWithValue("@LocacaoID", locacaoID);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int funcionarioID = reader.GetInt32(0);
                string nomeFuncionario = funcionarioController.BuscarNomeFuncionarioPorID(funcionarioID);
                funcionarios.Add(nomeFuncionario);
            }
            return funcionarios;
        }
        catch (SqlException ex)
        {
            throw new Exception("Erro ao buscar funcionários por locação: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao buscar funcionários por locação: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }
}