using Locadora.Controller.Interface;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Utils.Database;

namespace Locadora.Controller
{
    public class CategoriaController : ICategoria
    {
        public void AdicionarCategoria(Categoria categoria)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Categoria.INSERTCATEGORIA, connection, transaction);

                    command.Parameters.AddWithValue("@Nome", categoria.Nome);
                    command.Parameters.AddWithValue("@Descricao", categoria.Descricao ?? (object)DBNull.Value);
                    var p = command.Parameters.Add("@Diaria", SqlDbType.Decimal);
                    p.Precision = 10;
                    p.Scale = 2;
                    p.Value = categoria.Diaria;

                    int categoriaId = Convert.ToInt32(command.ExecuteScalar());
                    categoria.setCategoriaId(categoriaId);

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar categoria: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar categoria: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<Categoria> ListarTodasCategorias()
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(Categoria.SELECTALLCATEGORIAS, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<Categoria> listaCategoria = new List<Categoria>();

                while (reader.Read())
                {
                    var categoria = new Categoria(
                        reader["Nome"].ToString(),
                        reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : null,
                        Convert.ToDecimal(reader["Diaria"])
                        );

                    categoria.setCategoriaId(Convert.ToInt32(reader["CategoriaId"]));
                    listaCategoria.Add(categoria);
                }

                return listaCategoria;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar categorias: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao listar categorias: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public Categoria BuscarCategoriaPorNome(string nome)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand(Categoria.SELECTCATEGORIANOME, connection);

                command.Parameters.AddWithValue("@Nome", nome);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var categoria = new Categoria(
                    reader["Nome"].ToString(),
                    reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : null,
                    Convert.ToDecimal(reader["Diaria"])
                    );
                    categoria.setCategoriaId(Convert.ToInt32(reader["CategoriaId"]));
                    return categoria;
                }
                return null;
                
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar categoria por nome: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar categoria por nome: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public string BuscarNomeCategoriaPorId(int id)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand(Categoria.SELECTNOMECATEGORIAPORID, connection);
                command.Parameters.AddWithValue("@Id", id);

                string nomecategoria = String.Empty;

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    nomecategoria = reader["Nome"].ToString() ?? string.Empty;
                }
                return nomecategoria;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar categoria." + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar categoria." + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void AtualizarCategoria(string nome, decimal diaria, string descricao)
        {
            var categoriaEncontrada = BuscarCategoriaPorNome(nome);

            var connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            if (categoriaEncontrada is null)
            {
                throw new Exception("Categoria não encontrada.");
            }

            categoriaEncontrada.Diaria = diaria;
            categoriaEncontrada.Descricao = descricao;

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Categoria.UPDATECATEGORIA, connection, transaction);

                    command.Parameters.AddWithValue("@IdCategoria", categoriaEncontrada.CategoriaId);
                    command.Parameters.AddWithValue("@Descricao", categoriaEncontrada.Descricao ?? (object)DBNull.Value);
                    var p = command.Parameters.Add("@Diaria", SqlDbType.Decimal);
                    p.Precision = 10;
                    p.Scale = 2;
                    p.Value = categoriaEncontrada.Diaria;

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar categoria: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao atualizar categoria: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void DeletarCategoria(string nome)
        {
            var categoriaEncontrada = BuscarCategoriaPorNome(nome);

            if (categoriaEncontrada is null)
                throw new Exception("Nao existe nenhuma categoria com este nome");

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            try
            {
                SqlCommand command = new SqlCommand(Categoria.DELETECATEGORIA, connection);
                command.Parameters.AddWithValue("@IdCategoria", categoriaEncontrada.CategoriaId);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao deletar categoria: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao deletar categoria: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
