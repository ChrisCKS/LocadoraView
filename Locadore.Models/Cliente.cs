namespace Locadora.Models
{
    public class Cliente
    {
        public readonly static string INSERTCLIENTE = "INSERT INTO tblClientes (Nome, Email, Telefone) " +
                                                      "VALUES (@Nome, @Email, @Telefone); " +
                                                      "SELECT SCOPE_IDENTITY();";

        public readonly static string SELECTALLCLIENTES = "SELECT * FROM tblClientes";

        public readonly static string UPDATETELEFONECLIENTE = "UPDATE tblClientes SET Telefone = @Telefone " +
                                                              "WHERE ClienteID = @IdCliente";

        public readonly static string SELECTCLIENTEPOREMAIL = "SELECT * FROM tblClientes WHERE Email = @Email";

        public readonly static string DELETECLIENTEPORID = "DELETE FROM tblClientes WHERE ClienteID = @IdCliente";

        public int ClienteId { get; private set; }
        public string Nome { get; private set; }

        public string Email { get; private set; }

        public string Telefone { get; private set; } = String.Empty;

        public Cliente(string nome, string email)       //posso criar cliente sem telefone
        {
            Nome = nome;
            Email = email;
        }

        public Cliente (string nome, string email, string? telefone) : this (nome, email)
        {
            Telefone = telefone;
        }

        public void setClienteID (int clienteId)
        {
            ClienteId = clienteId;
        }

        public void setTelefone(string telefone) 
        {
            Telefone = telefone;
        }

        public override string? ToString()
        {
            return $"Nome: {Nome}, \nEmail: {Email}, \nTelefone: {Telefone}";
        }
    }
}
