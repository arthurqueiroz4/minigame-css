using CounterStrikeSharp.API.Core;

namespace Minigame;

public interface IMinigame
{
    public BasePlugin Plugin { get; }
    public string Name { get; }
    void Register(List<CCSPlayerController>? players = null);
    void Unregister();
}