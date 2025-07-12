using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public class OnlyP250Minigame : IMinigame
{
    public OnlyP250Minigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Only P250";

    public void Register(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        
        foreach (var player in targetPlayers)
        {
            if (player.PlayerPawn.Value != null && player.PawnIsAlive)
            {
                WeaponUtils.RemoveAllWeapons(player);
                player.GiveNamedItem("weapon_p250");
                player.GiveNamedItem("weapon_knife");
            }
        }
    }

    public void Unregister()
    {
        WeaponUtils.RemoveWeaponFromAllPlayers("weapon_p250");
    }
} 