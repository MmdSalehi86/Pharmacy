using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Company_List : Form
    {
        public Fr_Company_List()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;
        string org_qu = "Select * FROM Tbl_Company";

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
                var sql_delete = "DELETE FROM Tbl_Company WHERE id=@0";
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
            if (Check_Input(out bool name))
                return;
            Db.SetConnection(Sql_Commands.Select);
            var cmd = Db.Adap.SelectCommand;
            cmd.Parameters.Clear();
            var sql_search = org_qu + " WHERE";
            if (name)
            {
                sql_search += " Name LIKE '%'+@n+'%'";
                cmd.Parameters.AddWithValue("@n", txt_Name.Text.Trim());
            }
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
            Db.Adap.SelectCommand.CommandText = org_qu;
            ds.Tables["t"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["t"]);
        }

        bool Check_Input(out bool name)
        {
            name = false;
            if (IsNull(txt_Name.Text))
            {
                Load_Data();
                return true;
            }
            name = !IsNull(txt_Name.Text);
            return false;
        }

        void SetHeader()
        {
            dgv1.Columns["id"].Visible = false;
            dgv1.Columns["Name"].HeaderText = "نام شرکت";
            dgv1.Columns["Phone"].HeaderText = "تلفن";
            dgv1.Columns["Address"].HeaderText = "آدرس";
            dgv1.Columns["Description"].HeaderText = "توضیحات";
        }

        bool IsNull(string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            txt_Name.Text = "";
            Load_Data();
        }
    }
}
