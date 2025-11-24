using Locadora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Controller.Interface
{
    public interface ICategoriaController
    {
        public void AdicionarCategoria(Categoria categoria);

        public List<Categoria> ListarTodasCategorias();

        public Categoria BuscarCategoriaPorNome(string nome);

        public string BuscarNomeCategoriaPorId(int id);

        public void AtualizarCategoria(string nome, Categoria categoria);

        public void DeletarCategoria(string nome);


    }
}
