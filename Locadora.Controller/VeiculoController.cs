using Locadora.Controller.Interface;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Locadora.Controller
{
    public class VeiculoController : IVeiculoController
    {
        public void AdicionarVeiculo(Veiculo veiculo)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Veiculo.INSERTVEICULO, connection, transaction);
                    command.Parameters.AddWithValue("@CategoriaID", veiculo.CategoriaId);
                    command.Parameters.AddWithValue("@Placa", veiculo.Placa);
                    command.Parameters.AddWithValue("@Marca", veiculo.Marca);
                    command.Parameters.AddWithValue("@Modelo", veiculo.Modelo);
                    command.Parameters.AddWithValue("@Ano", veiculo.Ano);
                    command.Parameters.AddWithValue("@StatusVeiculo", veiculo.StatusVeiculo);

                    int veiculoId = Convert.ToInt32(command.ExecuteScalar());
                    veiculo.setVeiculoID(veiculoId);

                    //command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao adicionar veiculo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao adicionar veiculo: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<Veiculo> ListarTodosVeiculos()
        {
            var veiculos = new List<Veiculo>();
            var categoriaController = new CategoriaController();

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlCommand command = new SqlCommand(Veiculo.SELECTALLVEICULOS, connection))
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Veiculo veiculo = new Veiculo(
                            
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetInt32(4),
                            reader.GetString(5)
                            );

                            veiculo.setNomeCategoria(
                                categoriaController.BuscarCategoriaPorNome(veiculo.CategoriaId).Nome        /*========*/
                                );
                            veiculos.Add(veiculo);  

                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar veiculos: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao listar veiculos: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return veiculos;
            }
        }

        public Veiculo BuscarVeiculoPlaca(string placa) 
        {
            var categoriaController = new CategoriaController();

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            Veiculo veiculo = null;

            connection.Open();

            using (SqlCommand command = new SqlCommand(Veiculo.SELECTVEICULOPORPLACA, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@Placa", placa);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            veiculo = new Veiculo(
                            
                            reader.GetInt32(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetInt32(5),
                            reader.GetString(6)
                            );
                            veiculo.setVeiculoID(reader.GetInt32(0));
                            veiculo.setNomeCategoria(       //buscarnomecategoriaporid
                                categoriaController.BuscarCategoriaPorNome(veiculo.CategoriaID)      /*========*/
                             );
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar veiculo por placa: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao buscar veiculo por placa: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return veiculo ?? throw new Exception("Veiculo não encontrado");
            }
        }

        public void AtualizarStatusVeiculo(string statusVeiculo, string placa)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            var veiculo = BuscarVeiculoPlaca(placa ?? throw new Exception("Veiculo nao encontrado"));

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Veiculo.UPDATESTATUSVEICULO, connection, transaction);

                    command.Parameters.AddWithValue("@StatusVeiculo", statusVeiculo);
                    command.Parameters.AddWithValue("@IdVeiculo", veiculo.VeiculoId);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar status veiculo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao atualizar status veiculo: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void DeletarVeiculo(int IdVeiculo)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                
                try
                {
                    SqlCommand command = new SqlCommand(Veiculo.DELETEVEICULO, connection, transaction);
                    command.Parameters.AddWithValue("@IdVeiculo", IdVeiculo);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao deletar veiculo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao deletar veiculo: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }


        }


    }
}
