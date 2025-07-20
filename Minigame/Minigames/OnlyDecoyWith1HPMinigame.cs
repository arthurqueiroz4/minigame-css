using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Minigame.Utils;

namespace Minigame.Minigames;

public class OnlyDecoilWith1HPMinigame(BasePlugin plugin) : IMinigame
{
    public BasePlugin Plugin => plugin;

    public string Name => "Only Decoil With 1 HP";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterEventHandler(GrenadeThrownHandler, HookMode.Post);

        var targetPlayers = players ?? Utilities.GetPlayers();
        foreach (var player in targetPlayers)
        {
            if (player.PlayerPawn.Value != null && player.PawnIsAlive)
            {
                player.PlayerPawn.Value.Health = 1;
            }
        }
        GiveLoadout(targetPlayers);
    }

    public void Unregister()
    {
        Plugin.DeregisterEventHandler(GrenadeThrownHandler, HookMode.Post);
    }

    private BasePlugin.GameEventHandler<EventGrenadeThrown> GrenadeThrownHandler =>
        (@event, info) =>
        {
            var player = @event.Userid;
            if (player == null || player.PlayerPawn.Value == null)
                return HookResult.Continue;

            player.GiveNamedItem("weapon_decoy");
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
                player.GiveNamedItem("weapon_decoy");
                player.GiveNamedItem("weapon_knife");
            }
        }
    }
}
