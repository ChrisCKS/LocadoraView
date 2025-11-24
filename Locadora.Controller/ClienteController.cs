using Microsoft.Data.SqlClient;
using Utils.Database;
using Locadora.Models;
using System.Transactions;
using Locadora.Controller.Interface;

namespace Locadora.Controller
{
    public class ClienteController : IClienteController
    {
        public void AdicionarCliente(Cliente cliente, Documento documento)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try 
                {  
                    SqlCommand command = new SqlCommand(Cliente.INSERTCLIENTE, connection, transaction);

                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Email", cliente.Email); //if     //else
                    command.Parameters.AddWithValue("@Telefone", cliente.Telefone ?? (object)DBNull.Value);

                    int clienteId = Convert.ToInt32(command.ExecuteScalar());
                    cliente.setClienteID(clienteId);

                    //cliente.setClienteID(Convert.ToInt32(command.ExecuteScalar()));


                    var documentoController = new DocumentoController();

                    documento.setClienteID(clienteId);

                    documentoController.AdicionarDocumento(documento, connection, transaction);

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar cliente: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar cliente: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            } 
        }
        public List<Cliente> ListarTodosClientes() 
        { 
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(Cliente.SELECTALLCLIENTES, connection);

                SqlDataReader reader = command.ExecuteReader();

                List<Cliente> listaClientes = new List<Cliente>();

                while (reader.Read())
                {
                    var cliente = new Cliente(
                        reader["Nome"].ToString()!,
                        reader["Email"].ToString()!,
                        reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : null
                    );

                    //cliente.setClienteID(Convert.ToInt32(reader["ClienteId"]));

                    //listaClientes.Add(cliente);

                    var documento = new Documento(reader["TipoDocumento"].ToString(),
                                                  reader["Numero"].ToString(),
                                                  DateOnly.FromDateTime(reader.GetDateTime(5)),
                                                  DateOnly.FromDateTime(reader.GetDateTime(6))
                                                  );

                    cliente.setDocumento(documento);

                    listaClientes.Add(cliente);
                }
                return listaClientes;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar clientes: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao listar clientes: " + ex.Message);
            }
            finally 
            {
                connection.Close();
            }
        }

        public Cliente BuscaClientePorEmail(string email)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand(Cliente.SELECTCLIENTEPOREMAIL, connection);

                command.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var cliente = new Cliente(
                        reader["Nome"].ToString()!,
                        reader["Email"].ToString()!,
                        reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : null
                    );
                    cliente.setClienteID(Convert.ToInt32(reader["ClienteId"]));

                    var documento = new Documento(
                        reader["TipoDocumento"].ToString()!,
                        reader["Numero"].ToString()!,
                        DateOnly.FromDateTime(reader.GetDateTime(6)),
                        DateOnly.FromDateTime(reader.GetDateTime(7))
                    );
                    cliente.setDocumento(documento);

                    return cliente;
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception ("Erro ao buscar cliente por email" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar cliente por email: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public string BuscarNomeClientePorID(int clienteID) 
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(Cliente.SELECTCLIENTEPORID, connection);
                command.Parameters.AddWithValue("@ClienteID", clienteID);

                SqlDataReader reader = command.ExecuteReader();
                string nomeCliente = String.Empty;

                if (reader.Read())
                {
                    nomeCliente = reader["Nome"].ToString() ?? String.Empty;
                }
                return nomeCliente;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar nome do cliente: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar nome do cliente: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        public void AtualizarTelefoneCliente(string telefone, string email)
        {
            var clienteEncontrado =BuscaClientePorEmail(email);

            if (clienteEncontrado is null)
                throw new Exception("Nao existe cliente com este email");

            clienteEncontrado.setTelefone(telefone);

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand(Cliente.UPDATETELEFONECLIENTE, connection);

                command.Parameters.AddWithValue("@Telefone", clienteEncontrado.Telefone);
                command.Parameters.AddWithValue("@IdCliente", clienteEncontrado.ClienteId);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao atualizar telefone do cliente" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao atualizar telefone do cliente" + ex.Message);
            }
            finally 
            {
                connection.Close();
            }
        }

        public void AtualizarDocumentoCliente(Documento documento, string email)
        {
            var clienteEncontrado = BuscaClientePorEmail(email);

            if (clienteEncontrado is null)
                throw new Exception("Nao existe cliente com este email");

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    documento.SetClienteId(clienteEncontrado.ClienteId);
                    DocumentoController documentoController = new DocumentoController();

                    documentoController.AtualizarDocumento(documento, connection, transaction);

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar documento do cliente: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao atualizar documento do cliente: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }
        public void DeletarCliente(string email)
        {
            var clienteEncontrado = BuscaClientePorEmail(email);

            if (clienteEncontrado is null)
                throw new Exception("Nao existe cliente com este email");

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

                try
                {
                    SqlCommand command = new SqlCommand(Cliente.DELETECLIENTEPORID, connection);
                    command.Parameters.AddWithValue("@IdCliente", clienteEncontrado.ClienteId);
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao deletar cliente: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao deletar cliente" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
        }
    }
}