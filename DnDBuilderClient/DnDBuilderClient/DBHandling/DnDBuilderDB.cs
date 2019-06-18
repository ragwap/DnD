using System;
using System.IO;
using Mono.Data.Sqlite;

namespace DnDBuilderClient.DBHandling
{
    public class DnDBuilderDB
    {
        /*public SqliteConnection con;

        public DnDBuilderDB()
        {
            try
            {
                if (!File.Exists("DnDBuilderDB.sqlite"))
                {
                    SqliteConnection.CreateFile("DnDBuilderDB.sqlite");
                }

                using(con = new SqliteConnection("Data Source=DnDBuilderDB.sqlite;Version=3;"))
                {
                    con.Open();
                    string tblQry = "CREATE TABLE IF NOT EXISTS DnDCharacter(Name VARCHAR(100) PRIMARY KEY, Age INTEGER, Gender VARCHAR(100), Bio VARCHAR(500), Level INTEGER, Race VARCHAR(100), Class VARCHAR(100), Spellcaster VARCHAR(10), Hit_Points INTEGER, Ability_Score INTEGER)";
                    SqliteCommand command = new SqliteCommand(tblQry, con);
                    command.ExecuteNonQuery();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }*/

        /*public string Insert(string name, int age, string gender, string bio, int level, string race, string cClass, string spellcaster, int hitPoints, int abilityScore)
        {

        }*/

        /*public string Delete(string name)
        {
            string msg;

            try
            {
                using(con = new SqliteConnection("Data Source=DnDBuilderDB.sqlite;Version=3;"))
                {
                    con.Open();
                    SqliteCommand chkNameExist = new SqliteCommand("SELECT count(*) FROM DnDCharacter WHERE Name='" + name + "'", con);

                    int count = Convert.ToInt32(chkNameExist.ExecuteScalar());

                    if (count != 0)
                    {
                        SqliteCommand deleteCharacter = new SqliteCommand("DELETE FROM DnDCharacter WHERE Name = '" + name + "'", con);
                        deleteCharacter.ExecuteNonQuery();
                        msg = "Successfully deleted " + name;
                    }
                    else
                    {
                        msg = "No such record exists";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }*/

        /*public string Update(string name, int age, string gender, string bio, int level, string race, string cClass, string spellcaster, int hitPoints, int abilityScore)
        {
            string msg;

            try
            {
                using (con = new SqliteConnection("Data Source=DnDBuilderDB.sqlite;Version=3;"))
                {
                    con.Open();
                    SqliteCommand chkNameExist = new SqliteCommand("SELECT count(*) FROM DnDCharacter WHERE Name='" + name + "'", con);

                    int count = Convert.ToInt32(chkNameExist.ExecuteScalar());

                    if (count != 0)
                    {
                        SqliteCommand updateCharacter = new SqliteCommand(("UPDATE DnDCharacter SET Age = " + age + ", Gender = '" + gender + "', Bio = '" + bio + "', Level = " + level + ", Race = '" + race + "', Class = '" + cClass + "', Spellcaster = '" + spellcaster + "', Hit_Points = " + hitPoints + ", Ability_Score = " + abilityScore + " WHERE Name = '" + name + "'"), con);
                        updateCharacter.ExecuteNonQuery();
                        msg = "Successfully updated " + name;
                    }
                    else
                    {
                        Console.WriteLine("Name already exists");
                        msg = "Name doesn't exist to update";
                    }
                    con.Close();
                }
            }

            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }*/

        /*public Dictionary<> ViewAll()
        {

        }*/
    }
}
