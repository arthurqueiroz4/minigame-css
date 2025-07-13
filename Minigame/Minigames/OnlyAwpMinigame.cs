using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class OnlyAwpMinigame : BaseWeaponMinigame, IMinigame
{
    private readonly BasePlugin _plugin;

    public OnlyAwpMinigame(BasePlugin plugin)
    {
        _plugin = plugin;
    }

    public override BasePlugin Plugin => _plugin;
    protected override string WeaponName => "weapon_awp";
    public string Name => "Only AWP";
}
