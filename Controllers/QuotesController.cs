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

        [HttpPost]
        public string NewQuote(string firstName, string lastName, double deskWidth, double deskLength, string material, int shippingDays, int drawers){



            MySqlConnection myConnection = new MySqlConnection(connecitonString);
            MySqlCommand mySqlCommand = new MySqlCommand("insert_quote", myConnection);
            mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            mySqlCommand.Parameters.AddWithValue("pv_firstName", firstName);
            mySqlCommand.Parameters.AddWithValue("pv_lastName", lastName);
            mySqlCommand.Parameters.AddWithValue("pv_shippingDays", shippingDays);
            mySqlCommand.Parameters.AddWithValue("pv_shippingPrice", "35");
            mySqlCommand.Parameters.AddWithValue("pv_quoteAmount", "35.45");
            mySqlCommand.Parameters.AddWithValue("pv_description", material);  //mat des
            mySqlCommand.Parameters.AddWithValue("pv_price", "100.00");  //mat cost
            mySqlCommand.Parameters.AddWithValue("pv_deskWidth", deskWidth);
            mySqlCommand.Parameters.AddWithValue("pv_deskLength", deskLength);
            mySqlCommand.Parameters.AddWithValue("pv_drawers", drawers);

            myConnection.Open();
            MySqlDataReader myReader;
            myReader = mySqlCommand.ExecuteReader();

            string tmp = firstName + " " + lastName + " " + deskWidth + " " + deskLength + " " + material + " " + shippingDays;

            return tmp;

        }

    }

}
/*

public void ViewByID()
{

    {

        MySqlConnection myConnection = new MySqlConnection(connecitonString);
        MySqlCommand mySqlCommand = new MySqlCommand("QuotesViewByID", myConnection);
        mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
        mySqlCommand.Parameters.AddWithValue("_QuoteID", "1");
        myConnection.Open();
        MySqlDataReader myReader;
        myReader = mySqlCommand.ExecuteReader();
        // Always call Read before accessing data.
        while (myReader.Read())
        {
            Console.WriteLine(myReader.GetInt32(0) + ", " + myReader.GetString(1));
        }
        // always call Close when done reading.
        myReader.Close();
        // Close the connection when done with it.
        myConnection.Close();


    }

}

*/