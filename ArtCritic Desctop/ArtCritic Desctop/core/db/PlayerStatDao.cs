using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace ArtCritic_Desctop.core.db
{
    class PlayerStatDao
    {
        private static SQLiteConnection DbCon { get; set; }
        private static SQLiteCommand SqlCmd { get; set; }
        private static string DbFileName { get; set; }

        static PlayerStatDao()
        {
            DbFileName = MainWindow.DbFileName;
            DbCon = null;
            SqlCmd = null;
        }

        public static void Init()
        {
            if (!File.Exists(DbFileName))
                throw new SQLiteException("Отсутствует файл БД");

            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS player_stat (id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, played_games INTEGER NOT NULL, total_questions INTEGER NOT NULL, total_correct_answers INTEGER NOT NULL, current_result DOUBLE NOT NULL);";
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при создании таблицы player_stat: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }
        public static PlayerStat Get(int id)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, played_games, total_questions, total_correct_answers, current_result FROM player_stat WHERE id = '{0}';", id);
                SqlCmd.ExecuteNonQuery();

                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    PlayerStat result = new PlayerStat(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetDouble(4));
                    reader.Close();
                    return result;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при чтении статистики игрока из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        public static PlayerStat GetByPlayerId(int id)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, played_games, total_questions, total_correct_answers, current_result FROM player_stat WHERE id IN(SELECT DISTINCT player_stat_id FROM player WHERE player.id = '{0}');", id);
                SqlCmd.ExecuteNonQuery();

                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    PlayerStat result = new PlayerStat(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetDouble(4));
                    reader.Close();
                    return result;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при чтении статистики игрока из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        public static int Add()
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("INSERT INTO player_stat (played_games, total_questions, total_correct_answers, current_result) VALUES(0, 0, 0, 0.0);");
                SqlCmd.ExecuteNonQuery();

                SqlCmd.CommandText = "SELECT id FROM player_stat WHERE rowid = last_insert_rowid()";
                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                int statId;
                if (reader.HasRows)
                {
                    reader.Read();
                    statId = reader.GetInt32(0);
                    reader.Close();
                    return statId;
                }
                else
                {
                    throw new SQLiteException("Ошибка при получении id игрока");
                }
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при добавлении пака в таблицу: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        public static void Update(PlayerStat stat)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("UPDATE player_stat SET played_games = '{1}', total_questions = '{2}', total_correct_answers = '{3}', current_result = '{4}' WHERE id = '{0}';", stat.Id, stat.PlayedGames, stat.TotalQuestions, stat.TotalCorrectAnswers, stat.CurrentResult);
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при обновлении статистики игрока в таблице: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        public static void Delete(int id)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("DELETE FROM player_stat WHERE id = '{0}';", id);
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при удалении статистики игрока из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }
    }
}
