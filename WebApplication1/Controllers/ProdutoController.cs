using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProdutoController : Controller
    {
        // GET: Produto

        public ActionResult Index(Produto pro)
        {
            var lstProduto = new List<Produto>();
            using (var conexao = new Conexao())
            {
                string strProdutos = "SELECT * FROM produtos where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strProdutos, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var produto = new Produto
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Quantidade = Convert.ToInt32(dr["quantidade"])

                            };

                            lstProduto.Add(produto);
                        }
                    ViewBag.ListaProduto = lstProduto;
                }
            }

            using (var conexao = new Conexao())
            {

                string strProdutos = "SELECT * FROM produtos " +
                "WHERE nome like @nome and " +
                "isExcluido = false;";

                using (var comando = new MySqlCommand(strProdutos, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", pro.Nome + "%");

                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.HasRows)
                    {
                        var lstpro = new List<Produto>();

                        while (dr.Read())
                        {
                            var produto = new Produto
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Quantidade = Convert.ToInt32(dr["quantidade"])
                                // Para levar pra view, traz do banco de dados
                                // em formato DateTime e converte
                                // para string para formatar para o usuário
                            };

                            lstProduto.Add(produto);
                        }
                        ViewBag.ListaProduto = lstProduto;
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        public ActionResult NovoProduto()
        {
            var lstProduto = new List<Produto>();
            using (var conexao = new Conexao())
            {
                string strProduto = "SELECT * FROM produtos where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strProduto, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var produto = new Produto
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Quantidade = Convert.ToInt32(dr["quantidade"])
                            };
                            lstProduto.Add(produto);
                        }
                    ViewBag.ListaProduto = lstProduto;
                }
            }
            return View();
        }
        public ActionResult SalvarProduto(Produto produto)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "INSERT INTO produtos (nome, quantidade) " +
                                  "values (" +
                                  "@nome, @quantidade);";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", produto.Nome);
                    comando.Parameters.AddWithValue("@quantidade", produto.Quantidade);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult _FrmProduto()
        {
            return View();
        }
    }
}