using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public class OnlyAwpMinigame : IMinigame
{
    public OnlyAwpMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public BasePlugin Plugin { get; }
    public string Name => "Only AWP";

    public void Register(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        
        foreach (var player in targetPlayers)
        {
            if (player.PlayerPawn.Value != null && player.PawnIsAlive)
            {
                WeaponUtils.RemoveAllWeapons(player);
                player.GiveNamedItem("weapon_awp");
                player.GiveNamedItem("weapon_knife");
            }
        }
        
    }

    public void Unregister()
    {
        WeaponUtils.RemoveWeaponFromAllPlayers("weapon_awp");
    }
} 