using Locadora.Controller.Interface;
using Locadora.Models;
using Microsoft.Data.SqlClient;


namespace Locadora.Controller
{
    public class DocumentoController : IDocumento
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
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar documento: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar documento: " + ex.Message);
                }
        }

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

                command.Parameters.AddWithValue("@IdCliente", documento.ClienteId);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao atualizar documento: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao alterar documento: " + ex.Message);
            }
        }
    }
}
