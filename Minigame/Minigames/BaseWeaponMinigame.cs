using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public abstract class BaseWeaponMinigame
{
    public abstract BasePlugin Plugin { get; }
    protected abstract string WeaponName { get; }

    public virtual void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler<EventItemPurchase>(OnEventItemPurchase);

        var targetPlayers = players ?? Utilities.GetPlayers();
        foreach (var player in targetPlayers)
        {
            if (player.PlayerPawn.Value != null && player.PawnIsAlive)
            {
                GiveLoadout(player);
            }
        }
    }

    public virtual void Unregister()
    {
        Plugin.DeregisterEventHandler<EventItemPurchase>(OnEventItemPurchase);
        WeaponUtils.RemoveWeaponFromAllPlayers(WeaponName);
    }

    protected virtual HookResult OnEventItemPurchase(EventItemPurchase @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player?.PlayerPawn?.Value == null || !player.PawnIsAlive)
            return HookResult.Continue;

        GiveLoadout(player);
        return HookResult.Continue;
    }

    protected void GiveLoadout(CCSPlayerController player)
    {
        WeaponUtils.RemoveAllWeapons(player);
        player.GiveNamedItem(WeaponName);
        player.GiveNamedItem("weapon_knife");
    }
}
