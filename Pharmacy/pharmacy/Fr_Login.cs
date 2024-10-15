using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace pharmacy
{
    using Properties;
    public partial class Fr_Login : Form
    {
        public Fr_Login()
        {
            InitializeComponent();

            cmd = new SqlCommand();
        }

        SqlCommand cmd;

        private void btn_Show_Pass_Click(object sender, EventArgs e)
        {
            if (txt_Password.UseSystemPasswordChar = !txt_Password.UseSystemPasswordChar)
                btn_Show_Pass.BackgroundImage = Resources.hide;
            else
                btn_Show_Pass.BackgroundImage = Resources.show;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            Db.SetConnection(Sql_Commands.Select);
            Db.Adap.SelectCommand.CommandText = "SELECT id, username FROM Tbl_Users WHERE username=@0 AND password=@1";
            Db.AddParameters(Sql_Commands.Select, txt_Username.Text, txt_Password.Text);
            var reader = Db.Adap.SelectCommand.ExecuteReader();

            if (reader.Read())
            {
                User.Id = (int)reader["id"];
                User.Username = reader["username"].ToString();
                reader.Close();

                var fr_Main = new Fr_Main();
                if (User.Username == "adm")
                    fr_Main.isAdmin = true;
                else
                    fr_Main.isAdmin = false;
                this.Visible = false;
                fr_Main.ShowDialog();
                this.Visible = true;
            }
            else
            {
                reader.Close();
                MessageBox.Show("نام کاربری یا کلمه عبور اشتباه است", "احراز هویت", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
