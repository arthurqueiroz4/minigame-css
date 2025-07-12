using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Forn.Minigame;

public class HighJumpMinigame : IMinigame
{
    public HighJumpMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "High Jump Minigame";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Server.ExecuteCommand("sv_gravity 300");
    }

    public void Unregister()
    {
        Server.ExecuteCommand("sv_gravity 800");
    }
}