using CounterStrikeSharp.API.Core;

namespace Forn.CSSharp;

public partial class FornPlugin
{
    
    private Dictionary<WeaponType, string> WeaponsLabelsMap { get; } = new()
    {
        { WeaponType.P90, "P90" }
    };
    private Dictionary<WeaponType, string> WeaponsCommandMap { get; } = new()
    {
        { WeaponType.P90, "weapon_p90" },
        { WeaponType.Knife, "weapon_knife" },
    };
    
    void GiveWeaponsTo(CCSPlayerController? player, List<WeaponType> types)
    {
        foreach (var type in types)
        {
            player?.GiveNamedItem(WeaponsCommandMap[type]);
        }
    }
}