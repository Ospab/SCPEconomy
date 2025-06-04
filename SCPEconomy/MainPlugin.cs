using Exiled.API.Features;
using System;
using SCPLEconomy.Configs;
using HarmonyLib;

namespace SCPLEconomy
{
    public class MainPlugin : Plugin<Config>
    {
        private static readonly Harmony Harmony = new Harmony("com.yourname.scpeconomy");

        public override string Name => "SCP Economy";
        public override string Author => "Ospab";
        public override string Prefix => "SCP_Economy";
        public override Version Version => new Version(1, 1, 0);

        public static MainPlugin Instance { get; private set; }
        public Database Database { get; private set; }
        public EventHandlers EventHandlers { get; private set; }

        public override void OnEnabled()
        {
            try
            {
                Instance = this;
                Database = new Database();
                EventHandlers = new EventHandlers(Database);

                Exiled.Events.Handlers.Player.Died += EventHandlers.OnPlayerKilled;
                Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnded;

                Harmony.PatchAll();

                base.OnEnabled();
                Log.Info("SCPL Economy успешно загружен!");
            }
            catch (Exception ex)
            {
                Log.Error($"Ошибка при загрузке: {ex}");
            }
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnPlayerKilled;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnded;

            Database?.Dispose();
            Harmony.UnpatchAll();

            Instance = null;
            base.OnDisabled();
        }
    }
}