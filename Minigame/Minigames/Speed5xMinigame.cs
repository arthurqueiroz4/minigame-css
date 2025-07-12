using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using Minigame.Utils;

namespace Minigame.Minigames;

public class Speed5xMinigame : IMinigame
{
    public Speed5xMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Speed 5x";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Helper.RunCommand("sv_cheats", "1");
        Helper.RunCommand("host_timescale", "5");
    }

    public void Unregister()
    {
        Helper.RunCommand("host_timescale", "1");
    }
}
