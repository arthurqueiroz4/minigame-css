using System.Text.RegularExpressions;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;

namespace Minigame.Utils;

public static class Helper
{
    public static CCSGameRules GameRules =>
        Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").First().GameRules!;

    public static void WriteColor(string message, ConsoleColor color)
    {
        var pieces = Regex.Split(message, @"(\[[^\]]*\])");

        for (var i = 0; i < pieces.Length; i++)
        {
            var piece = pieces[i];

            if (piece.StartsWith("[") && piece.EndsWith("]"))
            {
                Console.ForegroundColor = color;
                piece = piece.Substring(1, piece.Length - 2);
            }

            Console.Write(piece);
            Console.ResetColor();
        }

        Console.WriteLine();
    }

    public static void RunCommand(string command, string value)
    {
        var cvarFound = ConVar.Find($"{command}");
        if (cvarFound == null)
            return;
        Server.ExecuteCommand($"{command} {value}");
    }

    public static void DeniedBuying()
    {
        RunCommand("mp_buytime", "0");
    }

    public static bool IsPlayerAlive(CCSPlayerController? player)
    {
        return player?.PawnIsAlive ?? false;
    }

    public static void RemoveBuyzones(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid)
        {
            WriteColor($"FornPlugin - [*{player?.PlayerName ?? "Unknown"}*] is not valid or is disconnected.",
                ConsoleColor.Red);
            return;
        }

        WriteColor("FornPlugin - Removing all buyzones.", ConsoleColor.Yellow);
    }
}
