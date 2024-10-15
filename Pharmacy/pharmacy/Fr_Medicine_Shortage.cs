using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Medicine_Shortage : Form
    {
        public Fr_Medicine_Shortage()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("t");
        }

        DataSet ds;
        string org_qu = "SELECT med.id, med.Name, Tbl_Group.GName, Tbl_Company.Name, Date1, Date2, Tedad, Price_Buy, Price_Sell, med.Description" +
                " FROM Tbl_Medicine AS med" +
                " LEFT JOIN Tbl_Group ON Tbl_Group.id=med.GName" +
                " LEFT JOIN Tbl_Company ON Tbl_Company.id=med.Company";

        private void Fr_Cash_List_Load(object sender, EventArgs e)
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
                var sql_delete = "DELETE FROM Tbl_Medicine WHERE id=@0";
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
            Db.SetConnection(Sql_Commands.Select);
            var cmd = Db.Adap.SelectCommand;
            cmd.Parameters.Clear();
            var sql_search = org_qu + " WHERE";
            sql_search += " Tedad BETWEEN @t1 AND @t2";
            cmd.Parameters.AddWithValue("@t1", num1.Text);
            cmd.Parameters.AddWithValue("@t2", num2.Text);
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

        void SetHeader()
        {
            dgv1.Columns["id"].Visible = false;
            dgv1.Columns["id"].HeaderText = "ردیف";
            dgv1.Columns["Name"].HeaderText = "نام دارو";
            dgv1.Columns["GName"].HeaderText = "نام گروه";
            dgv1.Columns["Name1"].HeaderText = "تولید کننده";
            dgv1.Columns["Date1"].HeaderText = "تاریخ تولید";
            dgv1.Columns["Date2"].HeaderText = "تاریخ انقضاء";
            dgv1.Columns["Tedad"].HeaderText = "تعداد";
            dgv1.Columns["Price_Buy"].HeaderText = "قیمت خرید";
            dgv1.Columns["Price_Sell"].HeaderText = "قیمت فروش";
            dgv1.Columns["Description"].HeaderText = "توضیحات";
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            num1.Value = 0;
            num2.Value = 0;
            Load_Data();
        }
    }
}
