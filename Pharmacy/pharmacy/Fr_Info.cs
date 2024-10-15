using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Info : Form
    {
        public Fr_Info()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;

        private void txt_Phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Db.SetConnection(Sql_Commands.Insert);
            Db.Adap.InsertCommand.CommandText = "INSERT INTO Tbl_ph_info (name, phone, modir, address)" +
                " VALUES (@0, @1, @2, @3)";
            Db.AddParameters(Sql_Commands.Insert, txt_Name.Text, txt_Phone.Text, txt_Modir.Text, txt_Address.Text);

            try
            {
                Db.Adap.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("عملیات موفقیت آمیز بود", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txt_Address.Text = "";
            txt_Phone.Text = "";
            txt_Name.Text = "";
            txt_Modir.Text = "";
            Load_Data();
        }

        private void Fr_Info_Load(object sender, EventArgs e)
        {
            Load_Data();

            cmb_Code.DataSource = ds.Tables["t"];
            cmb_Code.DisplayMember = "name";
            cmb_Code.ValueMember = "id";

            btn_Cancel_Click(null, null);
        }

        void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = "SELECT * FROM Tbl_ph_info";
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);
            if (ds.Tables["t"].Rows.Count > 0)
                cmb_Code.SelectedIndex = -1;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            cmb_Code.SelectedIndex = -1;
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;

            txt_Name.Text = "";
            txt_Modir.Text = "";
            txt_Phone.Text = "";
            txt_Address.Text = "";
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            var sql_update = "UPDATE Tbl_ph_info SET name=@0, modir=@1, phone=@2, address=@3" +
                " WHERE id=@4";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, txt_Name.Text, txt_Modir.Text, txt_Phone.Text,
                txt_Address.Text, Convert.ToInt32(cmb_Code.SelectedValue));

            try
            {
                Db.Adap.UpdateCommand.ExecuteNonQuery();
                MessageBox.Show("ویرایش موفقیت آمیز بود", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txt_Name.Text = "";
            txt_Modir.Text = "";
            txt_Phone.Text = "";
            txt_Address.Text = "";

            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            Load_Data();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (cmb_Code.SelectedIndex == -1)
            {
                MessageBox.Show("لطفا یک گزینه برای حذف انتخاب کنید");
                return;
            }
            var res = MessageBox.Show("آیا از حذف اطمینان دارید", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
                return;
            var sql_delete = "DELETE FROM Tbl_ph_info WHERE id=@0";
            Db.Adap.DeleteCommand.CommandText = sql_delete;
            Db.SetConnection(Sql_Commands.Delete);
            Db.AddParameters(Sql_Commands.Delete, int.Parse(cmb_Code.SelectedValue.ToString()));

            try
            {
                Db.Adap.DeleteCommand.ExecuteNonQuery();
                MessageBox.Show("عملیات با موفقیت انجام شد", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btn_Cancel_Click(null, null);
            Load_Data();
        }

        private void cmb_Code_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Code.SelectedIndex != -1)
            {
                btn_Cancel.Visible = true;
                btn_Edit.Enabled = true;
                btn_Save.Enabled = false;

                var current = ds.Tables["t"].Rows[cmb_Code.SelectedIndex];
                txt_Name.Text = current["name"].ToString();
                txt_Modir.Text = current["modir"].ToString();
                txt_Phone.Text = current["phone"].ToString();
                txt_Address.Text = current["address"].ToString();
            }
            else
                btn_Cancel_Click(null, null);
        }
    }
}
