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

        public static readonly string INSERTLOCACAO = "INSERT INTO tblLocacoes (ClienteID, VeiculoID, DataLocacao," +
                                                      " DataDevolucaoPrevista, DataDevolucaoReal, ValorDiaria, ValorTotal, Multa, Status) " +
                                                      "VALUES (@ClienteID, @VeiculoID, @DataLocacao, @DataDevolucaoPrevista," +
                                                      " @DataDevolucaoReal, @ValorDiaria, @ValorTotal, @Multa, @Status)" +
                                                      "SELECT SCOPE_IDENTITY()";

        public static readonly string SELECTLOCAOPORID = @"SELECT LocacaoID, ClienteID, VeiculoID, DataLocacao, DataDevolucaoPrevista,
                                                         DataDevolucaoReal, ValorDiaria, ValorTotal, Multa, Status 
                                                         FROM tblLocacoes WHERE LocacaoID = @LocacaoID";

        public static readonly string SELECTLOCAOPORVEICULOPLACA = @"SELECT LocacaoID, ClienteID, VeiculoID, DataLocacao,
                                                                   DataDevolucaoPrevista, DataDevolucaoReal, ValorDiaria, ValorTotal, Multa, Status 
                                                                   FROM tblLocacoes WHERE Placa = @Placa";

        public static readonly string SELECTLOCACOES = @"SELECT f.Nome, c.Nome, v.Marca, v.Modelo, l.ClienteID, l.VeiculoID, l.LocacaoID, 
                                                        l.DataLocacao, l.DataDevolucaoPrevista, l.DataDevolucaoReal, l.ValorDiaria, l.ValorTotal, l.Multa, l.Status 
                                                        FROM tblLocacoes l 
                                                        JOIN tblVeiculos v 
                                                        ON l.VeiculoID = v.VeiculoID 
                                                        JOIN tblClientes c 
                                                        on l.ClienteID = c.ClienteID 
                                                        JOIN tblLocacaoFuncionarios lf 
                                                        ON l.LocacaoID = lf.LocacaoID 
                                                        JOIN tblFuncionarios f 
                                                        ON lf.FuncionarioID = f.FuncionarioID";

        public static readonly string UPDATELOCACAO = @"UPDATE tblLocacoes 
                                                        SET Status = @Status, 
                                                        Multa = @Multa, 
                                                        DataDevolucaoReal = @DataDevolucaoReal, 
                                                        ValorTotal = @ValorTotal 
                                                        WHERE LocacaoID = @IdLocacao";
        public int LocacaoID { get; private set; }

        public string Funcionario { get; private set; }

        public int ClienteID { get; private set; }

        public string NomeCliente { get; private set; }

        public int VeiculoID { get; private set; }

        public string Marca { get; private set; }

        public string Modelo { get; private set; }

        public DateTime DataLocacao { get; private set; }

        public DateTime DataDevolucaoPrevista { get; private set; }
        public DateTime? DataDevolucaoReal { get; private set; }

        public decimal ValorDiaria { get; private set; }
        public decimal ValorTotal { get; private set; }
        public decimal Multa { get; private set; }

        public EStatusLocacao Status { get; private set; }


        public Locacao(int clienteID, int veiculoID, DateTime dataLocacao, int diasLocacao)
        {
            ClienteID = clienteID;
            VeiculoID = veiculoID;
            DataLocacao = DateTime.Now;;
            DataDevolucaoPrevista = DateTime.Now.AddDays(diasLocacao);
            Status = EStatusLocacao.Ativa;
        }

        public void SetLocacaoID(int locacaoID)
        {
            LocacaoID = locacaoID;
        }
        public void SetDataDevolucaoPrevista(DateTime dataDevolucaoPrevista)
        {
            DataDevolucaoPrevista = dataDevolucaoPrevista;
        }
        public void SetDataDevolucaoReal(DateTime? dataDevolucaoReal)
        {
            DataDevolucaoReal = dataDevolucaoReal;
        }
        public void SetValorDiaria(decimal valorDiaria)
        {
            ValorDiaria = valorDiaria;
        }
        public void SetValorTotal(decimal valorTotal)
        {
            ValorTotal = valorTotal;
        }
        public void SetMulta(decimal multa)
        {
            Multa = multa;
        }
        public void SetStatus(EStatusLocacao status)
        {
            Status = status;
        }
        public void SetFuncionario(string funcionario)
        {
            Funcionario = funcionario;
        }
        public void SetNomeCliente(string nomeCliente)
        {
            NomeCliente = nomeCliente;
        }
        public void SetMarca(string marca)
        {
            Marca = marca;
        }
        public void SetModelo(string modelo)
        {
            Modelo = modelo;
        }

        public override string ToString()
        {
            return $"Locação ID: {LocacaoID} | Cliente ID: {ClienteID} | Veículo ID: {VeiculoID} " +
                $"| Data Locação: {DataLocacao} | Data Devolução Prevista: {DataDevolucaoPrevista} |" +
                $" Valor Diária: {ValorDiaria} | Valor Total: {ValorTotal} | Multa: {Multa} | Status: {Status}";
        }
    }
}
