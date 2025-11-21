using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{

    public class Categoria
    {

        public readonly static string INSERTCATEGORIA = "INSERT INTO tblCategorias "  +
                                                        "VALUES (@Nome, @Descricao, @Diaria);"
                                                        + "SELECT SCOPE_IDENTITY()";   

        public readonly static string SELECTALLCATEGORIAS = "SELECT CategoriaId, Nome, Descricao, Diaria FROM tblCategorias;";

        //aq o prof mudou para categoria por id
        //e ai mudou la na busca tb
        public readonly static string SELECTCATEGORIANOME = "SELECT CategoriaId, Nome, Descricao, Diaria FROM tblCategorias WHERE Nome = @Nome";

        public readonly static string UPDATECATEGORIA = "UPDATE tblCategorias " +
                                                        "SET Descricao = @Descricao, Diaria = @Diaria " +
                                                        "WHERE CategoriaId = @IdCategoria;";

        public static readonly string DELETECATEGORIA = "DELETE FROM tblCategorias WHERE CategoriaId = @IdCategoria;";


        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Diaria { get; set; }

        public void setCategoriaId (int categoriaId)
        {
            CategoriaId = categoriaId;
        }

        public override string ToString()
        {
            return
                $"\nNome: {Nome}" +
                $"\nDescrição: {(Descricao != null ? Descricao : "Não tem descrição")}" +
                $"\nDiária: {Diaria}";
        }

        public Categoria(string nome, string? descricao, decimal diaria)
        {
            Nome = nome;
            Descricao = descricao;
            Diaria = diaria;
        }

        public Categoria(string nome, decimal diaria)
        {
            Nome = nome;
            Diaria = diaria;
        }
    }
}