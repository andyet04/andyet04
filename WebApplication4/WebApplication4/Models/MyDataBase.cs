using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

namespace WebApplication4.Models
{
    public class MyDataBase
    {
        MySqlConnection conn = new MySqlConnection();
        OracleConnection Con;
        public void Disconnect()
        {
            if (Con.State != ConnectionState.Closed)
            {
                Con.Close();
                Con.Dispose();
            }
        }

        public void Connect()
        {
            //string connString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
            //string A = System.Web.Configuration.WebConfigurationManager.AppSettings["webpages:Version"];
            //string connString = "server=127.0.0.1;port=3306;user id=root;password=29926080;database=andy;charset=utf8;";
            string connString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnOracleDB"].ConnectionString;
            Con = new OracleConnection(connString);
            if (Con.State != ConnectionState.Open)
            {
                Con.Open();
            }
        }
        public List<City> GetCityList()
        {
            try
            {
                Connect();
                string sql = @" SELECT  DISTINCT `city`, cityid FROM `Village`";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                List<City> list = new List<City>();

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        City city = new City();
                        city.CityId = dr["CityId"].ToString();
                        city.CityName = dr["city"].ToString();
                        list.Add(city);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
            finally
            {
                Disconnect();
            }
        }
        public bool JudgeCharacter(string DbPhonetic, string Phonetic)
        {
            DbPhonetic = DbPhonetic.Replace("˙", "");
            string[] subs = DbPhonetic.Split('|');
            
            for (int i = 0; i < subs.Length; i++)
            {
                if (subs[i].Substring(0, 1).Equals(Phonetic))
                {
                    return true;
                }
            }
            return false;
        }
        public List<National_Character> GetNational_Character(string Phonetic)
        {
            try
            {
                Connect();
                //string Phonetic = "ㄈ";
                string sql = @"SELECT * FROM national_character where " + "\"" + "Phonetic" + "\" like :Phonetic order by " +
                "\"" + "Unicode" + "\" asc";
                OracleCommand cmd = new OracleCommand();

                cmd.CommandText = sql;
                cmd.Connection = Con;
                cmd.Parameters.Add("Phonetic", OracleDbType.Varchar2).Value = "%" + Phonetic + "%";
                List<National_Character> list = new List<National_Character>();

                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        National_Character nationalCharacter = new National_Character();
                        
                        if (!JudgeCharacter(dr["Phonetic"].ToString(), Phonetic)) {
                            continue;
                        }
                        nationalCharacter.Character = dr["Character"].ToString();
                        list.Add(nationalCharacter);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
            finally
            {
                Disconnect();
            }
        }

        public National_Character GetNational_Unicode_One(string Unicode)
        {
            try
            {
                Connect();
                string sql = @"SELECT * FROM national_character where " + "\"" + "Unicode" + "\" = :Unicode";
                National_Character nationalCharacter = null;
                OracleCommand cmd = new OracleCommand();

                cmd.CommandText = sql;
                cmd.Connection = Con;
                cmd.Parameters.Add("Unicode",OracleDbType.Varchar2).Value = Unicode;
                
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        nationalCharacter = new National_Character();
                    }
                    while (dr.Read())
                    {
                        nationalCharacter.Unicode = dr["Unicode"].ToString();
                        nationalCharacter.BIG_5 = dr["BIG-5"].ToString();
                        nationalCharacter.CNS = dr["CNS"].ToString();
                        nationalCharacter.EUC = dr["EUC"].ToString();
                        nationalCharacter.Integer = Int32.Parse(dr["Integer"].ToString());
                        nationalCharacter.Character = dr["Character"].ToString();
                        nationalCharacter.Phonetic = dr["Phonetic"].ToString();
                        nationalCharacter.Radical = dr["Radical"].ToString();
                        nationalCharacter.Strokes_Total = Int32.Parse(dr["Strokes_Total"].ToString());
                        nationalCharacter.Strokes_Radical = Int32.Parse(dr["Strokes_Radical"].ToString());

                        nationalCharacter.Phonetic = nationalCharacter.Phonetic.Replace('|', '/');
                    }
                }
                return nationalCharacter;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
            finally
            {
                Disconnect();
            }
        }
        public National_Character GetNational_Character_One(string Character)
        {
            try
            {
                Connect();
                string sql = @"SELECT * FROM national_character where " + "\"" + "Character" + "\" = :Character";
                National_Character nationalCharacter = null;
                OracleCommand cmd = new OracleCommand();

                cmd.CommandText = sql;
                cmd.Connection = Con;
                cmd.Parameters.Add("Character", OracleDbType.Varchar2).Value = Character;

                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        nationalCharacter = new National_Character();
                    }
                    while (dr.Read())
                    {
                        nationalCharacter.Unicode = dr["Unicode"].ToString();
                        nationalCharacter.BIG_5 = dr["BIG-5"].ToString();
                        nationalCharacter.CNS = dr["CNS"].ToString();
                        nationalCharacter.EUC = dr["EUC"].ToString();
                        nationalCharacter.Integer = Int32.Parse(dr["Integer"].ToString());
                        nationalCharacter.Character = dr["Character"].ToString();
                        nationalCharacter.Phonetic = dr["Phonetic"].ToString();
                        nationalCharacter.Radical = dr["Radical"].ToString();
                        nationalCharacter.Strokes_Total = Int32.Parse(dr["Strokes_Total"].ToString());
                        nationalCharacter.Strokes_Radical = Int32.Parse(dr["Strokes_Radical"].ToString());

                        nationalCharacter.Phonetic = nationalCharacter.Phonetic.Replace('|', '/');
                    }
                }
                return nationalCharacter;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
            finally
            {
                Disconnect();
            }
        }
        
        public List<Village> GetVillageList(string id)
        {
            try
            {
                Connect();
                string sql = @" SELECT `VillageId`, `Village` FROM `Village` WHERE `CityId` = @cityId";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cityId", id);
                List<Village> list = new List<Village>();

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Village data = new Village();
                        data.VillageId = dr["VillageId"].ToString();
                        data.VillageName = dr["Village"].ToString();
                        list.Add(data);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
            finally
            {
                Disconnect();
            }
        }
        public bool AddUserData(UserData data)
        {
            try
            {
                Connect();
                string id = Guid.NewGuid().ToString();
                string strSQL = @"INSERT INTO `user` (`id`, `account`, `password`, `city`, `village`, `address`)
                          VALUES (@id, @account, @password, @city, @village, @address)";
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@account", MySqlDbType.VarChar).Value = data.account;
                cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = data.password1;
                cmd.Parameters.Add("@city", MySqlDbType.VarChar).Value = data.city;
                cmd.Parameters.Add("@village", MySqlDbType.VarChar).Value = data.village;
                cmd.Parameters.Add("@address", MySqlDbType.VarChar).Value = data.address;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Disconnect();
            }
        }
        public bool CheckUserData(string account, string password)
        {
            try
            {
                Connect();
                string strSQL = "SELECT 1 FROM `user` WHERE `account` = @account AND `password` = @password;";
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.Parameters.Add("@account", MySqlDbType.VarChar).Value = account;
                cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return false;
            }
            finally
            {
                Disconnect();
            }
        }
    }
}