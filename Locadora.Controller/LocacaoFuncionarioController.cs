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

    public void AdicionarRelação(int locacaoID, int funcionarioID)
    {
        throw new NotImplementedException();
    }
}