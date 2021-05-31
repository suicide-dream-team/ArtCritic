using ArtCritic_Desctop.core;
using System;
using System.Collections.Generic;
using System.Text;


namespace ArtCritic_Desctop.core.db
{
    /// <summary>
    /// Класс для сущности игрока.
    /// </summary>
    public class Player
    {
        public int Id { get; set; }
        public PlayerStat Stat { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Player() {}
        public Player(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }
        public Player(int id, PlayerStat stat, string name, string password)
        {
            this.Id = id;
            this.Name = name;
            this.Password = password;
            this.Stat = stat;
        }
    }
}
