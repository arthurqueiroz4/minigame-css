using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minigame.Minigames;

public class FakeGunSoundsMinigame : IMinigame
{
    public FakeGunSoundsMinigame(BasePlugin plugin)
    {
        Plugin = plugin;
    }
    public BasePlugin Plugin { get; }
    public string Name => "Fake Gun Sounds";

    private readonly Dictionary<CCSPlayerController, int> playersWithFakeGunSounds = new();
    private readonly List<(string, string, int, float)> fakeGunSounds = new()
    {
        ("Deagle", "Weapon_DEagle.Single", 3, 2.0f),
        ("M249", "Weapon_M249.Single", 16, 0.7f),
        ("AWP", "Weapon_AWP.Single", 1, 1f),
        ("Bizon", "Weapon_bizon.Single", 10, 1.5f),
        ("P90", "Weapon_P90.Single", 15, 1.1f),
        ("G3SG1", "Weapon_G3SG1.Single", 11, 1.1f),
        ("Negev", "Weapon_Negev.Single", 14, 0.7f),
        ("Nova", "Weapon_Nova.Single", 3, 2.5f),
        ("AUG", "Weapon_AUG.Single", 12, 1.1f),
        ("M4A1", "Weapon_M4A1.Single", 8, 0.9f)
    };
    private Random rng = new();

    public void Register(List<CCSPlayerController>? players = null)
    {
        var targetPlayers = players ?? Utilities.GetPlayers();
        foreach (var player in targetPlayers)
        {
            if (!playersWithFakeGunSounds.ContainsKey(player))
                playersWithFakeGunSounds.Add(player, (int)Server.CurrentTime + rng.Next(3, 10));
        }
        Plugin.RegisterListener<Listeners.OnTick>(OnTickFakeGunSounds);
    }

    public void Unregister()
    {
        Plugin.RemoveListener<Listeners.OnTick>(OnTickFakeGunSounds);
        playersWithFakeGunSounds.Clear();
    }

    private void OnTickFakeGunSounds()
    {
        if (playersWithFakeGunSounds.Count == 0) return;
        var copy = new Dictionary<CCSPlayerController, int>(playersWithFakeGunSounds);
        foreach (var (player, lastSound) in copy)
        {
            try
            {
                if (lastSound == 0
                    || lastSound >= (int)Server.CurrentTime
                    || player == null
                    || player.PlayerPawn == null || !player.PlayerPawn.IsValid || player.PlayerPawn.Value == null
                    || player.Buttons != 0
                    || player.PlayerPawn.Value.LifeState != (byte)LifeState_t.LIFE_ALIVE) continue;
                var (weaponName, soundName, playTotal, soundLength) = fakeGunSounds[rng.Next(fakeGunSounds.Count)];
                EmitFakeGunSounds(player.Handle, soundName, soundLength, playTotal);
                playersWithFakeGunSounds[player] = 0;
            }
            catch
            {
                playersWithFakeGunSounds.Remove(player);
            }
        }
    }

    private void EmitFakeGunSounds(nint playerHandle, string soundName, float soundLength, int playTotal, int playCount = 0)
    {
        playCount += 1;
        CCSPlayerController? player = new CCSPlayerController(playerHandle);
        if (player == null
            || player.PlayerPawn == null || !player.PlayerPawn.IsValid || player.PlayerPawn.Value == null
            || player.PlayerPawn.Value.LifeState != (byte)LifeState_t.LIFE_ALIVE
            || !playersWithFakeGunSounds.ContainsKey(player)) return;
        player.EmitSound(soundName);
        if (playCount >= playTotal)
        {
            playersWithFakeGunSounds[player] = (int)Server.CurrentTime + rng.Next(5, 15);
            return;
        }
        AddTimer(soundLength, () =>
        {
            if (playerHandle == IntPtr.Zero) return;
            float randomDelay = (float)(rng.NextDouble() * (soundLength / 4)) + (soundLength / 3);
            EmitFakeGunSounds(playerHandle, soundName, randomDelay, playTotal, playCount);
        });
    }

    private void AddTimer(float delay, Action action)
    {
        Plugin.AddTimer(delay, action);
    }
} 