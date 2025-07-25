using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Collections.Generic;

namespace Minigame.Minigames;

public class NoRecoilMinigame : IMinigame
{
    public NoRecoilMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "No Recoil";

    private readonly List<CCSPlayerController> playersWithoutRecoil = new();
    private BasePlugin.GameEventHandler<EventWeaponFire>? handler;

    public void Register(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        foreach (var player in targetPlayers)
        {
            if (!playersWithoutRecoil.Contains(player))
                playersWithoutRecoil.Add(player);
        }
        if (handler == null)
        {
            handler = EventOnWeaponFire;
            Plugin.RegisterEventHandler<EventWeaponFire>(handler);
        }
    }

    public void Unregister()
    {
        if (handler != null)
        {
            Plugin.DeregisterEventHandler<EventWeaponFire>(handler);
            handler = null;
        }
        playersWithoutRecoil.Clear();
    }

    private HookResult EventOnWeaponFire(EventWeaponFire @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;
        if (player == null
            || player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || player.PlayerPawn.Value.WeaponServices == null
            || player.PlayerPawn.Value.WeaponServices.ActiveWeapon == null
            || !player.PlayerPawn.Value.WeaponServices.ActiveWeapon.IsValid
            || player.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value == null) return HookResult.Continue;
        if (!playersWithoutRecoil.Contains(player)) return HookResult.Continue;
        CBasePlayerWeapon weapon = player.PlayerPawn!.Value!.WeaponServices!.ActiveWeapon!.Value!;
        
        player.PlayerPawn.Value.AimPunchAngle.X = 0;
        player.PlayerPawn.Value.AimPunchAngle.Y = 0;
        player.PlayerPawn.Value.AimPunchAngle.Z = 0;
        player.PlayerPawn.Value.AimPunchAngleVel.X = 0;
        player.PlayerPawn.Value.AimPunchAngleVel.Y = 0;
        player.PlayerPawn.Value.AimPunchAngleVel.Z = 0;
        player.PlayerPawn.Value.AimPunchTickBase = -1;
        player.PlayerPawn.Value.AimPunchTickFraction = 0;
        
        weapon.As<CCSWeaponBase>().FlRecoilIndex = 0;
        
        weapon.As<CCSWeaponBase>().AccuracyPenalty = 0;
        return HookResult.Continue;
    }
} 