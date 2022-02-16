using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomersApp
{
    public partial class CustomerList : Form
    {
        // Setup socket connection
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Socket> _serverSockets2 = new List<Socket>();
        private static byte[] _inputdata = new byte[1000];   

        public CustomerList()
        {
            InitializeComponent();
        }

        private void CustomerList_Load(object sender, EventArgs e)
        {
            // Setup socket connection
            StartConnection();            
        }

        private void StartConnection()
        {
            Console.WriteLine("Setting Connection...");

            // Binding socket
            _clientSocket.Bind(new IPEndPoint(IPAddress.Any, 100));

            //Start listening - at least can allocate at least 1 pending connection
            _clientSocket.Listen(1);
            _clientSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);

        }

        private void AcceptCallBack(IAsyncResult AR)
        {
            Socket socket = _clientSocket.EndAccept(AR);
            _serverSockets2.Add(socket);
            Console.WriteLine("Connected...");

            // We will start receiving the data
            socket.BeginReceive(_inputdata, 0, _inputdata.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
            _clientSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null); // this allow to accept more than 1 connection
        }

        private void ReceiveCallBack(IAsyncResult AR)
        {
            // Receiving embended data from server
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);

            //
            byte[] data = new byte[received];
            Array.Copy(_inputdata, data, received);
            Console.WriteLine("Received: " + data);

            // Lets now load the received customer list in the grid
            LoadDataGrid(data);
        }

        // Load grid with a list of customers
        private void LoadDataGrid(byte[] inputdata)
        {            
            // Deserialize converted object
            var obj = System.Text.Encoding.ASCII.GetString(inputdata);
            List<Customer> customers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(obj);

            // We will onvoke the data grid at run-time
            if (grdCustomers.InvokeRequired)
            {
                // First clear & re-create columns (required before adding rows)
                grdCustomers.Invoke(new Action(() => grdCustomers.Columns.Clear()));
                grdCustomers.Invoke(new Action(() => grdCustomers.Columns.Add("Id", "Id")));
                grdCustomers.Invoke(new Action(() => grdCustomers.Columns.Add("Name", "Name")));
                grdCustomers.Invoke(new Action(() => grdCustomers.Columns.Add("DateOfBirth", "DateOfBirth")));
                
                // Lets clear available rows and refresh the grid
                grdCustomers.Invoke(new Action(() => grdCustomers.Rows.Clear()));
                grdCustomers.Invoke(new Action(() => grdCustomers.Refresh()));

                // Loop through the results & post data to the grid
                foreach(var i in customers)
                {
                    // This adds the actual results to the grid
                    grdCustomers.Invoke(new Action(() => grdCustomers.Rows.Add(i.Id,i.Name, i.DateOfBirth)));
                }

            }
        }
    }
}
