using DevExpress.Data.Browsing;
using Lab4_04.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4_04
{
    public partial class Form1 : Form
    {
        private int sothutu = 1;
        QuanLyBanHangDB context = new QuanLyBanHangDB();
        public Form1()
        {
            InitializeComponent();
            this.dateTimeEnd.ValueChanged += new EventHandler(dateTimeStart_ValueChanged);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void FillBill(List<Invoice> listInvoice)
        {
            dgvOderBill.Rows.Clear();
            foreach (var item in listInvoice)
            {
                int index = dgvOderBill.Rows.Add();
                dgvOderBill.Rows[index].Cells[0].Value = sothutu;
                dgvOderBill.Rows[index].Cells[1].Value = item.InvoiceNo;
                dgvOderBill.Rows[index].Cells[2].Value = item.OrderDate;
                dgvOderBill.Rows[index].Cells[3].Value = item.DeliveryDate;
                dgvOderBill.Rows[index].Cells[4].Value = item.Orders.Sum(s => s.Quantity*s.Price);
                sothutu++;
            }
        }
        private void dateTimeDefault()
        {
            dateTimeStart.Value = new DateTime(2019, 10, 1);
            dateTimeEnd.Value = new DateTime(2019, 10, 31);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            try
            {
                txtSum.Text = "0";
                dateTimeDefault();
                List<Invoice> listInvoice = context.Invoices.ToList();
                FillBill(listInvoice);
                CountDgv();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void CountDgv()
        {
            int sumSoluong = 0;
            for (int i = 0; i < dgvOderBill.Rows.Count; i++)
            {
                if (dgvOderBill.Rows[i].Cells[0].Value != null)
                {
                    sumSoluong++;
                }
            }
            txtSum.Text = sumSoluong.ToString();
        }

        private void checkMonth_CheckedChanged(object sender, EventArgs e)
        {
            if(checkMonth.Checked)
            {
                DateTime today = DateTime.Today;
                DateTime firstDayofMonth = new DateTime(today.Year, today.Month, 1);
                DateTime lastDayofMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
                List<Invoice> fillInvoice = context.Invoices.ToList()
                    .Where(f => f.OrderDate >= firstDayofMonth && f.DeliveryDate <= lastDayofMonth).ToList();
                FillBill(fillInvoice);
                CountDgv();
                sothutu = 1;
            }
            else
            {
                Form1_Load(sender, e);
            }
        }

        private void dateTimeStart_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if(dateTimeStart.Value > dateTimeEnd.Value)
                {
                    dateTimeEnd.Value = dateTimeStart.Value;
                }
                else
                {
                    List<Invoice> fillInvoice = context.Invoices.ToList().Where(f => f.DeliveryDate >= dateTimeStart.Value && f.DeliveryDate <= dateTimeEnd.Value).ToList();
                    FillBill(fillInvoice);
                    CountDgv();
                    sothutu = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
