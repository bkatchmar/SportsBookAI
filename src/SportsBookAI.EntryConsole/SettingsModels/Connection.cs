namespace SportsBookAI.EntryConsole.SettingsModels;

public class Connection
{
    public string Key { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;

    public override string ToString() =>  this.Key;
    public override int GetHashCode()
    {
        return HashCode.Combine(Key);
    }
}