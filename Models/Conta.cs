using Newtonsoft.Json.Linq;
using System.IO;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace Looping.models
{
    public class Conta
    {
        public int Codigo { get;set; }
        public int Numero { get;set; }
        public int Agencia { get;set; }
        public int Tipo { get;set; }

        public double Saldo { get;set; }

        public static List<Conta> Lista(string sqlWhere = "")
        {
            var contas = new List<Conta>();
            JToken jAppSettings = JToken.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json")));
            string sqlCnn = jAppSettings["ConexaoMySql"].ToString();
            using (var connection = new MySqlConnection(sqlCnn))
            {
                connection.Open();

                using (var command = new MySqlCommand($"SELECT * FROM tbconta {sqlWhere}", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string v = reader["saldo"].ToString();
                            contas.Add(new Conta()
                            {
                                Codigo = Convert.ToInt32(reader["codigo"]),
                                Numero = Convert.ToInt32(reader["numero"]),
                                Agencia = Convert.ToInt32(reader["agencia"]),
                                Tipo = Convert.ToInt32(reader["tipo"]),
                                Saldo = Convert.ToDouble(reader["saldo"])
                            });
                        }
                    }
                }
            }

            return contas;
        }

        internal static List<Conta> BuscaPorCliente(int codigoInterno)
        {
                return Conta.Lista("where numero =" + codigoInterno);
        }

        public static Conta BuscaPorId(int id)
        {
            return Conta.Lista("where codcontas =" + id)[0];
        }

             
    }
}



