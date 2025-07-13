using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class OnlyDeagleMinigame : BaseWeaponMinigame, IMinigame
{
    private readonly BasePlugin _plugin;

    public OnlyDeagleMinigame(BasePlugin plugin)
    {
        _plugin = plugin;
    }

    public override BasePlugin Plugin => _plugin;
    protected override string WeaponName => "weapon_deagle";
    public string Name => "Only Deagle";
}
