namespace snakezz
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
         this.lbScore = new System.Windows.Forms.Label();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.selectLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.createLevelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.selectpanel = new System.Windows.Forms.Panel();
         this.btnDeleteSave = new System.Windows.Forms.Button();
         this.lbSaveGame = new System.Windows.Forms.Label();
         this.tbSaveGame = new System.Windows.Forms.TextBox();
         this.lbLoadGame = new System.Windows.Forms.Label();
         this.cmbLoadGame = new System.Windows.Forms.ComboBox();
         this.btnLoadGame = new System.Windows.Forms.Button();
         this.btnSaveGame = new System.Windows.Forms.Button();
         this.lbInterval = new System.Windows.Forms.Label();
         this.tbInterval = new System.Windows.Forms.TextBox();
         this.lbFoodNumber = new System.Windows.Forms.Label();
         this.tbFoodNumber = new System.Windows.Forms.TextBox();
         this.btSelectLevel = new System.Windows.Forms.Button();
         this.lbSelectlevel = new System.Windows.Forms.Label();
         this.cmbSelectLevel = new System.Windows.Forms.ComboBox();
         this.btStartLevel = new System.Windows.Forms.Button();
         this.createpanel = new System.Windows.Forms.Panel();
         this.blockPanel = new System.Windows.Forms.Panel();
         this.lbBlockTitle = new System.Windows.Forms.Label();
         this.tbBlockSize = new System.Windows.Forms.TextBox();
         this.tbBlockPoint = new System.Windows.Forms.TextBox();
         this.lbXYInfo1 = new System.Windows.Forms.Label();
         this.lbBlockPoint = new System.Windows.Forms.Label();
         this.lbXYInfo0 = new System.Windows.Forms.Label();
         this.lbBlockSize = new System.Windows.Forms.Label();
         this.btnAddSnake = new System.Windows.Forms.Button();
         this.btClearBlock = new System.Windows.Forms.Button();
         this.btAddBlock = new System.Windows.Forms.Button();
         this.btnCreateLvl = new System.Windows.Forms.Button();
         this.lbIntervalOpen = new System.Windows.Forms.Label();
         this.tbIntervalOpen = new System.Windows.Forms.TextBox();
         this.btSelectIntervalOpen = new System.Windows.Forms.Button();
         this.tbLevelName = new System.Windows.Forms.TextBox();
         this.lbLevelName = new System.Windows.Forms.Label();
         this.btnDeleteLevel = new System.Windows.Forms.Button();
         this.menuStrip1.SuspendLayout();
         this.selectpanel.SuspendLayout();
         this.createpanel.SuspendLayout();
         this.blockPanel.SuspendLayout();
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
         // lbScore
         // 
         this.lbScore.AutoSize = true;
         this.lbScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbScore.Location = new System.Drawing.Point(106, 32);
         this.lbScore.Name = "lbScore";
         this.lbScore.Size = new System.Drawing.Size(62, 16);
         this.lbScore.TabIndex = 2;
         this.lbScore.Text = "lbScore";
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
         this.selectpanel.Controls.Add(this.btnDeleteLevel);
         this.selectpanel.Controls.Add(this.btnDeleteSave);
         this.selectpanel.Controls.Add(this.lbSaveGame);
         this.selectpanel.Controls.Add(this.tbSaveGame);
         this.selectpanel.Controls.Add(this.lbLoadGame);
         this.selectpanel.Controls.Add(this.cmbLoadGame);
         this.selectpanel.Controls.Add(this.btnLoadGame);
         this.selectpanel.Controls.Add(this.btnSaveGame);
         this.selectpanel.Controls.Add(this.lbInterval);
         this.selectpanel.Controls.Add(this.tbInterval);
         this.selectpanel.Controls.Add(this.lbFoodNumber);
         this.selectpanel.Controls.Add(this.tbFoodNumber);
         this.selectpanel.Controls.Add(this.btSelectLevel);
         this.selectpanel.Controls.Add(this.lbSelectlevel);
         this.selectpanel.Controls.Add(this.cmbSelectLevel);
         this.selectpanel.Controls.Add(this.btStartLevel);
         this.selectpanel.Location = new System.Drawing.Point(316, 27);
         this.selectpanel.Name = "selectpanel";
         this.selectpanel.Size = new System.Drawing.Size(28, 24);
         this.selectpanel.TabIndex = 1;
         this.selectpanel.Visible = false;
         this.selectpanel.Paint += new System.Windows.Forms.PaintEventHandler(this.selectpanel_Paint);
         // 
         // btnDeleteSave
         // 
         this.btnDeleteSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnDeleteSave.Location = new System.Drawing.Point(1033, 82);
         this.btnDeleteSave.Name = "btnDeleteSave";
         this.btnDeleteSave.Size = new System.Drawing.Size(144, 25);
         this.btnDeleteSave.TabIndex = 15;
         this.btnDeleteSave.Text = "Delete Save";
         this.btnDeleteSave.UseVisualStyleBackColor = true;
         this.btnDeleteSave.Click += new System.EventHandler(this.btnDeleteSave_Click);
         // 
         // lbSaveGame
         // 
         this.lbSaveGame.AutoSize = true;
         this.lbSaveGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbSaveGame.Location = new System.Drawing.Point(789, 19);
         this.lbSaveGame.Name = "lbSaveGame";
         this.lbSaveGame.Size = new System.Drawing.Size(88, 16);
         this.lbSaveGame.TabIndex = 14;
         this.lbSaveGame.Text = "save name:";
         // 
         // tbSaveGame
         // 
         this.tbSaveGame.Location = new System.Drawing.Point(883, 18);
         this.tbSaveGame.Name = "tbSaveGame";
         this.tbSaveGame.Size = new System.Drawing.Size(144, 20);
         this.tbSaveGame.TabIndex = 13;
         this.tbSaveGame.TextChanged += new System.EventHandler(this.tbSaveGame_TextChanged);
         // 
         // lbLoadGame
         // 
         this.lbLoadGame.AutoSize = true;
         this.lbLoadGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbLoadGame.Location = new System.Drawing.Point(821, 53);
         this.lbLoadGame.Name = "lbLoadGame";
         this.lbLoadGame.Size = new System.Drawing.Size(55, 16);
         this.lbLoadGame.TabIndex = 12;
         this.lbLoadGame.Text = "saved:";
         // 
         // cmbLoadGame
         // 
         this.cmbLoadGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbLoadGame.FormattingEnabled = true;
         this.cmbLoadGame.Location = new System.Drawing.Point(882, 52);
         this.cmbLoadGame.Name = "cmbLoadGame";
         this.cmbLoadGame.Size = new System.Drawing.Size(144, 21);
         this.cmbLoadGame.TabIndex = 11;
         // 
         // btnLoadGame
         // 
         this.btnLoadGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnLoadGame.Location = new System.Drawing.Point(1033, 50);
         this.btnLoadGame.Name = "btnLoadGame";
         this.btnLoadGame.Size = new System.Drawing.Size(144, 25);
         this.btnLoadGame.TabIndex = 10;
         this.btnLoadGame.Text = "Load Game";
         this.btnLoadGame.UseVisualStyleBackColor = true;
         this.btnLoadGame.Click += new System.EventHandler(this.btnLoadGame_Click);
         // 
         // btnSaveGame
         // 
         this.btnSaveGame.Enabled = false;
         this.btnSaveGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnSaveGame.Location = new System.Drawing.Point(1033, 16);
         this.btnSaveGame.Name = "btnSaveGame";
         this.btnSaveGame.Size = new System.Drawing.Size(144, 25);
         this.btnSaveGame.TabIndex = 9;
         this.btnSaveGame.Text = "Save Game";
         this.btnSaveGame.UseVisualStyleBackColor = true;
         this.btnSaveGame.Click += new System.EventHandler(this.btnSaveGame_Click);
         // 
         // lbInterval
         // 
         this.lbInterval.AutoSize = true;
         this.lbInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbInterval.Location = new System.Drawing.Point(59, 100);
         this.lbInterval.Name = "lbInterval";
         this.lbInterval.Size = new System.Drawing.Size(63, 16);
         this.lbInterval.TabIndex = 8;
         this.lbInterval.Text = "Interval:";
         // 
         // tbInterval
         // 
         this.tbInterval.Location = new System.Drawing.Point(128, 99);
         this.tbInterval.Name = "tbInterval";
         this.tbInterval.Size = new System.Drawing.Size(156, 20);
         this.tbInterval.TabIndex = 7;
         // 
         // lbFoodNumber
         // 
         this.lbFoodNumber.AutoSize = true;
         this.lbFoodNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbFoodNumber.Location = new System.Drawing.Point(20, 68);
         this.lbFoodNumber.Name = "lbFoodNumber";
         this.lbFoodNumber.Size = new System.Drawing.Size(102, 16);
         this.lbFoodNumber.TabIndex = 6;
         this.lbFoodNumber.Text = "FoodNumber:";
         // 
         // tbFoodNumber
         // 
         this.tbFoodNumber.Location = new System.Drawing.Point(128, 67);
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
         this.btStartLevel.Click += new System.EventHandler(this.btnStartLevel_Click);
         // 
         // createpanel
         // 
         this.createpanel.Controls.Add(this.tbLevelName);
         this.createpanel.Controls.Add(this.lbLevelName);
         this.createpanel.Controls.Add(this.blockPanel);
         this.createpanel.Controls.Add(this.btnAddSnake);
         this.createpanel.Controls.Add(this.btClearBlock);
         this.createpanel.Controls.Add(this.btAddBlock);
         this.createpanel.Controls.Add(this.btnCreateLvl);
         this.createpanel.ImeMode = System.Windows.Forms.ImeMode.Off;
         this.createpanel.Location = new System.Drawing.Point(12, 60);
         this.createpanel.Name = "createpanel";
         this.createpanel.Size = new System.Drawing.Size(1200, 600);
         this.createpanel.TabIndex = 2;
         this.createpanel.Visible = false;
         this.createpanel.Paint += new System.Windows.Forms.PaintEventHandler(this.createpanel_Paint);
         // 
         // blockPanel
         // 
         this.blockPanel.Controls.Add(this.lbBlockTitle);
         this.blockPanel.Controls.Add(this.tbBlockSize);
         this.blockPanel.Controls.Add(this.tbBlockPoint);
         this.blockPanel.Controls.Add(this.lbXYInfo1);
         this.blockPanel.Controls.Add(this.lbBlockPoint);
         this.blockPanel.Controls.Add(this.lbXYInfo0);
         this.blockPanel.Controls.Add(this.lbBlockSize);
         this.blockPanel.Location = new System.Drawing.Point(159, 18);
         this.blockPanel.Name = "blockPanel";
         this.blockPanel.Size = new System.Drawing.Size(222, 111);
         this.blockPanel.TabIndex = 12;
         this.blockPanel.Visible = false;
         // 
         // lbBlockTitle
         // 
         this.lbBlockTitle.AutoSize = true;
         this.lbBlockTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbBlockTitle.Location = new System.Drawing.Point(43, 12);
         this.lbBlockTitle.Name = "lbBlockTitle";
         this.lbBlockTitle.Size = new System.Drawing.Size(96, 18);
         this.lbBlockTitle.TabIndex = 10;
         this.lbBlockTitle.Text = "lbBlockTitle";
         this.lbBlockTitle.Visible = false;
         // 
         // tbBlockSize
         // 
         this.tbBlockSize.Location = new System.Drawing.Point(100, 67);
         this.tbBlockSize.Name = "tbBlockSize";
         this.tbBlockSize.Size = new System.Drawing.Size(64, 20);
         this.tbBlockSize.TabIndex = 4;
         this.tbBlockSize.Visible = false;
         this.tbBlockSize.TextChanged += new System.EventHandler(this.tbBlockSize_TextChanged);
         // 
         // tbBlockPoint
         // 
         this.tbBlockPoint.Location = new System.Drawing.Point(100, 41);
         this.tbBlockPoint.Name = "tbBlockPoint";
         this.tbBlockPoint.Size = new System.Drawing.Size(64, 20);
         this.tbBlockPoint.TabIndex = 3;
         this.tbBlockPoint.Visible = false;
         this.tbBlockPoint.TextChanged += new System.EventHandler(this.tbBlockPoint_TextChanged);
         // 
         // lbXYInfo1
         // 
         this.lbXYInfo1.AutoSize = true;
         this.lbXYInfo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbXYInfo1.Location = new System.Drawing.Point(170, 68);
         this.lbXYInfo1.Name = "lbXYInfo1";
         this.lbXYInfo1.Size = new System.Drawing.Size(34, 15);
         this.lbXYInfo1.TabIndex = 9;
         this.lbXYInfo1.Text = "(x;y)";
         this.lbXYInfo1.Visible = false;
         // 
         // lbBlockPoint
         // 
         this.lbBlockPoint.AutoSize = true;
         this.lbBlockPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbBlockPoint.Location = new System.Drawing.Point(17, 42);
         this.lbBlockPoint.Name = "lbBlockPoint";
         this.lbBlockPoint.Size = new System.Drawing.Size(77, 15);
         this.lbBlockPoint.TabIndex = 6;
         this.lbBlockPoint.Text = "Start point:";
         this.lbBlockPoint.Visible = false;
         // 
         // lbXYInfo0
         // 
         this.lbXYInfo0.AutoSize = true;
         this.lbXYInfo0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbXYInfo0.Location = new System.Drawing.Point(170, 42);
         this.lbXYInfo0.Name = "lbXYInfo0";
         this.lbXYInfo0.Size = new System.Drawing.Size(34, 15);
         this.lbXYInfo0.TabIndex = 8;
         this.lbXYInfo0.Text = "(x;y)";
         this.lbXYInfo0.Visible = false;
         // 
         // lbBlockSize
         // 
         this.lbBlockSize.AutoSize = true;
         this.lbBlockSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbBlockSize.Location = new System.Drawing.Point(17, 69);
         this.lbBlockSize.Name = "lbBlockSize";
         this.lbBlockSize.Size = new System.Drawing.Size(76, 15);
         this.lbBlockSize.TabIndex = 7;
         this.lbBlockSize.Text = "Block size:";
         this.lbBlockSize.Visible = false;
         // 
         // btnAddSnake
         // 
         this.btnAddSnake.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnAddSnake.Location = new System.Drawing.Point(18, 116);
         this.btnAddSnake.Name = "btnAddSnake";
         this.btnAddSnake.Size = new System.Drawing.Size(123, 25);
         this.btnAddSnake.TabIndex = 11;
         this.btnAddSnake.Text = "Add snake";
         this.btnAddSnake.UseVisualStyleBackColor = true;
         this.btnAddSnake.Click += new System.EventHandler(this.btnAddSnake_Click);
         // 
         // btClearBlock
         // 
         this.btClearBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btClearBlock.Location = new System.Drawing.Point(18, 82);
         this.btClearBlock.Name = "btClearBlock";
         this.btClearBlock.Size = new System.Drawing.Size(123, 25);
         this.btClearBlock.TabIndex = 2;
         this.btClearBlock.Text = "Clear block";
         this.btClearBlock.UseVisualStyleBackColor = true;
         this.btClearBlock.Click += new System.EventHandler(this.btnClearBlock_Click);
         // 
         // btAddBlock
         // 
         this.btAddBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btAddBlock.Location = new System.Drawing.Point(18, 49);
         this.btAddBlock.Name = "btAddBlock";
         this.btAddBlock.Size = new System.Drawing.Size(123, 25);
         this.btAddBlock.TabIndex = 1;
         this.btAddBlock.Text = "Add block";
         this.btAddBlock.UseVisualStyleBackColor = true;
         this.btAddBlock.Click += new System.EventHandler(this.btnAddBlock_Click);
         // 
         // btnCreateLvl
         // 
         this.btnCreateLvl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnCreateLvl.Location = new System.Drawing.Point(18, 18);
         this.btnCreateLvl.Name = "btnCreateLvl";
         this.btnCreateLvl.Size = new System.Drawing.Size(123, 25);
         this.btnCreateLvl.TabIndex = 0;
         this.btnCreateLvl.Text = "Create Level";
         this.btnCreateLvl.UseVisualStyleBackColor = true;
         this.btnCreateLvl.Click += new System.EventHandler(this.btnCreateLvl_Click);
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
         this.tbIntervalOpen.TabIndex = 8;
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
         this.btSelectIntervalOpen.Click += new System.EventHandler(this.btnSelectIntervalOpen_Click);
         this.btSelectIntervalOpen.MouseHover += new System.EventHandler(this.btnSelectIntervalOpen_MouseHover);
         // 
         // tbLevelName
         // 
         this.tbLevelName.Location = new System.Drawing.Point(119, 159);
         this.tbLevelName.Name = "tbLevelName";
         this.tbLevelName.Size = new System.Drawing.Size(159, 20);
         this.tbLevelName.TabIndex = 13;
         this.tbLevelName.Visible = false;
         // 
         // lbLevelName
         // 
         this.lbLevelName.AutoSize = true;
         this.lbLevelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbLevelName.Location = new System.Drawing.Point(28, 160);
         this.lbLevelName.Name = "lbLevelName";
         this.lbLevelName.Size = new System.Drawing.Size(85, 15);
         this.lbLevelName.TabIndex = 14;
         this.lbLevelName.Text = "Level name:";
         this.lbLevelName.Visible = false;
         // 
         // btnDeleteLevel
         // 
         this.btnDeleteLevel.Enabled = false;
         this.btnDeleteLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnDeleteLevel.Location = new System.Drawing.Point(270, 28);
         this.btnDeleteLevel.Name = "btnDeleteLevel";
         this.btnDeleteLevel.Size = new System.Drawing.Size(144, 24);
         this.btnDeleteLevel.TabIndex = 16;
         this.btnDeleteLevel.Text = "Delete Level";
         this.btnDeleteLevel.UseVisualStyleBackColor = true;
         this.btnDeleteLevel.Click += new System.EventHandler(this.btnDeleteLevel_Click);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1232, 683);
         this.Controls.Add(this.createpanel);
         this.Controls.Add(this.btSelectIntervalOpen);
         this.Controls.Add(this.lbIntervalOpen);
         this.Controls.Add(this.tbIntervalOpen);
         this.Controls.Add(this.selectpanel);
         this.Controls.Add(this.lbScore);
         this.Controls.Add(this.gamepanel);
         this.Controls.Add(this.menuStrip1);
         this.DoubleBuffered = true;
         this.MainMenuStrip = this.menuStrip1;
         this.Name = "Form1";
         this.Text = "SnakeX";
         this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
         this.Resize += new System.EventHandler(this.Form1_Resize);
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.selectpanel.ResumeLayout(false);
         this.selectpanel.PerformLayout();
         this.createpanel.ResumeLayout(false);
         this.createpanel.PerformLayout();
         this.blockPanel.ResumeLayout(false);
         this.blockPanel.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Panel gamepanel;
      private System.Windows.Forms.Label lbScore;
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
      private System.Windows.Forms.Button btnCreateLvl;
      private System.Windows.Forms.Button btAddBlock;
      private System.Windows.Forms.Button btClearBlock;
      private System.Windows.Forms.Label lbBlockSize;
      private System.Windows.Forms.Label lbBlockPoint;
      private System.Windows.Forms.TextBox tbBlockPoint;
      private System.Windows.Forms.TextBox tbBlockSize;
      private System.Windows.Forms.Label lbXYInfo1;
      private System.Windows.Forms.Label lbXYInfo0;
      private System.Windows.Forms.Label lbBlockTitle;
      private System.Windows.Forms.Button btnAddSnake;
      private System.Windows.Forms.Panel blockPanel;
      private System.Windows.Forms.Label lbLoadGame;
      private System.Windows.Forms.ComboBox cmbLoadGame;
      private System.Windows.Forms.Button btnLoadGame;
      private System.Windows.Forms.Button btnSaveGame;
      private System.Windows.Forms.Label lbSaveGame;
      private System.Windows.Forms.TextBox tbSaveGame;
      private System.Windows.Forms.Button btnDeleteSave;
      private System.Windows.Forms.TextBox tbLevelName;
      private System.Windows.Forms.Label lbLevelName;
      private System.Windows.Forms.Button btnDeleteLevel;
   }
}

