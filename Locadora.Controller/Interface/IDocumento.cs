using Locadora.Models;
using Microsoft.Data.SqlClient;


namespace Locadora.Controller.Interface
{
    public interface IDocumento
    {
        public void AdicionarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction);

        public void AtualizarDocumento(Documento documento, SqlConnection connection, SqlTransaction transaction);
    }
}
