using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minigame.Minigames;

public class ReviveMinigame : IMinigame
{
    public ReviveMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "Revive Teammate";

    private const float ReviveRange = 100.0f;
    private const float ReviveTime = 3.0f;
    private const int MaxRevivesPerRound = 1;
    private readonly Dictionary<int, DiePosition?> deadPlayers = new();
    private readonly Dictionary<int, int> reviveCount = new();
    private readonly Dictionary<int, float?> useStartTime = new();

    public void Register(List<CCSPlayerController>? players = null)
    {
        Plugin.RegisterListener<Listeners.OnTick>(OnTick);
        deadPlayers.Clear();
        reviveCount.Clear();
        useStartTime.Clear();
        foreach (var player in Utilities.GetPlayers())
        {
            reviveCount[player.UserId ?? 0] = 0;
        }
        Plugin.RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
        Plugin.RegisterEventHandler<EventRoundStart>(OnRoundStart);
    }

    public void Unregister()
    {
        Plugin.RemoveListener<Listeners.OnTick>(OnTick);
        Plugin.DeregisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
        Plugin.DeregisterEventHandler<EventRoundStart>(OnRoundStart);
        deadPlayers.Clear();
        reviveCount.Clear();
        useStartTime.Clear();
    }

    private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player != null && player.PlayerPawn?.Value != null)
        {
            deadPlayers[player.UserId ?? 0] = new DiePosition(player.PlayerPawn.Value.AbsOrigin, player.PlayerPawn.Value.EyeAngles);
        }
        return HookResult.Continue;
    }

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        deadPlayers.Clear();
        useStartTime.Clear();
        foreach (var player in Utilities.GetPlayers())
        {
            reviveCount[player.UserId ?? 0] = 0;
        }
        return HookResult.Continue;
    }

    private void OnTick()
    {
        foreach (var player in Utilities.GetPlayers())
        {
            if (!player.IsValid || player.IsBot || !player.PawnIsAlive)
                continue;
            int playerId = player.UserId ?? 0;
     
            if (reviveCount.TryGetValue(playerId, out int count) && count >= MaxRevivesPerRound)
                continue;

            if (!player.Buttons.HasFlag(PlayerButtons.Use))
            {
                useStartTime[playerId] = null;
                continue;
            }

            var target = Utilities.GetPlayers().FirstOrDefault(p =>
                p.TeamNum == player.TeamNum &&
                p.UserId != player.UserId &&
                deadPlayers.TryGetValue(p.UserId ?? 0, out var diePos) && diePos.HasValue &&
                player.PlayerPawn?.Value != null &&
                CalculateDistance(player.PlayerPawn.Value.AbsOrigin, diePos.Value.Position) <= ReviveRange
            );
            if (target == null)
            {
                useStartTime[playerId] = null;
                continue;
            }
            if (!useStartTime[playerId].HasValue)
                useStartTime[playerId] = Server.CurrentTime;
            float elapsed = Server.CurrentTime - useStartTime[playerId].Value;
            int progressBarLength = 20;
            int filledLength = (int)(progressBarLength * (elapsed / ReviveTime));
            int emptyLength = progressBarLength - filledLength;
            string progressBar = new string('▌', filledLength) + new string('░', emptyLength);
            int percentage = (int)((elapsed / ReviveTime) * 100);
            player.PrintToCenterHtml($"<font color='#00ff00'>Reviving {target.PlayerName}: 『{progressBar}』 {percentage}%</font>");
            if (elapsed >= ReviveTime)
            {
                RespawnPlayer(target, deadPlayers[target.UserId ?? 0]);
                deadPlayers[target.UserId ?? 0] = null;
                reviveCount[playerId] = count + 1;
                useStartTime[playerId] = null;
                player.PrintToChat($"\x04[Minigame] \x01Revive complete: {target.PlayerName}");
            }
        }
    }

    private void RespawnPlayer(CCSPlayerController player, DiePosition? diePos)
    {
        if (player == null) return;
        player.Respawn();
        if (diePos.HasValue && player.PlayerPawn?.Value != null)
        {
            player.PlayerPawn.Value.Teleport(diePos.Value.Position, diePos.Value.Angle);
            player.PlayerPawn.Value.Health = 50;
            Server.NextFrame(() => Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth"));
        }
    }

    private float CalculateDistance(Vector a, Vector b)
    {
        return MathF.Sqrt(MathF.Pow(a.X - b.X, 2) + MathF.Pow(a.Y - b.Y, 2) + MathF.Pow(a.Z - b.Z, 2));
    }

    private struct DiePosition
    {
        public Vector Position { get; }
        public QAngle Angle { get; }
        public DiePosition(Vector position, QAngle angle)
        {
            Position = position;
            Angle = angle;
        }
    }
}