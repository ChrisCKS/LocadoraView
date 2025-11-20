using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Database;

namespace Locadora.Controller
{
    public class DocumentoController
    {
        public void AdicionarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction)
        {
                try
                {
                    SqlCommand command = new SqlCommand(Documento.INSERTEDOCUMENTO, connection, transaction);

                    command.Parameters.AddWithValue("@ClienteID", documento.ClienteId);
                    command.Parameters.AddWithValue("@TipoDocumento", documento.TipoDocumento); 
                    command.Parameters.AddWithValue("@Numero", documento.Numero);
                    command.Parameters.AddWithValue("@DataEmissao", documento.DataEmissao);
                    command.Parameters.AddWithValue("@DataValidade", documento.DataValidade);

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    //transaction.Rollback();
                    throw new Exception("Erro ao adicionar documento: " + ex.Message);
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar documento: " + ex.Message);
                }
        }

        public Documento BuscaDocumentoPorClienteID(int clienteId)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            try
            {
                string selectDocumentoPorClienteID = "SELECT TipoDocumento, Numero, DataEmissao, DataValidade " +
                                                     "FROM tblDocumentos " +
                                                     "WHERE ClienteID = @ClienteID";
                SqlCommand command = new SqlCommand(selectDocumentoPorClienteID, connection);
                command.Parameters.AddWithValue("@ClienteID", clienteId);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string tipoDocumento = reader.GetString(0);
                    string numero = reader.GetString(1);
                    DateOnly dataEmissao = DateOnly.FromDateTime(reader.GetDateTime(2));
                    DateOnly dataValidade = DateOnly.FromDateTime(reader.GetDateTime(3));
                    Documento documento = new Documento(tipoDocumento, numero, dataEmissao, dataValidade);
                    documento.setClienteID(clienteId);
                    return documento;
                }
                else
                {
                    throw new Exception("Documento não encontrado para o ClienteID fornecido.");
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar documento: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }   /**/

        public void AtualizarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                SqlCommand command = new SqlCommand(Documento.UPDATEDOCUMENTO, connection, transaction);

                command.Parameters.AddWithValue("@ClienteID", documento.ClienteId);
                command.Parameters.AddWithValue("@TipoDocumento", documento.TipoDocumento);
                command.Parameters.AddWithValue("@Numero", documento.Numero);
                command.Parameters.AddWithValue("@DataEmissao", documento.DataEmissao);
                command.Parameters.AddWithValue("@DataValidade", documento.DataValidade);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao atualizar documento: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
