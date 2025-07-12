using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Forn.Utils;

namespace Minigame;
public static class Orchestrator
{
    private static readonly List<IMinigame> Minigames = new();
    private static IMinigame? _currentMinigame;
    private static readonly Queue<IMinigame> MinigamesInCooldown = new();
    private static readonly Queue<IMinigame> MinigamesReadyToPlay = new();
    private static int _cooldownThreshold;

    public static void Setup(BasePlugin plugin)
    {
        InstanceMinigames(plugin);
        _cooldownThreshold = (int)(Minigames.Count * 0.5);
        plugin.RegisterEventHandler<EventRoundStart>(OnRoundStart);
        plugin.RegisterEventHandler<EventRoundEnd>(OnRoundEnd);
    }

    private static void InstanceMinigames(BasePlugin plugin)
    {
        var minigameTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IMinigame).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var minigameType in minigameTypes)
        {
            if (Activator.CreateInstance(minigameType, plugin) is IMinigame minigame)
            {
                Console.WriteLine($"Instance of minigame: {minigame.Name}");
                Minigames.Add(minigame);
            }
        }

        Minigames.Shuffle();
        Console.WriteLine($"Total minigames found: {Minigames.Count}");
        Minigames.ForEach(MinigamesReadyToPlay.Enqueue);
    }

    public static void Setup(BasePlugin plugin, IMinigame minigameToTest)
    {
        _cooldownThreshold = 0;
        MinigamesReadyToPlay.Enqueue(minigameToTest);
        plugin.RegisterEventHandler<EventRoundStart>(OnRoundStart);
        plugin.RegisterEventHandler<EventRoundEnd>(OnRoundEnd);
    }

    private static HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if (_currentMinigame != null) return HookResult.Continue;
        var minigame = MinigamesReadyToPlay.Dequeue();
        minigame.Register();
        Server.PrintToChatAll($"Minigame started: {minigame.Name}");
        _currentMinigame = minigame;
        return HookResult.Continue;
    }

    private static HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        if (_currentMinigame == null)
            return HookResult.Continue;
        
        _currentMinigame.Unregister();
        Server.PrintToChatAll($"Minigame ended: {_currentMinigame.Name}");
        MinigamesInCooldown.Enqueue(_currentMinigame);
        _currentMinigame = null;

        if (MinigamesInCooldown.Count > _cooldownThreshold)
        {
            MinigamesReadyToPlay.Enqueue(MinigamesInCooldown.Dequeue());
        }

        return HookResult.Continue;
    }
}