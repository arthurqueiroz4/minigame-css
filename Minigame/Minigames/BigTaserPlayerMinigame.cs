using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Collections.Generic;
using Minigame.Utils;

namespace Minigame.Minigames;

public class BigTaserPlayerMinigame : IMinigame
{
    public BigTaserPlayerMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "Big Taser Player";

    private List<CCSPlayerPawn> playersWithChangedModelSize = new();
    private float bigSize = 1.5f;

    public void Register(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        foreach (var player in targetPlayers)
        {
            GiveTaserAndScale(player);
        }
    }

    public void Unregister()
    {
        ResetAllPlayerSizes();
    }

    private void GiveTaserAndScale(CCSPlayerController? player)
    {
        if (player == null || player.PlayerPawn == null || !player.PlayerPawn.IsValid || player.PlayerPawn.Value == null)
            return;
        var playerPawn = player.PlayerPawn.Value;
        if (!playersWithChangedModelSize.Contains(playerPawn))
        {
            var playerSceneNode = playerPawn.CBodyComponent?.SceneNode;
            if (playerSceneNode != null)
            {
                playerSceneNode.GetSkeletonInstance().Scale = bigSize;
                playerPawn.AcceptInput("SetScale", null, null, bigSize.ToString());
                Server.NextFrame(() =>
                {
                    if (playerPawn == null) return;
                    Utilities.SetStateChanged(playerPawn, "CBaseEntity", "m_CBodyComponent");
                });
                playersWithChangedModelSize.Add(playerPawn);
            }
        }
        WeaponUtils.RemoveAllWeapons(player);
        player.GiveNamedItem("weapon_taser");
        player.GiveNamedItem("weapon_knife");
    }

    private void ResetAllPlayerSizes()
    {
        var copy = new List<CCSPlayerPawn>(playersWithChangedModelSize);
        foreach (var playerPawn in copy)
        {
            try
            {
                if (playerPawn == null) continue;
                var playerSceneNode = playerPawn.CBodyComponent?.SceneNode;
                if (playerSceneNode == null) continue;
                playerSceneNode.GetSkeletonInstance().Scale = 1.0f;
                playerPawn.AcceptInput("SetScale", null, null, "1.0");
                Server.NextFrame(() =>
                {
                    if (playerPawn == null) return;
                    Utilities.SetStateChanged(playerPawn, "CBaseEntity", "m_CBodyComponent");
                });
            }
            catch
            {
                // do nothing
            }
        }
        playersWithChangedModelSize.Clear();
    }
} 