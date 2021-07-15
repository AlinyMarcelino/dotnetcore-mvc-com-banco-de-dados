using Newtonsoft.Json.Linq;
using System.IO;
using MySqlConnector;
using System;
using System.Collections.Generic;
namespace Looping.models
{
    public class Cliente
    {
        public int CodigoInterno { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public List<Conta> Contas { get { return Conta.BuscaPorCliente(this.CodigoInterno); } }
        public static List<Cliente> Lista(string sqlWhere = "")
        {
            var clientes = new List<Cliente>();

            JToken jAppSettings = JToken.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json")));
            string sqlCnn = jAppSettings["ConexaoMySql"].ToString();
            using (var connection = new MySqlConnection(sqlCnn))

            {
                connection.Open();

            using (var command = new MySqlCommand($"SELECT * FROM tbcliente {sqlWhere}", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(new Cliente()
                        {
                            CodigoInterno = Convert.ToInt32(reader["codcliente"]),
                            Nome = reader["nome"].ToString(),
                            CPF = reader["cpf"].ToString(),
                            Email = reader["email"].ToString()
                        });
                    }
                }
            }
        }

            return clientes;
        }

    public static Cliente BuscaPorId(int id)
    {
        var clientes = Cliente.Lista("where codcliente =" + id);
        return clientes.Count > 0 ? clientes[0] : null;
    }

}
}



