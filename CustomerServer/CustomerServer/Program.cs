using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace CustomerServer
{
    class Program
    {
        // Setup Socket and database connection string
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string constring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        static void Main(string[] args)
        {            
            // Lets try to connect
            ConnectClient();

            // Sending out messages on birthdate request
            SendMessages();

            // We will keep the window running
            Console.ReadLine();
        }

        private static byte[] GetCustomers(string dateofbirth)
        {

            // This list will collect set of results
            List<Customer> customerList = new List<Customer>();

            try
            {
                Convert.ToDateTime(dateofbirth).ToString("dd-MM-yyyy");

                using (SqlConnection con = new SqlConnection(constring))
                {
                    // Create a new SqlCommand object
                    using (SqlCommand command = new SqlCommand("pr_GetCustomers", con))
                    {
                        // Setup command as stored procedure and setup parameter
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@dateofbirth", dateofbirth);

                        // Open connection if its closed
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            // Load results set to the generic list
                            customerList.Add(new Customer { Id = (int)reader.GetInt32("Id"), Name = reader.GetString("Name"), DateOfBirth = (DateTime)reader.GetDateTime("DateOfBirth") });
                        }
                    }
                }
            }
            catch (Exception) 
            {
                Console.WriteLine("Invalid Input!");

            }
            

            // Lets convert the results list into byte array so we can send to client
            var obj = System.Text.Json.JsonSerializer.Serialize(customerList);

            // return the converted object
            return System.Text.Encoding.ASCII.GetBytes(obj);
        }

        // Establish connection here
        private static void ConnectClient()
        {
            // count how many connections are there
            int attemps = 0;

            //Try to connect client, this will keep trying to connect
            while (!_clientSocket.Connected)
            {
                try
                {
                    attemps++;
                    _clientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    // In case there is no connection we will just clear the screen and show the error message
                    Console.Clear();
                    Console.WriteLine("Connection attemp failed: " + attemps.ToString());
                }
            }

            // Clear screen and show connected message
            Console.Clear();
            Console.WriteLine("Connected...");
        }


        // Send message to client
        private static void SendMessages()
        {
            while (true)
            {
                // Prompt for date of birth
                Console.Write("Enter Birth Date: ");
                string request = Console.ReadLine();

                // Run this to execute the stored proc & send message to customer app
                try
                {
                    _clientSocket.Send(GetCustomers(request));
                    Console.WriteLine("Sent!");
                }
                catch (SocketException)
                {
                    // If sending message attemp fails then display this error
                    Console.Clear();
                    Console.WriteLine("Connection failed");
                }
                
            }
        }

    }
}
