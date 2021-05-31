﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace ArtCritic_Desctop.core.db
{
    /// <summary>
    /// Класс, осуществляющий взаимодействие БД SQLite с сущностями игроков.
    /// Реализованы статические методы CRUD.
    /// </summary>
    public class PlayerDao
    {
        private static SQLiteConnection DbCon { get; set; }
        private static SQLiteCommand SqlCmd { get; set; }
        private static string DbFileName { get; set; }

        static PlayerDao()
        {
            DbFileName = MainWindow.DbFileName;
            DbCon = null;
            SqlCmd = null;
        }

        /// <summary>
        /// Инициализирует таблицу для хранения игроков в БД, если её ещё нет.
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

                SqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS player (id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, player_stat_id INTEGER REFERENCES player_stat (id) NOT NULL, name VARCHAR UNIQUE NOT NULL, password VARCHAR NOT NULL);";
                SqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при создании таблицы player: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Возвращает из БД игрока по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомого игрока.</param>
        /// <returns>Объект Player с идентификатором id, или же null, если записи с таким id нет в БД.</returns>
        public static Player Get(int id)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, player_stat_id, name, password FROM player WHERE id = '{0}';", id);
                SqlCmd.ExecuteNonQuery();

                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Player result = new Player(reader.GetInt32(0), PlayerStatDao.Get(reader.GetInt32(1)), reader.GetString(2), reader.GetString(3));
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
                throw new SQLiteException("Ошибка при чтении игрока из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Возвращает из БД игрока по его имени и паролю.
        /// </summary>
        /// <param name="name">Имя искомого игрока.</param>
        /// <param name="password">Пароль искомого игрока.</param>
        /// <returns>Объект Player с именем name и паролем password, если такая запись есть в БД.</returns>
        /// <exception cref="PlayerNotFoundException">Если игрок с таким именем не найден.</exception>
        /// <exception cref="PasswordIsIncorrectException">Если пароли не совпали.</exception>
        public static Player Get(string name, string password)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("SELECT id, player_stat_id, name, password FROM player WHERE name = '{0}';", name);
                SqlCmd.ExecuteNonQuery();

                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int _id = reader.GetInt32(0);
                    int _player_stat_id = reader.GetInt32(1);
                    string _name = reader.GetString(2);
                    string _password = reader.GetString(3);

                    Player result = null;
                    if (_password.Equals(password))
                        result = new Player(_id, PlayerStatDao.GetByPlayerId(_id), _name, _password);
                    else
                        throw new PasswordIsIncorrectException("Пароли не совпадают");

                    reader.Close();
                    return result;
                }
                else
                {
                    throw new PlayerNotFoundException("Пользователя с таким именем не существует");
                }
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при чтении игрока из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Возвращает список всех хранящихся в БД игроков.
        /// </summary>
        /// <returns>Список List с игроками Player, или пустой список, если в БД нет игроков.</returns>
        public static List<Player> GetPlayers()
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = "SELECT id, player_stat_id, name, password FROM player;";
                SqlCmd.ExecuteNonQuery();

                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                List<Player> players = new List<Player>();
                while(reader.Read())
                {
                    Player p = new Player(reader.GetInt32(0), PlayerStatDao.Get(reader.GetInt32(1)), reader.GetString(2), reader.GetString(3));
                    players.Add(p);
                }

                reader.Close();
                return players;
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при чтении игрока из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Добавляет в БД новую запись для игрока, после чего возвращает её с присвоенным ей идентификатором.
        /// </summary>
        /// <param name="p">Игрок, добавляемый в БД.</param>
        /// <returns>Объект Player хранящегося в БД только что добавленного игрока.</returns>
        /// <exception cref="SQLiteException">При неудачном добавлении игрока в БД.</exception>
        public static Player Add(Player p)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                int statId = PlayerStatDao.Add();

                SqlCmd.CommandText = String.Format("INSERT INTO player (player_stat_id, name,  password) VALUES('{0}', '{1}', '{2}');", statId, p.Name, p.Password);
                SqlCmd.ExecuteNonQuery();

                SqlCmd.CommandText = "SELECT id FROM player WHERE rowid = last_insert_rowid()";
                SQLiteDataReader reader = SqlCmd.ExecuteReader();
                int playerId;
                if (reader.HasRows)
                {
                    reader.Read();
                    playerId = reader.GetInt32(0);
                    reader.Close();
                    return PlayerDao.Get(playerId);
                }
                else
                {
                    throw new SQLiteException("Ошибка при получении id игрока");
                }
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при добавлении игрока в таблицу: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Обновляет уже хранящегося в БД игрока p. Обновление происходит по id игрока.
        /// </summary>
        /// <param name="p">Обновляемый игрок.</param>
        /// <returns>Объект Player хранящегося в БД только что обновлённого игрока.</returns>
        public static Player Update(Player p)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                SqlCmd.CommandText = String.Format("UPDATE player SET player_stat_id = '{1}', name = '{2}', password = '{3}' WHERE id = '{0}';", p.Id, p.Stat.Id, p.Name, p.Password);
                SqlCmd.ExecuteNonQuery();

                return PlayerDao.Get(p.Id);
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при обновлении игрока в таблице: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }

        /// <summary>
        /// Удаляет игрока из БД.
        /// </summary>
        /// <param name="p">Удаляемый из БД игрок.</param>
        public static void Delete(Player p)
        {
            Delete(p.Id);
        }

        /// <summary>
        /// Удаляет игрока из БД по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого из БД игрока.</param>
        public static void Delete(int id)
        {
            try
            {
                SqlCmd = new SQLiteCommand();
                DbCon = new SQLiteConnection("Data Source=" + DbFileName + ";Version=3;");
                DbCon.Open();
                SqlCmd.Connection = DbCon;

                Player p = PlayerDao.Get(id);

                SqlCmd.CommandText = String.Format("DELETE FROM player WHERE id = '{0}';", id);
                SqlCmd.ExecuteNonQuery();

                PlayerStatDao.Delete(p.Stat.Id);
            }
            catch (SQLiteException ex)
            {
                throw new SQLiteException("Ошибка при удалении игрока из таблицы: ", ex);
            }
            finally
            {
                DbCon.Close();
            }
        }
    }

    /// <summary>
    /// Пользователь не найден в БД.
    /// </summary>
    public class PlayerNotFoundException : Exception
    {
        public PlayerNotFoundException(string message)
        : base(message)
        { }
    }

    /// <summary>
    /// Пароль пользователя не совпадает с хранящимся в БД.
    /// </summary>
    public class PasswordIsIncorrectException : Exception
    {
        public PasswordIsIncorrectException(string message)
        : base(message)
        { }
    }

}
