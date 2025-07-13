using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public class BHopMinigame : IMinigame
{
    public BHopMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Bunny Hop";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Helper.RunCommand("sv_autobunnyhopping", "1");
        Helper.RunCommand("sv_enablebunnyhopping", "1");
        Helper.RunCommand("sv_airaccelerate", "2000");
        Helper.RunCommand("sv_staminajumpcost", "0");
        Helper.RunCommand("sv_staminalandcost", "0");
        Helper.RunCommand("sv_staminarecoveryrate", "0");

    }

    public void Unregister()
    {
        Helper.RunCommand("sv_autobunnyhopping", "0");
        Helper.RunCommand("sv_enablebunnyhopping", "0");
        Helper.RunCommand("sv_airaccelerate", "12");
        Helper.RunCommand("sv_staminajumpcost", "0.080");
        Helper.RunCommand("sv_staminalandcost", "0.050");
        Helper.RunCommand("sv_staminarecoveryrate", "60");
    }
}
