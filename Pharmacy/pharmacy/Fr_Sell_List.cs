using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Sell_List : Form
    {
        public Fr_Sell_List()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;
        string org_qu = "SELECT Distinct Code, Date, Cus_Name, Cus_Phone, Cus_Address, Price_Med, Price_Kh, Price_Takh, Price, Description" +
            " FROM Tbl_Factor_F";

        private void btn_Get_Date_Click(object sender, EventArgs e)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            DateTime dateTime = DateTime.Parse(DateTime.Now.ToShortDateString());
            int day = pc.GetDayOfMonth(dateTime);
            int month = pc.GetMonth(dateTime);
            int yr = pc.GetYear(dateTime);
            txt_Date1.Text = yr.ToString() + "/" + month.ToString("00") + "/" + day.ToString("00");
            txt_Date2.Text = yr.ToString() + "/" + month.ToString("00") + "/" + day.ToString("00");
        }

        private void Fr_Sell_List_Load(object sender, EventArgs e)
        {
            Load_Data();

            dgv1.DataSource = ds.Tables["t"];
            SetHeader();
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (Output.SaveToExcel(dgv1))
            {
                this.Focus();
                MessageBox.Show("خروجی در اکسل گرفته شد ", "Saved successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            var current = dgv1.CurrentRow;
            if (current != null)
            {
                var res = MessageBox.Show("آیا از حذف مطمئن هستید", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.No)
                    return;
                var sql_delete = "DELETE FROM Tbl_Factor_F WHERE code=@0";
                Db.Adap.DeleteCommand.CommandText = sql_delete;
                Db.SetConnection(Sql_Commands.Delete);
                Db.AddParameters(Sql_Commands.Delete, Convert.ToInt32(current.Cells["code"].Value));

                try
                {
                    Db.Adap.DeleteCommand.ExecuteNonQuery();
                    MessageBox.Show("با موفقیت حذف شد", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Load_Data();
            }
            else
                MessageBox.Show("لطفا آیتمی برای حذف انتخاب کنید");
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if (Check_Input(out bool date))
                return;
            Db.SetConnection(Sql_Commands.Select);
            var cmd = Db.Adap.SelectCommand;
            cmd.Parameters.Clear();
            var sql_search = org_qu +
                " WHERE";
            if (date)
            {
                sql_search += " Date BETWEEN @d1 AND @d2";
                cmd.Parameters.AddWithValue("@d1", txt_Date1.Text);
                cmd.Parameters.AddWithValue("@d2", txt_Date2.Text);
            }
            cmd.CommandText = sql_search;
            try
            {
                ds.Tables["t"].Rows.Clear();
                Db.Adap.Fill(ds.Tables["t"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = org_qu;
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);
        }

        bool Check_Input(out bool date)
        {
            date = false;
            var iDate1 = txt_Date1.Text.IndexOf(" ");
            var iDate2 = txt_Date2.Text.IndexOf(" ");

            if (iDate1 != -1 && iDate2 != -1)
            {
                Load_Data();
                return true;
            }
            if ((iDate1 == -1 && iDate2 != -1) || (iDate1 != -1 && iDate2 == -1))
            {
                MessageBox.Show("هر دو مقدار تاریخ را وارد کنید");
                return true;
            }
            if (iDate1 == -1 && iDate2 == -1)
            {
                date = true;
            }
            return false;
        }

        void SetHeader()
        {
            dgv1.Columns["Code"].HeaderText = "فاکتور";
            dgv1.Columns["Date"].HeaderText = "تاریخ";
            dgv1.Columns["Cus_Name"].HeaderText = "نام فرد";
            dgv1.Columns["Cus_Phone"].HeaderText = "تلفن";
            dgv1.Columns["Cus_Address"].HeaderText = "آدرس";
            dgv1.Columns["Price_Med"].HeaderText = "قیمت داروها";
            dgv1.Columns["Price_Kh"].HeaderText = "خدمات";
            dgv1.Columns["Price_Takh"].HeaderText = "تخفیف";
            dgv1.Columns["Price"].HeaderText = "تخفیف";
            dgv1.Columns["Description"].HeaderText = "توضیحات";
        }

        bool IsNull(string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            txt_Date1.Text = "";
            txt_Date2.Text = "";
            Load_Data();
        }
    }
}
