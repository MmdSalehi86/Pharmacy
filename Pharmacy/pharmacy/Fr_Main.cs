using System;
using System.Globalization;
using System.Windows.Forms;

namespace pharmacy
{
    public partial class Fr_Main : Form
    {
        public Fr_Main()
        {
            InitializeComponent();
        }

        public bool isAdmin = false;

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            Fr_Info fr_Info = new Fr_Info();
            fr_Info.ShowDialog();
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
            Fr_User fr_User = new Fr_User();
            fr_User.ShowDialog();
        }

        private void timer_Time_Tick(object sender, EventArgs e)
        {
            lbl_Time.Text = DateTime.Now.ToLongTimeString();
        }

        private void Fr_Main_Load(object sender, EventArgs e)
        {
            lbl_Date.Text = GetDate();
            lbl_Time.Text = DateTime.Now.ToLongTimeString();

            if (!isAdmin)
            {
                btn_Add_User.Enabled = false;
                btn_Buy.Enabled = false;
                btn_Buy_List.Enabled = false;
                btn_Ch_D.Enabled = false;
                btn_Ch_P.Enabled = false;
                btn_DarooKh.Enabled = false;
            }
        }

        private string GetDate()
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime dateTime = DateTime.Parse(DateTime.Now.ToShortDateString());
            int day = pc.GetDayOfMonth(dateTime);
            int month = pc.GetMonth(dateTime);
            int yr = pc.GetYear(dateTime);
            return yr.ToString() + "/" + month.ToString("0#") + "/" + day.ToString("0#");
        }

        private void buttonItem5_Click(object sender, EventArgs e)
        {
            var fr_Group = new Fr_Group_List();
            fr_Group.ShowDialog();
        }

        private void buttonItem4_Click(object sender, EventArgs e)
        {
            var fr_Group = new Fr_Grouping();
            fr_Group.ShowDialog();
        }

        private void buttonItem6_Click(object sender, EventArgs e)
        {
            var fr_Com = new Fr_Company();
            fr_Com.ShowDialog();
        }

        private void buttonItem7_Click(object sender, EventArgs e)
        {
            var fr_Med = new Fr_Medicine();
            fr_Med.ShowDialog();
        }

        private void buttonItem8_Click(object sender, EventArgs e)
        {
            var fr_Gr_List = new Fr_Group_List();
            fr_Gr_List.ShowDialog();
        }

        private void buttonItem10_Click(object sender, EventArgs e)
        {
            var fr_Com_List = new Fr_Company_List();
            fr_Com_List.ShowDialog();
        }

        private void buttonItem11_Click(object sender, EventArgs e)
        {
            var fr_Ins = new Fr_Insurance();
            fr_Ins.ShowDialog();
        }

        private void buttonItem9_Click(object sender, EventArgs e)
        {
            var fr_Med_List = new Fr_Medicine_List();
            fr_Med_List.ShowDialog();
        }

        private void buttonItem12_Click(object sender, EventArgs e)
        {
            var fr_Pr_List = new Fr_Personnel_List();
            fr_Pr_List.ShowDialog();
        }

        private void buttonItem16_Click(object sender, EventArgs e)
        {
            var fr_Ca_List = new Fr_Cash_List();
            fr_Ca_List.ShowDialog();
        }

        private void buttonItem15_Click(object sender, EventArgs e)
        {
            var fr_Ca = new Fr_Cash();
            fr_Ca.ShowDialog();
        }

        private void buttonItem5_Click_1(object sender, EventArgs e)
        {
            var fr_Per = new Fr_Personnel();
            fr_Per.ShowDialog();
        }

        private void buttonItem14_Click(object sender, EventArgs e)
        {
            var fr_Aco = new Fr_Bank_Account();
            fr_Aco.ShowDialog();
        }

        private void buttonItem17_Click(object sender, EventArgs e)
        {
            var fr_Check_D = new Fr_Check();
            fr_Check_D.ShowDialog();
        }

        private void buttonItem18_Click(object sender, EventArgs e)
        {
            var fr_Ch_P = new Fr_Check_P();
            fr_Ch_P.ShowDialog();
        }

        private void buttonItem13_Click(object sender, EventArgs e)
        {
            var fr_Bk_List = new Fr_Bank_Account_List();
            fr_Bk_List.ShowDialog();
        }

        private void buttonItem19_Click(object sender, EventArgs e)
        {
            var fr_Sh_Med = new Fr_Medicine_Shortage();
            fr_Sh_Med.ShowDialog();
        }

        private void buttonItem20_Click(object sender, EventArgs e)
        {
            var fr_Ch_List = new Fr_Check_List();
            fr_Ch_List.ShowDialog();
        }

        private void buttonItem21_Click(object sender, EventArgs e)
        {
            var fr_Ch_P_List = new Fr_Check_P_List();
            fr_Ch_P_List.ShowDialog();
        }

        private void buttonItem22_Click(object sender, EventArgs e)
        {
            var fr_In_List = new Fr_Insurance_List();
            fr_In_List.ShowDialog();
        }

        private void buttonItem23_Click(object sender, EventArgs e)
        {
            var fr_Fac = new Fr_Buy();
            fr_Fac.ShowDialog();
        }

        private void buttonItem24_Click(object sender, EventArgs e)
        {
            var fr_Fac_List = new Fr_Buy_List();
            fr_Fac_List.ShowDialog();
        }

        private void buttonItem26_Click(object sender, EventArgs e)
        {
            var fr_Sell_List = new Fr_Sell_List();
            fr_Sell_List.ShowDialog();
        }

        private void buttonItem25_Click(object sender, EventArgs e)
        {
            var fr_Sell = new Fr_Sell();
            fr_Sell.ShowDialog();
        }
    }
}
