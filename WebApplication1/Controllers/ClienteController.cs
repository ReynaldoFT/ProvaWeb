using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index(Cliente cli)
        {
            var lstVendedores = new List<Cliente>();
            using (var conexao = new Conexao())
            {
                string strVendedores = "SELECT * FROM usuarios where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strVendedores, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var cliente = new Cliente
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"])
                            };

                            lstVendedores.Add(cliente);
                        }
                    ViewBag.ListaVendedores = lstVendedores;
                }
            }

            using (var conexao = new Conexao())
            {

                string strClientes = "SELECT * FROM clientes " +
                "WHERE nome like @nome and " +
                "isExcluido = false;";

                using (var comando = new MySqlCommand(strClientes, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", cli.Nome + "%");

                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.HasRows)
                    {
                        var lstClientes = new List<Cliente>();

                        while (dr.Read())
                        {
                            var cliente = new Cliente
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Telefone = Convert.ToString(dr["telefone"]),
                                EMail = Convert.ToString(dr["email"]),
                                // Para levar pra view, traz do banco de dados
                                // em formato DateTime e converte
                                // para string para formatar para o usuário
                                DataNasc = Convert.ToDateTime(dr["dataNasc"]).ToString("dd/MM/yyyy")
                            };

                            lstClientes.Add(cliente);
                        }
                        ViewBag.ListaClientes = lstClientes;
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        public ActionResult NovoCliente()
        {
            var lstVendedores = new List<Cliente>();
            using (var conexao = new Conexao())
            {
                string strVendedores = "SELECT * FROM usuarios where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strVendedores, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var cliente = new Cliente
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"])
                            };
                            lstVendedores.Add(cliente);
                        }
                    ViewBag.ListaVendedores = lstVendedores;
                }
            }
            return View();
        }

        public ActionResult SalvarCliente(Cliente cliente)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "INSERT INTO clientes (nome, telefone, email, dataNasc) " +
                                  "values (" +
                                  "@nome, @telefone, @email, @dataNasc);";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", cliente.Nome);
                    comando.Parameters.AddWithValue("@telefone", cliente.Telefone);
                    comando.Parameters.AddWithValue("@email", cliente.EMail);
                    comando.Parameters.AddWithValue("@dataNasc", cliente.DataNasc);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult EditarCliente()
        {

            return View();
        }
    }
}