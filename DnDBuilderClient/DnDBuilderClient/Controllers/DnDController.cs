/*References:
 * https://youtu.be/3RGMuaO--MU
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using Mono.Data.Sqlite;
using Newtonsoft.Json;

namespace DnDBuilderClient.Controllers
{
    public class DnDController : ApiController
    {
        public DnDController()
        {

        }

        [HttpGet]
        [Route("Race/List")]
        public Dictionary<int, string> RaceList()
        {
            string strmRes = null;
            string url = String.Format("http://dnd5eapi.co/api/races");

            WebRequest request = WebRequest.Create(url);

            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader streamReader = new StreamReader(stream);
                strmRes = streamReader.ReadToEnd();
                streamReader.Close();
            }

            var serializer = new JavaScriptSerializer();

            DnDRace dnDRaces = (DnDRace)serializer.Deserialize(strmRes, typeof(DnDRace));

            int i = 0;
            Dictionary<int, string> pairs = new Dictionary<int, string>();

            foreach (Results dRace in dnDRaces.Results)
            {
                string name = dRace.Name;
                ++i;
                pairs.Add(i, name);
            }

            return pairs;

        }

        [HttpGet]
        [Route("Race/List/{index}")]
        public int[] RaceList(int index)
        {
            string url = $"http://dnd5eapi.co/api/races/{index}";
            int[] abilityScores = new int[6];
            string strmRes = null;

            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                strmRes = reader.ReadToEnd();
                reader.Close();
            }

            var serializer = new JavaScriptSerializer();

            DnDRace dnDRace = (DnDRace)serializer.Deserialize(strmRes, typeof(DnDRace));

            for (int i = 0; i < 6; ++i)
            {
                abilityScores[i] = dnDRace.Ability_bonuses[i];
            }

            return abilityScores;

        }

        [HttpGet]
        [Route("Class/List")]
        public Dictionary<int, string> ClassList()
        {
            string strmRes = null;
            string url = String.Format("http://dnd5eapi.co/api/classes");

            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader streamReader = new StreamReader(stream);
                strmRes = streamReader.ReadToEnd();
                streamReader.Close();
            }

            var serializer = new JavaScriptSerializer();

            DnDClass dnDClass = (DnDClass)serializer.Deserialize(strmRes, typeof(DnDClass));

            int i = 0;

            Dictionary<int, string> pairs = new Dictionary<int, string>();

            foreach (Results dClass in dnDClass.Results)
            {
                string name = dClass.Name;
                ++i;
                pairs.Add(i, name);
            }

            return pairs;
        }

        [HttpGet]
        [Route("Class/List/{index}")]
        public Dictionary<string, int> IsSpellcaster(int index)
        {
            string strmRes = null;
            string path = String.Format($"http://dnd5eapi.co/api/classes/{index}");

            WebRequest request = WebRequest.Create(path);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader streamReader = new StreamReader(stream);
                strmRes = streamReader.ReadToEnd();
                streamReader.Close();
            }

            var serializer = new JavaScriptSerializer();

            DnDClass specificClass = (DnDClass)serializer.Deserialize(strmRes, typeof(DnDClass));

            Dictionary<string, int> pairs = new Dictionary<string, int>();


            if (specificClass.SpellCasting == null)
            {
                pairs.Add("no_spell", specificClass.Hit_die);
            }
            else
            {
                pairs.Add("spell", specificClass.Hit_die);
            }

            return pairs;
        }

        public SqliteConnection con;

        [HttpGet]
        [Route("DB/Connection")]
        public void DBConnection()
        {
            try
            {
                if (!File.Exists("DnDBuilderDB.sqlite"))
                {
                    SqliteConnection.CreateFile("DnDBuilderDB.sqlite");
                }

                using (con = new SqliteConnection("Data Source=DnDBuilderDB.sqlite;Version=3;"))
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

        }

        [HttpGet]
        [Route("Insert/{name}/{age}/{gender}/{bio}/{level}/{race}/{cClass}/{spellcaster}/{hitPoints}/{abilityScore}")]
        public string InsertCharacteer(string name, int age, string gender, string bio, int level, string race, string cClass, string spellcaster, int hitPoints, int abilityScore)
        {

            string msg;

            try
            {
                using (con = new SqliteConnection("Data Source=DnDBuilderDB.sqlite;Version=3;"))
                {
                    con.Open();
                    SqliteCommand chkNameExist = new SqliteCommand("SELECT count(*) FROM DnDCharacter WHERE Name='" + name + "'", con);

                    int count = Convert.ToInt32(chkNameExist.ExecuteScalar());

                    if (count == 0)
                    {
                        SqliteCommand insertCharacter = new SqliteCommand("INSERT INTO DnDCharacter VALUES('" + name + "'," + age + ",'" + gender + "','" + bio + "'," + level + ",'" + race + "','" + cClass + "','" + spellcaster + "'," + hitPoints + "," + abilityScore + ")", con);
                        insertCharacter.ExecuteNonQuery();
                        msg = "Successfully created " + name + " character";
                    }
                    else
                    {
                        Console.WriteLine("Name already exist");
                        msg = "Name already exist";
                    }
                    con.Close();
                }
            }

            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        [HttpGet]
        [Route("Delete/{name}")]
        public string DeleteCharacter(string name)
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
        }

        [HttpGet]
        [Route("Update/{name}/{age}/{gender}/{bio}/{level}/{race}/{cClass}/{spellcaster}/{hitPoints}/{abilityScore}")]
        public string UpdateCharacter(string name, int age, string gender, string bio, int level, string race, string cClass, string spellcaster, int hitPoints, int abilityScore)
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
        }

        [HttpGet]
        [Route("All/List")]
        public Dictionary<string, List<string>> ViewAll()
        {
            Dictionary<string, List<string>> allResults = new Dictionary<string, List<string>>();
            List<string> names = new List<string>();
            List<string> age = new List<string>();
            List<string> gender = new List<string>();
            List<string> bio = new List<string>();
            List<string> level = new List<string>();
            List<string> race = new List<string>();
            List<string> cClass = new List<string>();
            List<string> spell = new List<string>();
            List<string> hits = new List<string>();
            List<string> ability = new List<string>();

            try
            {
                using (con = new SqliteConnection("Data Source=DnDBuilderDB.sqlite;Version=3;"))
                {
                    con.Open();

                    SqliteCommand chkNameExist = new SqliteCommand("SELECT count(*) FROM DnDCharacter", con);

                    int count = Convert.ToInt32(chkNameExist.ExecuteScalar());

                    if (count > 0)
                    {
                        SqliteCommand vAll = new SqliteCommand("SELECT * FROM DnDCharacter", con);
                        var ret = vAll.ExecuteReader();

                        while (ret.Read())
                        {
                            names.Add(ret.GetString(0));

                            age.Add(Convert.ToString(ret.GetInt32(1)));

                            gender.Add(ret.GetString(2));

                            bio.Add(ret.GetString(3));

                            level.Add(Convert.ToString(ret.GetInt32(4)));

                            race.Add(ret.GetString(5));

                            cClass.Add(ret.GetString(6));

                            spell.Add(ret.GetString(7));

                            hits.Add(Convert.ToString(ret.GetInt32(8)));

                            ability.Add(Convert.ToString(ret.GetInt32(9)));

                        }
                        ret.NextResult();

                        allResults.Add("Name", names);
                        allResults.Add("Age", age);
                        allResults.Add("Gender", gender);
                        allResults.Add("Bio", bio);
                        allResults.Add("Level", level);
                        allResults.Add("Race", race);
                        allResults.Add("Class", cClass);
                        allResults.Add("Spellcaster", spell);
                        allResults.Add("Hit Points", hits);
                        allResults.Add("Ability Scores", ability);

                    }

                    else
                    {
                        List<string> err = new List<string>();
                        err.Add("No Records available");
                        allResults.Add("Error", err);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return allResults;
        }

        [HttpGet]
        [Route("View/Character/{name}")]
        public Dictionary<string, string> SingleCharacter(string name)
        {
            Dictionary<string, string> dbResults = new Dictionary<string, string>();

            try
            {
                using (con = new SqliteConnection("Data Source=DnDBuilderDB.sqlite;Version=3;"))
                {
                    con.Open();

                    SqliteCommand chkNameExist = new SqliteCommand("SELECT count(*) FROM DnDCharacter WHERE Name='" + name + "'", con);

                    int count = Convert.ToInt32(chkNameExist.ExecuteScalar());

                    if (count != 0)
                    {
                        SqliteCommand vSingle = new SqliteCommand("SELECT * FROM DnDCharacter WHERE Name='" + name + "'", con);
                        var ret = vSingle.ExecuteReader();

                        dbResults.Add("Name", (string)ret[0]);
                        dbResults.Add("Age", Convert.ToString(ret[1]));
                        dbResults.Add("Gender", (string)ret[2]);
                        dbResults.Add("Bio", (string)ret[3]);
                        dbResults.Add("Level", Convert.ToString(ret[4]));
                        dbResults.Add("Race", (string)ret[5]);
                        dbResults.Add("Class", (string)ret[6]);
                        dbResults.Add("Spellcaster", (string)ret[7]);
                        dbResults.Add("Hit Points", Convert.ToString(ret[8]));
                        dbResults.Add("Ability Score", Convert.ToString(ret[9]));


                        con.Close();
                    }
                    else
                    {
                        dbResults.Add("error", "Given name does not exist");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dbResults;
        }

        [HttpGet]
        [Route("Download/Character/{name}")]
        public string Download(string name)
        {
            string msg = null;
            DataTable dt = new DataTable();
            dt.TableName = "dndCharacter";

            try
            {
                using (con = new SqliteConnection("Data Source=DnDBuilderDB.sqlite;Version=3;"))
                {
                    con.Open();

                    SqliteCommand chkNameExist = new SqliteCommand("SELECT count(*) FROM DnDCharacter WHERE Name='" + name + "'", con);

                    int count = Convert.ToInt32(chkNameExist.ExecuteScalar());

                    if (count != 0)
                    {
                        SqliteCommand vSingle = new SqliteCommand("SELECT * FROM DnDCharacter WHERE Name='" + name + "'", con);
                        var ret = vSingle.ExecuteReader();

                        DataColumn dc1 = new DataColumn("Name");
                        DataColumn dc2 = new DataColumn("Age");
                        DataColumn dc3 = new DataColumn("Gender");
                        DataColumn dc4 = new DataColumn("Bio");
                        DataColumn dc5 = new DataColumn("Level");
                        DataColumn dc6 = new DataColumn("Race");
                        DataColumn dc7 = new DataColumn("Class");
                        DataColumn dc8 = new DataColumn("Spellcaster");
                        DataColumn dc9 = new DataColumn("Hit Points");
                        DataColumn dc10 = new DataColumn("Ability Score");

                        dt.Columns.Add(dc1);
                        dt.Columns.Add(dc2);
                        dt.Columns.Add(dc3);
                        dt.Columns.Add(dc4);
                        dt.Columns.Add(dc5);
                        dt.Columns.Add(dc6);
                        dt.Columns.Add(dc7);
                        dt.Columns.Add(dc8);
                        dt.Columns.Add(dc9);
                        dt.Columns.Add(dc10);

                        dt.Rows.Add(ret[0], Convert.ToString(ret[1]), ret[2], ret[3], Convert.ToString(ret[4]), ret[5], ret[6], ret[7], Convert.ToString(ret[8]), Convert.ToString(ret[9]));

                        DataSet ds = new DataSet();

                        ds.Tables.Add(dt);

                        ds.WriteXml(ret[0] + ".xml");

                        msg = "Downloaded successfully";

                        con.Close();
                    }
                    else
                    {
                        msg = "Record does not exist";
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return msg;
        }
    } 
}
