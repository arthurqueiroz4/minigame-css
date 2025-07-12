using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class FastVelocityMinigame : IMinigame
{
    public FastVelocityMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Fast Velocity";
    private void OnTickFastVelocity()
    {
        var players = Utilities.GetPlayers();
        foreach (var player in players)
        {
            if (player.PlayerPawn.Value != null)
            {
                player.PlayerPawn.Value!.VelocityModifier = 2.0f;
            }
        }
    }

    public void Register(List<CCSPlayerController>? players = null)
    {
        
        Plugin.RegisterListener<Listeners.OnTick>(OnTickFastVelocity);        
    }

        public void Unregister()
        {
        Plugin.RemoveListener<Listeners.OnTick>(OnTickFastVelocity);
        }
} 