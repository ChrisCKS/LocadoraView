using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Veiculo
    {
        public int VeiculoId { get; private set; }
        public int CategoriaId { get; private set; }
        public string Placa { get; private set; }
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public int Ano { get; private set; }

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

        public override string? ToString()
        {
            return $"Placa: {Placa}," + $" \nMarca: {Marca}, \nModelo: {Modelo}, \n" +
                   $"Ano: {Ano}, \nStatus do Veículo: {StatusVeiculo}";
        }
    }
}
