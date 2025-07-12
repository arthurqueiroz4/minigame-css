using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class SlowVelocityMinigame : IMinigame
{
    public SlowVelocityMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Slow Velocity";

    public void Register(List<CCSPlayerController>? players = null)
    {
        if (players != null)
        {
            foreach (var player in players)
            {
                if (player.PlayerPawn.Value != null)
                {
                    player.PlayerPawn.Value.VelocityModifier = 0.3f;
                }
            }
        }
        
        Server.ExecuteCommand("sv_maxspeed 100");
        Server.PrintToChatAll(" \x04[Slow Velocity Mode]\x01 Players move at reduced speed!");
    }

    public void Unregister()
    {
        var players = Utilities.GetPlayers();
        foreach (var player in players)
        {
            if (player.PlayerPawn.Value != null)
            {
                player.PlayerPawn.Value.VelocityModifier = 1.0f;
            }
        }
        
        // Restore normal max speed
        Server.ExecuteCommand("sv_maxspeed 320");
        Server.PrintToChatAll(" \x04[Slow Velocity Mode]\x01 Speed restored to normal!");
    }
} 