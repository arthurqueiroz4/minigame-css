using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class HighJumpMinigame : IMinigame
{
    public HighJumpMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "High Jump";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Server.ExecuteCommand("sv_gravity 300");
    }

    public void Unregister()
    {
        Server.ExecuteCommand("sv_gravity 800");
    }
}