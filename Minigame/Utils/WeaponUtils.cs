using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Minigame.Utils;

public static class WeaponUtils
{
    public static void RemoveAllWeapons(CCSPlayerController player)
    {
        if (player.PlayerPawn.Value?.WeaponServices?.MyWeapons == null) return;

        foreach (var weapon in player.PlayerPawn.Value.WeaponServices.MyWeapons)
        {
            if (weapon.IsValid && weapon.Value != null)
            {
                weapon.Value.Remove();
            }
        }
    }

    public static void RemoveSpecificWeapon(CCSPlayerController player, string weaponName)
    {
        if (player.PlayerPawn.Value?.WeaponServices?.MyWeapons == null) return;

        foreach (var weapon in player.PlayerPawn.Value.WeaponServices.MyWeapons)
        {
            if (weapon.IsValid && weapon.Value != null && weapon.Value.DesignerName == weaponName)
            {
                weapon.Value.Remove();
                break;
            }
        }
    }

    public static void RemoveWeaponFromAllPlayers(string weaponName)
    {
        var players = Utilities.GetPlayers();
        foreach (var player in players)
        {
            if (player.PlayerPawn.Value != null && player.PawnIsAlive)
            {
                RemoveSpecificWeapon(player, weaponName);
            }
        }
    }
}
