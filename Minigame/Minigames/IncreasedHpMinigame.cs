using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class IncreasedHpMinigame : IMinigame
{
    public IncreasedHpMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Increased HP Minigame";

    public void Register(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        
        foreach (var player in targetPlayers)
        {
            if (player.PlayerPawn.Value != null && player.PawnIsAlive)
            {
                player.PlayerPawn.Value.Health = 500;
            }
        }
    }

    public void Unregister()
    {
        var players = Utilities.GetPlayers();
        foreach (var player in players)
        {
            if (player.PlayerPawn.Value != null && player.PawnIsAlive)
            {
                player.PlayerPawn.Value.Health = 100;
            }
        }
    }
} 