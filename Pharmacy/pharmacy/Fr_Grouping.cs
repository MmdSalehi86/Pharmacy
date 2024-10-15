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
    public partial class Fr_Grouping : Form
    {
        public Fr_Grouping()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;

        private void btn_List_Click(object sender, EventArgs e)
        {
            Fr_Group_List fr_Group = new Fr_Group_List();
            fr_Group.ShowDialog();
        }

        private void Fr_Grouping_Load(object sender, EventArgs e)
        {
            Load_Data();

            cmb.DataSource = ds.Tables["t"];
            cmb.DisplayMember = "GName";
            cmb.ValueMember = "id";

            btn_Cancel_Click(null, null);
        }

        private void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = "SELECT * FROM Tbl_Group";
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);
            if (ds.Tables["t"].Rows.Count > 0)
                cmb.SelectedIndex = -1;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            btn_Cancel.Visible = false;
            btn_Edit.Enabled = false;
            btn_Save.Enabled = true;
            cmb.SelectedIndex = -1;
            txt_Group_Name.Text = "";
        }

        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb.SelectedIndex != -1)
            {
                btn_Cancel.Visible = true;
                btn_Edit.Enabled = true;
                btn_Save.Enabled = false;

                var current = ds.Tables["t"].Rows[cmb.SelectedIndex];
                txt_Group_Name.Text = current["GName"].ToString();
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
            var res = MessageBox.Show("آیا از حذف مطمئن هستید", "Delete", MessageBoxButtons.YesNo);
            if (res == DialogResult.No)
                return;
            var sql_delete = "DELETE FROM Tbl_Group WHERE id=@0";
            Db.Adap.DeleteCommand.CommandText = sql_delete;
            Db.SetConnection(Sql_Commands.Delete);
            Db.AddParameters(Sql_Commands.Delete, cmb.SelectedValue);

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

            txt_Group_Name.Text = "";
            Load_Data();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Check_Input())
                return;
            var sql_insert = "INSERT INTO Tbl_Group (GName) VALUES (@0)";
            Db.Adap.InsertCommand.CommandText = sql_insert;
            Db.SetConnection(Sql_Commands.Insert);
            Db.AddParameters(Sql_Commands.Insert, txt_Group_Name.Text);

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

            txt_Group_Name.Text = "";
            Load_Data();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (Check_Input())
                return;
            var sql_update = "UPDATE Tbl_Group SET GName=@0 WHERE id=@1";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, txt_Group_Name.Text, Convert.ToInt32(cmb.SelectedValue));

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

            txt_Group_Name.Text = "";
            Load_Data();
        }


        private bool Check_Input()
        {
            if (string.IsNullOrWhiteSpace(txt_Group_Name.Text))
            {
                MessageBox.Show("همه مقادیر را وارد کنید");
                return true;
            }
            return false;
        }
    }
}
