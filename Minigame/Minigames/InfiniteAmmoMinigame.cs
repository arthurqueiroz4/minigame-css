using CounterStrikeSharp.API.Core;

namespace Minigame.Minigames;

public class InfiniteAmmoMinigame : IMinigame
{
    public InfiniteAmmoMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Infinite Ammo";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler<EventWeaponFire>(OnPlayerShootAddAmmo, HookMode.Post);
    }

    public void Unregister()
    {
        Plugin.DeregisterEventHandler<EventWeaponFire>(OnPlayerShootAddAmmo, HookMode.Post);
    }

    private HookResult OnPlayerShootAddAmmo(EventWeaponFire @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null || !player.IsValid || !player.PawnIsAlive)
            return HookResult.Continue;

        var weapon = player.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value;
        if (weapon == null || !weapon.IsValid)
            return HookResult.Continue;

        weapon.Clip1 = 30;

        return HookResult.Continue;
    }
}
