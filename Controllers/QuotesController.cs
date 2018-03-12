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
        public IActionResult Index(string search, string sortOrder)
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
                                    " on m.materialID = d.materialID ";
            
            if (search != null) {  mySelectQuery += " WHERE q.lastName LIKE '%" + search + "%'";  }
            if (sortOrder != null) { mySelectQuery += " ORDER BY " + sortOrder; }

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
                        Date = DateTime.Parse(myReader["dateAdded"].ToString()),
                        FirstName = myReader["firstName"].ToString(),
                        LastName = myReader["lastName"].ToString(),
                        DeskWidth = Double.Parse(myReader["deskWidth"].ToString()),
                        DeskLength = Double.Parse(myReader["deskLength"].ToString()),
                        Drawers = Int32.Parse(myReader["drawers"].ToString()),
                        Material = myReader["description"].ToString(),
                        MaterialPrice = Double.Parse(myReader["price"].ToString()),
                        ShippingDays = Int32.Parse(myReader["shippingDays"].ToString()),
                        ShippingPrice = Double.Parse(myReader["shippingPrice"].ToString()),
                        Amount = Double.Parse(myReader["quoteAmount"].ToString())


                    });
            }

            QuotesViewModel vm = new QuotesViewModel
            {
                myQuotes = myQuotes,
                search = search,
                sort = sortOrder

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

        [HttpPost]
        public IActionResult NewQuote(NewQuotesViewModel newQuote)
        {

            if (ModelState.IsValid)
            {

                //calculate total quote price
               



                    double quotePrice = 200;

                    double sqInSize = (Double) (newQuote.deskWidth * newQuote.deskLength);
                    quotePrice += (sqInSize) / 1000;
                    quotePrice += (Double)(newQuote.drawers * 50);

                    double materialFee = 0;
                    //material price comes from database
                    string materialPrice = "SELECT price FROM Material WHERE description = '" + newQuote.material + "';";

                    MySqlConnection myConnection = new MySqlConnection(connecitonString);
                    MySqlCommand myCommand = new MySqlCommand(materialPrice, myConnection);
                    myConnection.Open();
                    MySqlDataReader myReader;
                    myReader = myCommand.ExecuteReader();
                    // Always call Read before accessing data.
                    while (myReader.Read())
                    {
                        materialFee = Double.Parse(myReader["price"].ToString());
                        quotePrice += materialFee;
                    }
                    myConnection.Close();

                //add in shipping cost
                double shippingFee = 0;
                switch (newQuote.shippingDays)
                    {
                        case 14:
                            //no additional cose
                            break;
                        case 3:
                            if (sqInSize < 1000)
                            {
                                shippingFee = 60;
                                quotePrice += 60;
                            }
                            else if (sqInSize >= 1000 && sqInSize < 2000)
                            {
                                shippingFee = 70;
                                quotePrice += 70;
                            }
                            else
                            {
                                shippingFee = 80;
                                quotePrice += 80;
                            }
                            break;
                        case 5:
                            if (sqInSize < 1000)
                            {
                                shippingFee = 40;
                                quotePrice += 40;
                            }
                            else if (sqInSize >= 1000 && sqInSize < 2000)
                            {
                                shippingFee = 50;
                                quotePrice += 50;
                            }
                            else
                            {
                                shippingFee = 60;
                                quotePrice += 60;
                            }
                            break;
                        case 7:
                            if (sqInSize < 1000)
                            {
                                shippingFee = 30;
                                quotePrice += 30;
                            }
                            else if (sqInSize >= 1000 && sqInSize < 2000)
                            {
                                shippingFee = 30;
                                quotePrice += 35;
                            }
                            else
                            {
                                shippingFee = 40;
                                quotePrice += 40;
                            }
                            break;
                    }

                    quotePrice = Math.Round(quotePrice, 2);

    
                myConnection = new MySqlConnection(connecitonString);
                MySqlCommand mySqlCommand = new MySqlCommand("insert_quote", myConnection);
                mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("pv_firstName", newQuote.FirstName);
                mySqlCommand.Parameters.AddWithValue("pv_lastName", newQuote.LastName);
                mySqlCommand.Parameters.AddWithValue("pv_shippingDays", newQuote.shippingDays);
                mySqlCommand.Parameters.AddWithValue("pv_shippingPrice", shippingFee);
                mySqlCommand.Parameters.AddWithValue("pv_quoteAmount", quotePrice);
                mySqlCommand.Parameters.AddWithValue("pv_description", newQuote.material);  //mat des
                mySqlCommand.Parameters.AddWithValue("pv_price", materialFee);  //mat cost
                mySqlCommand.Parameters.AddWithValue("pv_deskWidth", newQuote.deskWidth);
                mySqlCommand.Parameters.AddWithValue("pv_deskLength", newQuote.deskLength);
                mySqlCommand.Parameters.AddWithValue("pv_drawers", newQuote.drawers);

                myConnection.Open();
                //MySqlDataReader myReader;
                myReader = mySqlCommand.ExecuteReader();
                myConnection.Close();

                //string tmp = firstName + " " + lastName + " " + deskWidth + " " + deskLength + " " + material + " " + shippingDays;

                //return tmp;
                return View("View");

            }
            else
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

                MaterialsViewModel vm = new MaterialsViewModel { material = myMaterials };

                NewQuotesViewModelResponce vm2 = new NewQuotesViewModelResponce
                {
                    responceVM = newQuote,
                    materialsVM = vm
                };

                return View("NewQuoteWithErrors", vm2);
            }
        }


        public IActionResult Delete(int Id)
        {
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
                                    " on m.materialID = d.materialID " +
                                    " WHERE q.quoteId = " + Id;
            MySqlConnection myConnection = new MySqlConnection(connecitonString);
            MySqlCommand myCommand = new MySqlCommand(mySelectQuery, myConnection);
            myConnection.Open();
            MySqlDataReader myReader;
            myReader = myCommand.ExecuteReader();
            Quote quote = new Quote();
            // Always call Read before accessing data
            while (myReader.Read())
            {
                quote.Id = Int32.Parse(myReader["quoteID"].ToString());
                quote.Date = DateTime.Parse(myReader["dateAdded"].ToString());
                quote.FirstName = myReader["firstName"].ToString();
                quote.LastName = myReader["lastName"].ToString();
                quote.DeskWidth = Double.Parse(myReader["deskWidth"].ToString());
                quote.DeskLength = Double.Parse(myReader["deskLength"].ToString());
                quote.Drawers = Int32.Parse(myReader["drawers"].ToString());
                quote.Material = myReader["description"].ToString();
                quote.MaterialPrice = Double.Parse(myReader["price"].ToString());
                quote.ShippingDays = Int32.Parse(myReader["shippingDays"].ToString());
                quote.ShippingPrice = Double.Parse(myReader["shippingPrice"].ToString());
                quote.Amount = Double.Parse(myReader["quoteAmount"].ToString());
            }
            return View(quote);
        }


        public IActionResult DeleteRecord(int Id)
        {
            MySqlConnection myConnection = new MySqlConnection(connecitonString);
            MySqlCommand mySqlCommand = new MySqlCommand("deleterecord", myConnection);
            mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            mySqlCommand.Parameters.AddWithValue("pv_quoteID", Id);

            myConnection.Open();
            MySqlDataReader myReader;
            myReader = mySqlCommand.ExecuteReader();
            myConnection.Close();

            return RedirectToAction("Index");
        }
    }

}