namespace OpenModServer.Structures;

public enum TemporaryAlertType
{
    Primary,
    Secondary,
    Success,
    Danger,
    Warning,
    Info,
    Light,
    Dark
}
public record class TemporaryAlert(TemporaryAlertType Type, string Name, string? Text);