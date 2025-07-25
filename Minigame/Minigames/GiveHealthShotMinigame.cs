using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;

namespace Minigame.Minigames;

public class GiveHealthShotMinigame : IMinigame
{
    public GiveHealthShotMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "Give Health Shot (1-3)";

    private int minHealthshots = 1;
    private int maxHealthshots = 3;
    private Random rng = new();

    public void Register(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        foreach (var player in targetPlayers)
        {
            GiveHealthShots(player);
        }
    }

    public void Unregister() { }

    private void GiveHealthShots(CCSPlayerController player)
    {
        int amount = rng.Next(minHealthshots, maxHealthshots + 1);
        for (int i = 0; i < amount; i++)
        {
            player.GiveNamedItem("weapon_healthshot");
        }
    }
} 