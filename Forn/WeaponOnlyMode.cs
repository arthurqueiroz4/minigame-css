using CounterStrikeSharp.API;

namespace Forn.CSSharp;

public enum WeaponType
{
    P90, Knife
}


public partial class FornPlugin
{
    void CreateWeaponsOnlyMode(List<WeaponType> types)
    {
        var players = Utilities.GetPlayers();
        DeniedBuying();

        foreach (var player in players)
        {
            if (!IsPlayerAlive(player)) continue;
            RemoveAllWeapons(player);
            GiveWeaponsTo(player, types);
        }
    }
}