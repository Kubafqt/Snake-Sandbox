namespace snake
{
    partial class Form1
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
            this.gamepanel = new System.Windows.Forms.Panel();
            this.lbOne = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createLevelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectpanel = new System.Windows.Forms.Panel();
            this.lbInterval = new System.Windows.Forms.Label();
            this.tbInterval = new System.Windows.Forms.TextBox();
            this.lbFoodNumber = new System.Windows.Forms.Label();
            this.tbFoodNumber = new System.Windows.Forms.TextBox();
            this.btSelectLevel = new System.Windows.Forms.Button();
            this.lbSelectlevel = new System.Windows.Forms.Label();
            this.cmbSelectLevel = new System.Windows.Forms.ComboBox();
            this.btStartLevel = new System.Windows.Forms.Button();
            this.createpanel = new System.Windows.Forms.Panel();
            this.lbIntervalOpen = new System.Windows.Forms.Label();
            this.tbIntervalOpen = new System.Windows.Forms.TextBox();
            this.btSelectIntervalOpen = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.selectpanel.SuspendLayout();
            this.createpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gamepanel
            // 
            this.gamepanel.Location = new System.Drawing.Point(12, 60);
            this.gamepanel.Name = "gamepanel";
            this.gamepanel.Size = new System.Drawing.Size(55, 46);
            this.gamepanel.TabIndex = 1;
            this.gamepanel.Paint += new System.Windows.Forms.PaintEventHandler(this.gamepanel_Paint);
            // 
            // lbOne
            // 
            this.lbOne.AutoSize = true;
            this.lbOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbOne.Location = new System.Drawing.Point(104, 32);
            this.lbOne.Name = "lbOne";
            this.lbOne.Size = new System.Drawing.Size(49, 16);
            this.lbOne.TabIndex = 2;
            this.lbOne.Text = "lbOne";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.selectLevelToolStripMenuItem,
            this.createLevelsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1232, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.gameToolStripMenuItem.Text = "game";
            this.gameToolStripMenuItem.Click += new System.EventHandler(this.gameToolStripMenuItem_Click);
            // 
            // selectLevelToolStripMenuItem
            // 
            this.selectLevelToolStripMenuItem.Name = "selectLevelToolStripMenuItem";
            this.selectLevelToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.selectLevelToolStripMenuItem.Text = "select level";
            this.selectLevelToolStripMenuItem.Click += new System.EventHandler(this.selectLevelToolStripMenuItem_Click);
            // 
            // createLevelsToolStripMenuItem
            // 
            this.createLevelsToolStripMenuItem.Name = "createLevelsToolStripMenuItem";
            this.createLevelsToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.createLevelsToolStripMenuItem.Text = "create levels";
            this.createLevelsToolStripMenuItem.Click += new System.EventHandler(this.createLevelsToolStripMenuItem_Click);
            // 
            // selectpanel
            // 
            this.selectpanel.Controls.Add(this.lbInterval);
            this.selectpanel.Controls.Add(this.tbInterval);
            this.selectpanel.Controls.Add(this.lbFoodNumber);
            this.selectpanel.Controls.Add(this.tbFoodNumber);
            this.selectpanel.Controls.Add(this.btSelectLevel);
            this.selectpanel.Controls.Add(this.lbSelectlevel);
            this.selectpanel.Controls.Add(this.cmbSelectLevel);
            this.selectpanel.Controls.Add(this.btStartLevel);
            this.selectpanel.Location = new System.Drawing.Point(264, 12);
            this.selectpanel.Name = "selectpanel";
            this.selectpanel.Size = new System.Drawing.Size(39, 39);
            this.selectpanel.TabIndex = 4;
            this.selectpanel.Visible = false;
            this.selectpanel.Paint += new System.Windows.Forms.PaintEventHandler(this.selectpanel_Paint);
            // 
            // lbInterval
            // 
            this.lbInterval.AutoSize = true;
            this.lbInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbInterval.Location = new System.Drawing.Point(118, 165);
            this.lbInterval.Name = "lbInterval";
            this.lbInterval.Size = new System.Drawing.Size(63, 16);
            this.lbInterval.TabIndex = 8;
            this.lbInterval.Text = "Interval:";
            // 
            // tbInterval
            // 
            this.tbInterval.Location = new System.Drawing.Point(187, 164);
            this.tbInterval.Name = "tbInterval";
            this.tbInterval.Size = new System.Drawing.Size(156, 20);
            this.tbInterval.TabIndex = 7;
            // 
            // lbFoodNumber
            // 
            this.lbFoodNumber.AutoSize = true;
            this.lbFoodNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbFoodNumber.Location = new System.Drawing.Point(79, 133);
            this.lbFoodNumber.Name = "lbFoodNumber";
            this.lbFoodNumber.Size = new System.Drawing.Size(102, 16);
            this.lbFoodNumber.TabIndex = 6;
            this.lbFoodNumber.Text = "FoodNumber:";
            // 
            // tbFoodNumber
            // 
            this.tbFoodNumber.Location = new System.Drawing.Point(187, 132);
            this.tbFoodNumber.Name = "tbFoodNumber";
            this.tbFoodNumber.Size = new System.Drawing.Size(156, 20);
            this.tbFoodNumber.TabIndex = 5;
            // 
            // btSelectLevel
            // 
            this.btSelectLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btSelectLevel.Location = new System.Drawing.Point(916, 514);
            this.btSelectLevel.Name = "btSelectLevel";
            this.btSelectLevel.Size = new System.Drawing.Size(123, 66);
            this.btSelectLevel.TabIndex = 4;
            this.btSelectLevel.Text = "Select";
            this.btSelectLevel.UseVisualStyleBackColor = true;
            this.btSelectLevel.Click += new System.EventHandler(this.btSelectLevel_Click);
            // 
            // lbSelectlevel
            // 
            this.lbSelectlevel.AutoSize = true;
            this.lbSelectlevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbSelectlevel.Location = new System.Drawing.Point(28, 30);
            this.lbSelectlevel.Name = "lbSelectlevel";
            this.lbSelectlevel.Size = new System.Drawing.Size(94, 16);
            this.lbSelectlevel.TabIndex = 3;
            this.lbSelectlevel.Text = "Select level:";
            // 
            // cmbSelectLevel
            // 
            this.cmbSelectLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectLevel.FormattingEnabled = true;
            this.cmbSelectLevel.Location = new System.Drawing.Point(128, 29);
            this.cmbSelectLevel.Name = "cmbSelectLevel";
            this.cmbSelectLevel.Size = new System.Drawing.Size(121, 21);
            this.cmbSelectLevel.TabIndex = 1;
            this.cmbSelectLevel.SelectedIndexChanged += new System.EventHandler(this.cmbSelectLevel_SelectedIndexChanged);
            // 
            // btStartLevel
            // 
            this.btStartLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btStartLevel.Location = new System.Drawing.Point(1055, 514);
            this.btStartLevel.Name = "btStartLevel";
            this.btStartLevel.Size = new System.Drawing.Size(123, 66);
            this.btStartLevel.TabIndex = 0;
            this.btStartLevel.Text = "Start";
            this.btStartLevel.UseVisualStyleBackColor = true;
            this.btStartLevel.Click += new System.EventHandler(this.btStartLevel_Click);
            // 
            // createpanel
            // 
            this.createpanel.Controls.Add(this.button1);
            this.createpanel.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.createpanel.Location = new System.Drawing.Point(12, 60);
            this.createpanel.Name = "createpanel";
            this.createpanel.Size = new System.Drawing.Size(1200, 600);
            this.createpanel.TabIndex = 2;
            this.createpanel.Visible = false;
            this.createpanel.Paint += new System.Windows.Forms.PaintEventHandler(this.createpanel_Paint);
            // 
            // lbIntervalOpen
            // 
            this.lbIntervalOpen.AutoSize = true;
            this.lbIntervalOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbIntervalOpen.Location = new System.Drawing.Point(853, 32);
            this.lbIntervalOpen.Name = "lbIntervalOpen";
            this.lbIntervalOpen.Size = new System.Drawing.Size(63, 16);
            this.lbIntervalOpen.TabIndex = 10;
            this.lbIntervalOpen.Text = "Interval:";
            // 
            // tbIntervalOpen
            // 
            this.tbIntervalOpen.Location = new System.Drawing.Point(922, 31);
            this.tbIntervalOpen.Name = "tbIntervalOpen";
            this.tbIntervalOpen.ReadOnly = true;
            this.tbIntervalOpen.Size = new System.Drawing.Size(156, 20);
            this.tbIntervalOpen.TabIndex = 9;
            this.tbIntervalOpen.MouseHover += new System.EventHandler(this.tbIntervalOpen_MouseHover);
            // 
            // btSelectIntervalOpen
            // 
            this.btSelectIntervalOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btSelectIntervalOpen.Location = new System.Drawing.Point(1084, 27);
            this.btSelectIntervalOpen.Name = "btSelectIntervalOpen";
            this.btSelectIntervalOpen.Size = new System.Drawing.Size(69, 27);
            this.btSelectIntervalOpen.TabIndex = 9;
            this.btSelectIntervalOpen.Text = "Select";
            this.btSelectIntervalOpen.UseVisualStyleBackColor = true;
            this.btSelectIntervalOpen.Click += new System.EventHandler(this.btSelectIntervalOpen_Click);
            this.btSelectIntervalOpen.MouseHover += new System.EventHandler(this.btSelectIntervalOpen_MouseHover);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(66, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 678);
            this.Controls.Add(this.btSelectIntervalOpen);
            this.Controls.Add(this.lbIntervalOpen);
            this.Controls.Add(this.tbIntervalOpen);
            this.Controls.Add(this.createpanel);
            this.Controls.Add(this.selectpanel);
            this.Controls.Add(this.lbOne);
            this.Controls.Add(this.gamepanel);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "SnakeX";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.selectpanel.ResumeLayout(false);
            this.selectpanel.PerformLayout();
            this.createpanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel gamepanel;
        private System.Windows.Forms.Label lbOne;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectLevelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createLevelsToolStripMenuItem;
        private System.Windows.Forms.Panel selectpanel;
        private System.Windows.Forms.Panel createpanel;
        private System.Windows.Forms.ComboBox cmbSelectLevel;
        private System.Windows.Forms.Button btStartLevel;
        private System.Windows.Forms.Label lbSelectlevel;
        private System.Windows.Forms.Button btSelectLevel;
        private System.Windows.Forms.Label lbInterval;
        private System.Windows.Forms.TextBox tbInterval;
        private System.Windows.Forms.Label lbFoodNumber;
        private System.Windows.Forms.TextBox tbFoodNumber;
        private System.Windows.Forms.Label lbIntervalOpen;
        private System.Windows.Forms.TextBox tbIntervalOpen;
        private System.Windows.Forms.Button btSelectIntervalOpen;
        private System.Windows.Forms.Button button1;
    }
}

