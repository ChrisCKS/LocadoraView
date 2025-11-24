using Locadora.Models;

namespace Locadora.Controller.Interfaces;

public interface ILocacaoController
{
    public void AdicionarLocacao(Locacao locacao, string cpf);
    public List<Locacao> ListarLocacoes();
    public Locacao BuscarLocacaoPorID(int locacaoID);
    public void AtualizarStatusLocacao(string placa);
    public void EncerrarLocacao(Locacao locacao, string placa);
}
