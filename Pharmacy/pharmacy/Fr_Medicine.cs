using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Medicine : Form
    {
        public Fr_Medicine()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
            ds.Tables.Add("t1");
            ds.Tables.Add("t2");
        }

        DataSet ds;
        bool cancel = false;

        private void btn_List_Click(object sender, EventArgs e)
        {
            var fr_Med_List = new Fr_Medicine_List();
            fr_Med_List.ShowDialog();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            ClearText();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (cmb.SelectedIndex == -1)
            {
                MessageBox.Show("لطفا گزینه ای برای حذف انتخاب کنید");
                return;
            }
            var res = MessageBox.Show("آیا از حذف مطمئن هستید", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
                return;
            var sql_delete = "DELETE FROM Tbl_Medicine WHERE id=@0";
            Db.Adap.DeleteCommand.CommandText = sql_delete;
            Db.SetConnection(Sql_Commands.Delete);
            Db.AddParameters(Sql_Commands.Delete, Convert.ToInt32(cmb.SelectedValue));

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
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Check_Input())
                return;
            var sql_insert = "INSERT INTO Tbl_Medicine (Name, GName, Company, Date1, Date2, tedad, Price_Buy, Price_Sell, Description)" +
                " VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)";
            Db.Adap.InsertCommand.CommandText = sql_insert;
            Db.SetConnection(Sql_Commands.Insert);
            Db.AddParameters(Sql_Commands.Insert, txt_Name.Text, cmb_Group.SelectedValue, cmb_Company.SelectedValue,
                txt_Date1.Text, txt_Date2.Text, txt_Num.Value, txt_Price_Buy.Text, txt_Price_Sell.Text, txt_Description.Text);

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
            var sql_update = "UPDATE Tbl_Medicine SET Name=@0, GName=@1, Company=@2, Date1=@3, Date2=@4," +
                " Tedad=@5, Price_Buy=@6, Price_Sell=@7, Description=@8" +
                " WHERE id=@9";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, txt_Name.Text, cmb_Group.SelectedValue, cmb_Company.SelectedValue,
                txt_Date1.Text, txt_Date2.Text, txt_Num.Value, txt_Price_Buy.Text, txt_Price_Sell.Text, txt_Description.Text, cmb.SelectedValue);

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

        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cancel)
            {
                return;
            }
            if (cmb.SelectedIndex != -1)
            {
                btn_Cancel.Visible = true;
                btn_Edit.Enabled = true;
                btn_Save.Enabled = false;

                var current = ds.Tables["t"].Rows[cmb.SelectedIndex];
                txt_Name.Text = current["Name"].ToString();
                cmb_Group.SelectedValue = current["GName"].ToString();
                cmb_Company.SelectedValue = current["Company"].ToString();
                txt_Date1.Text = current["Date1"].ToString();
                txt_Date2.Text = current["Date2"].ToString();
                txt_Num.Value = Convert.ToInt32(current["Tedad"]);
                txt_Price_Buy.Text = current["Price_Buy"].ToString();
                txt_Price_Sell.Text = current["Price_Sell"].ToString();
                txt_Description.Text = current["Description"].ToString();
            }
            else
                btn_Cancel_Click(null, null);
        }

        private void Fr_Medicine_Load(object sender, EventArgs e)
        {
            Load_Data();

            cancel = true;
            cmb.DataSource = ds.Tables["t"];
            cmb.DisplayMember = "Name";
            cmb.ValueMember = "id";
            cancel = false;

            cmb_Group.DataSource = ds.Tables["t1"];
            cmb_Group.DisplayMember = "GName";
            cmb_Group.ValueMember = "id";
            cmb_Group.SelectedIndex = -1;

            cmb_Company.DataSource = ds.Tables["t2"];
            cmb_Company.DisplayMember = "Name";
            cmb_Company.ValueMember = "id";
            cmb_Company.SelectedIndex = -1;

            btn_Cancel_Click(null, null);
        }

        private void txt_Validation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }


        private void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = "SELECT * FROM Tbl_Medicine";
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);

            ds.Tables["t1"].Rows.Clear();
            Db.Adap.SelectCommand.CommandText = "Select * FROM Tbl_Group";
            Db.Adap.Fill(ds.Tables["t1"]);

            ds.Tables["t2"].Rows.Clear();
            Db.Adap.SelectCommand.CommandText = "Select id, Name FROM Tbl_Company";
            Db.Adap.Fill(ds.Tables["t2"]);
        }

        void ClearText()
        {
            txt_Name.Text = "";
            cmb_Group.SelectedIndex = -1;
            cmb_Company.SelectedIndex = -1;
            txt_Date1.Text = "";
            txt_Date2.Text = "";
            txt_Num.Value = 0;
            txt_Price_Buy.Text = "";
            txt_Price_Sell.Text = "";
            txt_Description.Text = "";
            cmb.SelectedIndex = -1;
        }

        bool Check_Input()
        {
            if (IsNull(txt_Name.Text) || cmb_Group.SelectedIndex == -1 || cmb_Company.SelectedIndex == -1 ||
                IsNull(txt_Date1.Text) || IsNull(txt_Date2.Text) || IsNull(txt_Num.Text) ||
                IsNull(txt_Price_Buy.Text) || IsNull(txt_Price_Sell.Text))
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
