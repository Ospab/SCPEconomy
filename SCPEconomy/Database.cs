using Exiled.API.Features;
using LiteDB;
using System;
using System.IO;

namespace SCPLEconomy
{
    public sealed class Database : IDisposable
    {
        private readonly LiteDatabase _db;

        public Database()
        {
            string dbPath = Path.Combine(Paths.Plugins, "SCPLEconomy", "Economy.litedb");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

            _db = new LiteDatabase(dbPath);
            var collection = _db.GetCollection<PlayerData>("players");
            collection.EnsureIndex(x => x.UserId, true);
        }

        public int GetCoins(string userId)
        {
            var collection = _db.GetCollection<PlayerData>("players");
            var data = collection.FindOne(x => x.UserId == userId);
            return data?.Coins ?? 0;
        }

        public void AddCoins(string userId, int amount)
        {
            var collection = _db.GetCollection<PlayerData>("players");
            var data = collection.FindOne(x => x.UserId == userId) ?? new PlayerData { UserId = userId };

            data.Coins += amount;
            collection.Upsert(data);
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }

    public class PlayerData
    {
        [BsonId]
        public string UserId { get; set; }
        public int Coins { get; set; }
    }
}