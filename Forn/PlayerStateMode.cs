using CounterStrikeSharp.API.Core;

namespace Forn;

public partial class FornPlugin
{
    private Dictionary<Mode, Action<List<CCSPlayerController>>> ConstructorStateModes { get; } = new()
    {
        {
            Mode.HighJump, _ =>
            {
                CurrentLabel = "High Jump Mode";
                CreateHighJumpMode();
            }
        },
        {
            Mode.IncreasedHp, players =>
            {
                CurrentLabel = "Increased Hp Mode";
                CreateIncreasedHpMode(players);
            }
        },
        {
            //TODO Ao levar tiro a velocidade está voltando ao normal
            Mode.SpeedVelocity, players =>
            {
                CurrentLabel = "Speed Velocity Mode";
                CreateSpeedVelocityMode(players);
            }
        },
        {
            Mode.SlowVelocity, players =>
            {
                CurrentLabel = "Slow Velocity Mode";
                CreateSlowVelocityMode(players);
            }
        }
    };

    private static void CreateIncreasedHpMode(List<CCSPlayerController> players)
    {
        foreach (var player in players)
        {
            player.PlayerPawn.Value!.Health = 500;
            CurrentLabel = "500 HP";
        }
    }

    private static void TurnOffIncreasedHpMode(List<CCSPlayerController> players)
    {
        foreach (var player in players) player.PlayerPawn.Value!.Health = 100;
    }

    private static void CreateHighJumpMode()
    {
        RunCommand("sv_gravity", "300");
    }

    private static void TurnOffHighJumpMode()
    {
        RunCommand("sv_gravity", "800");
    }

    private static void CreateSpeedVelocityMode(List<CCSPlayerController> players)
    {
        foreach (var player in players) player.PlayerPawn.Value!.VelocityModifier = 2.0f;
    }

    private static void CreateSlowVelocityMode(List<CCSPlayerController> players)
    {
        foreach (var player in players) player.PlayerPawn.Value!.VelocityModifier = 0.3f;

        RunCommand("sv_maxspeed", "100");
    }

    private static void TurnOffVelocityMode(List<CCSPlayerController> players)
    {
        foreach (var player in players) player.PlayerPawn.Value!.VelocityModifier = 1.0f;

        RunCommand("sv_maxspeed", "320");
    }
}