using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Check_P_List : Form
    {
        public Fr_Check_P_List()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;
        string org_qu = "SELECT * FROM Tbl_Check_P";

        private void Fr_Check_List_Load(object sender, EventArgs e)
        {
            Load_Data();

            dgv1.DataSource = ds.Tables["t"];
            SetHeader();
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (Output.SaveToExcel(dgv1, true))
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
                var sql_delete = "DELETE FROM Tbl_Check_P WHERE id=@0";
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
            if (Check_Input(out bool date))
                return;
            Db.SetConnection(Sql_Commands.Select);
            var cmd = Db.Adap.SelectCommand;
            cmd.Parameters.Clear();
            var sql_search = org_qu + " WHERE";
            if (date)
            {
                sql_search += " Date2 BETWEEN @d1 AND @d2";
                cmd.Parameters.AddWithValue("@d1", txt_Date1.Text);
                cmd.Parameters.AddWithValue("@d2", txt_Date2.Text);
            }
            if (cmb_Status.SelectedIndex != -1)
            {
                sql_search += " Status=@st";
                cmd.Parameters.AddWithValue("@st", cmb_Status.Text);
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

        private void btn_Vosool_Click(object sender, EventArgs e)
        {
            var current = dgv1.CurrentRow;
            if (current == null)
            {
                MessageBox.Show("لطفا یک چک را انتخاب کنید");
                return;
            }
            if (current.Cells["Status"].Value.ToString() == "وصول شده")
            {
                MessageBox.Show("این چک قبلا وصول شده");
                return;
            }
            var res = MessageBox.Show("آیا می خواهید این چک را وصول کنید", "Vosool", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
                return;
            var sql_find_sh = "SELECT Balance FROM Tbl_Bank_Account WHERE SH=@0";
            Db.Adap.SelectCommand.CommandText = sql_find_sh;
            Db.SetConnection(Sql_Commands.Select);
            Db.AddParameters(Sql_Commands.Select, current.Cells["SH"].Value);

            try
            {
                var balance = Db.Adap.SelectCommand.ExecuteScalar();
                if (balance == null)
                {
                    MessageBox.Show("شماره حساب پیدا نشد");
                    return;
                }
                if ((int)balance < int.Parse(current.Cells["Price"].Value.ToString()))
                {
                    MessageBox.Show("مبلغ چک بیشتر از موجودی حساب است");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var sql_update = "UPDATE Tbl_Bank_Account SET Balance=Balance-@0 WHERE SH=@1";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, current.Cells["Price"].Value, current.Cells["SH"].Value);
            try
            {
                Db.Adap.UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            sql_update = "UPDATE Tbl_Check_P SET Status=@0";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.AddParameters(Sql_Commands.Update, "وصول شده");

            try
            {
                Db.Adap.UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("چک با موفقیت وصول شد");
            btn_Clear_Click(null, null);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            cmb_Status.SelectedIndex = -1;
            txt_Date1.Text = "";
            txt_Date2.Text = "";
            Load_Data();
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
            var d1 = txt_Date1.Text.IndexOf(" ");
            var d2 = txt_Date2.Text.IndexOf(" ");
            if (d1 != -1 && d2 != -1 && cmb_Status.SelectedIndex == -1)
            {
                Load_Data();
                return true;
            }
            if ((d1 == -1 && d2 != -1) || (d1 != -1 && d2 == -1))
            {
                MessageBox.Show("هر دو مقدار تاریخ را وارد کنید");
                return true;
            }
            if (d1 == -1 && d2 == -1)
            {
                date = true;
            }
            return false;
        }

        void SetHeader()
        {
            dgv1.Columns["id"].HeaderText = "سریال";
            dgv1.Columns["Name_H"].HeaderText = "نام حساب";
            dgv1.Columns["Name_P"].HeaderText = "فرد";
            dgv1.Columns["SH"].HeaderText = "ش. حساب";
            dgv1.Columns["Price"].HeaderText = "مبلغ";
            dgv1.Columns["Date1"].HeaderText = "صدور";
            dgv1.Columns["Date2"].HeaderText = "سررسید";
            dgv1.Columns["Status"].HeaderText = "وضعیت";
            dgv1.Columns["Description"].HeaderText = "توضیحات";
        }

        bool IsNull(string str)
        {
            return string.IsNullOrWhiteSpace(str);
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
