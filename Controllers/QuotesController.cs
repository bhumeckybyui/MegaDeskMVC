using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MegaDeskMVC.Models;
using MySql.Data.MySqlClient;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MegaDeskMVC.Controllers
{
    public class QuotesController : Controller
    {

        string connecitonString = @"Server=132.148.86.237;Database=MegaDesk;Uid=megadesk;Pwd=megadesk";
        // GET: /<controller>/
        public IActionResult Index()
        {

            List<Quote> myQuotes = new List<Quote>();


            string mySelectQuery = "SELECT" +
                                    " q.quoteID " +
                                    ", q.dateAdded " +
                                    ", q.firstName " +
                                    ", q.lastName " +
                                    ", q.deskID " +
                                    ", q.shippingDays " +
                                    ", q.shippingPrice " +
                                    ", q.quoteAmount " +
                                    ", d.deskWidth " +
                                    ", d.deskLength " +
                                    ", d.drawers " +
                                    ", d.materialID " +
                                    ", m.materialID " +
                                    ", m.description " +
                                    ", m.price " +
                                    " FROM Quote q " +
                                    " INNER JOIN(" +
                                    " SELECT" +
                                    " deskId, " +
                                    " deskWidth, " +
                                    " deskLength, " +
                                    " drawers, " +
                                    " materialID " +
                                    " FROM Desk ) AS d " +
                                    " on d.deskId = q.deskID " +
                                    " INNER JOIN(" +
                                    " SELECT " +
                                    " materialID," +
                                    " description," +
                                    " price " +
                                    " FROM Material) AS m " +
                                    " on m.materialID = d.materialID; ";

            MySqlConnection myConnection = new MySqlConnection(connecitonString);
            MySqlCommand myCommand = new MySqlCommand(mySelectQuery, myConnection);
            myConnection.Open();
            MySqlDataReader myReader;
            myReader = myCommand.ExecuteReader();
            // Always call Read before accessing data.
            while (myReader.Read())
            {
                myQuotes.Add(
                new Quote
                {

                        Id = Int32.Parse(myReader["quoteID"].ToString()),
                        Date = myReader["dateAdded"].ToString(),
                        FirstName = myReader["firstName"].ToString(),
                        LastName = myReader["lastName"].ToString(),
                        deskWidth = Double.Parse(myReader["deskWidth"].ToString()),
                        deskLength = Double.Parse(myReader["deskLength"].ToString()),
                        drawers = Int32.Parse(myReader["drawers"].ToString()),
                        matrial = myReader["description"].ToString(),
                        materialPrice = Double.Parse(myReader["price"].ToString()),
                        shippingDays = Int32.Parse(myReader["shippingDays"].ToString()),
                        shippingPrice = Double.Parse(myReader["shippingPrice"].ToString()),
                        Amount = Double.Parse(myReader["quoteAmount"].ToString())


                    });
            }

            QuotesViewModel vm = new QuotesViewModel
            {
                myQuotes = myQuotes
            };


           

            return View(vm);
        }
        public IActionResult NewQuote()
        {

            List<Material> myMaterials = new List<Material>();
            string sql = "SELECT * FROM Material ORDER BY price;";
            MySqlConnection myConnection = new MySqlConnection(connecitonString);
            MySqlCommand myCommand = new MySqlCommand(sql, myConnection);
            myConnection.Open();
            MySqlDataReader myReader;
            myReader = myCommand.ExecuteReader();
            // Always call Read before accessing data.
            while (myReader.Read())
            {
                myMaterials.Add(
                    new Material
                    {
                        Id = Int32.Parse(myReader["materialID"].ToString()),
                        Description = myReader["description"].ToString(),
                        Price = Double.Parse(myReader["price"].ToString())
                    });

            }

            MaterialsViewModel vm = new MaterialsViewModel{ material = myMaterials };

            return View(vm);
        }

    }


    /*
    [HttpPost]
    public IActionResult NewQuote(string firstName, string lastName, double deskWidth, double deskLength, int material, int shippingDays ){
        //public string NewQuote(string firstName, string lastName, double deskWidth, double deskLength, int material, int shippingDays ){

        return View();

    }

    */
}
