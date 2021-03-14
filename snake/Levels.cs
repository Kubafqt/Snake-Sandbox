using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace snakezz
{
   class Levels
   {
      /// <summary>
      /// Add current new level to database.
      /// </summary>
      /// <param name="levelName">Input level name ID.</param>
      /// <param name="foodnumber">Food number to level (default called - 1).</param>
      /// <param name="proceedAddToCombobox">Bool to check if everything proceed fine and add new level to combobox.</param>
      public static bool AddLevel(string levelName, int foodnumber)
      {
         //try
         //{
            //test if level under this name already exist:
            if (TestLevelExist(levelName)) //check if save game name already exist, then ask if overwrite level or not
            {
               DialogResult dialogResult = MessageBox.Show("Level s tímto názvem již existuje. Chcete přepsat level?", "Přepsat level?", MessageBoxButtons.YesNo);
               if (dialogResult == DialogResult.Yes)
               {
                  DeleteLevel(levelName, true); //delete saved level with this name
               }
               else if (dialogResult == DialogResult.No) //dont delete saved level with this name
               {
                  return false;                 
               }
            }           
            //save level info:
            SqlConnection connection = new SqlConnection(game.connString);
            int passableEdges = game.passableEdges ? 1 : 0;
            string cmdText = $"INSERT INTO levels_info " + "(levelNameID, foodNumber)" + $"VALUES (@levelNameID, @foodNumber)";
            SqlCommand command = new SqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@levelNameID", levelName);
            command.Parameters.AddWithValue("@foodNumber", foodnumber);
            connection.Open();
            command.ExecuteNonQuery();
            //save level blocks:
            foreach (Point point in Form1.blockPointList.ToList()) //bude to takto přepisovaný fungovat? - mělo byt (test)
            {
               cmdText = $"INSERT INTO levels_blocks levelNameID, blockPosX, blockPosY) VALUES (@levelNameID, {point.X}, {point.Y})";
               command = new SqlCommand(cmdText, connection);
               command.Parameters.AddWithValue("@levelNameID", levelName);
               command.ExecuteNonQuery();
            }
            //save level snakes:
            //foreach (snakes snake in snakes.Snakes.ToList()) //later add snakes to database
            //{
            //}
            connection.Close();
            return true;
         //}
         //catch (Exception e)
         //{
         //   return false;
         //   MessageBox.Show($"V metodě AddLevel se vyskytla chyba - {e.GetType()}");
         //}
      }

      /// <summary>
      /// Load level from database. (all level tables)
      /// </summary>
      /// <param name="levelName">Input level name ID.</param>
      /// <returns>True: Level exist and everything proceed fine. False: Level is not exist in database or something is not proceed fine.</returns>
      public static bool LoadLevel(string levelName)
      {
         //try
         //{
            if (TestLevelExist(levelName)) //test level exist in database (by name)
            {
               SqlConnection connection = new SqlConnection(game.connString);
               //load level information:
               string cmdtext = "SELECT * FROM level_info WHERE levelNameID = @levelName";
               SqlCommand cmd = new SqlCommand(cmdtext, connection);
               cmd.Parameters.AddWithValue("@levelName", levelName);
               connection.Open(); //open SQL server connection
               SqlDataReader reader = cmd.ExecuteReader();
               while (reader.Read()) //load level info
               {
                  game.interval = Convert.IsDBNull((int)reader["interval"]) ? game.interval : (int)reader["interval"];
                  game.foodNumber = (int)reader["foodNumber"];
                  game.passableEdges = (bool)reader["passableEdges"];
               }
               //load level blocks:
               cmdtext = "SELECT * FROM level_blocks WHERE levelNameID = @levelName";
               cmd = new SqlCommand(cmdtext, connection);
               reader = cmd.ExecuteReader();
               while (reader.Read())
               {
                  Point blockPoint = new Point((int)reader["blockPosX"], (int)reader["blockPosY"]);
                  Form1.blockPointList.Add(blockPoint);
                  Form1.blockArr[blockPoint.X, blockPoint.Y] = "hardblock"; //everythings is hardblock now
               }
               //load level snakes: (později)
               //cmdtext = "SELECT * FROM level_snakes WHERE levelNameID = @levelName";
               //cmd = new SqlCommand(cmdtext, connection);
               //reader = cmd.ExecuteReader();
               //while (reader.Read())
               //{

               //}
               connection.Close();
               return true;
            }
            return false;
         //}
         //catch (Exception e)
         //{
         //   MessageBox.Show($"V metodě LoadLevel se vyskytla chyba - {e.GetType()}");
         //   return false;
         //}
      }

      /// <summary>
      /// Delete level from database.
      /// </summary>
      /// <param name="levelName">level name ID to delete</param>
      /// <param name="alreadyExist">level is already tested to exist in database</param>
      public static void DeleteLevel(string levelName, bool alreadyCheckedExist = false)
      {
         //try
         //{

         if (!alreadyCheckedExist && !TestLevelExist(levelName)) //test level exist in db, when it is not already checked
         {
            MessageBox.Show("Tento level je defaultní a v databázi custom levelů neexistuje!");
            return;
         }
         //delete level from tables:
         SqlConnection connection = new SqlConnection(game.connString);
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
         //}
         //catch (Exception e)
         //{
         //   MessageBox.Show($"V metodě DeleteSave se vyskytla chyba! - {e.GetType()}");
         //}
      }

      /// <summary>
      /// Test if level exist in database.
      /// </summary>
      /// <param name="levelName">input level name ID</param>
      /// <returns> True: level exist in database. False: level is not exist in database.</returns>
      private static bool TestLevelExist(string levelName)
      {
         //try
         //{
            SqlConnection connection = new SqlConnection(game.connString);
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
         //}
         //catch (Exception e)
         //{
         //   MessageBox.Show($"V metodě TestLevelExist se vyskytla chyba - {e.GetType()}");
         //   return false;
         //}
      }

   }
}
