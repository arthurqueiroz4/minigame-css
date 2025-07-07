namespace Forn.CSSharp;

public partial class FornPlugin
{
    public string? CurrentLabel { get; set; } = null;
    
    void OnlyP90Mode()
    {
        CurrentLabel = "Only P90 Mode";
        CreateWeaponsOnlyMode([WeaponType.P90, WeaponType.Knife]);
    }
}