using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Funcionario

    {
        public int FuncionarioId { get; private set;}
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

        public override string ToString()
        {
            return $"ID: {FuncionarioId} | Nome: {Nome} | CPF: {CPF} | Email: {Email} | Salário: {Salario:C}";
        }
    }
}
