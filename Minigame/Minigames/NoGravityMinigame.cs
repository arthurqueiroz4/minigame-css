using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class NoGravityMinigame : IMinigame
{
    public NoGravityMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "No Gravity Minigame";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Server.ExecuteCommand("sv_gravity 0");
    }

    public void Unregister()
    {
        Server.ExecuteCommand("sv_gravity 800");
    }
} 