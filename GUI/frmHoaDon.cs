using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.ClsData;

namespace GUI
{
    public partial class frmHoaDon : Form
    {
        ChiTietHoaDon frm;
     static   DataTable dt = new DataTable();
        public frmHoaDon(string email, object sender, EventArgs e)
        {
            InitializeComponent();

            frm = new ChiTietHoaDon(email);
            frmHoaDon_Load(sender, e);
        }
        public frmHoaDon(object sender, EventArgs e)
        {
            InitializeComponent();
            frmHoaDon_Load(sender, e);
        }    
        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            Methods.Connect();
          string  sqlQuery = "SELECT*FROM HOADON";
        
            dt = Methods.GetDataToTable(sqlQuery);
            gvBill.DataSource = dt;
            Methods.Disconnect();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
          
           
            frm.Show();
            
        }

        private void gvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = gvBill.Rows[e.RowIndex];
            txtMaHD.Text = Convert.ToString(row.Cells["MAHD"].Value);
        }

        private void btnxem_Click(object sender, EventArgs e)
        {
            if (txtMaHD.Text != "")
            {
                ChiTietHoaDon form = new ChiTietHoaDon(txtMaHD.Text);
                form.Show();
            }
            else
                MessageBox.Show("Vui lòng điền thông tin");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM CHITIETHOADON WHERE MAHD='" + txtMaHD.Text + "'";
            Methods.Connect();
            Methods.RunSQL(sql);
            sql="DELETE FROM HOADON WHERE MAHD='"+txtMaHD.Text+"'";
            Methods.Connect();
            Methods.RunSQL(sql);
            frmHoaDon_Load(sender,e);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            InHoaDon frm = new InHoaDon(txtMaHD.Text);
            frm.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string sql = "SELECT*FROM HOADON WHERE MAHD LIKE N'%"+txtSearch.Text+"%'";
            DataTable dt = new DataTable();
            dt = Methods.GetDataToTable(sql);
            gvBill.DataSource = dt;
        }
    }
}
