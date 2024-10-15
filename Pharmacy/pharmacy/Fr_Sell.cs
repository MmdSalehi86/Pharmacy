using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Sell : Form
    {
        public Fr_Sell()
        {
            InitializeComponent();

            ds = new DataSet();
            ds.Tables.Add("med");
        }

        DataSet ds;
        string org_qu_med = "Select id, Name, Tedad, Price_Sell, Tedad FROM Tbl_Medicine";
        bool cancel = false;

        private void txt_Validation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }


        private void Load_Data()
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = org_qu_med;
            ds.Tables["med"].Rows.Clear();
            Db.Adap.Fill(ds.Tables["med"]);
        }

        void ClearText()
        {
            txt_Code.Text = "";
            txt_Date.Text = "";

            txt_Cus_Name.Text = "";
            txt_Cus_Phone.Text = "";
            txt_Cus_Address.Text = "";

            cmb_Med.SelectedIndex = -1;
            txt_Med_Num.Value = 0;

            txt_Desc.Text = "";

            txt_Kh.Text = "0";
            txt_Takh.Text = "0";
            txt_Total.Text = "0";
            txt_Sum_Med.Text = "0";

            txt_SH.Text = "";

            dgv.Rows.Clear();
        }

        bool Check_Input()
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("حداقل یک دارو اضافه کنید");
                return true;
            }
            var index_Date = txt_Date.Text.IndexOf(" ");
            if (IsNull(txt_Code.Text) || index_Date != -1 || IsNull(txt_Cus_Name.Text) || IsNull(txt_Date.Text) ||
                txt_Med_Price.Text == "0")
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
        private void cmb_Med_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cancel)
                return;
            if (cmb_Med.SelectedIndex != -1)
            {
                var current = ds.Tables["med"].Rows[cmb_Med.SelectedIndex];
                txt_Med_Num.MaxValue = int.Parse(current["Tedad"].ToString());
                txt_Med_Price.Text = (int.Parse(current["Price_Sell"].ToString()) * txt_Med_Num.Value).ToString();
            }
        }

        private void btn_Add_Med_Click(object sender, EventArgs e)
        {
            if (cmb_Med.SelectedIndex == -1)
            {
                MessageBox.Show("لطفا یک دارو انتخاب کنید");
                return;
            }
            if (txt_Med_Num.Value == 0)
            {
                MessageBox.Show("تعداد باید بیشتر از صفر باشد");
                return;
            }
            bool flag = false;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                var row = dgv.Rows[i];
                if (row.Cells[0].Value.ToString() == cmb_Med.SelectedValue.ToString())
                {
                    flag = true;
                    row.Cells[2].Value = (Convert.ToInt32(row.Cells[2].Value) + txt_Med_Num.Value).ToString();
                    row.Cells[3].Value = (Convert.ToInt32(row.Cells[3].Value) + int.Parse(txt_Med_Price.Text));
                }
            }
            if (!flag)
                dgv.Rows.Add(cmb_Med.SelectedValue, cmb_Med.Text, txt_Med_Num.Text, txt_Med_Price.Text);
            var current = ds.Tables["med"].Rows[cmb_Med.SelectedIndex];
            current["Tedad"] = int.Parse(current["Tedad"].ToString()) - txt_Med_Num.Value;
            cmb_Med.SelectedIndex = -1;
            txt_Med_Num.Value = 0;
            txt_Med_Price.Text = "";
            btn_Calc_Click(null, null);
        }

        private void btn_Remove_Med_Click(object sender, EventArgs e)
        {
            var current = dgv.CurrentRow;
            if (current == null)
            {
                MessageBox.Show("باید یک دارو از جدول انتخاب کنید");
                return;
            }
            var rows = ds.Tables["med"].Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i]["id"].ToString() == current.Cells[0].Value.ToString())
                    rows[i]["Tedad"] = Convert.ToInt32(rows[i]["Tedad"]) + Convert.ToInt32(current.Cells[2].Value);
            }
            dgv.Rows.Remove(current);
            btn_Calc_Click(null, null);
        }

        private void btn_Calc_Click(object sender, EventArgs e)
        {
            if (IsNull(txt_Kh.Text))
                txt_Kh.Text = "0";
            if (IsNull(txt_Takh.Text))
                txt_Takh.Text = "0";
            var sum_med = 0;
            for (int i = 0; i < dgv.Rows.Count; i++)
                sum_med += Convert.ToInt32(dgv.Rows[i].Cells[3].Value);
            txt_Sum_Med.Text = sum_med.ToString();
            txt_Total.Text = (sum_med + int.Parse(txt_Kh.Text) - int.Parse(txt_Takh.Text)).ToString();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Check_Input())
                return;
            btn_Calc_Click(null, null);
            var sql_find_factor = "SELECT COUNT(code) FROM Tbl_Factor_F WHERE Code=@0";

            Db.Adap.SelectCommand.CommandText = sql_find_factor;

            Db.SetConnection(Sql_Commands.Select);
            Db.AddParameters(Sql_Commands.Select, txt_Code.Text);
            try
            {
                var count = (int)Db.Adap.SelectCommand.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("این کد فاکتور قبلا ثبت شده است");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Db.SetConnection(Sql_Commands.Insert);
            Db.SetConnection(Sql_Commands.Update);
            var sql_insert = "INSERT INTO Tbl_Factor_F (Code, Date, Cus_Name, Cus_Phone, Cus_Address, id_Med, Tedad, Price_Med, Price_Kh, Price_Takh, Price, Description)" +
                "VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11)";
            Db.Adap.InsertCommand.CommandText = sql_insert;

            var sql_update = "UPDATE Tbl_Medicine SET Tedad=Tedad-@0 WHERE id=@1";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                var current = dgv.Rows[i];
                Db.AddParameters(Sql_Commands.Insert, txt_Code.Text, txt_Date.Text, txt_Cus_Name.Text,
                    txt_Cus_Phone.Text, txt_Cus_Address.Text, current.Cells[0].Value, current.Cells[2].Value,
                    txt_Sum_Med.Text, txt_Kh.Text, txt_Takh.Text, txt_Total.Text, txt_Desc.Text);
                Db.AddParameters(Sql_Commands.Update, current.Cells[2].Value, current.Cells[0].Value);
                try
                {
                    Db.Adap.InsertCommand.ExecuteNonQuery();
                    Db.Adap.UpdateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            MessageBox.Show("با موفقیت ثبت شد");

        }

        private void btn_List_Click(object sender, EventArgs e)
        {
            var fr_Sell_List = new Fr_Sell_List();
            fr_Sell_List.ShowDialog();
        }

        private void btn_Buy_Check_Click(object sender, EventArgs e)
        {
            if (IsNull(txt_Code.Text) || txt_Total.Text == "0" || IsNull(txt_Cus_Name.Text))
            {
                MessageBox.Show("اطلاعات ناقص است");
                return;
            }
            var fr_Ch_P = new Fr_Check();
            fr_Ch_P.txt_Name_P.Text = txt_Cus_Name.Text;
            fr_Ch_P.txt_Price.Text = txt_Total.Text;
            fr_Ch_P.txt_Desc.Text = "پرداخت فاکتور فروش به صورت چک به شماره فاکتور " + txt_Code.Text;
            fr_Ch_P.ShowDialog();
            ClearText();
        }

        private void btn_Buy_Nagh_Click(object sender, EventArgs e)
        {
            if (Check_Input())
                return;
            var sql_get_balance = "SELECT Balance FROM Tbl_Bank_Account WHERE SH=@0";

            Db.Adap.SelectCommand.CommandText = sql_get_balance;
            Db.SetConnection(Sql_Commands.Select);
            Db.AddParameters(Sql_Commands.Select, txt_SH.Text);
            try
            {
                var balance = Db.Adap.SelectCommand.ExecuteScalar();
                if (balance == null)
                {
                    MessageBox.Show("شماره حساب پیدا نشد");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var sql_update = "UPDATE Tbl_Bank_Account SET Balance=Balance+@0 WHERE SH=@1";
            Db.Adap.UpdateCommand.CommandText = sql_update;
            Db.SetConnection(Sql_Commands.Update);
            Db.AddParameters(Sql_Commands.Update, int.Parse(txt_Total.Text), txt_SH.Text);
            try
            {
                Db.Adap.UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("با موفقیت پرداخت شد");
            ClearText();
        }

        private void Fr_Buy_Load(object sender, EventArgs e)
        {
            txt_Med_Num.Value = 0;

            Load_Data();

            cancel = true;
            cmb_Med.DataSource = ds.Tables["med"];
            cmb_Med.DisplayMember = "Name";
            cmb_Med.ValueMember = "id";
            cmb_Med.SelectedIndex = -1;
            cancel = false;
        }

        private void txt_Med_Num_ValueChanged(object sender, EventArgs e)
        {
            if (cmb_Med.SelectedIndex != -1)
            {
                var current = ds.Tables["med"].Rows[cmb_Med.SelectedIndex];
                txt_Med_Price.Text = (Convert.ToInt32(current["Price_Sell"]) * txt_Med_Num.Value).ToString();
            }
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
