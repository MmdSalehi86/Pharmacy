using System;
using pr = pharmacy.Properties;
using System.Windows.Forms;
using System.Data;

namespace pharmacy
{
    public partial class Fr_User : Form
    {
        public Fr_User()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;

        private void btn_Browse_Click(object sender, EventArgs e)
        {
            if (cmb.SelectedIndex == -1)
            {
                MessageBox.Show("لطفا یک کاربر را انتخاب کنید");
                return;
            }
            btn_Cancel.Visible = true;
            btn_Edit.Enabled = true;
            btn_Save.Enabled = false;

            var current = ds.Tables["t"].Rows[cmb.SelectedIndex];
            txt_Username.Text = current["username"].ToString();
            txt_Password.Text = current["password"].ToString();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            cmb.SelectedIndex = -1;
            txt_Username.Text = "";
            txt_Password.Text = "";
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            var sql_update = "UPDATE Tbl_Users SET username=@0, password=@1" +
                " WHERE id=@2";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, txt_Username.Text, txt_Password.Text, Convert.ToInt32(cmb.SelectedValue));

            try
            {
                Db.Adap.UpdateCommand.ExecuteNonQuery();
                MessageBox.Show("با موفقیت ثبت شد", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txt_Username.Text = "";
            txt_Password.Text = "";
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            cmb.SelectedIndex = -1;
            Load_Data();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (cmb.Text == "adm")
            {
                MessageBox.Show("نمی توانید یوزر اصلی را حذف کنید");
                return;
            }
            var res = MessageBox.Show("آیا از حذف مطمئن هستید", "Delete", MessageBoxButtons.YesNo);
            if (res == DialogResult.No)
                return;
            if (cmb.SelectedIndex == -1)
            {
                MessageBox.Show("لطفا یک گزینه برای حذف انتخاب کنید");
                return;
            }
            var sql_delete = "DELETE FROM Tbl_Users WHERE id=@0";
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
            txt_Username.Text = "";
            txt_Password.Text = "";
            cmb.SelectedIndex = -1;
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            Load_Data();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            var sql_insert = "INSERT INTO Tbl_Users (username, password) VALUES (@0, @1)";
            Db.Adap.InsertCommand.CommandText = sql_insert;
            Db.SetConnection(Sql_Commands.Insert);
            Db.AddParameters(Sql_Commands.Insert, txt_Username.Text, txt_Password.Text);

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
            txt_Password.Text = "";
            txt_Username.Text = "";
            Load_Data();
        }

        private void Fr_User_Load(object sender, EventArgs e)
        {
            Load_Data();

            dgv1.DataSource = ds.Tables["t"];
            cmb.DataSource = ds.Tables["t"];
            cmb.DisplayMember = "username";
            cmb.ValueMember = "id";

            btn_Cancel_Click(null, null);
        }

        private void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = "SELECT * FROM Tbl_Users";
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);
            if (ds.Tables["t"].Rows.Count > 0)
                cmb.SelectedIndex = -1;
        }

        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb.SelectedIndex != -1)
            {
                btn_Cancel.Visible = true;
                btn_Edit.Enabled = true;
                btn_Save.Enabled = false;

                var current = ds.Tables["t"].Rows[cmb.SelectedIndex];
                txt_Username.Text = current["username"].ToString();
                txt_Password.Text = current["password"].ToString();
            }
            else
                btn_Cancel_Click(null, null);
        }

        private void pic1_Click(object sender, EventArgs e)
        {
            if (txt_Password.UseSystemPasswordChar = !txt_Password.UseSystemPasswordChar)
                pic1.Image = pr.Resources.hide;
            else
                pic1.Image = pr.Resources.show;
        }
    }
}
