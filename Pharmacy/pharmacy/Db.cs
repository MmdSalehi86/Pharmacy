using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using static pharmacy.Setting_Bin;

namespace pharmacy
{
    enum Sql_Commands { Select, Update, Delete, Insert }
    class Db
    {
        public static SqlDataAdapter Adap = new SqlDataAdapter()
        {
            InsertCommand = new SqlCommand(),
            DeleteCommand = new SqlCommand(),
            SelectCommand = new SqlCommand(),
            UpdateCommand = new SqlCommand()
        };

        private static SqlConnection connection = null;

        public static void SetConnection(Sql_Commands sql)
        {
            if (sql == Sql_Commands.Delete)
                Adap.DeleteCommand.Connection = Build_Connection();
            else if (sql == Sql_Commands.Select)
                Adap.SelectCommand.Connection = Build_Connection();
            else if (sql == Sql_Commands.Update)
                Adap.UpdateCommand.Connection = Build_Connection();
            else
                Adap.InsertCommand.Connection = Build_Connection();
        }

        public static void ClearParameters(Sql_Commands sql)
        {
            if (sql == Sql_Commands.Select)
                Adap.SelectCommand.Parameters.Clear();
            else if (sql == Sql_Commands.Update)
                Adap.UpdateCommand.Parameters.Clear();
            else if (sql == Sql_Commands.Delete)
                Adap.DeleteCommand.Parameters.Clear();
            else
                Adap.InsertCommand.Parameters.Clear();
        }

        public static void AddParameters(Sql_Commands commands, params object[] value)
        {
            ClearParameters(commands);
            switch (commands)
            {
                case Sql_Commands.Select:
                    AddParametersToAdapter(Adap.SelectCommand);
                    break;
                case Sql_Commands.Update:
                    AddParametersToAdapter(Adap.UpdateCommand);
                    break;
                case Sql_Commands.Delete:
                    AddParametersToAdapter(Adap.DeleteCommand);
                    break;
                case Sql_Commands.Insert:
                    AddParametersToAdapter(Adap.InsertCommand);
                    break;
            }
            void AddParametersToAdapter(SqlCommand cmd)
            {
                for (int i = 0; i < value.Length; i++)
                    cmd.Parameters.AddWithValue($"@{i}", value[i]);
            }
        }

        public static void AddParametersToCommand(SqlCommand command, params object[] value)
        {
            for (int i = 0; i < value.Length; i++)
                command.Parameters.AddWithValue($"@{i}", value[i]);
        }

        public static SqlConnection Build_Connection(bool createNewConnection = false)
        {
            if (connection == null || createNewConnection)
            {
                connection = new SqlConnection
                {
                    ConnectionString = $"Server={Server},1433; User Id=sa; password='{Password}'; database=pharmacy_management; Connection Timeout=3;"
                };
            }
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                return connection;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 18456)
                {
                    MessageBox.Show("رمز دیتابیس اشتباه است", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (createNewConnection)
                {
                    MessageBox.Show(ex.Message, "Server not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
                else
                    Build_Connection(true);
            }
            return connection;
        }

        public async static Task<SqlConnection> Build_ConnectionAsync()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = $"Server={Server},1433; User Id=sa; password='{Password}'; database=task_Manager; Connection Timeout=5;";
            try
            {
                await conn.OpenAsync();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 18456)
                {
                    MessageBox.Show("رمز دیتابیس اشتباه است", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Environment.Exit(0);
                }
                else
                    MessageBox.Show(ex.Message, "Error Connection Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch
            {
                return null;
            }
            return conn;
        }

        public static SqlConnection Test_Connection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = $"Server={Server},1433; User Id=sa; password='{Password}'; database=task_Manager; Connection Timeout=7;";
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return conn;
        }
    }
}
