using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MarcaController : Controller
    {
        // GET: Marca

        public ActionResult Index(Marca mar)
        {
            var lstMarca = new List<Marca>();
            using (var conexao = new Conexao())
            {
                string strMarcas = "SELECT * FROM marcas where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strMarcas, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var marca = new Marca
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),

                            };

                            lstMarca.Add(marca);
                        }
                    ViewBag.ListaMarca = lstMarca;
                }
            }

            using (var conexao = new Conexao())
            {

                string strClientes = "SELECT * FROM marcas " +
                "WHERE nome like @nome and " +
                "isExcluido = false;";

                using (var comando = new MySqlCommand(strClientes, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", mar.Nome + "%");

                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.HasRows)
                    {
                        var lstMar = new List<Marca>();

                        while (dr.Read())
                        {
                            var marca = new Marca
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                // Para levar pra view, traz do banco de dados
                                // em formato DateTime e converte
                                // para string para formatar para o usuário
                            };

                            lstMar.Add(marca);
                        }
                        ViewBag.ListaMarca = lstMar;
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        public ActionResult NovoMarca()
        {
            var lstMarca = new List<Marca>();
            using (var conexao = new Conexao())
            {
                string strMarca = "SELECT * FROM marcas where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strMarca, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var marca = new Marca
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                            };
                            lstMarca.Add(marca);
                        }
                    ViewBag.ListaVendedores = lstMarca;
                }
            }
            return View();
        }
        public ActionResult Salvarmarca(Marca marca)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "INSERT INTO marcas (nome) " +
                                  "values (" +
                                  "@nome);";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", marca.Nome);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult _FrmMarca()
        {
            return View();
        }
    }
}