using BUS;
using GUI.ClsData;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GUI
{
    public partial class frmBill : Form
    {
        frmBillInfo fBillInfo;
        BUS_Bill busBill = new BUS_Bill();

        public frmBill(string email)
        {
            InitializeComponent();
            fBillInfo = new frmBillInfo(email);
        }
        public frmBill(object sender, EventArgs e)
        {
            frmBill_Load(sender, e);
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.Hide();
            fBillInfo.ShowDialog();
            this.Show();
          //  gvBill.DataSource = busBill.ListOfBills();
            LoadGridView();
        }

        private void LoadGridView()
        {
            gvBill.Columns[0].HeaderText = "Mã HD";
            gvBill.Columns[2].HeaderText = "Mã khách hàng";
            gvBill.Columns[1].HeaderText = "Thời gian";
            gvBill.Columns[3].HeaderText = "Mã nhân viên";
            gvBill.Columns[4].HeaderText = "Tổng tiền";
            foreach (DataGridViewColumn item in gvBill.Columns)
            {
                item.DividerWidth = 1;
            }
            gvBill.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gvBill.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gvBill.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string name = txtSearch.Text.Trim();
            if (name == "")
            {
                frmBill_Load(sender, e);
                txtSearch.Focus();
            }
            else
            {
                DataTable data = busBill.SearchCustomerInBill(txtSearch.Text);
                gvBill.DataSource = data;
            }
        }

        private void frmBill_Load(object sender, EventArgs e)
        {
            //gvBill.DataSource = busBill.ListOfBills();
            //LoadGridView();
            Methods.Connect();
            // For Print PDF Bill
            SqlConnection sqlCon;
            string conString = null;
            string sqlQuery;

          
            sqlQuery = "SELECT*FROM HOADON";
            DataTable dt = new DataTable();
            dt = Methods.GetDataToTable(sqlQuery);

          //  LoadGridView();
            gvBill.DataSource = dt;
            Methods.Disconnect();

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvBill.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Output.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("Không thể ghi dữ liệu tới ổ đĩa. Mô tả lỗi:" + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable = new PdfPTable(gvBill.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn column in gvBill.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                pdfTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in gvBill.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfTable.AddCell(cell.Value.ToString());
                                }
                            }

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Dữ liệu Export thành công!!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Mô tả lỗi :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có bản ghi nào được Export!!!", "Info");
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
