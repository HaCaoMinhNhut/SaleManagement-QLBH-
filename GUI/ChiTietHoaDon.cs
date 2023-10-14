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
    public partial class ChiTietHoaDon : Form
    {
        public ChiTietHoaDon(string email)
        {
            InitializeComponent();

            LoadTenNV(email);
        }
    
     static   DataTable dtKH = new DataTable();
      static  DataTable dtSP=new DataTable();
        static DataTable dtHD=new DataTable();
      static  string tennv="";
      static  string manv="";
        private DateTime dateTime = new DateTime();
        void LoadTenNV(string email)
        {
          Methods.Connect();
            DataTable dt= new DataTable();  
            string sql = "SELECT MANV,HOTEN FROM NHANVIEN WHERE EMAIL=N'"+email+"'";
                dt=Methods.GetDataToTable(sql);
            if (Methods.CheckKey(sql))
            {
                 manv = dt.Rows[0]["MANV"].ToString().Trim();
              tennv = dt.Rows[0]["HOTEN"].ToString();
                txtEmployeeIdName.Text = manv + "|" + tennv;
                dateTime = DateTime.Now;
                txtDateTime.Text = dateTime.ToString("MM/dd/yyyy") + " " + dateTime.ToString("HH:mm:ss");
                txtmaHD.Text = "HD" + dateTime.ToString("ddMM") + dateTime.ToString("HHmm");
            }
            else//trường hợp email là mã hóa đơn => là đang xem chi tiết
            { 
                sql = "SELECT*FROM HOADON WHERE MAHD='"+email+"'";
            dt=Methods.GetDataToTable(sql);
                cboCustomerIdName.Text = dt.Rows[0]["MAKH"].ToString();
                LoadKH();
                txtDateTime.Text = dt.Rows[0]["NGAY"].ToString();
                cboCustomerIdName.Enabled = false;
                txtmaHD.Text = email;
                LoadCTHD(dt.Rows[0]["MAHD"].ToString());
                LoadHD();
            }
        }

      

        private void ChiTietHoaDon_Load(object sender, EventArgs e)
        {
            Methods.Connect();
            LoadKH();
            LoadSP();

        }

        void LoadCTHD(string ma)
        {
            int s = 0;
            string sql = "SELECT MASP,TENSP,SOLUONG,GIABAN,THANHTIEN FROM CHITIETHOADON WHERE MAHD=N'"+ma+"'";
            DataTable dt = Methods.GetDataToTable(sql);
            gvBill.DataSource = dt;
            for(int i=0;i<dt.Rows.Count;i++)
            {
                s += Int32.Parse(dt.Rows[i]["THANHTIEN"].ToString());
            }    
            txtTongtien.Text=s.ToString();
            sql = "UPDATE HOADON SET TONGTIEN='" +txtTongtien.Text+"' WHERE MAHD='"+txtmaHD.Text+"'";
            Methods.RunSQL(sql);
        }
        void LoadHD()
        {
            string sql = "SELECT*FROM HOADON WHERE MAHD='"+txtmaHD.Text+"'";
            dtHD=Methods.GetDataToTable(sql);
            string ma = dtHD.Rows[0]["MANV"].ToString();
            sql = "SELECT MANV,HOTEN FROM NHANVIEN WHERE MANV='"+ma+"'";
            dtHD = Methods.GetDataToTable(sql);
            txtEmployeeIdName.Text = dtHD.Rows[0]["MANV"].ToString().Trim()+"|"+ dtHD.Rows[0]["HOTEN"].ToString();

        }
     
        void LoadSP()
        {
        
            string sql = "SELECT MASP,TENSP,GIABAN,SOLUONG FROM SANPHAM";
            dtSP=Methods.GetDataToTable(sql);
            for(int i=0;i<dtSP.Rows.Count;i++)
            {
                string ma = dtSP.Rows[i]["MASP"].ToString();
              //  string sl = dtSP.Rows[i]["SOLUONG"].ToString();
                string ten = dtSP.Rows[i]["TENSP"].ToString();
                cboProductNameQuantity.Items.Add(ma.Trim());
            }
        }
        void LoadGia()
        {
            String[] sp = cboProductNameQuantity.Text.Split('|');
            string sql = "SELECT*FROM SANPHAM WHERE MASP=N'" + sp[0] + "'";
            DataTable dt = new DataTable();
            dt = Methods.GetDataToTable(sql);
            txtUnitPrice.Text = dt.Rows[0]["GIABAN"].ToString(); 
            //else
            //    MessageBox.Show("Lỗi của Trần Tiến");

        }
        void LoadKH()
        {

            string sql = "SELECT MAKH,TENKH FROM KHACHHANG WHERE MAKH='"+cboCustomerIdName.Text+"'";
            dtKH= Methods.GetDataToTable(sql);
            if(!Methods.CheckKey(sql))
            {
                sql = "SELECT MAKH,TENKH FROM KHACHHANG";
                dtKH = Methods.GetDataToTable(sql);
                for (int i = 0; i < dtKH.Rows.Count; i++)
                {
                    string ma = dtKH.Rows[i]["MAKH"].ToString();
                    string ten = dtKH.Rows[i]["TENKH"].ToString();
                    cboCustomerIdName.Items.Add(ma.Trim() + "|" + ten);
                }
            }
            if(Methods.CheckKey(sql))
            {
                string ma = dtKH.Rows[0]["MAKH"].ToString();
                string ten = dtKH.Rows[0]["TENKH"].ToString();
                cboCustomerIdName.Text = ma.Trim() + "|" + ten;
            }    
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string manv =txtEmployeeIdName.Text.Substring(0,4);
            string makh=cboCustomerIdName.Text.Substring(0,4);
            string masp=cboProductNameQuantity.Text;
            String[] sp = txtTenSP.Text.Split('|');
         
            {
                string sql = "SELECT*FROM HOADON WHERE MAHD='" + txtmaHD.Text + "'";
                if (!Methods.CheckKey(sql))
                {
                    sql = "INSERT INTO HOADON(MAHD,NGAY,MAKH,MANV,TONGTIEN)";
                    sql += " VALUES(N'" + txtmaHD.Text + "','" + DateTime.Parse(txtDateTime.Text) + "','" + makh + "','" + manv + "','')";
                    Methods.Connect();
                    Methods.RunSQL(sql);
                }
                sql = "INSERT INTO CHITIETHOADON(MAHD,MASP,TENSP,SOLUONG,GIABAN,THANHTIEN)";
                sql += " VALUES(N'" + txtmaHD.Text + "','" + masp + "',N'" + sp[0].Trim() + "','" + txtQuantity.Text + "','" + txtUnitPrice.Text + "','" + txtTotalPrice.Text + "')";
                Methods.Connect();
                Methods.RunSQL(sql);
                LoadCTHD(txtmaHD.Text);
                txtTotalPrice.Text = "";
            }
          
            
        }

    

        private void btnPay_Click(object sender, EventArgs e)
        {
            int sl = Int32.Parse(txtQuantity.Text);
            int giaban=Int32.Parse(txtUnitPrice.Text);
            txtTotalPrice.Text = (sl*giaban).ToString();
        }

        private void gvBill_CellContentClick_1(object sender,  DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = gvBill.Rows[e.RowIndex];
            cboCustomerIdName.Text= Convert.ToString(row.Cells["MASP"].Value);
            txtTenSP.Text = Convert.ToString(row.Cells["TENSP"].Value);
            txtQuantity.Text = Convert.ToString(row.Cells["SOLUONG"].Value);
            txtUnitPrice.Text = Convert.ToString(row.Cells["GIABAN"].Value);
            txtTotalPrice.Text = Convert.ToString(row.Cells["THANHTIEN"].Value);


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string sp = cboProductNameQuantity.SelectedItem.ToString();
            String[] tensp = txtTenSP.Text.Split('|');

            if (Int32.Parse(tensp[1]) > Int32.Parse(txtQuantity.Text))
            {
                string sql = "UPDATE CHITIETHOADON SET  SOLUONG='" + txtQuantity.Text + "' WHERE MASP='" + sp + "'";
                if (Methods.CheckKey(sql))
                {
                    Methods.Connect();
                    Methods.RunSQL(sql);
                }
                else
                { MessageBox.Show("không có sản phẩm trong bảng"); }
                LoadCTHD(txtmaHD.Text);
            }
            else
            {     
                MessageBox.Show("Vui nhập lại số lượng");
                txtQuantity.Text = "";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           String[] sp = txtTenSP.Text.Split('|');

            string sql = "DELETE FROM CHITIETHOADON WHERE TENSP='" + sp[0] + "'";
            if (Methods.CheckKey(sql))
            {
                Methods.Connect();
                Methods.RunSQL(sql);
            }
        
            LoadCTHD(txtmaHD.Text);

        }

        private void txtIn_Click(object sender, EventArgs e)
        {
            InHoaDon frm = new InHoaDon(txtmaHD.Text);
            frm.Show();
        }

        private void cboProductNameQuantity_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string masp = cboProductNameQuantity.SelectedItem.ToString();
            String[] sp = txtTenSP.Text.Split('|');
            string sql = "SELECT*FROM SANPHAM WHERE MASP=N'" + masp + "'";
            DataTable dt = new DataTable();
            dt = Methods.GetDataToTable(sql);
            txtTenSP.Text = dt.Rows[0]["TENSP"].ToString() + "|" + dt.Rows[0]["SOLUONG"];
            txtUnitPrice.Text = dt.Rows[0]["GIABAN"].ToString();
        }

        private void cboProductNameQuantity_TextChanged(object sender, EventArgs e)
        {
            string sp = cboProductNameQuantity.Text;
            string sql = "SELECT*FROM SANPHAM WHERE MASP='" + sp + "'";
            DataTable dt = new DataTable();
            dt = Methods.GetDataToTable(sql);
            txtUnitPrice.Text = dt.Rows[0]["GIABAN"].ToString();
        }

        private void txtTenSP_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTenSP.Text = "";
            txtQuantity.Text = "";
            txtUnitPrice.Text = "";
            txtTotalPrice.Text = "";
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
