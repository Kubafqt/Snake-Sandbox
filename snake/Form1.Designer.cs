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
         this.levelpanel = new System.Windows.Forms.Panel();
         this.btnEditLevel = new System.Windows.Forms.Button();
         this.btnDeleteLevel = new System.Windows.Forms.Button();
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
         this.btnChangeDetail = new System.Windows.Forms.Button();
         this.lbSelectlevel = new System.Windows.Forms.Label();
         this.cmbSelectedLevel = new System.Windows.Forms.ComboBox();
         this.btStartLevel = new System.Windows.Forms.Button();
         this.createpanel = new System.Windows.Forms.Panel();
         this.tbCFoodnumber = new System.Windows.Forms.TextBox();
         this.lbCFoodNumber = new System.Windows.Forms.Label();
         this.tbLevelName = new System.Windows.Forms.TextBox();
         this.lbLevelName = new System.Windows.Forms.Label();
         this.blockPanel = new System.Windows.Forms.Panel();
         this.lbGameSize = new System.Windows.Forms.Label();
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
         this.createpanelUI = new System.Windows.Forms.Panel();
         this.checkBoxPassableEdges = new System.Windows.Forms.CheckBox();
         this.btnCreateLevelStart = new System.Windows.Forms.Button();
         this.selectChBoxPassableEdges = new System.Windows.Forms.CheckBox();
         this.menuStrip1.SuspendLayout();
         this.levelpanel.SuspendLayout();
         this.blockPanel.SuspendLayout();
         this.createpanelUI.SuspendLayout();
         this.SuspendLayout();
         // 
         // gamepanel
         // 
         this.gamepanel.Location = new System.Drawing.Point(189, 37);
         this.gamepanel.Name = "gamepanel";
         this.gamepanel.Size = new System.Drawing.Size(32, 24);
         this.gamepanel.TabIndex = 1;
         this.gamepanel.Paint += new System.Windows.Forms.PaintEventHandler(this.gamepanel_Paint);
         // 
         // lbScore
         // 
         this.lbScore.AutoSize = true;
         this.lbScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbScore.Location = new System.Drawing.Point(108, 37);
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
         this.menuStrip1.Size = new System.Drawing.Size(1511, 24);
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
         // levelpanel
         // 
         this.levelpanel.Controls.Add(this.selectChBoxPassableEdges);
         this.levelpanel.Controls.Add(this.btnEditLevel);
         this.levelpanel.Controls.Add(this.btnDeleteLevel);
         this.levelpanel.Controls.Add(this.btnDeleteSave);
         this.levelpanel.Controls.Add(this.lbSaveGame);
         this.levelpanel.Controls.Add(this.tbSaveGame);
         this.levelpanel.Controls.Add(this.lbLoadGame);
         this.levelpanel.Controls.Add(this.cmbLoadGame);
         this.levelpanel.Controls.Add(this.btnLoadGame);
         this.levelpanel.Controls.Add(this.btnSaveGame);
         this.levelpanel.Controls.Add(this.lbInterval);
         this.levelpanel.Controls.Add(this.tbInterval);
         this.levelpanel.Controls.Add(this.lbFoodNumber);
         this.levelpanel.Controls.Add(this.tbFoodNumber);
         this.levelpanel.Controls.Add(this.btnChangeDetail);
         this.levelpanel.Controls.Add(this.lbSelectlevel);
         this.levelpanel.Controls.Add(this.cmbSelectedLevel);
         this.levelpanel.Controls.Add(this.btStartLevel);
         this.levelpanel.Location = new System.Drawing.Point(243, 38);
         this.levelpanel.Name = "levelpanel";
         this.levelpanel.Size = new System.Drawing.Size(640, 415);
         this.levelpanel.TabIndex = 1;
         this.levelpanel.Visible = false;
         this.levelpanel.Paint += new System.Windows.Forms.PaintEventHandler(this.levelpanel_Paint);
         // 
         // btnEditLevel
         // 
         this.btnEditLevel.Enabled = false;
         this.btnEditLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnEditLevel.Location = new System.Drawing.Point(328, 166);
         this.btnEditLevel.Name = "btnEditLevel";
         this.btnEditLevel.Size = new System.Drawing.Size(144, 24);
         this.btnEditLevel.TabIndex = 17;
         this.btnEditLevel.Text = "Edit Level";
         this.btnEditLevel.UseVisualStyleBackColor = true;
         // 
         // btnDeleteLevel
         // 
         this.btnDeleteLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnDeleteLevel.Location = new System.Drawing.Point(328, 128);
         this.btnDeleteLevel.Name = "btnDeleteLevel";
         this.btnDeleteLevel.Size = new System.Drawing.Size(144, 24);
         this.btnDeleteLevel.TabIndex = 16;
         this.btnDeleteLevel.Text = "Delete Level";
         this.btnDeleteLevel.UseVisualStyleBackColor = true;
         this.btnDeleteLevel.Click += new System.EventHandler(this.btnDeleteLevel_Click);
         // 
         // btnDeleteSave
         // 
         this.btnDeleteSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnDeleteSave.Location = new System.Drawing.Point(992, 118);
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
         this.lbSaveGame.Location = new System.Drawing.Point(740, 56);
         this.lbSaveGame.Name = "lbSaveGame";
         this.lbSaveGame.Size = new System.Drawing.Size(88, 16);
         this.lbSaveGame.TabIndex = 14;
         this.lbSaveGame.Text = "save name:";
         // 
         // tbSaveGame
         // 
         this.tbSaveGame.Location = new System.Drawing.Point(834, 55);
         this.tbSaveGame.Name = "tbSaveGame";
         this.tbSaveGame.Size = new System.Drawing.Size(144, 20);
         this.tbSaveGame.TabIndex = 13;
         this.tbSaveGame.TextChanged += new System.EventHandler(this.tbSaveGame_TextChanged);
         // 
         // lbLoadGame
         // 
         this.lbLoadGame.AutoSize = true;
         this.lbLoadGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbLoadGame.Location = new System.Drawing.Point(772, 90);
         this.lbLoadGame.Name = "lbLoadGame";
         this.lbLoadGame.Size = new System.Drawing.Size(55, 16);
         this.lbLoadGame.TabIndex = 12;
         this.lbLoadGame.Text = "saved:";
         // 
         // cmbLoadGame
         // 
         this.cmbLoadGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbLoadGame.FormattingEnabled = true;
         this.cmbLoadGame.Location = new System.Drawing.Point(833, 89);
         this.cmbLoadGame.Name = "cmbLoadGame";
         this.cmbLoadGame.Size = new System.Drawing.Size(144, 21);
         this.cmbLoadGame.TabIndex = 11;
         // 
         // btnLoadGame
         // 
         this.btnLoadGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnLoadGame.Location = new System.Drawing.Point(992, 86);
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
         this.btnSaveGame.Location = new System.Drawing.Point(992, 52);
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
         this.lbInterval.Location = new System.Drawing.Point(92, 122);
         this.lbInterval.Name = "lbInterval";
         this.lbInterval.Size = new System.Drawing.Size(63, 16);
         this.lbInterval.TabIndex = 8;
         this.lbInterval.Text = "Interval:";
         // 
         // tbInterval
         // 
         this.tbInterval.Location = new System.Drawing.Point(161, 121);
         this.tbInterval.Name = "tbInterval";
         this.tbInterval.Size = new System.Drawing.Size(54, 20);
         this.tbInterval.TabIndex = 7;
         // 
         // lbFoodNumber
         // 
         this.lbFoodNumber.AutoSize = true;
         this.lbFoodNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbFoodNumber.Location = new System.Drawing.Point(53, 90);
         this.lbFoodNumber.Name = "lbFoodNumber";
         this.lbFoodNumber.Size = new System.Drawing.Size(102, 16);
         this.lbFoodNumber.TabIndex = 6;
         this.lbFoodNumber.Text = "FoodNumber:";
         // 
         // tbFoodNumber
         // 
         this.tbFoodNumber.Location = new System.Drawing.Point(161, 89);
         this.tbFoodNumber.Name = "tbFoodNumber";
         this.tbFoodNumber.Size = new System.Drawing.Size(54, 20);
         this.tbFoodNumber.TabIndex = 5;
         // 
         // btnChangeDetail
         // 
         this.btnChangeDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.btnChangeDetail.Location = new System.Drawing.Point(328, 90);
         this.btnChangeDetail.Name = "btnChangeDetail";
         this.btnChangeDetail.Size = new System.Drawing.Size(144, 24);
         this.btnChangeDetail.TabIndex = 4;
         this.btnChangeDetail.Text = "Change Detail";
         this.btnChangeDetail.UseVisualStyleBackColor = true;
         this.btnChangeDetail.Click += new System.EventHandler(this.btnChangeDetail_Click);
         // 
         // lbSelectlevel
         // 
         this.lbSelectlevel.AutoSize = true;
         this.lbSelectlevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbSelectlevel.Location = new System.Drawing.Point(61, 52);
         this.lbSelectlevel.Name = "lbSelectlevel";
         this.lbSelectlevel.Size = new System.Drawing.Size(94, 16);
         this.lbSelectlevel.TabIndex = 3;
         this.lbSelectlevel.Text = "Select level:";
         // 
         // cmbSelectedLevel
         // 
         this.cmbSelectedLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbSelectedLevel.FormattingEnabled = true;
         this.cmbSelectedLevel.Location = new System.Drawing.Point(161, 51);
         this.cmbSelectedLevel.Name = "cmbSelectedLevel";
         this.cmbSelectedLevel.Size = new System.Drawing.Size(125, 21);
         this.cmbSelectedLevel.TabIndex = 1;
         // 
         // btStartLevel
         // 
         this.btStartLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
         this.btStartLevel.Location = new System.Drawing.Point(328, 52);
         this.btStartLevel.Name = "btStartLevel";
         this.btStartLevel.Size = new System.Drawing.Size(144, 24);
         this.btStartLevel.TabIndex = 0;
         this.btStartLevel.Text = "Start Level";
         this.btStartLevel.UseVisualStyleBackColor = true;
         this.btStartLevel.Click += new System.EventHandler(this.btnStartLevel_Click);
         // 
         // createpanel
         // 
         this.createpanel.ImeMode = System.Windows.Forms.ImeMode.Off;
         this.createpanel.Location = new System.Drawing.Point(18, 70);
         this.createpanel.Name = "createpanel";
         this.createpanel.Size = new System.Drawing.Size(1200, 600);
         this.createpanel.TabIndex = 2;
         this.createpanel.Visible = false;
         this.createpanel.Paint += new System.Windows.Forms.PaintEventHandler(this.createpanel_Paint);
         // 
         // tbCFoodnumber
         // 
         this.tbCFoodnumber.Enabled = false;
         this.tbCFoodnumber.Location = new System.Drawing.Point(118, 131);
         this.tbCFoodnumber.Name = "tbCFoodnumber";
         this.tbCFoodnumber.Size = new System.Drawing.Size(41, 20);
         this.tbCFoodnumber.TabIndex = 2;
         this.tbCFoodnumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         // 
         // lbCFoodNumber
         // 
         this.lbCFoodNumber.AutoSize = true;
         this.lbCFoodNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbCFoodNumber.Location = new System.Drawing.Point(16, 133);
         this.lbCFoodNumber.Name = "lbCFoodNumber";
         this.lbCFoodNumber.Size = new System.Drawing.Size(96, 15);
         this.lbCFoodNumber.TabIndex = 16;
         this.lbCFoodNumber.Text = "Food number:";
         // 
         // tbLevelName
         // 
         this.tbLevelName.Enabled = false;
         this.tbLevelName.Location = new System.Drawing.Point(107, 104);
         this.tbLevelName.Name = "tbLevelName";
         this.tbLevelName.Size = new System.Drawing.Size(139, 20);
         this.tbLevelName.TabIndex = 1;
         // 
         // lbLevelName
         // 
         this.lbLevelName.AutoSize = true;
         this.lbLevelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbLevelName.Location = new System.Drawing.Point(16, 105);
         this.lbLevelName.Name = "lbLevelName";
         this.lbLevelName.Size = new System.Drawing.Size(85, 15);
         this.lbLevelName.TabIndex = 14;
         this.lbLevelName.Text = "Level name:";
         // 
         // blockPanel
         // 
         this.blockPanel.Controls.Add(this.lbGameSize);
         this.blockPanel.Controls.Add(this.lbBlockTitle);
         this.blockPanel.Controls.Add(this.tbBlockSize);
         this.blockPanel.Controls.Add(this.tbBlockPoint);
         this.blockPanel.Controls.Add(this.lbXYInfo1);
         this.blockPanel.Controls.Add(this.lbBlockPoint);
         this.blockPanel.Controls.Add(this.lbXYInfo0);
         this.blockPanel.Controls.Add(this.lbBlockSize);
         this.blockPanel.Location = new System.Drawing.Point(20, 273);
         this.blockPanel.Name = "blockPanel";
         this.blockPanel.Size = new System.Drawing.Size(222, 118);
         this.blockPanel.TabIndex = 1;
         this.blockPanel.Visible = false;
         // 
         // lbGameSize
         // 
         this.lbGameSize.AutoSize = true;
         this.lbGameSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbGameSize.Location = new System.Drawing.Point(20, 95);
         this.lbGameSize.Name = "lbGameSize";
         this.lbGameSize.Size = new System.Drawing.Size(108, 15);
         this.lbGameSize.TabIndex = 17;
         this.lbGameSize.Text = "game size (x,y):";
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
         // 
         // tbBlockSize
         // 
         this.tbBlockSize.Location = new System.Drawing.Point(100, 67);
         this.tbBlockSize.Name = "tbBlockSize";
         this.tbBlockSize.Size = new System.Drawing.Size(64, 20);
         this.tbBlockSize.TabIndex = 7;
         this.tbBlockSize.TextChanged += new System.EventHandler(this.tbBlockSize_TextChanged);
         // 
         // tbBlockPoint
         // 
         this.tbBlockPoint.Location = new System.Drawing.Point(100, 41);
         this.tbBlockPoint.Name = "tbBlockPoint";
         this.tbBlockPoint.Size = new System.Drawing.Size(64, 20);
         this.tbBlockPoint.TabIndex = 6;
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
         // 
         // btnAddSnake
         // 
         this.btnAddSnake.Enabled = false;
         this.btnAddSnake.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnAddSnake.Location = new System.Drawing.Point(70, 232);
         this.btnAddSnake.Name = "btnAddSnake";
         this.btnAddSnake.Size = new System.Drawing.Size(123, 26);
         this.btnAddSnake.TabIndex = 5;
         this.btnAddSnake.Text = "Add snake";
         this.btnAddSnake.UseVisualStyleBackColor = true;
         this.btnAddSnake.Click += new System.EventHandler(this.btnAddSnake_Click);
         // 
         // btClearBlock
         // 
         this.btClearBlock.Enabled = false;
         this.btClearBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btClearBlock.Location = new System.Drawing.Point(70, 201);
         this.btClearBlock.Name = "btClearBlock";
         this.btClearBlock.Size = new System.Drawing.Size(123, 26);
         this.btClearBlock.TabIndex = 4;
         this.btClearBlock.Text = "Clear blocks";
         this.btClearBlock.UseVisualStyleBackColor = true;
         this.btClearBlock.Click += new System.EventHandler(this.btnClearBlock_Click);
         // 
         // btAddBlock
         // 
         this.btAddBlock.Enabled = false;
         this.btAddBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btAddBlock.Location = new System.Drawing.Point(70, 168);
         this.btAddBlock.Name = "btAddBlock";
         this.btAddBlock.Size = new System.Drawing.Size(123, 26);
         this.btAddBlock.TabIndex = 3;
         this.btAddBlock.Text = "Add block";
         this.btAddBlock.UseVisualStyleBackColor = true;
         this.btAddBlock.Click += new System.EventHandler(this.btnAddBlock_Click);
         // 
         // btnCreateLvl
         // 
         this.btnCreateLvl.Enabled = false;
         this.btnCreateLvl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnCreateLvl.Location = new System.Drawing.Point(70, 524);
         this.btnCreateLvl.Name = "btnCreateLvl";
         this.btnCreateLvl.Size = new System.Drawing.Size(124, 32);
         this.btnCreateLvl.TabIndex = 8;
         this.btnCreateLvl.Text = "Create Level";
         this.btnCreateLvl.UseVisualStyleBackColor = true;
         this.btnCreateLvl.Click += new System.EventHandler(this.btnCreateLvl_Click);
         // 
         // lbIntervalOpen
         // 
         this.lbIntervalOpen.AutoSize = true;
         this.lbIntervalOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.lbIntervalOpen.Location = new System.Drawing.Point(889, 38);
         this.lbIntervalOpen.Name = "lbIntervalOpen";
         this.lbIntervalOpen.Size = new System.Drawing.Size(63, 16);
         this.lbIntervalOpen.TabIndex = 10;
         this.lbIntervalOpen.Text = "Interval:";
         // 
         // tbIntervalOpen
         // 
         this.tbIntervalOpen.Location = new System.Drawing.Point(958, 37);
         this.tbIntervalOpen.Name = "tbIntervalOpen";
         this.tbIntervalOpen.ReadOnly = true;
         this.tbIntervalOpen.Size = new System.Drawing.Size(156, 20);
         this.tbIntervalOpen.TabIndex = 8;
         this.tbIntervalOpen.MouseHover += new System.EventHandler(this.tbIntervalOpen_MouseHover);
         // 
         // btSelectIntervalOpen
         // 
         this.btSelectIntervalOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btSelectIntervalOpen.Location = new System.Drawing.Point(1120, 35);
         this.btSelectIntervalOpen.Name = "btSelectIntervalOpen";
         this.btSelectIntervalOpen.Size = new System.Drawing.Size(69, 23);
         this.btSelectIntervalOpen.TabIndex = 9;
         this.btSelectIntervalOpen.Text = "Select";
         this.btSelectIntervalOpen.UseVisualStyleBackColor = true;
         this.btSelectIntervalOpen.Click += new System.EventHandler(this.btnSelectIntervalOpen_Click);
         this.btSelectIntervalOpen.MouseHover += new System.EventHandler(this.btnSelectIntervalOpen_MouseHover);
         // 
         // createpanelUI
         // 
         this.createpanelUI.Controls.Add(this.checkBoxPassableEdges);
         this.createpanelUI.Controls.Add(this.btnCreateLevelStart);
         this.createpanelUI.Controls.Add(this.tbCFoodnumber);
         this.createpanelUI.Controls.Add(this.btnCreateLvl);
         this.createpanelUI.Controls.Add(this.lbCFoodNumber);
         this.createpanelUI.Controls.Add(this.btAddBlock);
         this.createpanelUI.Controls.Add(this.tbLevelName);
         this.createpanelUI.Controls.Add(this.btClearBlock);
         this.createpanelUI.Controls.Add(this.lbLevelName);
         this.createpanelUI.Controls.Add(this.btnAddSnake);
         this.createpanelUI.Controls.Add(this.blockPanel);
         this.createpanelUI.Location = new System.Drawing.Point(1232, 70);
         this.createpanelUI.Name = "createpanelUI";
         this.createpanelUI.Size = new System.Drawing.Size(262, 600);
         this.createpanelUI.TabIndex = 1;
         this.createpanelUI.Visible = false;
         this.createpanelUI.Paint += new System.Windows.Forms.PaintEventHandler(this.createpanelUI_Paint);
         // 
         // checkBoxPassableEdges
         // 
         this.checkBoxPassableEdges.AutoSize = true;
         this.checkBoxPassableEdges.Enabled = false;
         this.checkBoxPassableEdges.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.checkBoxPassableEdges.Location = new System.Drawing.Point(66, 414);
         this.checkBoxPassableEdges.Name = "checkBoxPassableEdges";
         this.checkBoxPassableEdges.Size = new System.Drawing.Size(129, 19);
         this.checkBoxPassableEdges.TabIndex = 17;
         this.checkBoxPassableEdges.Text = "Passable Edges";
         this.checkBoxPassableEdges.UseVisualStyleBackColor = true;
         // 
         // btnCreateLevelStart
         // 
         this.btnCreateLevelStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.btnCreateLevelStart.Location = new System.Drawing.Point(35, 30);
         this.btnCreateLevelStart.Name = "btnCreateLevelStart";
         this.btnCreateLevelStart.Size = new System.Drawing.Size(189, 43);
         this.btnCreateLevelStart.TabIndex = 0;
         this.btnCreateLevelStart.Text = "Start Creating Level";
         this.btnCreateLevelStart.UseVisualStyleBackColor = true;
         this.btnCreateLevelStart.Click += new System.EventHandler(this.btnCreateLevelStart_Click);
         // 
         // selectChBoxPassableEdges
         // 
         this.selectChBoxPassableEdges.AutoSize = true;
         this.selectChBoxPassableEdges.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
         this.selectChBoxPassableEdges.Location = new System.Drawing.Point(86, 156);
         this.selectChBoxPassableEdges.Name = "selectChBoxPassableEdges";
         this.selectChBoxPassableEdges.Size = new System.Drawing.Size(129, 19);
         this.selectChBoxPassableEdges.TabIndex = 18;
         this.selectChBoxPassableEdges.Text = "Passable Edges";
         this.selectChBoxPassableEdges.UseVisualStyleBackColor = true;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1511, 693);
         this.Controls.Add(this.levelpanel);
         this.Controls.Add(this.createpanelUI);
         this.Controls.Add(this.createpanel);
         this.Controls.Add(this.btSelectIntervalOpen);
         this.Controls.Add(this.lbIntervalOpen);
         this.Controls.Add(this.tbIntervalOpen);
         this.Controls.Add(this.lbScore);
         this.Controls.Add(this.gamepanel);
         this.Controls.Add(this.menuStrip1);
         this.DoubleBuffered = true;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MainMenuStrip = this.menuStrip1;
         this.MaximizeBox = false;
         this.Name = "Form1";
         this.Text = "SnakeX";
         this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.levelpanel.ResumeLayout(false);
         this.levelpanel.PerformLayout();
         this.blockPanel.ResumeLayout(false);
         this.blockPanel.PerformLayout();
         this.createpanelUI.ResumeLayout(false);
         this.createpanelUI.PerformLayout();
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
      private System.Windows.Forms.Panel levelpanel;
      private System.Windows.Forms.Panel createpanel;
      private System.Windows.Forms.ComboBox cmbSelectedLevel;
      private System.Windows.Forms.Button btStartLevel;
      private System.Windows.Forms.Label lbSelectlevel;
      private System.Windows.Forms.Button btnChangeDetail;
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
      private System.Windows.Forms.TextBox tbCFoodnumber;
      private System.Windows.Forms.Label lbCFoodNumber;
      private System.Windows.Forms.Panel createpanelUI;
      private System.Windows.Forms.Button btnCreateLevelStart;
      private System.Windows.Forms.Label lbGameSize;
      private System.Windows.Forms.Button btnEditLevel;
      private System.Windows.Forms.CheckBox checkBoxPassableEdges;
      private System.Windows.Forms.CheckBox selectChBoxPassableEdges;
   }
}

