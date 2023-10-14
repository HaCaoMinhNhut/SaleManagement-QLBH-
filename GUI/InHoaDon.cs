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
    public partial class InHoaDon : Form
    {

        static DataTable HD = new DataTable();
        public InHoaDon(string mahd)
        {
            InitializeComponent();
            lbMaHD.Text = mahd;
        }

        private void InHoaDon_Load(object sender, EventArgs e)
        {

            loadCTHD();
            loadTen();
        }
        private void loadTen()
        {
            DataTable dt = new DataTable();

            string sql = "SELECT* FROM HOADON WHERE MAHD='" + lbMaHD.Text + "'";
            HD = Methods.GetDataToTable(sql);
            lbNgayIn.Text = HD.Rows[0]["NGAY"].ToString();
            lbTongtien.Text = HD.Rows[0]["TONGTIEN"].ToString()+"VNĐ";
            string manv = HD.Rows[0]["MANV"].ToString();
            string makh = HD.Rows[0]["MAKH"].ToString();
            sql = "SELECT TENKH FROM KHACHHANG WHERE MAKH='" + makh + "'";
            dt = Methods.GetDataToTable(sql);
            lbTenKH.Text = dt.Rows[0][0].ToString();
            sql = "SELECT HOTEN FROM NHANVIEN WHERE MANV='" + manv + "'";
            dt = Methods.GetDataToTable(sql);
            lbTenNV.Text = dt.Rows[0][0].ToString();
        }
       
        private void loadCTHD()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT TENSP,SOLUONG,GIABAN,THANHTIEN FROM CHITIETHOADON WHERE MAHD ='" + lbMaHD.Text + "'";
            dt = Methods.GetDataToTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListViewItem bikip = new ListViewItem((i + 1).ToString());

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    bikip.SubItems.Add(dt.Rows[i][j].ToString());
                }
                listView1.Items.Add(bikip);

            }

        }
    }
}
