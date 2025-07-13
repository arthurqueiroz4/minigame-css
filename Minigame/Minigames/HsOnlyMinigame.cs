using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public class HsOnlyMinigame(BasePlugin plugin) : IMinigame
{
    public BasePlugin Plugin => plugin;

    public string Name => "Only Headshot";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Helper.RunCommand("mp_damage_headshot_only", "1");
    }

    public void Unregister()
    {
        Helper.RunCommand("mp_damage_headshot_only", "0");
    }
}
