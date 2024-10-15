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
    public partial class Fr_Cash : Form
    {
        public Fr_Cash()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;
        bool cancel = false;

        private void txt_Validation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btn_List_Click(object sender, EventArgs e)
        {
            var fr_Ca_List = new Fr_Cash_List();
            fr_Ca_List.ShowDialog();
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
                txt_Balance.Text = current["Balance"].ToString();
                txt_Desc.Text = current["Description"].ToString();
            }
            else
                btn_Cancel_Click(null, null);
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
            var sql_delete = "DELETE FROM Tbl_Cash WHERE id=@0";
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
            var sql_insert = "INSERT INTO Tbl_Cash (Name, Balance, Description)" +
                " VALUES (@0, @1, @2)";
            Db.Adap.InsertCommand.CommandText = sql_insert;
            Db.SetConnection(Sql_Commands.Insert);
            Db.AddParameters(Sql_Commands.Insert, txt_Name.Text, txt_Balance.Text, txt_Desc.Text);

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
            var sql_update = "UPDATE Tbl_Cash SET Name=@0, Balance=@1, Description=@2" +
                " WHERE id=@3";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, txt_Name.Text, txt_Balance.Text, txt_Desc.Text,
                Convert.ToInt32(cmb.SelectedValue));

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

        private void Fr_Cash_Load(object sender, EventArgs e)
        {
            Load_Data();

            cancel = true;
            cmb.DataSource = ds.Tables["t"];
            cmb.DisplayMember = "Name";
            cmb.ValueMember = "id";
            cancel = false;
            ClearText();
        }


        void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = "SELECT * FROM Tbl_Cash";
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);
        }

        void ClearText()
        {
            cmb.SelectedIndex = -1;
            txt_Name.Text = "";
            txt_Balance.Text = "";
            txt_Desc.Text = "";
        }

        bool Check_Input()
        {
            if (IsNull(txt_Name.Text) || IsNull(txt_Balance.Text))
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
    }
}
