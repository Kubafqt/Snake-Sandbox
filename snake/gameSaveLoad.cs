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
         //check if name already exist, then ask if overwrite level or not
         //save all game properties and add it to combobox

         //try
         //{

         SqlConnection connection = new SqlConnection(game.connString);
         //test:
         //string cmdText = $"INSERT INTO savegame_info (saveGameNameID, foodNumber, passableEdges) VALUES (@saveName, {25}, {0})";
         //SqlCommand cmd = new SqlCommand(cmdText, connection);
         //cmd.Parameters.AddWithValue("@saveName", saveName);
         //check if level exists first:
         string testSaveExistCmd = $"SELECT saveGameNameID FROM savegame_info WHERE saveGameNameID = @saveName";
         SqlCommand commd = new SqlCommand(testSaveExistCmd, connection);
         commd.Parameters.AddWithValue("@saveName", saveName);
         connection.Open();
         bool saveExists = false;
         SqlDataReader reader = commd.ExecuteReader();
         while(reader.Read())
         {
            saveExists = true;
         }
         connection.Close();
         if (saveExists)
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
         //save all snakes:
         foreach (snakes snake in snakes.Snakes.ToList())
         {
            //string commandText = !saveExists ? 
            //   "INSERT INTO savegame_snakes " +
            //   "(saveGameNameID, snakeID, snakeLenght, posX, posY, direction, insideSnake, superSnake)" + 
            //   $"VALUES (@saveName, {snake.snakeNumber}, {snake.snakeLength}, {snake.x}, {snake.y}, {snake.direction}, {snake.superSnake}, {snake.insideSnake})"
            //   : 
            //   "UPDATE savegame_snakes " +
            //   $"SET saveGameNameID = @saveName, snakeID = {snake.snakeNumber}, snakeLenght = {snake.snakeLength}, posX = {snake.x}, posY = {snake.y}, direction = {snake.direction}, insideSnake = {snake.superSnake}, superSnake = {snake.insideSnake})" +
            //   "WHERE saveGameNameID = @saveName";
            int superSnake = snake.superSnake ? 1 : 0;
            int insideSnake = snake.insideSnake ? 1 : 0;
            int isPlayerSnake = snake == snakes.PlayerSnake ? 1 : 0;
            string commandText = "INSERT INTO savegame_snakes " + "(saveGameNameID, snakeID, snakeLenght, posX, posY, insideSnake, superSnake, snakeColorID, playerSnake)" +
               $" VALUES (@saveName, {snake.snakeNumber}, {snake.snakeLength}, {snake.x}, {snake.y}, {superSnake}, {insideSnake}, {snakes.snakeColorsList.IndexOf(snake.color)}, {isPlayerSnake})";
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
               queuePos++; //otázka jestli to jde odzadu nebo odpředu
            }
         }
         //save all blocks (for now, then level only (maybe)):
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
            //connection.Open();
            command.ExecuteNonQuery();
            //connection.Close();
         }
         connection.Close();
         proceed = true;
         //}
         //catch (Exception e)
         //{
         //   addToComboBox = false;
         //   MessageBox.Show($"Vyskytla se chyba při zápisu do DB: {e.GetType()}");
         //}
      }

      #endregion

      #region LoadGame methods
      /// <summary>
      /// Load selected game from combobox and database.
      /// </summary>
      /// <param name="loadName">SaveGameNameID - ID name of saved game.</param>
      public static void LoadGame(string loadName)
      {         
         SqlConnection connection = new SqlConnection(game.connString);
         //test:
         //string cmdtext = "SELECT saveGameNameID FROM savegame_info";
         //SqlCommand cmd = new SqlCommand(cmdtext, connection);
         //connection.Open();
         //SqlDataReader reader = cmd.ExecuteReader();
         //while (reader.Read())
         //{
         //   MessageBox.Show((string)reader["saveGameNameID"]);
         //}
         //connection.Close();
         game.resetGame();
         LoadSaveGame_info(loadName, connection);
         LoadSaveGame_snakes(loadName, connection);
         LoadSaveGame_snakesPointQueue(loadName, connection);
         LoadSaveGame_blocks(loadName, connection);
         
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="loadName"></param>
      /// <param name="connection"></param>
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
            //game.passableEdges = (int)reader["passableEdges"] == 1 ? true : false;
            game.interval = (int)reader["interval"];
         }
         game.passableEdges = true;
         connection.Close();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="loadName"></param>
      /// <param name="connection"></param>
      private static void LoadSaveGame_snakes(string loadName, SqlConnection connection)
      {
         connection.Open();
         string cmdtext = "SELECT * FROM savegame_snakes WHERE saveGameNameID = @loadName";
         SqlCommand cmd = new SqlCommand(cmdtext, connection);
         cmd.Parameters.AddWithValue("@loadName", loadName);
         SqlDataReader reader = cmd.ExecuteReader();
         while (reader.Read()) //nahraje tam všechny hady (kde je saveGameID == loadName a jede všechny snakeID)
         {     
            //snake.direction = (string)reader["direction"];
            if ((bool)reader["playerSnake"] == true) //playerSnake
            {
               snakes.PlayerSnake = new snakes((int)reader["posX"], (int)reader["posY"], (int)reader["snakeLenght"], snakes.snakeColorsList[(int)reader["snakeColorID"]]);
               snakes.Snakes.Add(snakes.PlayerSnake);
            }
            else //botSnakes
            {
               snakes snake = new snakes((int)reader["posX"], (int)reader["posY"], (int)reader["snakeLenght"], snakes.snakeColorsList[(int)reader["snakeColorID"]], (int)reader["snakeID"]);
               snake.insideSnake = Convert.IsDBNull((bool)reader["insideSnake"]) ? false : (bool)reader["insideSnake"];
               snake.superSnake = Convert.IsDBNull((bool)reader["superSnake"]) ? false : (bool)reader["superSnake"];
               snakes.Snakes.Add(snake);
            }

         }
         connection.Close();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="loadName"></param>
      /// <param name="connection"></param>
      private static void LoadSaveGame_snakesPointQueue(string loadName, SqlConnection connection)
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
      /// 
      /// </summary>
      /// <param name="loadName"></param>
      /// <param name="connection"></param>
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
      /// 
      /// </summary>
      /// <param name="deleteName"></param>
      /// <param name="deleteFromComboBox"></param>
      public static void ToDeleteSave(string deleteName, out bool deleteFromComboBox)
      {
         DialogResult dialogResult = MessageBox.Show("Opravdu chcete vymazat uloženou hru?", "vymazat uloženou hru", MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.No)
         {
            deleteFromComboBox = false;
            return;
         }
         //try
         //{
            deleteFromComboBox = true;
            DeleteSave(deleteName);
         //}
         //catch (Exception e)
         //{
         //   deleteFromComboBox = false;
         //   MessageBox.Show($"Vyskytla se chyba při pokus o vymazání saveGame záznamu z DB: {e.GetType()}");
         //}
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="deleteName"></param>
      public static void DeleteSave(string deleteName)
      {
         SqlConnection connection = new SqlConnection(game.connString);
         string[] tableNames = new string[] { "savegame_info", "savegame_blocks", "savegame_snakes", "savegame_snakesPointQueue" }; 
         //basic: multiple delete statements, advanced: cascading deletes rows from multiple tables (later can be)
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
