using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace ArtCritic_Desctop.core.db
{
    /// <summary>
    /// Класс, осуществляющий взаимодействие БД SQLite с сущностями вопросов.
    /// Реализованы статические методы CRUD.
    /// </summary>
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

        /// <summary>
        /// Инициализирует таблицу для хранения вопросов в БД, если её ещё нет.
        /// </summary>
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

        /// <summary>
        /// Возвращает вопрос из БД по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомого вопроса.</param>
        /// <returns>Объект Question с идентификатором id, или же null, если записи с таким id нет в БД.</returns>
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

        /// <summary>
        /// Возвращает список вопросов из БД для пакета pack.
        /// </summary>
        /// <param name="pack">Пакет, для которого нужно вернуть список вопросов.</param>
        /// <returns>Список List с вопросами Question, или же пустой список, если в БД нет вопросов для пакета pack.</returns>
        public static List<Question> getQuestionsForPack(Pack pack)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, pack_id, type, text, answer, file_name FROM question WHERE pack_id = '{0}';", pack.Id);
                SqlCmd.ExecuteNonQuery();

                List<Question> questions = new List<Question>();
                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                while(reader.Read())
                {
                    Question q = new Question(reader.GetInt32(0), pack, reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
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

        /// <summary>
        /// Добавляет в БД новую запись для вопроса, после чего возвращает её с присвоенным ей идентификатором.
        /// </summary>
        /// <param name="q">Вопрос, добавляемый в БД.</param>
        /// <returns>Объект Question хранящегося в БД только что добавленного вопроса.</returns>
        /// <exception cref="SQLiteException">При неудачном добавлении вопроса в БД.</exception>
        public static Question Add(Question q)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("INSERT INTO question (pack_id, type, text, answer, file_name) VALUES('{0}', '{1}', '{2}', '{3}', '{4}');", q.Pack.Id, (int)q.Type, q.Text, q.Answer, q.FileName);
                SqlCmd.ExecuteNonQuery();

                SqlCmd.CommandText = "SELECT id FROM question WHERE rowid = last_insert_rowid()";
                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                int statId;
                if (reader.HasRows)
                {
                    reader.Read();
                    statId = reader.GetInt32(0);
                    reader.Close();
                    return QuestionDao.Get(statId);
                }
                else
                {
                    throw new SQLiteException("Ошибка при получении id игрока");
                }

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

        /// <summary>
        /// Обновляет уже хранящийся в БД вопрос q. Обновление происходит по id вопроса.
        /// </summary>
        /// <param name="q">Обновляемый вопрос.</param>
        /// <returns>Объект Question хранящегося в БД только что обновлённого вопроса.</returns>
        public static Question Update(Question q)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("UPDATE question SET pack_id = '{1}', type = '{2}', text = '{3}', answer = '{4}', file_name = '{5}' WHERE id = '{0}';", q.Id, q.Pack.Id, (int)q.Type, q.Text, q.Answer, q.FileName);
                SqlCmd.ExecuteNonQuery();

                return QuestionDao.Get(q.Id);
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

        /// <summary>
        /// Удаляет вопрос из БД.
        /// </summary>
        /// <param name="q">Удаляемый из БД вопрос.</param>
        public static void Delete(Question q)
        {
            Delete(q.Id);
        }

        /// <summary>
        /// Удаляет вопрос из БД по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого из БД вопроса.</param>
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

        /// <summary>
        /// Удаляет для пакета pack все связанные с ним вопросы.
        /// </summary>
        /// <param name="pack">Пакет, для которого нужно удалить все вопросы.</param>
        public static void Delete(Pack pack)
        {
            DeleteByPackId(pack.Id);
        }

        /// <summary>
        /// Удаляет для пакета с идентификатором id все связанные с ним вопросы.
        /// </summary>
        /// <param name="id">Идентификатор пакета, для которого нужно удалить все вопросы.</param>
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
