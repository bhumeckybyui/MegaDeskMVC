using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MegaDeskMVC.Models;


namespace mysqltesting.Models
{
    public class DataBase
    {

        string connecitonString = @"Server=132.148.86.237;Database=MegaDesk;Uid=megadesk;Pwd=megadesk";
        public List<Quote> myQuotes = new List<Quote>();


        public DataBase()
        {

            FillQuotes();

        }

        public void CheckDatabase(){


            using(MySqlConnection mysqlConn = new MySqlConnection(connecitonString)){
                mysqlConn.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("QuotesViewByID", mysqlConn);
                mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_QuoteID", "1");


            }

        }


        public void FillQuotes()
        {
            string mySelectQuery = "SELECT" +
                                    "q.quoteID" +
                                    ", q.dateAdded" +
                                    ", q.firstName" +
                                    ", q.last_name" +
                                    ", q.deskID" +
                                    ", q.shippingDays" +
                                    ", q.shippingPrice" +
                                    ", q.quoteAmount" +
                                    ", d.deskWidth" +
                                    ", d.deskLength" +
                                    ", d.drawers" +
                                    ", d.materialID" +
                                    ", m.materialID" +
                                    ", m.description" +
                                    ", m.price" +
                                    "FROM Quote q" +
                                    "INNER JOIN(" +
                                    "SELECT" +
                                    "deskId," +
                                    "deskWidth," +
                                    "deskLength," +
                                    "drawers," +
                                    "materialID" +
                                    "FROM Desk) AS d" +
                                    "on d.deskId = q.deskID" +
                                    "INNER JOIN(" +
                                    "SELECT" +
                                    "materialID," +
                                    "description," +
                                    "price" +
                                    "FROM Material) AS m" +
                                    "on m.materialID = d.materialID;";
            
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
                    Id = Int32.Parse(myReader["firstName"].ToString()),
                    Date = DateTime.Parse(myReader["dateAdded"].ToString()),
                    FirstName = myReader["firstName"].ToString(),
                    LastName = myReader["last_name"].ToString(),
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
        }

        public void CheckDataBase2(){


                
            string mySelectQuery = "SELECT" +
                                    "q.quoteID" +
                                    ", q.dateAdded" +
                                    ", q.firstName" +
                                    ", q.last_name" +
                                    ", q.deskID" +
                                    ", q.shippingDays" +
                                    ", q.shippingPrice" +
                                    ", q.quoteAmount" +
                                    ", d.deskWidth" +
                                    ", d.deskLength" +
                                    ", d.drawers" +
                                    ", d.materialID" +
                                    ", m.materialID" +
                                    ", m.description" +
                                    ", m.price" +
                                    "FROM Quote q" +
                                    "INNER JOIN(" +
                                    "SELECT" +
                                    "deskId," +
                                    "deskWidth," +
                                    "deskLength," +
                                    "drawers," +
                                    "materialID" +
                                    "FROM Desk) AS d" +
                                    "on d.deskId = q.deskID" +
                                    "INNER JOIN(" +
                                    "SELECT" +
                                    "materialID," +
                                    "description," +
                                    "price" +
                                    "FROM Material) AS m" +
                                    "on m.materialID = d.materialID;";
            
                    MySqlConnection myConnection = new MySqlConnection(connecitonString);
                    MySqlCommand myCommand = new MySqlCommand(mySelectQuery, myConnection);
                    myConnection.Open();
                    MySqlDataReader myReader;
                    myReader = myCommand.ExecuteReader();
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

        public void insert(){
            
            {
               
                MySqlConnection myConnection = new MySqlConnection(connecitonString);
                string myInsertQuery = "INSERT INTO Orders (id, customerId, amount) Values(1001, 23, 30.66)";
                MySqlCommand myCommand = new MySqlCommand(myInsertQuery);
                myCommand.Connection = myConnection;
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();

            }

        }

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
    }
}
