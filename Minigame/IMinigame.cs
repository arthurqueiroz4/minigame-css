using CounterStrikeSharp.API.Core;

namespace Forn;

public interface IMinigame
{
    public BasePlugin Plugin { get; }
    public string Name { get; }
    void Register(List<CCSPlayerController>? players = null);
    void Unregister();
}