using BUS;
using DTO;
using System;
using System.Data;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmCustomer : Form
    {
        BUS_Customer busCustomer = new BUS_Customer();
        DTO_Customer dtoCustomer;
        static DataTable dt= new DataTable();
        public frmCustomer()
        {
            InitializeComponent();
        }
        public frmCustomer(object sender, EventArgs e) :this()
        {
            frmCustomer_Load(sender, e);
        }

        private void SetValue(bool param, bool isLoad)
        {
            txtId.Text = null;
          txtId.Enabled = !param;

            txtPhoneNumber.Text = null;
            txtAddress.Text = null;
            btnInsert.Enabled = param;
            txtName.Text = null;
            if (isLoad)
            {
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnUpdate.Enabled = !param;
                btnDelete.Enabled = !param;
            }
        }

        private void LoadGridView()
        {
            gvCustomer.Columns[0].HeaderText = "Mã KH";
            gvCustomer.Columns[1].HeaderText = "Tên khách hàng";
            gvCustomer.Columns[2].HeaderText = "Địa chỉ";
            gvCustomer.Columns[3].HeaderText = "Số điện thoại";
            foreach (DataGridViewColumn item in gvCustomer.Columns)
            {
                item.DividerWidth = 1;
            }
            gvCustomer.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gvCustomer.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void MsgBox(string message, bool isError = false)
        {
            if (isError)
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            bool isTrueSDT = int.TryParse(txtPhoneNumber.Text, out int sdt);

            if (isTrueSDT && txtPhoneNumber.Text != "" && txtName.Text != "")
            {
                dtoCustomer = new DTO_Customer(txtId.Text,txtName.Text, txtAddress.Text, txtPhoneNumber.Text);
                if (busCustomer.InsertKhachHang(dtoCustomer))
                {
                    MsgBox("Thêm khách hàng thành công!");
                    gvCustomer.DataSource = busCustomer.ListOfCustomers();
                    LoadGridView();
                    SetValue(true, false);
                }
                else
                    MsgBox("Thêm khách hàng không thành công", true);
            }
            else
                MsgBox("Vui lòng kiểm tra lại dữ liệu", true);
        }

        private void frmCustomer_Load(object sender, EventArgs e)
        {
            gvCustomer.DataSource = busCustomer.ListOfCustomers();
            dt = busCustomer.ListOfCustomers();
            LoadGridView();
            SetValue(true, false);
            txtName.Focus();
            int ma = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["MAKH"].ToString().Substring(2, 2));
            themid(ma);
            if(txtId.Text=="")
            {
                ma = ma + 1;
                if (ma > 9)
                    txtId.Text = "KH" + ma.ToString();
                if (ma <= 9)
                {
                    txtId.Text = "KH0" + ma.ToString();
                }
            }    
        }
        void themid(int ma)
        {
            for (int i = 1; i < ma; i++)
            {
                int cout = 0;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    int bien = Convert.ToInt32(dt.Rows[j]["MAKH"].ToString().Substring(2, 2));
                    if (i != bien)
                    {
                        cout = 1;
                    }
                    else
                    {
                        cout = 0;
                        break;
                    }
                }
                if (cout != 0)
                {
                    if (i > 9)
                        txtId.Text = "KH" + i.ToString();
                    if (i <= 9)
                    {
                        txtId.Text = "KH0" + i.ToString();
                    }
                    break;
                }
            }
        }    

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (busCustomer.DeleteKhachHang(txtId.Text))
                {
                    MsgBox("Xóa thành công");
                    gvCustomer.DataSource = busCustomer.ListOfCustomers();
                    LoadGridView();
                    SetValue(true, false);
                }
                else
                    MsgBox("Không xóa được", true);
            }
        }

        private void gvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvCustomer.Rows.Count > 0)
            {
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;

                txtId.Text = gvCustomer.CurrentRow.Cells[0].Value.ToString();
                txtName.Text = gvCustomer.CurrentRow.Cells[1].Value.ToString();
                txtAddress.Text = gvCustomer.CurrentRow.Cells[2].Value.ToString();
                txtPhoneNumber.Text = gvCustomer.CurrentRow.Cells[3].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool isTruePhoneNumber = int.TryParse(txtPhoneNumber.Text, out int num);

            if (isTruePhoneNumber && txtPhoneNumber.Text != "" && txtName.Text != "")
            {
                if (MessageBox.Show("Bạn có chắc muốn sửa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dtoCustomer = new DTO_Customer(txtId.Text, txtName.Text, txtAddress.Text, txtPhoneNumber.Text);
                    if (busCustomer.UpdateCustomer(dtoCustomer))
                    {
                        MsgBox("Sửa khách hàng thành công!");
                        gvCustomer.DataSource = busCustomer.ListOfCustomers();
                        LoadGridView();
                        SetValue(true, false);
                    }
                    else
                        MsgBox("Sửa khách hàng không thành công", true);
                }
            }
            else
                MsgBox("Vui lòng kiểm tra lại dữ liệu", true);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SetValue(true, false);
            frmCustomer_Load(sender, e);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string name = txtSearch.Text.Trim();
            if (name == "")
            {
                frmCustomer_Load(sender, e);
                txtSearch.Focus();
            }
            else
            {
                DataTable data = busCustomer.SearchCustomer(txtSearch.Text);
                gvCustomer.DataSource = data;
            }
        }
    }
}
