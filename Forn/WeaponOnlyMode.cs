using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Forn;

public enum WeaponType
{
    P90,
    Knife,
    Awp,
    Deagle,
    P250
}

public partial class FornPlugin
{
    private static Dictionary<WeaponType, string> WeaponsCommandMap { get; } = new()
    {
        { WeaponType.P90, "weapon_p90" },
        { WeaponType.Knife, "weapon_knife" },
        { WeaponType.Awp, "weapon_awp" },
        { WeaponType.Deagle, "weapon_deagle" },
        { WeaponType.P250, "weapon_p250" },
    };

    static void CreateWeaponsOnlyMode(List<WeaponType> types, List<CCSPlayerController> players)
    {
        foreach (var player in players)
        {
            if (!IsPlayerAlive(player)) continue;
            RemoveAllWeapons(player);
            DeniedBuying();
            GiveWeaponsTo(player, types);
        }
    }

    static void GiveWeaponsTo(CCSPlayerController? player, List<WeaponType> types)
    {
        foreach (var type in types)
        {
            player?.GiveNamedItem(WeaponsCommandMap[type]);
        }
    }
}