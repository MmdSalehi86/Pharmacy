namespace pharmacy
{
    partial class Fr_Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_Username = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btn_login = new DevComponents.DotNetBar.ButtonX();
            this.btn_Exit = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txt_Password = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btn_Show_Pass = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // txt_Username
            // 
            // 
            // 
            // 
            this.txt_Username.Border.Class = "TextBoxBorder";
            this.txt_Username.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txt_Username.DisabledBackColor = System.Drawing.Color.White;
            this.txt_Username.Location = new System.Drawing.Point(93, 138);
            this.txt_Username.Name = "txt_Username";
            this.txt_Username.PreventEnterBeep = true;
            this.txt_Username.Size = new System.Drawing.Size(244, 39);
            this.txt_Username.TabIndex = 0;
            // 
            // btn_login
            // 
            this.btn_login.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_login.BackColor = System.Drawing.Color.Transparent;
            this.btn_login.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.btn_login.Font = new System.Drawing.Font("Vazir", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_login.ImageTextSpacing = 5;
            this.btn_login.Location = new System.Drawing.Point(80, 311);
            this.btn_login.Name = "btn_login";
            this.btn_login.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(10);
            this.btn_login.Size = new System.Drawing.Size(315, 52);
            this.btn_login.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn_login.Symbol = "";
            this.btn_login.SymbolColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_login.SymbolSize = 20F;
            this.btn_login.TabIndex = 2;
            this.btn_login.Text = "ورود";
            this.btn_login.TextColor = System.Drawing.Color.Black;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Exit.BackColor = System.Drawing.Color.Transparent;
            this.btn_Exit.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.btn_Exit.Font = new System.Drawing.Font("Vazir", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Exit.ImageTextSpacing = 5;
            this.btn_Exit.Location = new System.Drawing.Point(80, 381);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(10);
            this.btn_Exit.Size = new System.Drawing.Size(315, 51);
            this.btn_Exit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btn_Exit.Symbol = "";
            this.btn_Exit.SymbolColor = System.Drawing.Color.Red;
            this.btn_Exit.SymbolSize = 20F;
            this.btn_Exit.TabIndex = 3;
            this.btn_Exit.Text = "خروج";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.FontBold = true;
            this.labelX1.Location = new System.Drawing.Point(358, 140);
            this.labelX1.Name = "labelX1";
            this.labelX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelX1.SingleLineColor = System.Drawing.Color.Black;
            this.labelX1.Size = new System.Drawing.Size(85, 34);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "نام کاربری";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.FontBold = true;
            this.labelX2.Location = new System.Drawing.Point(143, 38);
            this.labelX2.Name = "labelX2";
            this.labelX2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelX2.Size = new System.Drawing.Size(200, 34);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "نرم افزار مدریت داروخانه";
            // 
            // txt_Password
            // 
            // 
            // 
            // 
            this.txt_Password.Border.Class = "TextBoxBorder";
            this.txt_Password.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txt_Password.DisabledBackColor = System.Drawing.Color.White;
            this.txt_Password.Location = new System.Drawing.Point(93, 204);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.PreventEnterBeep = true;
            this.txt_Password.Size = new System.Drawing.Size(244, 39);
            this.txt_Password.TabIndex = 1;
            this.txt_Password.UseSystemPasswordChar = true;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.FontBold = true;
            this.labelX3.Location = new System.Drawing.Point(358, 204);
            this.labelX3.Name = "labelX3";
            this.labelX3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelX3.Size = new System.Drawing.Size(68, 34);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "رمز عبور";
            // 
            // btn_Show_Pass
            // 
            this.btn_Show_Pass.BackgroundImage = global::pharmacy.Properties.Resources.hide;
            this.btn_Show_Pass.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            // 
            // 
            // 
            this.btn_Show_Pass.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.btn_Show_Pass.FontBold = true;
            this.btn_Show_Pass.Location = new System.Drawing.Point(36, 204);
            this.btn_Show_Pass.Name = "btn_Show_Pass";
            this.btn_Show_Pass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btn_Show_Pass.Size = new System.Drawing.Size(47, 39);
            this.btn_Show_Pass.TabIndex = 2;
            this.btn_Show_Pass.Click += new System.EventHandler(this.btn_Show_Pass_Click);
            // 
            // Fr_Login
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(487, 498);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.btn_Show_Pass);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.txt_Username);
            this.Font = new System.Drawing.Font("Vazir", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Fr_Login";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txt_Username;
        private DevComponents.DotNetBar.ButtonX btn_login;
        private DevComponents.DotNetBar.ButtonX btn_Exit;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_Password;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX btn_Show_Pass;
    }
}