using Locadora.Controller.Interfaces;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Database;

namespace Locadora.Controller;

public class FuncionarioController : IFuncionarioController
{
    public void AdicionarFuncionario(Funcionario funcionario)
    {
        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();

        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            try
            {
                SqlCommand command = new SqlCommand(Funcionario.INSERTFUNCIONARIO, connection, transaction);

                command.Parameters.AddWithValue("@Nome", funcionario.Nome);
                command.Parameters.AddWithValue("@CPF", funcionario.CPF);
                command.Parameters.AddWithValue("@Email", funcionario.Email);
                command.Parameters.AddWithValue("@Salario", funcionario.Salario);
                int funcionarioID = Convert.ToInt32(command.ExecuteScalar());
                funcionario.setFuncionarioID(funcionarioID);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao adicionar funcionario: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
    public List<Funcionario> ListarTodosFuncionarios()
    {
        var connectionString = ConnectionDB.GetConnectionString();

        var connection = new SqlConnection(connectionString);

        try
        {
            connection.Open();
            SqlCommand command = new SqlCommand(Funcionario.SELECTALLFUNCIONARIO, connection);
            SqlDataReader reader = command.ExecuteReader();
            List<Funcionario> funcionarios = new List<Funcionario>();
            while (reader.Read())
            {
                var funcionario = new Funcionario(
                    reader["Nome"].ToString(),
                    reader["CPF"].ToString(),
                    reader["Email"].ToString(),
                    (decimal)reader["Salario"]
                );
                funcionario.setFuncionarioID((int)reader["FuncionarioID"]);
                funcionarios.Add(funcionario);
            }

            return funcionarios;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao buscar os funcionarios" + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }
    public Funcionario BuscarFuncionarioPorCPF(string cpf)
    {
        SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

        connection.Open();
        try
        {
            SqlCommand command = new SqlCommand(Funcionario.SELECTFUNCIONARIOPORCPF, connection);

            command.Parameters.AddWithValue("@CPF", cpf);

            SqlDataReader reader = command.ExecuteReader();
            Funcionario funcionario = null;
            if (reader.Read())
            {
                funcionario = new Funcionario(
                    reader["Nome"].ToString(),
                    reader["CPF"].ToString(),
                    reader["Email"].ToString(),
                    (decimal)reader["Salario"]
                );
                funcionario.setFuncionarioID(Convert.ToInt32(reader["FuncionarioID"]));
            }

            return funcionario;
        }
        catch (SqlException ex)
        {
            throw new Exception("Erro ao buscar funcionario por cpf: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao buscar funcionario por cpf: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    public string BuscarNomeFuncionarioPorID(int id)
    {
        SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

        connection.Open();
        try
        {
            SqlCommand command = new SqlCommand(Funcionario.SELECTFUNCIONARIOPORCPF, connection);

            command.Parameters.AddWithValue("@IdFuncionario", id);

            SqlDataReader reader = command.ExecuteReader();
            string nome = String.Empty;
            if (reader.Read())
            {
                nome = reader["Nome"].ToString();
            }

            return nome;
        }
        catch (SqlException ex)
        {
            throw new Exception("Erro ao buscar funcionario por cpf: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao buscar funcionario por cpf: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    public void AtualizarSalario(string cpf, decimal salario)
    {
        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();
        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            var funcionarioEncontrado = BuscarFuncionarioPorCPF(cpf);

            if (funcionarioEncontrado is null)
                throw new Exception("Funcionário não encontrado");

            funcionarioEncontrado.setSalarioFuncionario(salario);

            try
            {
                SqlCommand command = new SqlCommand(Funcionario.UPDATEFUNCIONARIOSALARIO, connection, transaction);
                command.Parameters.AddWithValue("@Salario", funcionarioEncontrado.Salario);
                command.Parameters.AddWithValue("@IdFuncionario", funcionarioEncontrado.FuncionarioID);

                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao atualizar salário do funcionário: " + ex.Message);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro inesperado ao atualizar salário do funcionário" + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
    public void DeletarFuncionario(string cpf)
    {
        var connection = new SqlConnection(ConnectionDB.GetConnectionString());
        connection.Open();
        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            var funcionarioEncontrado = BuscarFuncionarioPorCPF(cpf);

            if (funcionarioEncontrado is null)
                throw new Exception("Funcionário não encontrado");

            try
            {
                SqlCommand command = new SqlCommand(Funcionario.DELETEFUNCIONARIO, connection, transaction);
                command.Parameters.AddWithValue("@IdFuncionario", funcionarioEncontrado.FuncionarioID);
                command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao deletar funcionário: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao deletar funcionário" + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}