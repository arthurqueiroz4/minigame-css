using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public class SlowMotionMinigame : IMinigame
{
    public SlowMotionMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Slow Motion";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Helper.RunCommand("sv_cheats", "1");
        Helper.RunCommand("host_timescale", "0.5");
    }

    public void Unregister()
    {
        Helper.RunCommand("host_timescale", "1");
    }
}
