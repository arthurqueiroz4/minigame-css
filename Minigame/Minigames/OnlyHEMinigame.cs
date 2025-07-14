using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public class OnlyGrenadeHEMinigame : IMinigame
{
    public BasePlugin Plugin { get; }
    public string Name => "Infinite HE Only";

    public OnlyGrenadeHEMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler(WeaponFireHandler, HookMode.Post);
        GiveLoadout(players);
    }

    public void Unregister()
    {
        Plugin.DeregisterEventHandler(WeaponFireHandler, HookMode.Post);
    }

    private BasePlugin.GameEventHandler<EventGrenadeThrown> WeaponFireHandler =>
        (@event, info) =>
        {
            var player = @event.Userid;
            Server.PrintToChatAll("Você entrou no minigame de granadas HE infinitas!");
            if (player == null || player.PlayerPawn.Value == null)
                return HookResult.Continue;

            player.GiveNamedItem("weapon_hegrenade");
            return HookResult.Continue;
        };

    private void GiveLoadout(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        foreach (var player in targetPlayers)
        {
            if (player.PlayerPawn.Value != null && player.PawnIsAlive)
            {
                WeaponUtils.RemoveAllWeapons(player);
                player.GiveNamedItem("weapon_hegrenade");
                player.GiveNamedItem("weapon_knife");
            }
        }
    }
}
