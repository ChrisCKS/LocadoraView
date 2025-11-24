using Locadora.Controller;
using Locadora.Models;
using Locadora.Models.Enums;
using Locadora.View;
using Microsoft.Data.SqlClient;
using Utils.Database;

int op = 0;

do
{
    MenuView.ExibirMenuPrincipal();
} while (op != 0);
