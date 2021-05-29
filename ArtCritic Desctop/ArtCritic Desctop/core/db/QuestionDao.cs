using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace ArtCritic_Desctop.core.db
{
    class QuestionDao
    {
        private static SQLiteConnection DbCon { get; set; }
        private static SQLiteCommand SqlCmd { get; set; }
        private static String DbFileName { get; set; }
        static QuestionDao()
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

                SqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS question (id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, pack_id INTEGER REFERENCES pack (id) NOT NULL, type INTEGER NOT NULL, text VARCHAR, answer VARCHAR NOT NULL, file_name VARCHAR);";
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при создании таблицы question: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        public static Question Get(int id)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, pack_id, type, text, answer, file_name FROM question WHERE id = '{0}';", id);
                SqlCmd.ExecuteNonQuery();

                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Question result = new Question(reader.GetInt32(0), PackDao.Get(reader.GetInt32(1)), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
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
                throw new SQLiteException("Ошибка при чтении вопроса из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        public static List<Question> getQuestionsForPack(Pack p)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, pack_id, type, text, answer, file_name FROM question WHERE pack_id = '{0}';", p.Id);
                SqlCmd.ExecuteNonQuery();

                List<Question> questions = new List<Question>();
                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                while(reader.Read())
                {
                    Question q = new Question(reader.GetInt32(0), p, reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
                    questions.Add(q);
                }

                reader.Close();
                return questions;
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при чтении вопроса из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        public static void Add(Question q)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("INSERT INTO question (pack_id, type, text, answer, file_name) VALUES('{0}', '{1}', '{2}', '{3}', '{4}');", q.Pack.Id, q.Type, q.Text, q.Answer, q.FileName);
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при добавлении вопроса в таблицу: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        public static void Update(Question q)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("UPDATE question SET pack_id = '{1}', type = '{2}', text = '{3}', answer = '{4}', file_name = '{5}' WHERE id = '{0}';", q.Id, q.Pack.Id, q.Type, q.Text, q.Answer, q.FileName);
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при обновлении вопроса в таблице: ", ex);
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

                SqlCmd.CommandText = String.Format("DELETE FROM question WHERE id = '{0}';", id);
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при удалении вопроса из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        public static void DeleteByPackId(int id)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("DELETE FROM question WHERE pack_id = '{0}';", id);
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при удалении вопроса из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }
    }
}
