using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Check_P : Form
    {
        public Fr_Check_P()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
            ds.Tables.Add("t1");
        }

        DataSet ds;
        bool cancel = false;
        string pre_id = "";

        private void txt_Validation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btn_List_Click(object sender, EventArgs e)
        {
            var fr_Ch_P_List = new Fr_Check_P_List();
            fr_Ch_P_List.ShowDialog();
        }


        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (IsNull(txt_Serial.Text))
            {
                MessageBox.Show("لطفا آیتمی برای حذف وارد کنید");
                return;
            }
            var res = MessageBox.Show("آیا از حذف مطمئن هستید", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
                return;
            var sql_delete = "DELETE FROM Tbl_Check_P WHERE id=@0";
            Db.Adap.DeleteCommand.CommandText = sql_delete;
            Db.SetConnection(Sql_Commands.Delete);
            Db.AddParameters(Sql_Commands.Delete, Convert.ToInt32(txt_Serial.Text));

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
            ClearText();
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Check_Input())
                return;
            var sql_insert = "INSERT INTO Tbl_Check_P (id, SH, Name_H, Name_P, Price, Date1, Date2, Status, Description)" +
                " VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)";
            Db.Adap.InsertCommand.CommandText = sql_insert;
            Db.SetConnection(Sql_Commands.Insert);
            Db.AddParameters(Sql_Commands.Insert, txt_Serial.Text, cmb_SH.Text, txt_Name_H.Text,
                txt_Name_P.Text, txt_Price.Text, txt_Date1.Text, txt_Date2.Text, cmb_Status.Text, txt_Desc.Text);

            try
            {
                Db.Adap.InsertCommand.ExecuteNonQuery();
                MessageBox.Show("با موفقیت ثبت شد", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Load_Data();
            ClearText();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (Check_Input())
                return;
            var sql_update = "UPDATE Tbl_Check_P SET id=@0, SH=@1, Name_H=@2, Name_P=@3, Price=@4, Date1=@5, Date2=@6, Status=@7, Description=@8" +
                " WHERE id=@9";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, txt_Serial.Text, cmb_SH.Text, txt_Name_H.Text,
                txt_Name_P.Text, txt_Price.Text, txt_Date1.Text, txt_Date2.Text, cmb_Status.Text, txt_Desc.Text,
                Convert.ToInt32(pre_id));

            try
            {
                Db.Adap.UpdateCommand.ExecuteNonQuery();
                MessageBox.Show("با موفقیت ویرایش شد", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            Load_Data();
            ClearText();
        }
        
        private void btn_Browse_Click(object sender, EventArgs e)
        {
            bool flag = false;
            for (int i = 0; i < ds.Tables["t"].Rows.Count; i++)
            {
                var current = ds.Tables["t"].Rows[i];
                if (current["id"].ToString() == txt_Serial.Text)
                {
                    flag = true;
                    txt_Date1.Text = current["Date1"].ToString();
                    txt_Date2.Text = current["Date2"].ToString();
                    txt_Desc.Text = current["Description"].ToString();
                    txt_Name_H.Text = current["Name_H"].ToString();
                    txt_Name_P.Text = current["Name_P"].ToString();
                    cmb_SH.Text = current["SH"].ToString();
                    cmb_Status.Text = current["Status"].ToString();
                    txt_Price.Text = current["Price"].ToString();
                    pre_id = current["id"].ToString();
                    break;
                }
            }
            if (flag)
            {
                btn_Cancel.Visible = true;
                btn_Edit.Enabled = true;
                btn_Save.Enabled = false;
            }
            else
                MessageBox.Show("متاسفانه اطلاعات مورد نظر پیدا نشد");
        }

        private void cmb_SH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cancel)
            {
                return;
            }
            if (cmb_SH.SelectedIndex != -1)
            {
                var current = ds.Tables["t1"].Rows[cmb_SH.SelectedIndex];
                txt_Name_H.Text = current["Name"].ToString();
            }
        }

        private void Fr_Check_Load(object sender, EventArgs e)
        {
            Load_Data();

            cancel = true;
            cmb_SH.DataSource = ds.Tables["t1"];
            cmb_SH.DisplayMember = "SH";
            cmb_SH.ValueMember = "SH";
            cancel = false;
            ClearText();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            ClearText();
        }

        void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = "SELECT * FROM Tbl_Check_P";
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);

            ds.Tables["t1"].Rows.Clear();
            Db.Adap.SelectCommand.CommandText = "SELECT SH, Name FROM Tbl_Bank_Account";
            Db.Adap.Fill(ds.Tables["t1"]);
        }

        void ClearText()
        {
            txt_Serial.Text = "";
            cmb_SH.SelectedIndex = -1;
            txt_Name_H.Text = "";
            txt_Name_P.Text = "";
            txt_Price.Text = "";
            txt_Desc.Text = "";
            txt_Date1.Text = "";
            txt_Date2.Text = "";
            cmb_Status.SelectedIndex = -1;
            txt_Desc.Text = "";
        }

        bool Check_Input()
        {
            if (IsNull(txt_Date1.Text) || IsNull(txt_Date2.Text) || IsNull(txt_Name_H.Text) ||
                IsNull(txt_Name_P.Text) || IsNull(txt_Price.Text) || IsNull(txt_Serial.Text) ||
                IsNull(cmb_SH.Text) || cmb_Status.SelectedIndex == -1)
            {
                MessageBox.Show("همه مقادیر را وارد کنید");
                return true;
            }
            return false;
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
