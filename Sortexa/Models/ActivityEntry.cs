namespace Sortexa.Models;

public sealed record ActivityEntry(
    DateTimeOffset Timestamp,
    string Action,
    string Status,
    string Details);
