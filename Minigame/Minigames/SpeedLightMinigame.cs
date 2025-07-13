using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public class SpeedLightMinigame : IMinigame
{
    public SpeedLightMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Speed Light";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Helper.RunCommand("sv_cheats", "1");
        Helper.RunCommand("host_timescale", "10");
    }

    public void Unregister()
    {
        Helper.RunCommand("host_timescale", "1");
    }
}
