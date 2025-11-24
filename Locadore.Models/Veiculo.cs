using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Veiculo
    {

        public readonly static string INSERTVEICULO = @"INSERT INTO tblVeiculos (CategoriaID, Placa, Marca, Modelo, Ano, StatusVeiculo) 
                                                        OUTPUT INSERTED.VeiculoID
                                                        VALUES (@CategoriaID, @Placa, @Marca, @Modelo, @Ano, @StatusVeiculo)";

        public readonly static string SELECTALLVEICULOS = @"SELECT CategoriaID, 
                                                            Placa, Marca, Modelo, Ano, StatusVeiculo
                                                            FROM tblVeiculos";

        public readonly static string SELECTVEICULOPORPLACA = @"SELECT VeiculoID, CategoriaID, Placa, Marca, Modelo, Ano, StatusVeiculo
                                                                FROM tblVeiculos
                                                                WHERE Placa = @Placa";

        public static readonly string SELECTDIARIAPORVEICULO = @"SELECT c.Diaria
                                                                FROM tblVeiculos v
                                                                JOIN tblCategorias c
                                                                ON v.CategoriaID = c.CategoriaID
                                                                WHERE VeiculoID = @VeiculoID";

        public static readonly string SELECTVEICULOPORID = @"SELECT Marca, Modelo, StatusVeiculo, Placa 
                                                            FROM tblVeiculos 
                                                             WHERE VeiculoID = @VeiculoID";

        public readonly static string UPDATESTATUSVEICULO = @"UPDATE tblVeiculos 
                                                            SET StatusVeiculo = @StatusVeiculo
                                                            WHERE VeiculoID = @IdVeiculo";

        public readonly static string DELETEVEICULO = @"DELETE FROM tblVeiculos 
                                                        WHERE VeiculoID = @IdVeiculo";

        public int VeiculoId { get; private set; }
        public int CategoriaId { get; private set; }
        public string Placa { get; private set; }
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public int Ano { get; private set; }

        public string  NomeCategoria { get; set; }

        public string StatusVeiculo { get; private set; }

        public Veiculo(int categoriaId, string placa, string marca, string modelo, int ano, string statusVeiculo)
        {
            CategoriaId = categoriaId;
            Placa = placa;
            Marca = marca;
            Modelo = modelo;
            Ano = ano;
            StatusVeiculo = statusVeiculo;
        }

        public void setVeiculoID(int veiculoId)
        {
            VeiculoId = veiculoId;
        }

        public void setStatusVeiculo(string statusVeiculo)
        {
            StatusVeiculo = statusVeiculo;
        }

        public void setNomeCategoria(string nomeCategoria)         
        {
            NomeCategoria = nomeCategoria;
        }

        public override string? ToString()
        {
            return $"Placa: {Placa}," + $" \nMarca: {Marca}, \nModelo: {Modelo}, \n" +
                   $"Ano: {Ano}, \nStatus do Veículo: {StatusVeiculo} \nNome Categoria: {NomeCategoria}";
        }
    }
}
