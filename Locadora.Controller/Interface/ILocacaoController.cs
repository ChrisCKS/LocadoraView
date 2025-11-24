using Locadora.Models;

namespace Locadora.Controller.Interfaces;

public interface ILocacaoController
{
    public void AdicionarLocacao(Locacao locacao, string cpf);
    public List<Locacao> ListarLocacoes();
    public void CancelarLocacao(int locacaoID);
    public void EncerrarLocacao(int locacaoID);
}
