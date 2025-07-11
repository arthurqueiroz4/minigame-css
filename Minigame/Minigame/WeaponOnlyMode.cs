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

public partial class Plugin
{
    // private Dictionary<Mode, Action<List<CCSPlayerController>>> ConstructorWeaponModes { get; } = new()
    // {
    //     {
    //         Mode.OnlyP90, players =>
    //         {
    //             CurrentLabel = "Only P90 Mode";
    //             CreateWeaponsOnlyMode([WeaponType.P90, WeaponType.Knife], players);
    //         }
    //     },
    //     {
    //         Mode.OnlyAwp, players =>
    //         {
    //             CurrentLabel = "Only Awp Mode";
    //             CreateWeaponsOnlyMode([WeaponType.Awp, WeaponType.Knife], players);
    //         }
    //     },
    //     {
    //         Mode.OnlyDeagle, players =>
    //         {
    //             CurrentLabel = "Only Deagle Mode";
    //             CreateWeaponsOnlyMode([WeaponType.Deagle, WeaponType.Knife], players);
    //         }
    //     },
    //     {
    //         Mode.OnlyP250, players =>
    //         {
    //             CurrentLabel = "Only P250 Mode";
    //             CreateWeaponsOnlyMode([WeaponType.P250, WeaponType.Knife], players);
    //         }
    //     }
    // };
    //
    // private static Dictionary<WeaponType, string> WeaponsCommandMap { get; } = new()
    // {
    //     { WeaponType.P90, "weapon_p90" },
    //     { WeaponType.Knife, "weapon_knife" },
    //     { WeaponType.Awp, "weapon_awp" },
    //     { WeaponType.Deagle, "weapon_deagle" },
    //     { WeaponType.P250, "weapon_p250" }
    // };
    //
    // private static void CreateWeaponsOnlyMode(List<WeaponType> types, List<CCSPlayerController> players)
    // {
    //     foreach (var player in players)
    //     {
    //         if (!IsPlayerAlive(player)) continue;
    //         RemoveAllWeapons(player);
    //         DeniedBuying();
    //         GiveWeaponsTo(player, types);
    //     }
    // }
    //
    // private static void GiveWeaponsTo(CCSPlayerController? player, List<WeaponType> types)
    // {
    //     foreach (var type in types) player?.GiveNamedItem(WeaponsCommandMap[type]);
    // }
}