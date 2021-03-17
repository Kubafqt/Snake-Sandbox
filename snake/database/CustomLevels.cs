using System;
using System.Linq;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace snake_sandbox01
{
   class CustomLevels
   {
      private static Random random = new Random();

      /// <summary>
      /// Add created level to database.
      /// </summary>
      /// <param name="levelName">Input level name ID.</param>
      /// <param name="foodnumber">Food number to level (default called - 1).</param>
      /// <param name="proceedAddToCombobox">Bool to check if everything proceed fine and add new level to combobox.</param>
      public static bool AddLevel(string levelName, int foodnumber, bool passableEdges)
      {
         try
         {
            //test if level under this name already exist:
            if (TestLevelExist(levelName)) //check if save game name already exist, then ask if overwrite level or not
            {
               DialogResult dialogResult = MessageBox.Show("Level s tímto názvem již existuje. Chcete přepsat level?", "Přepsat level?", MessageBoxButtons.YesNo); //+delete from combobox
               if (dialogResult == DialogResult.Yes)
               {
                  DeleteLevel(levelName, true); //delete saved level with this name
                  //Form1.levelComboboxItemsToDelete.Push(levelName); //stack for delete combobox item (call in changepanel)
               }
               else if (dialogResult == DialogResult.No) //dont delete saved level with this name
               {
                  return false;
               }
            }

            //save level info:
            SqlConnection connection = new SqlConnection(Game.connString);
            int passableEdge = passableEdges ? 1 : 0;
            string cmdText = $"INSERT INTO level_info " + "(levelNameID, foodNumber, passableEdges)" + $"VALUES (@levelNameID, @foodNumber, {passableEdge})";
            SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@levelNameID", levelName);
            command.Parameters.AddWithValue("@foodNumber", foodnumber);
            connection.Open();
            command.ExecuteNonQuery();

            //save level blocks:
            foreach (Point point in Form1.blockPointList.ToList()) //bude to takto přepisovaný fungovat? - mělo byt (test)
            {
               cmdText = $"INSERT INTO level_blocks (levelNameID, blockPosX, blockPosY) VALUES (@levelNameID, {point.X}, {point.Y})";
               command = new SqlCommand(cmdText, connection);
               command.Parameters.AddWithValue("@levelNameID", levelName);
               command.ExecuteNonQuery();
            }
            //save level snakes:
            foreach (Snakes snake in Snakes.snakesList.ToList())
            {
               cmdText = $"INSERT INTO level_snakes (levelNameID, snakeID, startSnakeLength, posX, posY) VALUES (@levelNameID, {Game.snakeID}, {snake.startSnakeLength}, {snake.startX}, {snake.startY})";
               command = new SqlCommand(cmdText, connection);
               command.Parameters.AddWithValue("@levelNameID", levelName);
               command.ExecuteNonQuery();
               Game.snakeID++;
            }
            connection.Close();
            return true;
         }
         catch (Exception e)
         {
            MessageBox.Show($"CustomLevels.AddLevel method exception - {e.GetType()}");
            return false;
         }
      }

      /// <summary>
      /// Load level from database.
      /// </summary>
      /// <param name="levelName">Input level name ID.</param>
      /// <returns>True: Level exist and everything proceed fine. False: Level is not exist in database or something is not proceed fine.</returns>
      public static bool LoadLevel(string levelName, bool alreadyTestedExist = false)
      {
         try
         {
            if (alreadyTestedExist || TestLevelExist(levelName)) //test level exist in database (by name)
            {
               SqlConnection connection = new SqlConnection(Game.connString);
               //load level information:
               string cmdText = "SELECT * FROM level_info WHERE levelNameID = @levelName";
               SqlCommand command = new SqlCommand(cmdText, connection);
               command.Parameters.AddWithValue("@levelName", levelName);
               connection.Open(); //open SQL server connection
               SqlDataReader reader = command.ExecuteReader();
               while (reader.Read()) //load level info
               {
                  //game.interval = Convert.IsDBNull((bool)reader["interval"]) ? game.interval : (int)reader["interval"]; later
                  Game.foodNumber = (int)reader["foodNumber"];
                  Game.passableEdges = (bool)reader["passableEdges"];
               }
               connection.Close();

               //load level blocks:              
               cmdText = "SELECT * FROM level_blocks WHERE levelNameID = @levelName";
               command = new SqlCommand(cmdText, connection);
               command.Parameters.AddWithValue("@levelName", levelName);
               connection.Open();
               reader = command.ExecuteReader();
               while (reader.Read())
               {
                  Point blockPoint = new Point((int)reader["blockPosX"], (int)reader["blockPosY"]);
                  Form1.blockPointList.Add(blockPoint);
                  Form1.blockArr[blockPoint.X, blockPoint.Y] = "hardblock"; //everythings is hardblock now
               }
               connection.Close();

               //load level snakes:

               cmdText = "SELECT * FROM level_snakes WHERE levelNameID = @levelName";
               command = new SqlCommand(cmdText, connection);
               command.Parameters.AddWithValue("@levelName", levelName);
               connection.Open();
               reader = command.ExecuteReader();
               while (reader.Read())
               {
                  //random snake color for now - later better choose to save
                  Snakes.snakesList.Add(new Snakes((int)reader["posX"], (int)reader["posY"], (int)reader["startSnakeLength"], Snakes.snakeColorsList[random.Next(Snakes.snakeColorsList.Count)], Game.snakeID));
               }
               connection.Close();
               return true;
            }
            return false;
         }
         catch (Exception e)
         {
            MessageBox.Show($"CustomLevels.LoadLevel method exception - {e.GetType()}");
            return false;
         }
      }

      /// <summary>
      /// Load only blocks from level - usable when load saved game.
      /// </summary>
      /// <param name="levelName">level name ID</param>
      /// <param name="alreadyTestedExist">true if already tested that level exist in db, default: false</param>
      /// <returns>True: level exist and load completed. False: level is not exist or exception.</returns>
      public static bool LoadLevelBlocks(string levelName, bool alreadyTestedExist = false)
      {
         try
         {
            if (alreadyTestedExist || TestLevelExist(levelName)) //test level exist in database (by name)
            {
               SqlConnection connection = new SqlConnection(Game.connString);
               //load level blocks:              
               string cmdText = "SELECT * FROM level_blocks WHERE levelNameID = @levelName";
               SqlCommand command = new SqlCommand(cmdText, connection);
               command = new SqlCommand(cmdText, connection);
               command.Parameters.AddWithValue("@levelName", levelName);
               connection.Open();
               SqlDataReader reader = command.ExecuteReader();
               while (reader.Read())
               {
                  Point blockPoint = new Point((int)reader["blockPosX"], (int)reader["blockPosY"]);
                  Form1.blockPointList.Add(blockPoint);
                  Form1.blockArr[blockPoint.X, blockPoint.Y] = "hardblock"; //everythings is hardblock now
               }
               connection.Close();
               return true;
            }
            return false;
         }
         catch (Exception e)
         {
            MessageBox.Show($"CustomLevels.LoadLevelBlocks method exception - {e.GetType()}");
            return false;
         }
      }

      /// <summary>
      /// Delete level from database.
      /// </summary>
      /// <param name="levelName">level name ID to delete</param>
      /// <param name="alreadyExist">level is already tested to exist in database</param>
      public static bool DeleteLevel(string levelName, bool alreadyCheckedExist = false)
      {
         try
         {
            if (!alreadyCheckedExist && !TestLevelExist(levelName)) //test level exist in db, when it is not already checked
            {
               MessageBox.Show("Tento level je defaultní a v databázi custom levelů neexistuje!");
               return false;
            }
            //delete level from tables:
            SqlConnection connection = new SqlConnection(Game.connString);
            string[] tableNames = new string[] { "level_info", "level_blocks", "level_snakes" };
            foreach (string tableName in tableNames) //delete all level table records by levelNameID
            {
               string cmdText = $"DELETE FROM {tableName} WHERE levelNameID = @deleteName";
               SqlCommand cmd = new SqlCommand(cmdText, connection);
               cmd.Parameters.AddWithValue("@deleteName", levelName);
               connection.Open();
               cmd.ExecuteNonQuery();
               connection.Close();
            }
            return true;
         }
         catch (Exception e)
         {
            MessageBox.Show($"CustomLevels.DeleteSave method exception - {e.GetType()}");
            return false;
         }
      }

      /// <summary>
      /// Test if level exist in database.
      /// </summary>
      /// <param name="levelName">input level name ID</param>
      /// <returns> True: level exist in database. False: level is not exist in database.</returns>
      public static bool TestLevelExist(string levelName)
      {
         try
         {
            SqlConnection connection = new SqlConnection(Game.connString);
            string cmdText = $"SELECT levelNameID FROM level_info WHERE levelNameID = @levelName";
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            cmd.Parameters.AddWithValue("@levelName", levelName);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) //check if saveGameNameID already exists
            {
               connection.Close();
               return true;
            }
            connection.Close();
            return false;
         }
         catch (Exception e)
         {
            MessageBox.Show($"CustomLevels.TestLevelExist method exception - {e.GetType()}");
            return false;
         }
      }

   }
}