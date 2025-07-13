using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class OnlyP250Minigame : BaseWeaponMinigame, IMinigame
{
    private readonly BasePlugin _plugin;

    public OnlyP250Minigame(BasePlugin plugin)
    {
        _plugin = plugin;
    }

    public override BasePlugin Plugin => _plugin;
    protected override string WeaponName => "weapon_p250";
    public string Name => "Only P250";
}
