using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Funcionario

    {

        public static readonly string INSERTFUNCIONARIO = @"INSERT INTO tblFuncionarios VALUES(@Nome, @CPF, @Email, @Salario);";

        public static readonly string SELECTFUNCIONARIOPORCPF = @"SELECT * FROM tblFuncionarios WHERE CPF = @CPF";

        public static readonly string SELECTFUNCIONARIOPORID = @"SELECT Nome FROM tblFuncionarios WHERE FuncionarioID = @IdFuncionario";

        public static readonly string UPDATEFUNCIONARIOSALARIO = @"UPDATE tblFuncionarios SET Salario  = 
                                                                    @Salario WHERE FuncionarioID = @IdFuncionario";

        public static readonly string DELETEFUNCIONARIO = @"DELETE FROM tblFuncionarios WHERE FuncionarioID = @IdFuncionario";

        public static readonly string SELECTALLFUNCIONARIO = @"SELECT * FROM tblFuncionarios";


        public int FuncionarioID { get; private set; }
        public string Nome { get; private set;}
        public string CPF { get; private set;}

        public string Email { get; private set; }

        public decimal? Salario { get; private set; }

        public Funcionario( string nome, string cpf, string email)
        {
            Nome = nome;
            CPF = cpf;
            Email = email;
        }

        public Funcionario(string nome, string cpf, string email, decimal salario) : this(nome, cpf, email)
        {
            Salario = salario;
        }

        public void setFuncionarioID(int funcionarioID)
    {
        FuncionarioID = funcionarioID;
    }

    public void setSalarioFuncionario(decimal salario)
    {
        Salario = salario;
    }


        public override string ToString()
        {
            return $"ID: {FuncionarioID} | Nome: {Nome} | CPF: {CPF} | Email: {Email} | Salário: {Salario:C}";
        }
    }
}
