using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.ClsData
{
   
     internal static class Methods
    {
        public static SqlConnection Con; //Khai báo đối tượng kết nối        

        public static void Connect()
        {
            Con = new SqlConnection(); //Khởi tạo đối tượng
            Con.ConnectionString = "server=DESKTOP-F3ASEVO; Database=QLBH; Integrated Security=True";
            //Mở kết nối

            //Kiểm tra kết nối
            //if (Con.State != ConnectionState.Open)
            //    MessageBox.Show("Kết nối thành công");
            //else MessageBox.Show("Không thể kết nối với dữ liệu");
            Con.Open();
        }

        public static void Disconnect()
        {
            if (Con.State != ConnectionState.Open)
            {
                Con.Close();    //Đóng kết nối
                Con.Dispose();  //Giải phóng tài nguyên
                Con = null;
            }
        }

        public static bool CheckKey(string sql)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, Con);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return (table.Rows.Count > 0);
        }

        public static void RunSQL(string sql)
        {
            SqlCommand cmd; // Đối tượng của lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = Con;   // Gán kết nối
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            // Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery();// Thực hiện câu lệnh SQL
            }
            catch (Exception e)
            {
                throw e;
            }
            cmd.Dispose();  // Giải phóng bộ nhớ
            cmd = null;
        }

        public static DataTable GetDataToTable(string sql)
        {
            // Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            SqlDataAdapter adapter = new SqlDataAdapter();
            // Tạo đối tượng thuộc lớp SqlCommand
            adapter.SelectCommand = new SqlCommand();
            adapter.SelectCommand.Connection = Methods.Con;  // Kết nối DB
            adapter.SelectCommand.CommandText = sql;  // Lệnh SQL

            // Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
    }
}
