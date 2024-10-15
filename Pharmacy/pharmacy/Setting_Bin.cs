using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy
{
    public class Setting_Bin
    {
        public static string Server { get { return _server; } set { _server = value; _change = true; } }
        public static string Password { get { return _password; } set { _password = value; _change = true; } }

        private static string _server;
        private static string _password;
        private static bool _change = false;

        public static bool Save()
        {
            if (_change)
            {
                bool complete = false;
                BinaryFormatter bf = null;
                FileStream fs = null;
                DialogResult result;
                do
                {
                    result = DialogResult.None;
                    try
                    {
                        fs = new FileStream("setting.bin", FileMode.Create, FileAccess.ReadWrite);
                        bf = new BinaryFormatter();
                        Setting_Bin set = new Setting_Bin() { save_Password = Password, save_Server = Server };
                        bf.Serialize(fs, set);
                        complete = true;
                    }
                    catch (SecurityException exSec)
                    {
                        MessageBox.Show(exSec.Message, "Error File Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = MessageBox.Show("برنامه به فایل دسترسی ندارد", "Error File Access", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }
                    catch (IOException exIo)
                    {
                        MessageBox.Show(exIo.Message, "Error IO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = MessageBox.Show("هنگام ذخیره اطلاعات برنامه به مشکل خورد", "Error IO", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }
                } while (result == DialogResult.Retry);
                if (complete)
                {
                    _change = false;
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool Load()
        {
            if (File.Exists("setting.bin"))
            {
                bool complete = false;
                BinaryFormatter bf = null;
                FileStream fs = null;
                var result = DialogResult.None;
                var tryAgain = false;
                do
                {
                    tryAgain = false;
                    try
                    {
                        fs = new FileStream("setting.bin", FileMode.Create, FileAccess.ReadWrite);
                        bf = new BinaryFormatter();
                        var set = (Setting_Bin)bf.Deserialize(fs);
                        Server = set.save_Server;
                        Password = set.save_Password;
                        _change = false;
                        complete = true;
                    }
                    catch (SecurityException exSec)
                    {
                        MessageBox.Show(exSec.Message, "Error File Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = MessageBox.Show("برنامه به فایل دسترسی ندارد", "Error File Access", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }
                    catch (IOException exIo)
                    {
                        MessageBox.Show(exIo.Message, "Error IO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = MessageBox.Show("هنگام خواندن اطلاعات برنامه به مشکل خورد", "Error IO", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                        if (result == DialogResult.Retry)
                            tryAgain = true;
                    }
                } while (tryAgain);
                if (complete)
                    return true;
                return false;
            }
            return false;
        }

        public string save_Server;
        public string save_Password;
    }
}
