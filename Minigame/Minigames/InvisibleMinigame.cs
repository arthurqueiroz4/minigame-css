using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minigame.Minigames;

public class InvisibleMinigame : IMinigame
{
    public InvisibleMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "Invisible Players";

    private List<float> invisTimes = new();
    private float roundTime = 0f;
    private bool isInvisible = false;
    private float invisEndTime = 0f;
    private Random rng = new();

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterListener<Listeners.OnTick>(OnTickInvisible);

        invisTimes = Enumerable.Range(0, 3).Select(_ => (float)rng.Next(10, 115)).OrderBy(x => x).ToList();
        roundTime = 0f;
        isInvisible = false;
        invisEndTime = 0f;
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
        roundTime += Server.TickInterval;

        if (!isInvisible && invisTimes.Count > 0 && roundTime >= invisTimes[0])
        {
            isInvisible = true;
            invisEndTime = roundTime + 10f;
            invisTimes.RemoveAt(0);
            Server.PrintToChatAll("\x04[Minigame] \x01 All players are now \x04INVISIBLE\x01 for 10 seconds!");
        }

        if (isInvisible)
        {
            var players = Utilities.GetPlayers();
            foreach (var player in players)
                ChangePlayerVisible(player, 0);

            if (roundTime >= invisEndTime)
            {
                isInvisible = false;
                foreach (var player in players)
                    ChangePlayerVisible(player, 255);
                Server.PrintToChatAll("\x04[Minigame] \x01 All players are \x02NO LONGER\x01 invisible!");
            }
        }
    }

    private void ChangePlayerVisible(CCSPlayerController? player, int count)
    {
        if (player == null) return;
        var playerPawn = player.PlayerPawn.Value;
        if (playerPawn == null)
            return;

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