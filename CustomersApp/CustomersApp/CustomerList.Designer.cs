
using System.Configuration;
using System.Data.SqlClient;

namespace CustomersApp
{
    partial class CustomerList
    {
        private System.ComponentModel.IContainer components = null;       
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grdCustomers = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdCustomers)).BeginInit();
            this.SuspendLayout();
                                     
            // Customers grid settings            
            this.grdCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCustomers.Location = new System.Drawing.Point(12, 12);
            this.grdCustomers.Name = "grdCustomers";
            this.grdCustomers.RowTemplate.Height = 25;
            this.grdCustomers.Size = new System.Drawing.Size(422, 490);
            this.grdCustomers.TabIndex = 0;
                        
            // Customer list window settings
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 514);
            this.Controls.Add(this.grdCustomers);
            this.Name = "CustomerList";
            this.Text = "List of Customers";
            this.Load += new System.EventHandler(this.CustomerList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCustomers)).EndInit();
            this.ResumeLayout(false);            
        }

        private System.Windows.Forms.DataGridView grdCustomers;
    }
}

