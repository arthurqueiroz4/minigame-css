using System.Text.RegularExpressions;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;

namespace Forn;

public partial class Plugin
{
    private CCSGameRules GameRules =>
        Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").First().GameRules!;

    private static void WriteColor(string message, ConsoleColor color)
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

    private static void RunCommand(string command, string value)
    {
        var cvarFound = ConVar.Find($"{command}");
        if (cvarFound == null)
        {
            WriteColor($"FornPlugin - Command [*{command}*] not found.", ConsoleColor.Red);
            return;
        }

        WriteColor("FornPlugin - Running command: [*" + command + "*] with value: " + value, ConsoleColor.Yellow);
        Server.ExecuteCommand($"{command} {value}");
    }

    private static void DeniedBuying()
    {
        RunCommand("mp_buytime", "0");
    }

    private static bool IsPlayerAlive(CCSPlayerController? player)
    {
        return player?.PawnIsAlive ?? false;
    }

    private static bool RemoveAllWeapons(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || !player.PawnIsAlive)
        {
            WriteColor($"FornPlugin - [*{player?.PlayerName ?? "Unknown"}*] is not valid or is disconnected.",
                ConsoleColor.Red);
            return false;
        }

        var pawn = player.Pawn.Value;
        if (pawn == null)
        {
            WriteColor($"FornPlugin - [*{player.PlayerName}*] pawn is null.", ConsoleColor.Red);
            return false;
        }

        foreach (var weapon in pawn.WeaponServices!.MyWeapons)
            if (weapon is { IsValid: true, Value.IsValid: true })
                weapon.Value.Remove();

        return true;
    }

    private static void RemoveBuyzones(CCSPlayerController? player)
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