namespace Locadora.Models;

public class LocacaoFuncionario
{
    public static readonly string INSERTRELACAO = @"INSERT INTO tblLocacaoFuncionarios(LocacaoID, FuncionarioID) VALUES(@LocacaoID, @FuncionarioID)";
}