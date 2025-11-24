namespace Locadora.Models;

public class LocacaoFuncionario
{
    public static readonly string INSERTRELACAO = @"INSERT INTO tblLocacaoFuncionarios(LocacaoID, FuncionarioID) VALUES(@LocacaoID, @FuncionarioID)";

    public static readonly string SELECTFUNCIONARIOSPORLOCACAO = @"SELECT lf.FuncionarioID
                                                                        FROM tblLocacaoFuncionarios lf
                                                                        JOIN tblLocacoes l
                                                                        ON lf.LocacaoID = l.LocacaoID
                                                                        WHERE lf.LocacaoID = @LocacaoID";
}