using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Drawing;

namespace Minigame.Minigames;

public class InvisibleMinigame : IMinigame
{
    public InvisibleMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "Invisible Players";

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterListener<Listeners.OnTick>(OnTickInvisible);
    }

    public void Unregister()
    {
        Plugin.RemoveListener<Listeners.OnTick>(OnTickInvisible);
        var targetPlayers = Utilities.GetPlayers();
        foreach (var player in targetPlayers)
        {
            ChangePlayerVisible(player, 255);
        }
    }

    private void OnTickInvisible()
    {
        var players = Utilities.GetPlayers();
        foreach (var player in players)
        {
            ChangePlayerVisible(player, 0);
        }
    }

    private void ChangePlayerVisible(CCSPlayerController? player, int count)
    {
        if (player == null) return;
        var playerPawn = player.PlayerPawn.Value;
        if (playerPawn == null)
            return;

        // Torna o jogador invisível (0) ou visível (255)
        playerPawn.Render = Color.FromArgb(count, 255, 255, 255);
        Utilities.SetStateChanged(playerPawn, "CBaseModelEntity", "m_clrRender");

        var weapons = playerPawn.WeaponServices?.MyWeapons;
        if (weapons == null) return;
        foreach (var weapon in weapons)
        {
            var w = weapon.Value;
            if (w == null) continue;
            w.Render = Color.FromArgb(count, 255, 255, 255);
            w.ShadowStrength = 0f;
            Utilities.SetStateChanged(w, "CBaseModelEntity", "m_clrRender");
        }
    }
} 