using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace snakezz
{
   class Levels
   {
      /// <summary>
      /// Add current new level to database.
      /// </summary>
      public static void AddLevel(string levelName)
      {
         try
         {
            SqlConnection connection = new SqlConnection(game.connString);
            string testLevelExistCmdText = $"SELECT levelNameID FROM levels_info WHERE levelNameID = @levelName";
            SqlCommand cmd = new SqlCommand(testLevelExistCmdText, connection);
            cmd.Parameters.AddWithValue("@levelName", levelName);
            connection.Open();
            bool levelExist = false;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) //check if saveGameNameID already exists
            {
               levelExist = true; //save on this name exists
            }
            connection.Close();
            if (levelExist) //check if save game name already exist, then ask if overwrite level or not
            {
               //proceed = false;
               DialogResult dialogResult = MessageBox.Show("Level s tímto názvem již existuje. Chcete přepsat level?", "Přepsat level?", MessageBoxButtons.YesNo);
               if (dialogResult == DialogResult.Yes)
               {
                  DeleteLevel(levelName, true); //first delete last save game
               }
               else if (dialogResult == DialogResult.No)
               {
                  //proceed = false;
                  return;
               }
            }


         }
         catch (Exception e)
         {
            MessageBox.Show($"V metodě AddLevel se vyskytla chyba - {e.GetType()}");
         }
      }

      /// <summary>
      /// Delete selected level from database.
      /// </summary>
      /// <param name="levelName">level name ID to delete</param>
      /// <param name="alreadyExist">level is already tested to exist in database</param>
      public static void DeleteLevel(string levelName, bool alreadyExist = false)
      {
         //try
         //{
            SqlConnection connection = new SqlConnection(game.connString);
            if (!alreadyExist)
            {
               string testLevelExistCmdText = $"SELECT levelNameID FROM levels_info WHERE levelNameID = @levelName";
               SqlCommand cmd = new SqlCommand(testLevelExistCmdText, connection); //check if saveGameNameID already exists
               cmd.Parameters.AddWithValue("@levelName", levelName);
               connection.Open();
               bool levelExist = false;
               SqlDataReader reader = cmd.ExecuteReader();
               while (reader.Read())
               {
                  levelExist = true; //save on this name exists
               }
               connection.Close();
               if (!levelExist)
               {
                  MessageBox.Show("Tento level bude defaultní, v databázi custom levelů neexistuje!");
                  return;
               }
            }
            string[] tableNames = new string[] { "levels_info", "level_sblocks", "levels_snakes" };
            foreach (string tableName in tableNames) //delete all levels by levelNameID
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
   }
}
