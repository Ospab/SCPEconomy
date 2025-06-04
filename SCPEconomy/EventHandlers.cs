using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;

namespace SCPLEconomy
{
    public class EventHandlers
    {
        private readonly Database database;

        public EventHandlers(Database db)
        {
            database = db;
        }

        public void OnPlayerKilled(DiedEventArgs ev)
        {
            if (ev.Attacker != null && ev.Attacker != ev.Player)
            {
                database.AddCoins(ev.Attacker.UserId, MainPlugin.Instance.Config.KillReward);
                ev.Attacker.ShowHint($"+{MainPlugin.Instance.Config.KillReward} coins! \n Total: {database.GetCoins(ev.Attacker.UserId)}", 3f);
            }
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            switch (ev.LeadingTeam)
            {
                case (LeadingTeam)Team.FoundationForces:
                    foreach (Player player in Player.List)
                        if (player.Role.Team == Team.FoundationForces)
                            database.AddCoins(player.UserId, MainPlugin.Instance.Config.MtfWinReward);
                    break;

                case (LeadingTeam)Team.ChaosInsurgency:
                    foreach (Player player in Player.List)
                        if (player.Role.Team == Team.ChaosInsurgency)
                            database.AddCoins(player.UserId, MainPlugin.Instance.Config.ChaosWinReward);
                    break;
            }
        }
    }
}