using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Personnel : Form
    {
        public Fr_Personnel()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;
        bool cancel = false;

        private void btn_List_Click(object sender, EventArgs e)
        {
            var fr_Per_List = new Fr_Personnel_List();
            fr_Per_List.ShowDialog();
        }

        private void txt_Validation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            ClearText();
            btn_Edit.Enabled = false;
            btn_Cancel.Visible = false;
            btn_Save.Enabled = true;
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
                txt_Phone.Text = current["Phone"].ToString();
                txt_Salary.Text = current["Salary"].ToString();
                txt_Date.Text = current["Date_Recr"].ToString();
                txt_Status.Text = current["Status"].ToString();
                txt_Desc.Text = current["Description"].ToString();
            }
            else
                btn_Cancel_Click(null, null);
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
            var sql_delete = "DELETE FROM Tbl_Personnel WHERE id=@0";
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
            var sql_insert = "INSERT INTO Tbl_Personnel (Name, Phone, Status, Salary, Date_Recr, Description)" +
                " VALUES (@0, @1, @2, @3, @4, @5)";
            Db.Adap.InsertCommand.CommandText = sql_insert;
            Db.SetConnection(Sql_Commands.Insert);
            Db.AddParameters(Sql_Commands.Insert, txt_Name.Text, txt_Phone.Text, txt_Status.Text, int.Parse(txt_Salary.Text),
                txt_Date.Text, txt_Desc.Text);

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
            var sql_update = "UPDATE Tbl_Personnel SET Name=@0, Phone=@1, Status=@2, Salary=@3, Date_Recr=@4, Description=@5" +
                " WHERE id=@6";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, txt_Name.Text, txt_Phone.Text, txt_Status.Text, int.Parse(txt_Salary.Text),
                txt_Date.Text, txt_Desc.Text, Convert.ToInt32(cmb.SelectedValue));

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

        private void Fr_Personnel_Load(object sender, EventArgs e)
        {
            Load_Data();
            cancel = true;
            cmb.DataSource = ds.Tables["t"];
            cmb.DisplayMember = "Name";
            cmb.ValueMember = "id";
            ClearText();
            cancel = false;
        }


        void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = "SELECT * FROM Tbl_Personnel";
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);
        }

        bool Check_Input()
        {
            if (IsNull(txt_Name.Text) || IsNull(txt_Phone.Text) || IsNull(txt_Salary.Text) || IsNull(txt_Status.Text) ||
                IsNull(txt_Date.Text))
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

        void ClearText()
        {
            cmb.SelectedIndex = -1;
            txt_Name.Text = "";
            txt_Phone.Text = "";
            txt_Date.Text = "";
            txt_Salary.Text = "";
            txt_Status.Text = "";
            txt_Desc.Text = "";
        }

        private void btn_Get_Date_Click(object sender, EventArgs e)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            DateTime dateTime = DateTime.Parse(DateTime.Now.ToShortDateString());
            int day = pc.GetDayOfMonth(dateTime);
            int month = pc.GetMonth(dateTime);
            int yr = pc.GetYear(dateTime);
            txt_Date.Text = yr.ToString() + "/" + month.ToString("00") + "/" + day.ToString("00");
        }
    }
}
