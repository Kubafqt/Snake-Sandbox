using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace snakezz
{
   class gameSaveLoad
   {
      #region SaveGame methods
      /// <summary>
      /// 
      /// </summary>
      /// <param name="saveName"></param>
      /// <param name="proceed"></param>
      public static void SaveGame(string saveName, out bool proceed)
      {   
         try
         {
            SqlConnection connection = new SqlConnection(game.connString);
            string testSaveExistCmd = $"SELECT saveGameNameID FROM savegame_info WHERE saveGameNameID = @saveName";
            SqlCommand commd = new SqlCommand(testSaveExistCmd, connection); //check if saveGameNameID already exists
            commd.Parameters.AddWithValue("@saveName", saveName);
            connection.Open();
            bool saveExists = false;
            SqlDataReader reader = commd.ExecuteReader();
            while (reader.Read())
            {
               saveExists = true; //save on this name exists
            }
            connection.Close();
            if (saveExists) //check if save game name already exist, then ask if overwrite level or not
            {
               proceed = false;
               DialogResult dialogResult = MessageBox.Show("Uložená hra pod tímto názvem již existuje. Chcete přepsat uloženou hru?", "Přepsat uloženou hru?", MessageBoxButtons.YesNo);
               if (dialogResult == DialogResult.Yes)
               {
                  DeleteSave(saveName); //first delete last save game
               }
               else if (dialogResult == DialogResult.No)
               {
                  proceed = false;
                  return;
               }
            }
            //save game info:
            int passableEdges = game.passableEdges ? 1 : 0;
            string cmdText = $"INSERT INTO savegame_info " + "(saveGameNameID, levelNameID, foodNumber, passableEdges, interval)" + $"VALUES (@saveName, NULL, {game.foodNumber}, {passableEdges}, {game.interval})";
            SqlCommand comm = new SqlCommand(cmdText, connection);
            comm.Parameters.AddWithValue("@saveName", saveName);
            connection.Open();
            comm.ExecuteNonQuery();
            //save all snakes properties:
            foreach (snakes snake in snakes.Snakes.ToList())
            {
               int superSnake = snake.superSnake ? 1 : 0;
               int insideSnake = snake.insideSnake ? 1 : 0;
               int isPlayerSnake = snake == snakes.PlayerSnake ? 1 : 0;
               string commandText = "INSERT INTO savegame_snakes " + "(saveGameNameID, snakeID, snakeLenght, posX, posY, direction, insideSnake, superSnake, snakeColorID, playerSnake)" +
                  $" VALUES (@saveName, {snake.snakeNumber}, {snake.snakeLength}, {snake.x}, {snake.y}, {snake.direction}, {superSnake}, {insideSnake}, {snakes.snakeColorsList.IndexOf(snake.color)}, {isPlayerSnake})";
               SqlCommand command = new SqlCommand(commandText, connection);
               command.Parameters.AddWithValue("@saveName", saveName);
               command.ExecuteNonQuery();
               int queuePos = 0;
               //save snakePointQueue inside of snake instance:
               foreach (Point p in snake.snakePointQueue.ToList())
               {
                  string queueCmdText = $"INSERT INTO savegame_snakesPointQueue" + "(saveGameNameID, snakeID, posX, posY, queuePos)" +
                     $" VALUES (@saveName, {snake.snakeNumber}, {p.X}, {p.Y}, {queuePos})";
                  SqlCommand cmd = new SqlCommand(queueCmdText, connection);
                  cmd.Parameters.AddWithValue("@saveName", saveName);
                  cmd.ExecuteNonQuery();
                  queuePos++; //otázka jestli to jde odzadu nebo odpředu (!)
               }
            }
            //save all blocks (for now, then can be level only):
            foreach (Point p in Form1.blockPoint.ToList())
            {
               string commandText = $"INSERT INTO savegame_blocks " + "(saveGameNameID, blockPosX, blockPosY, blockType)" +
                  $" VALUES (@saveName, {p.X}, {p.Y}, 'hardblock')";
               SqlCommand command = new SqlCommand(commandText, connection);
               command.Parameters.AddWithValue("@saveName", saveName);
               command.ExecuteNonQuery();
            }
            //save all food blocks:
            foreach (Point p in Form1.foodPoint.ToList())
            {
               string commandText = $"INSERT INTO savegame_blocks " + "(saveGameNameID, blockPosX, blockPosY, blockType)" +
                  $" VALUES (@saveName, {p.X}, {p.Y}, 'food')";
               SqlCommand command = new SqlCommand(commandText, connection);
               command.Parameters.AddWithValue("@saveName", saveName);
               command.ExecuteNonQuery();
            }
            connection.Close();
            proceed = true; //for add it to combobox and other little things
         }
         catch (Exception e)
         {
            proceed = false;
            MessageBox.Show($"Vyskytla se chyba při zápisu do DB: {e.GetType()}");
         }
      }

      #endregion

      #region LoadGame methods
      /// <summary>
      /// Load selected saved game in combobox from database.
      /// </summary>
      /// <param name="loadName">saveGameNameID (name ID of saved game)</param>
      public static void LoadGame(string loadName)
      {
         try
         {
            SqlConnection connection = new SqlConnection(game.connString);
            game.Resetgame();
            LoadSaveGame_info(loadName, connection);
            LoadSaveGame_snakes(loadName, connection);
            LoadSaveGame_snakePointQueue(loadName, connection);
            LoadSaveGame_blocks(loadName, connection);
         }
         catch (Exception e)
         {
            MessageBox.Show($"Vyskytla se chyba při pokus o nahrání saveGame záznamu z DB: {e.GetType()}");
         }
      }

      /// <summary>
      /// Load saved game properties from database. (savegame_info table)
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
            game.foodNumber = (int)reader["foodNumber"];
            game.passableEdges = (bool)reader["passableEdges"];
            game.interval = (int)reader["interval"];
         }
         game.passableEdges = true;
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
            if ((bool)reader["playerSnake"] == true) //playerSnake
            {
               snakes.PlayerSnake = new snakes((int)reader["posX"], (int)reader["posY"], (int)reader["snakeLenght"], snakes.snakeColorsList[(int)reader["snakeColorID"]]);
               snakes.PlayerSnake.direction = (string)reader["direction"];
               snakes.Snakes.Add(snakes.PlayerSnake);
            }
            else //botSnakes
            {
               snakes snake = new snakes((int)reader["posX"], (int)reader["posY"], (int)reader["snakeLenght"], snakes.snakeColorsList[(int)reader["snakeColorID"]], (int)reader["snakeID"]);
               snake.direction = (string)reader["direction"];
               snake.insideSnake = Convert.IsDBNull((bool)reader["insideSnake"]) ? false : (bool)reader["insideSnake"];
               snake.superSnake = Convert.IsDBNull((bool)reader["superSnake"]) ? false : (bool)reader["superSnake"];
               snakes.Snakes.Add(snake);
            }
         }
         connection.Close();
      }

      /// <summary>
      /// Load saved snakePointQueues from database. (savegame_snakesPointQueue table)
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
            snakes.Snakes[(int)reader["snakeID"] - 1].snakePointQueue.Enqueue(new Point((int)reader["posX"], (int)reader["posY"])); //každopádně by to mělo fungovat správně - test it!
         }
         connection.Close();
      }

      /// <summary>
      /// Load saved blocks from database. (savegame_blocks table)
      /// </summary>
      /// <param name="loadName">saveGameNameID (name ID of saved game)</param>
      /// <param name="connection">MSSQL server connection</param>
      private static void LoadSaveGame_blocks(string loadName, SqlConnection connection)
      {
         connection.Open();
         string cmdtext = "SELECT * FROM savegame_blocks WHERE saveGameNameID = @loadName";
         SqlCommand cmd = new SqlCommand(cmdtext, connection);
         cmd.Parameters.AddWithValue("@loadName", loadName);
         SqlDataReader reader = cmd.ExecuteReader();
         while (reader.Read())
         {
            int posX = (int)reader["blockPosX"];
            int posY = (int)reader["blockPosY"];
            string type = (string)reader["blockType"];
            Form1.blockArr[posX, posY] = type;
            if (type == "food")
            {
               Form1.foodPoint.Add(new Point(posX, posY));
            }
            else if (type == "hardblock")
            {
               Form1.blockPoint.Add(new Point(posX, posY));
            }
         }
         connection.Close();
      }

      #endregion

      #region DeleteSave methods
      /// <summary>
      /// First ask if user want really delete selected saved game, then delete selected saved game.
      /// </summary>
      /// <param name="deleteName">saveGameNameID to delete</param>
      /// <param name="deleteFromCombo">delete record form combobox after proceed fine</param>
      public static void ToDeleteSave(string deleteName, out bool deleteFromCombo)
      {
         DialogResult dialogResult = MessageBox.Show("Opravdu chcete vymazat uloženou hru?", "vymazat uloženou hru", MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.No)
         {
            deleteFromCombo = false;
            return;
         }
         try
         {
            deleteFromCombo = true;
            DeleteSave(deleteName);
         }
         catch (Exception e)
         {
            deleteFromCombo = false;
            MessageBox.Show($"Vyskytla se chyba při pokus o vymazání saveGame záznamu z DB: {e.GetType()}");
         }
      }

      /// <summary>
      /// Delete selected saved game.
      /// </summary>
      /// <param name="deleteName">saveGameNameID to delete</param>
      public static void DeleteSave(string deleteName)
      {
         SqlConnection connection = new SqlConnection(game.connString);
         string[] tableNames = new string[] { "savegame_info", "savegame_blocks", "savegame_snakes", "savegame_snakesPointQueue" }; 
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
      }

      #endregion

   }
}
