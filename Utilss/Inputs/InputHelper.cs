using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Inputs
{
    public class InputHelper
    {
        public static string LerString(string mensagem, string mensagemErro, string valorPadrao = "")
        {
            while (true)
            {
                Console.Write(mensagem);
                string? digitado = Console.ReadLine();

                if (string.IsNullOrEmpty(digitado) && valorPadrao != "")
                    return valorPadrao;

                if (!string.IsNullOrWhiteSpace(digitado))
                    return digitado;

                Console.WriteLine(mensagemErro);
            }
        }

        public static string? LerStringNulo(string mensagem, string mensagemErro, bool permitirNulo = false)
        {
            while (true)
            {
                Console.Write(mensagem);
                string? digitado = Console.ReadLine();

                if (string.IsNullOrEmpty(digitado) && permitirNulo)
                    return null;

                if (!string.IsNullOrWhiteSpace(digitado))
                    return digitado;

                Console.WriteLine(mensagemErro);
            }
        }

        public static int LerInt(string mensagem, string mensagemErro, int valorPadrao = 0)
        {
            while (true)
            {
                Console.Write(mensagem);
                string? digitado = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(digitado) && valorPadrao != 0)
                    return valorPadrao;

                if (int.TryParse(digitado, out int valor))
                    return valor;

                Console.WriteLine(mensagemErro);
            }
        }

        public static decimal LerDecimal(string mensagem, string mensagemErro, decimal valorPadrao = 0)
        {
            while (true)
            {
                Console.Write(mensagem);
                string? digitado = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(digitado) && valorPadrao != 0)
                    return valorPadrao;

                if (decimal.TryParse(digitado, out decimal valor))
                    return valor;

                Console.WriteLine(mensagemErro);
            }
        }

        public static decimal? LerDecimalNulo(string mensagem, string mensagemErro, bool permitirNulo = false)
        {
            while (true)
            {
                Console.Write(mensagem);
                string? digitado = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(digitado) && permitirNulo)
                    return null;

                if (decimal.TryParse(digitado, out decimal valor))
                    return valor;

                Console.WriteLine(mensagemErro);
            }
        }

        public static DateOnly LerData(string mensagem, string mensagemErro, DateOnly? valorPadrao = null)
        {
            while (true)
            {
                Console.Write(mensagem);
                string? digitado = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(digitado) && valorPadrao != null)
                    return valorPadrao.Value;

                if (DateOnly.TryParse(digitado, out DateOnly data))
                    return data;

                Console.WriteLine(mensagemErro);
            }
        }

        public static DateOnly? LerDataNulo(string mensagem, string mensagemErro, bool permitirNulo = false)
        {
            while (true)
            {
                Console.Write(mensagem);
                string? digitado = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(digitado) && permitirNulo)
                    return null;

                if (DateOnly.TryParse(digitado, out DateOnly data))
                    return data;

                Console.WriteLine(mensagemErro);
            }
        }
    }
}
