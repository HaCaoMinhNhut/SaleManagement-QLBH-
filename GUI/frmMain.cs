using BUS;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmMain : Form
    {
        BUS_Employee busEmployee = new BUS_Employee();
        frmEmployee fEmployee = new frmEmployee();
        frmProduct fProduct = new frmProduct();
        frmCustomer fCustomer = new frmCustomer();
        frmAccount fAccount;
        frmBill fBill;
        frmHoaDon frmHD;
        static string Email;

        public frmMain(string email)
        {
            InitializeComponent();
            if (!busEmployee.GetEmployeeRole(email))
            {
                btnEmployee.Visible = false;
                btnProduct.Checked = true;
                fProduct.TopLevel = false;
                fProduct.Dock = DockStyle.Fill;
                pnlBody.Controls.Add(fProduct);
                fProduct.Show();
            }
       
            fAccount = new frmAccount(email);
            //   fBill = new frmBill(email);
            Email = email;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            frmEmployee frmEmployee = new frmEmployee(sender,e);
            pnlBody.Controls.Clear();
            fEmployee.TopLevel = false;
            fEmployee.Dock = DockStyle.Fill;
            pnlBody.Controls.Add(fEmployee);
            fEmployee.Show();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            frmProduct frm = new frmProduct(sender, e);
            pnlBody.Controls.Clear();
            frm.TopLevel = false;
            pnlBody.Controls.Add(frm);
            frm.Dock = DockStyle.Fill;
            frm.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            frmCustomer frm = new frmCustomer(sender, e);
            pnlBody.Controls.Clear();
            fCustomer.TopLevel = false;
            pnlBody.Controls.Add(fCustomer);
            fCustomer.Dock = DockStyle.Fill;
            fCustomer.Show();
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
           // fBill = new frmBill(Email);
            frmHoaDon frm = new frmHoaDon(Email,sender,e);
            {

               pnlBody.Controls.Clear();
                frm.TopLevel = false;
            pnlBody.Controls.Add(frm);
                frm.Dock = DockStyle.Fill;
                frm.Show();
            }
          
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            pnlBody.Controls.Clear();
            fAccount.TopLevel = false;
            pnlBody.Controls.Add(fAccount);
            fAccount.Dock = DockStyle.Fill;
            fAccount.Show();
        }
    }
}
