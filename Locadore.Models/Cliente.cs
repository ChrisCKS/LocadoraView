namespace Locadora.Models
{
    public class Cliente
    {
        public readonly static string INSERTCLIENTE = "INSERT INTO tblClientes VALUES(@Nome, @Email, @Telefone); " +
                                                        "SELECT SCOPE_IDENTITY()";

        public readonly static string SELECTALLCLIENTES = @"SELECT c.Nome, c.Email, c.Telefone,
		                                                    d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade
                                                            FROM tblClientes c
                                                            JOIN tblDocumentos d
                                                            ON c.ClienteID = d.ClienteID";

        public readonly static string UPDATETELEFONECLIENTE = "UPDATE tblClientes SET Telefone = @Telefone " +
                                                              "WHERE ClienteID = @IdCliente";

        public readonly static string SELECTCLIENTEPOREMAIL = @"SELECT c.ClienteID, c.Nome, c.Email, c.Telefone,
		                                                    d.TipoDocumento, d.Numero, d.DataEmissao, d.DataValidade
                                                            FROM tblClientes c
                                                            JOIN tblDocumentos d
                                                            ON c.ClienteID = d.ClienteID
                                                            WHERE c.Email = @Email";

        public static readonly string SELECTCLIENTEPORID = @"SELECT Nome 
                                                            FROM tblClientes 
                                                            WHERE ClienteID = @ClienteID";

        public static readonly string SELECTLOCACOESATIVASDOCLIENTE = @"SELECT COUNT(*) FROM tblLocacoes WHERE ClienteID = @ClienteID AND Status = 'Ativa'";


        public readonly static string DELETECLIENTEPORID = "DELETE FROM tblClientes WHERE ClienteID = @IdCliente";

        public int ClienteId { get; private set;}
        public string Nome { get; private set;}

        public string Email { get; private set;}

        public string? Telefone { get; private set;} = String.Empty;

        public Documento Documento { get; private set;}


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

        public void setDocumento(Documento documento)
        {
            Documento = documento;
        }

        public override string? ToString()
        {
            return $"Nome: {Nome}, \nEmail: {Email}, \nTelefone: {Telefone},  Documento:  {Documento}";
        }
    }
}
