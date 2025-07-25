using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VectorSystem = System.Numerics;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Utils;

namespace Minigame.Minigames;

public class ThirdPersonMinigame : IMinigame
{
    public ThirdPersonMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "Third Person";

    private readonly Dictionary<CCSPlayerController, CDynamicProp> thirdPersonPool = new();

    public void Register(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        foreach (var player in targetPlayers)
        {
            if (player == null || !player.IsValid || !player.PawnIsAlive)
                continue;
            if (thirdPersonPool.ContainsKey(player))
                continue;
            CDynamicProp? camera = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
            if (camera == null) continue;
            camera.DispatchSpawn();
            var pos = ThirdPersonUtils.CalculatePositionInFront(player, -110, 90); // Usa utilitário interno
            camera.Teleport(pos, player.PlayerPawn.Value!.V_angle, new Vector(0,0,0));
            player.PlayerPawn.Value!.CameraServices!.ViewEntity.Raw = camera.EntityHandle.Raw;
            Utilities.SetStateChanged(player.PlayerPawn.Value!, "CBasePlayerPawn", "m_pCameraServices");
            thirdPersonPool.Add(player, camera);
        }
        Plugin.RegisterListener<Listeners.OnTick>(OnTick);
    }

    public void Unregister()
    {
        Plugin.RemoveListener<Listeners.OnTick>(OnTick);
        foreach (var kv in thirdPersonPool)
        {
            var player = kv.Key;
            var camera = kv.Value;
            if (player != null && player.IsValid && player.PlayerPawn?.Value != null)
            {
                player.PlayerPawn.Value.CameraServices!.ViewEntity.Raw = uint.MaxValue;
                Utilities.SetStateChanged(player.PlayerPawn.Value, "CBasePlayerPawn", "m_pCameraServices");
            }
            if (camera != null && camera.IsValid)
                camera.Remove();
        }
        thirdPersonPool.Clear();
    }

    private void OnTick()
    {
        foreach (var kv in thirdPersonPool)
        {
            var player = kv.Key;
            var camera = kv.Value;
            if (player == null || !player.IsValid || !player.PawnIsAlive || camera == null || !camera.IsValid)
                continue;
            var pos = ThirdPersonUtils.CalculatePositionInFront(player, -110, 90);
            camera.Teleport(pos, player.PlayerPawn.Value!.V_angle, new Vector(0,0,0));
        }
    }

    private static class ThirdPersonUtils
    {
        public static Vector CalculatePositionInFront(CCSPlayerController player, float offSetXY, float offSetZ = 0)
        {
            var pawn = player.PlayerPawn?.Value;
            if (pawn?.AbsOrigin == null || pawn.EyeAngles == null)
                return new Vector(0, 0, 0);

            float yawAngleRadians = (float)(pawn.EyeAngles.Y * System.Math.PI / 180.0);
            float offsetX = offSetXY * (float)System.Math.Cos(yawAngleRadians);
            float offsetY = offSetXY * (float)System.Math.Sin(yawAngleRadians);

            return new Vector
            {
                X = pawn.AbsOrigin.X + offsetX,
                Y = pawn.AbsOrigin.Y + offsetY,
                Z = pawn.AbsOrigin.Z + offSetZ,
            };
        }
    }
}