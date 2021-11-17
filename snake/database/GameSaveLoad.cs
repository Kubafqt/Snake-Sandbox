using System;
using System.Linq;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace snake_sandbox01
{
   class GameSaveLoad
   {
      private static Random random = new Random();

      #region SaveGame
      /// <summary>
      /// Save game to database.
      /// </summary>
      /// <param name="saveName">save game name (ID)</param>
      /// <param name="proceed">True: everything proceed fine. False: not everything proceed fine.</param>
      public static void SaveGame(string saveName, out bool proceed)
      {
         try
         {
            //check if saved game already exists in database:
            SqlConnection connection = new SqlConnection(Game.connString);

            if (SaveExists(connection, saveName)) //check if save game name already exist, then ask if overwrite level or not
            {
               proceed = false;
               DialogResult dialogResult = MessageBox.Show("Uložená hra pod tímto názvem již existuje. Chcete přepsat uloženou hru?", "Přepsat uloženou hru?", MessageBoxButtons.YesNo); //+delete from combobox
               if (dialogResult == DialogResult.Yes)
               {
                  DeleteSave(saveName); //first delete last save game
                  //Form1.saveComboboxItemsToDelete.Push(saveName); //stack for delete combobox item (call in changepanel)
               }
               else if (dialogResult == DialogResult.No)
               {
                  proceed = false;
                  return;
               }
            }

            SaveGameInfo(connection, saveName); //save game info
            SaveAllSnakeProperties(connection, saveName); //save all snakes properties
            SaveAllFoodPositions(connection, saveName); //save snakePointQueue inside of snake instance

            proceed = true; //for add it to combobox and other little things
         }
         catch (Exception e)
         {
            proceed = false;
            MessageBox.Show($"Vyskytla se chyba při zápisu do DB: {e.GetType()}");
         }
      }

      /// <summary>
      /// Check if savegame with this name already exists in db.
      /// </summary>
      /// <param name="connection">SQLconnection</param>
      /// <param name="saveName">Save game name (id)</param>
      /// <returns>True: save with this name exists, False: save with this name is not exists</returns>
      private static bool SaveExists(SqlConnection connection, string saveName)
      {
         string testSaveExistSql = $"SELECT saveGameNameID FROM savegame_info WHERE saveGameNameID = @saveName";
         SqlCommand cmd = new SqlCommand(testSaveExistSql, connection);
         cmd.Parameters.AddWithValue("@saveName", saveName);
         connection.Open();
         SqlDataReader reader = cmd.ExecuteReader();
         while (reader.Read())
         {
            connection.Close();
            return true; //save on this name exists
         }
         connection.Close();
         return false;
      }

      /// <summary>
      /// Save game info.
      /// </summary>
      /// <param name="connection">SQLconnection</param>
      /// <param name="saveName">Save game name (id)</param>
      private static void SaveGameInfo(SqlConnection connection, string saveName)
      {
         int passableEdges = Game.passableEdges ? 1 : 0;
         string cmdText = $"INSERT INTO savegame_info " + "(saveGameNameID, levelNameID, foodNumber, passableEdges, interval) " + $"VALUES (@saveName, @levelName, {Game.foodNumber}, {passableEdges}, {Game.interval})";
         SqlCommand cmd = new SqlCommand(cmdText, connection);
         cmd.Parameters.AddWithValue("@saveName", saveName);
         cmd.Parameters.AddWithValue("@levelName", Game.selectedLevelName);
         connection.Open();
         cmd.ExecuteNonQuery();
         connection.Close();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="connection"></param>
      /// <param name="saveName"></param>
      private static void SaveAllSnakeProperties(SqlConnection connection, string saveName)
      {
         connection.Open();
         foreach (Snakes snake in Snakes.snakesList.ToList())
         {
            int superSnake = snake.superSnake ? 1 : 0;
            int insideSnake = snake.insideSnake ? 1 : 0;
            int isPlayerSnake = snake == Snakes.PlayerSnake ? 1 : 0;
            string commandText = "INSERT INTO savegame_snakes " + "(saveGameNameID, snakeID, snakeLength, startSnakeLength, posX, posY, direction, insideSnake, superSnake, snakeColorID, playerSnake)" +
               $" VALUES (@saveName, {snake.snakeNumber}, {snake.snakeLength}, {snake.startSnakeLength}, {snake.x}, {snake.y}, @direction, {superSnake}, {insideSnake}, {Snakes.snakeColorsList.IndexOf(snake.color)}, {isPlayerSnake})";
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@saveName", saveName);
            command.Parameters.AddWithValue("@direction", snake.direction);
            command.ExecuteNonQuery();

            int queuePos = 0;

            foreach (Point p in snake.snakePointQueue.ToList())
            {
               string queueCmdText = $"INSERT INTO savegame_snakesPointQueue" + "(saveGameNameID, snakeID, posX, posY, queuePos)" +
                  $" VALUES (@saveName, {snake.snakeNumber}, {p.X}, {p.Y}, {queuePos})";
               SqlCommand cmd = new SqlCommand(queueCmdText, connection);
               cmd.Parameters.AddWithValue("@saveName", saveName);
               cmd.ExecuteNonQuery();
               queuePos++; //otázka jestli to jde odzadu nebo odpředu (asi ok zatím)
            }
         }
         connection.Close();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="conection"></param>
      /// <param name="saveName"></param>
      private static void SaveAllFoodPositions(SqlConnection connection, string saveName)
      {
         connection.Open();
         //save all food positions:
         foreach (Point p in Form1.foodPointList.ToList())
         {
            string commandText = $"INSERT INTO savegame_foods " + "(saveGameNameID, blockPosX, blockPosY, foodType)" +
               $" VALUES (@saveName, {p.X}, {p.Y}, 'food')";
            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@saveName", saveName);
            command.ExecuteNonQuery();
         }
         connection.Close();
      }


      #endregion

      #region LoadGame
      /// <summary>
      /// Load selected saved game in combobox from database.
      /// </summary>
      /// <param name="loadName">saveGameNameID (name ID of saved game)</param>
      public static bool LoadGame(string loadName)
      {
         try
         {
            SqlConnection connection = new SqlConnection(Game.connString);
            LoadSaveGame_info(loadName, connection);
            LoadSaveGame_foods(loadName, connection);
            LoadSaveGame_snakes(loadName, connection);
            LoadSaveGame_snakePointQueue(loadName, connection);
            return true;
         }
         catch (Exception e)
         {
            MessageBox.Show($"GameSaveLoad.LoadGame - Vyskytla se chyba při pokus o nahrání saveGame záznamu z DB: {e.GetType()}");
            return false;
         }
      }

      /// <summary>
      /// Load saved game info properties from database. (savegame_info table)
      /// </summary>
      /// <param name="loadName">saveGameNameID (name ID of saved game)</param>
      /// <param name="connection">MSSQL server connection</param>
      private static void LoadSaveGame_info(string loadName, SqlConnection connection)
      {
         connection.Open();
         string cmdtext = "SELECT * FROM savegame_info WHERE saveGameNameID = @loadName";
         SqlCommand cmd = new SqlCommand(cmdtext, connection);
         cmd.Parameters.AddWithValue("@loadName", loadName);
         SqlDataReader reader = cmd.ExecuteReader();
         while (reader.Read())
         {
            Game.selectedLevelName = (string)reader["levelNameID"];
            Game.foodNumber = (int)reader["foodNumber"];
            Game.passableEdges = (bool)reader["passableEdges"];
            Game.interval = (int)reader["interval"];
         }
         Game.passableEdges = true;
         connection.Close();
      }

      /// <summary>
      /// Load saved snakes properites from database. (savegame_snakes table)
      /// </summary>
      /// <param name="loadName">saveGameNameID (name ID of saved game)</param>
      /// <param name="connection">MSSQL server connection</param>
      private static void LoadSaveGame_snakes(string loadName, SqlConnection connection)
      {
         connection.Open();
         string cmdtext = "SELECT * FROM savegame_snakes WHERE saveGameNameID = @loadName";
         SqlCommand cmd = new SqlCommand(cmdtext, connection);
         cmd.Parameters.AddWithValue("@loadName", loadName);
         SqlDataReader reader = cmd.ExecuteReader();
         while (reader.Read()) //nahraje tam všechny hady (kde je saveGameID == loadName a jede všechny snakeID)
         {
            int startSnakeLength = Convert.IsDBNull((int)reader["startSnakeLength"]) ? 0 : (int)reader["startSnakeLength"];
            if (!(bool)reader["playerSnake"] == true) //playerSnake
            {
               Snakes snake = new Snakes(random.Next(Form1.width), random.Next(Form1.height), startSnakeLength, Snakes.snakeColorsList[(int)reader["snakeColorID"]], (int)reader["snakeID"]);
               snake.x = (int)reader["posX"];
               snake.y = (int)reader["posY"];
               snake.snakeLength = (int)reader["snakeLength"];
               //snake.direction = (string)reader["direction"];
               snake.insideSnake = Convert.IsDBNull((bool)reader["insideSnake"]) ? false : (bool)reader["insideSnake"];
               snake.superSnake = Convert.IsDBNull((bool)reader["superSnake"]) ? false : (bool)reader["superSnake"];
               Snakes.snakesList.Add(snake);
            }
            else //botSnakes
            {
               Snakes.PlayerSnake = new Snakes(Form1.width / 2, Form1.height / 2, startSnakeLength, Snakes.snakeColorsList[(int)reader["snakeColorID"]]);
               Snakes.PlayerSnake.x = (int)reader["posX"];
               Snakes.PlayerSnake.y = (int)reader["posY"];
               Snakes.PlayerSnake.snakeLength = (int)reader["snakeLength"];
               Snakes.PlayerSnake.direction = (string)reader["direction"];
               Snakes.snakesList.Add(Snakes.PlayerSnake);
            }
         }
         connection.Close();
      }

      /// <summary>
      /// Load saved snakePointQueue from database. (savegame_snakesPointQueue table)
      /// </summary>
      /// <param name="loadName">saveGameNameID (name ID of saved game)</param>
      /// <param name="connection">MSSQL server connection</param>
      private static void LoadSaveGame_snakePointQueue(string loadName, SqlConnection connection)
      {
         connection.Open();
         string cmdtext = "SELECT * FROM savegame_snakesPointQueue WHERE saveGameNameID = @loadName ORDER BY snakeID, queuePos ASC"; //mělo by to být vždy - snake id, all queue pos - idk
         SqlCommand cmd = new SqlCommand(cmdtext, connection);
         cmd.Parameters.AddWithValue("@loadName", loadName);
         SqlDataReader reader = cmd.ExecuteReader();
         while (reader.Read())
         {
            try
            {
               Snakes.snakesList[(int)reader["snakeID"] - 1].snakePointQueue.Enqueue(new Point((int)reader["posX"], (int)reader["posY"])); //každopádně by to mělo fungovat správně
            }
            catch (Exception e) //when is some weird error with out of range index
            {
               MessageBox.Show($"LoadSaveGame_snakePointQueue exception {e.Message}");
               continue;
            }
         }
         connection.Close();
      }

      /// <summary>
      /// Load saved blocks from database. (savegame_blocks table)
      /// </summary>
      /// <param name="loadName">saveGameNameID (name ID of saved game)</param>
      /// <param name="connection">MSSQL server connection</param>
      private static void LoadSaveGame_foods(string loadName, SqlConnection connection) //load level
      {
         connection.Open();
         string cmdtext = "SELECT * FROM savegame_foods WHERE saveGameNameID = @loadName";
         SqlCommand cmd = new SqlCommand(cmdtext, connection);
         cmd.Parameters.AddWithValue("@loadName", loadName);
         SqlDataReader reader = cmd.ExecuteReader();
         while (reader.Read())
         {
            int posX = (int)reader["blockPosX"];
            int posY = (int)reader["blockPosY"];
            string type = (string)reader["foodType"];
            Form1.blockArr[posX, posY] = type;
            Form1.foodPointList.Add(new Point(posX, posY));
         }
         connection.Close();
      }

      #endregion

      #region DeleteSave
      /// <summary>
      /// First ask if user want really delete selected saved game, then delete selected saved game.
      /// </summary>
      /// <param name="deleteName">saveGameNameID to delete</param>
      /// <param name="deleteFromCombo">delete record form combobox after proceed fine</param>
      public static bool ToDeleteSave(string deleteName)
      {
         DialogResult dialogResult = MessageBox.Show("Opravdu chcete vymazat uloženou hru?", "vymazat uloženou hru", MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.No)
         {
            return false;
         }
         return DeleteSave(deleteName);
      }

      /// <summary>
      /// Delete selected saved game from database.
      /// </summary>
      /// <param name="deleteName">saveGameNameID to delete</param>
      public static bool DeleteSave(string deleteName)
      {
         try
         {
            SqlConnection connection = new SqlConnection(Game.connString);
            string[] tableNames = new string[] { "savegame_info", "savegame_foods", "savegame_snakes", "savegame_snakesPointQueue" };
            //basic: multiple delete statements, advanced: cascading deletes rows from multiple tables (can be tested later, not important)
            foreach (string tableName in tableNames)
            {
               string cmdText = $"DELETE FROM {tableName} WHERE saveGameNameID = @deleteName";
               SqlCommand cmd = new SqlCommand(cmdText, connection);
               cmd.Parameters.AddWithValue("@deleteName", deleteName);
               connection.Open();
               cmd.ExecuteNonQuery();
               connection.Close();
            }
            return true;
         }
         catch (Exception e)
         {
            MessageBox.Show($"Vyskytla se chyba při pokus o vymazání saveGame záznamu z DB: {e.GetType()}");
            return false;
         }
      }

      #endregion

   }
}