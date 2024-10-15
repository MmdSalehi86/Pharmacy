using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Medicine_List : Form
    {
        public Fr_Medicine_List()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;

        private void Fr_Medicine_List_Load(object sender, EventArgs e)
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
                var sql_delete = "DELETE FROM Tbl_Medicine WHERE id=@0";
                Db.Adap.DeleteCommand.CommandText = sql_delete;
                Db.SetConnection(Sql_Commands.Delete);
                Db.AddParameters(Sql_Commands.Delete, Convert.ToInt32(current.Cells["id"].Value));

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
            if (Check_Input(out bool name, out bool group, out bool date1, out bool date2))
                return;
            Db.SetConnection(Sql_Commands.Select);
            var cmd = Db.Adap.SelectCommand;
            cmd.Parameters.Clear();
            var sql_search = "SELECT med.id, med.Name, Tbl_Group.GName, Tbl_Company.Name, Date1, Date2, Tedad, Price_Buy, Price_Sell, med.Description" +
                " FROM Tbl_Medicine AS med" +
                " LEFT JOIN Tbl_Group ON Tbl_Group.id=med.GName" +
                " LEFT JOIN Tbl_Company ON Tbl_Company.id=med.Company" +
                " WHERE";
            if (name)
            {
                sql_search += " med.Name LIKE '%'+@n+'%' AND";
                cmd.Parameters.AddWithValue("@n", txt_Name.Text.Trim());
            }
            if (group)
            {
                sql_search += " Tbl_Group.GName LIKE '%'+@g+'%' AND";
                cmd.Parameters.AddWithValue("@g", txt_Group.Text.Trim());
            }
            if (date1 && date2)
            {
                sql_search += " Date2 BETWEEN @d1 AND @d2";
                cmd.Parameters.AddWithValue("@d1", txt_Date1.Text);
                cmd.Parameters.AddWithValue("@d2", txt_Date2.Text);
            }
            if (sql_search.LastIndexOf("AND") == sql_search.Length - 3)
                sql_search = sql_search.Remove(sql_search.Length - 3, 3);
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
            Db.Adap.SelectCommand.CommandText = "SELECT Tbl_Medicine.id, Tbl_Medicine.Name, Tbl_Group.GName, Tbl_Company.Name, Date1, Date2, Tedad, Price_Buy, Price_Sell, Tbl_Medicine.Description" +
                " FROM Tbl_Medicine" +
                " LEFT JOIN Tbl_Group ON Tbl_Group.id=Tbl_Medicine.GName" +
                " LEFT JOIN Tbl_Company ON Tbl_Company.id=Tbl_Medicine.Company";
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);
        }

        bool Check_Input(out bool name, out bool group, out bool date1, out bool date2)
        {
            name = group = date1 = date2 = false;
            if (IsNull(txt_Name.Text) && IsNull(txt_Group.Text) && txt_Date1.Text.IndexOf(" ") != -1 && txt_Date2.Text.IndexOf(" ") != -1)
            {
                Load_Data();
                return true;
            }
            if ((txt_Date1.Text.IndexOf(" ") == -1 && txt_Date2.Text.IndexOf(" ") != -1) || (txt_Date1.Text.IndexOf(" ") != -1 && txt_Date2.Text.IndexOf(" ") == -1))
            {
                MessageBox.Show("هر دو مقدار تاریخ را وارد کنید");
                return true;
            }
            if (txt_Date1.Text.IndexOf(" ") == -1 && txt_Date2.Text.IndexOf(" ") == -1)
            {
                date1 = true;
                date2 = true;
            }
            name = !IsNull(txt_Name.Text);
            group = !IsNull(txt_Group.Text);
            return false;
        }

        void SetHeader()
        {
            dgv1.Columns["id"].Visible = false;
            dgv1.Columns["id"].HeaderText = "ردیف";
            dgv1.Columns["Name"].HeaderText = "نام دارو";
            dgv1.Columns["GName"].HeaderText = "نام گروه";
            dgv1.Columns["Name1"].HeaderText = "تولید کننده";
            dgv1.Columns["Date1"].HeaderText = "تاریخ تولید";
            dgv1.Columns["Date2"].HeaderText = "تاریخ انقضاء";
            dgv1.Columns["Tedad"].HeaderText = "تعداد";
            dgv1.Columns["Price_Buy"].HeaderText = "قیمت خرید";
            dgv1.Columns["Price_Sell"].HeaderText = "قیمت فروش";
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
            txt_Name.Text = "";
            txt_Group.Text = "";
            Load_Data();
        }

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
    }
}
