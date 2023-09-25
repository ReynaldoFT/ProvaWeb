using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VendedorController : Controller
    {
        // GET: Vendedor

        public ActionResult Index(Vendedor ven)
        {
            var lstVendedores = new List<Vendedor>();
            using (var conexao = new Conexao())
            {
                string strVendedores = "SELECT * FROM vendedor where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strVendedores, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var vendedor = new Vendedor
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                
                            };

                            lstVendedores.Add(vendedor);
                        }
                    ViewBag.ListaVendedores = lstVendedores;
                }
            }

            using (var conexao = new Conexao())
            {

                string strVendedores = "SELECT * FROM vendedor " +
                "WHERE nome like @nome and " +
                "isExcluido = false;";

                using (var comando = new MySqlCommand(strVendedores, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", ven.Nome + "%");

                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.HasRows)
                    {
                        var lstVendedor = new List<Vendedor>();

                        while (dr.Read())
                        {
                            var vendedor = new Vendedor
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Cpf = Convert.ToString(dr["cpf"]),
                                DataNasc = Convert.ToDateTime(dr["dataNasc"]).ToString("dd/MM/yyyy")
                                // Para levar pra view, traz do banco de dados
                                // em formato DateTime e converte
                                // para string para formatar para o usuário
                            };

                            lstVendedor.Add(vendedor);
                        }
                        ViewBag.ListaVendedores = lstVendedor;
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        public ActionResult NovoVendedor()
        {
            var lstVendedores = new List<Vendedor>();
            using (var conexao = new Conexao())
            {
                string strVendedores = "SELECT * FROM vendedor where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strVendedores, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var vendedor = new Vendedor
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Cpf = Convert.ToString(dr["cpf"])
                            };
                            lstVendedores.Add(vendedor);
                        }
                    ViewBag.ListaVendedores = lstVendedores;
                }
            }
            return View();
        }
        public ActionResult SalvarVendedor(Vendedor vendedor)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "INSERT INTO vendedor (nome, cpf, dataNasc) " +
                                  "values (" +
                                  "@nome, @cpf, @dataNasc);";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", vendedor.Nome);
                    comando.Parameters.AddWithValue("@cpf", vendedor.Cpf);
                    comando.Parameters.AddWithValue("@dataNasc", vendedor.DataNasc);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult _FrmVendedor()
        {
            return View();
        }
    }
}