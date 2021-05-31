using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace ArtCritic_Desctop.core.db
{
    /// <summary>
    /// Класс, осуществляющий взаимодействие БД SQLite с сущностями пакетов.
    /// Реализованы статические методы CRUD.
    /// </summary>
    public class PackDao
    {
        private static SQLiteConnection DbCon { get; set; }
        private static SQLiteCommand SqlCmd { get; set; }
        private static string DbFileName { get; set; }

        static PackDao()
        {
            DbFileName = MainWindow.DbFileName;
            DbCon = null;
            SqlCmd = null;
        }

        /// <summary>
        /// Инициализирует таблицу для хранения пакетов в БД, если её ещё нет.
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

                SqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS pack (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, name VARCHAR UNIQUE NOT NULL, path VARCHAR NOT NULL UNIQUE, type INTEGER NOT NULL);";
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при создании таблицы pack: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Возвращает из БД пакет по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомого пакета.</param>
        /// <returns>Объект Pack с идентификатором id, или же null, если записи с таким id нет в БД.</returns>
        public static Pack Get(int id)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, name, path, type FROM pack WHERE id = '{0}';", id);
                SqlCmd.ExecuteNonQuery();

                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Pack result = new Pack(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
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
                throw new SQLiteException("Ошибка при чтении пака из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Возвращает список всех хранящихся в БД пакетов вопросов.
        /// </summary>
        /// <returns>Список List с пакетами Pack, или пустой список, если в БД нет пакетов.</returns>
        public static List<Pack> GetPacks()
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, name, path, type FROM pack;");
                SqlCmd.ExecuteNonQuery();

                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                List<Pack> packs = new List<Pack>();

                while(reader.Read())
                {
                    Pack p = new Pack(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                    packs.Add(p);
                }

                reader.Close();
                return packs;
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при чтении паков из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Возвращает из БД пакет по его имени.
        /// </summary>
        /// <param name="name">Имя искомого пакета.</param>
        /// <returns>Объект Pack с именем name, или же null, если пакета с таким именем нет в БД.</returns>
        public static Pack GetByName(string name)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, name, path, type FROM pack WHERE name = '{0}';", name);
                SqlCmd.ExecuteNonQuery();

                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Pack result = new Pack(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
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
                throw new SQLiteException("Ошибка при чтении пака из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Добавляет в БД новую запись для пакета, после чего возвращает её с присвоенным ей идентификатором.
        /// </summary>
        /// <exception>
        /// Выбрасывает исключение SQLiteException при неудачном добавлении пакета в БД.
        /// </exception>
        /// <param name="pack">Пакет, добавляемый в БД.</param>
        /// <returns>Объект Pack хранящегося в БД только что добавленного пакета.</returns>
        public static Pack Add(Pack pack)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("INSERT INTO pack (name, path, type) VALUES('{0}', '{1}', '{2}');", pack.Name, pack.Path, (int)pack.Type);
                SqlCmd.ExecuteNonQuery();

                SqlCmd.CommandText = "SELECT id FROM pack WHERE rowid = last_insert_rowid()";
                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                int packId;
                if (reader.HasRows)
                {
                    reader.Read();
                    packId = reader.GetInt32(0);
                    reader.Close();
                    return PackDao.Get(packId);
                }
                else
                {
                    // Если строки в БД нет - значит, произошла ошибка при добавлении записи в БД
                    // Выбрасываем исключение
                    throw new SQLiteException("Ошибка при получении id пакета");
                }

            }
            catch(SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при добавлении пака в таблицу: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Обновляет уже хранящийся в БД пакет pack. Обновление происходит по id пакета.
        /// </summary>
        /// <param name="pack">Обновляемый пакет.</param>
        /// <returns>Объект Pack хранящегося в БД только что обновлённого пакета.</returns>
        public static Pack Update(Pack pack)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("UPDATE pack SET name = '{1}', path = '{2}', type = '{3}' WHERE id = '{0}';", pack.Id, pack.Name, pack.Path, (int)pack.Type);

                SqlCmd.ExecuteNonQuery();

                return PackDao.Get(pack.Id);
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при обновлении пака в таблице: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Удаляет пакет из БД.
        /// </summary>
        /// <param name="pack">Удаляемый из БД пакет.</param>
        public static void Delete(Pack pack)
        {
            Delete(pack.Id);
        }

        /// <summary>
        /// Удаляет пакет из БД по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого из БД пакета.</param>
        public static void Delete(int id)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("DELETE FROM pack WHERE id = '{0}';", id);
                SqlCmd.ExecuteNonQuery();
                QuestionDao.DeleteByPackId(id);
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при удалении пака из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }
    }
}
