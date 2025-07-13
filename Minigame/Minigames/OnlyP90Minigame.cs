using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class OnlyP90Minigame : BaseWeaponMinigame, IMinigame
{
    private readonly BasePlugin _plugin;

    public OnlyP90Minigame(BasePlugin plugin)
    {
        _plugin = plugin;
    }

    public override BasePlugin Plugin => _plugin;
    protected override string WeaponName => "weapon_p90";
    public string Name => "Only P90";
}
