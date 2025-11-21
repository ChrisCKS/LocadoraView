using Locadora.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Locacao

    {
        public int LocacaoID { get; private set; }

        public int ClienteID { get; private set; }

        public int VeiculoID { get; private set; }

        public DateTime DataLocacao { get; private set; }

        public DateTime DataDevolucaoPrevista { get; private set; }
        public DateTime? DataDevolucaoReal { get; private set; }

        public decimal ValorDiaria { get; private set; }
        public decimal ValorTotal { get; private set; }
        public decimal Multa { get; private set; }

        public EStatusLocacao Status { get; private set; }


        public Locacao(int clienteID, int veiculoID, decimal valorDiaria, int diasLocacao)
        {
            ClienteID = clienteID;
            VeiculoID = veiculoID;
            DataLocacao = DateTime.Now;
            ValorDiaria = valorDiaria;
            ValorTotal = valorDiaria * diasLocacao;
            DataDevolucaoPrevista = DateTime.Now.AddDays(diasLocacao);
            Status = EStatusLocacao.Ativa;
        }

        public override string ToString()
        {
            return $"Locação ID: {LocacaoID} | Cliente ID: {ClienteID} | Veículo ID: {VeiculoID} " +
                $"| Data Locação: {DataLocacao} | Data Devolução Prevista: {DataDevolucaoPrevista} |" +
                $" Valor Diária: {ValorDiaria} | Valor Total: {ValorTotal} | Multa: {Multa} | Status: {Status}";
        }
    }
}
