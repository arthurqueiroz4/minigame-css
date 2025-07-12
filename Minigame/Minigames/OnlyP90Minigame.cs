using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public class OnlyP90Minigame : IMinigame
{
    public OnlyP90Minigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Only P90";

    public void Register(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        
        foreach (var player in targetPlayers)
        {
            if (player.PlayerPawn.Value != null && player.PawnIsAlive)
            {
                WeaponUtils.RemoveAllWeapons(player);
                player.GiveNamedItem("weapon_p90");
                player.GiveNamedItem("weapon_knife");
            }
        }
    }

    public void Unregister()
    {
        WeaponUtils.RemoveWeaponFromAllPlayers("weapon_p90");
    }
} 