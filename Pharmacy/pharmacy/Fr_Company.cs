using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Company : Form
    {
        public Fr_Company()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;

        private void txt_KeyPress_Validation(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb.SelectedIndex != -1)
            {
                btn_Cancel.Visible = true;
                btn_Edit.Enabled = true;
                btn_Save.Enabled = false;

                var current = ds.Tables["t"].Rows[cmb.SelectedIndex];
                txt_Name.Text = current["Name"].ToString();
                txt_Phone.Text = current["Phone"].ToString();
                txt_Address.Text = current["Address"].ToString();
                txt_Description.Text = current["Description"].ToString();
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
            var sql_delete = "DELETE FROM Tbl_Company WHERE id=@0";
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

            txt_Name.Text = "";
            txt_Phone.Text = "";
            txt_Address.Text = "";
            txt_Description.Text = "";
            Load_Data();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Check_Input())
                return;
            var sql_insert = "INSERT INTO Tbl_Company (Name, Phone, Address, Description)" +
                " VALUES (@0, @1, @2, @3)";
            Db.Adap.InsertCommand.CommandText = sql_insert;
            Db.SetConnection(Sql_Commands.Insert);
            Db.AddParameters(Sql_Commands.Insert, txt_Name.Text, txt_Phone.Text, txt_Address.Text, txt_Description.Text);

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

            txt_Name.Text = "";
            txt_Phone.Text = "";
            txt_Address.Text = "";
            txt_Description.Text = "";
            Load_Data();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (Check_Input())
                return;
            var sql_update = "UPDATE Tbl_Company SET Name=@0, Phone=@1, Address=@2, Description=@3" +
                " WHERE id=@4";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, txt_Name.Text, txt_Phone.Text, txt_Address.Text,
                txt_Description.Text, Convert.ToInt32(cmb.SelectedValue));

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
            txt_Name.Text = "";
            txt_Phone.Text = "";
            txt_Address.Text = "";
            txt_Description.Text = "";

            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            Load_Data();
        }

        private void btn_List_Click(object sender, EventArgs e)
        {
            var fr_Com_List = new Fr_Company_List();
            fr_Com_List.ShowDialog();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            cmb.SelectedIndex = -1;
            txt_Name.Text = "";
            txt_Phone.Text = "";
            txt_Address.Text = "";
            txt_Description.Text = "";
        }

        private void Fr_Company_Load(object sender, EventArgs e)
        {
            Load_Data();

            cmb.DataSource = ds.Tables["t"];
            cmb.DisplayMember = "Name";
            cmb.ValueMember = "id";

            btn_Cancel_Click(null, null);
        }


        private bool Check_Input()
        {
            if (string.IsNullOrWhiteSpace(txt_Name.Text) || string.IsNullOrWhiteSpace(txt_Phone.Text) ||
                string.IsNullOrWhiteSpace(txt_Address.Text) || string.IsNullOrWhiteSpace(txt_Description.Text))
            {
                MessageBox.Show("همه مقادیر را وارد کنید");
                return true;
            }
            return false;
        }

        private void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = "SELECT * FROM Tbl_Company";
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);
            if (ds.Tables["t"].Rows.Count > 0)
                cmb.SelectedIndex = -1;
        }

    }
}
