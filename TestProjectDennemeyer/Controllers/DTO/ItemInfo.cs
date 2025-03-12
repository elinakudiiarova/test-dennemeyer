namespace TestProjectDennemeyer.Controllers.DTO;

/// <summary>
/// Represents information about an item.
/// </summary>
public class ItemInfo
{
    /// <summary>
    /// The name of the item.
    /// </summary>
    /// <example>Golden Ring</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The monetary value of the item.
    /// </summary>
    /// <remarks>
    /// This value must be a positive decimal number.
    /// </remarks>
    /// <example>1000.50</example>
    public decimal Value { get; set; }

    /// <summary>
    /// The date and time when the item was created.
    /// </summary>
    /// <remarks>
    /// This is stored as a UTC timestamp.
    /// </remarks>
    /// <example>2024-03-11T14:30:00Z</example>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Information about the party that owns the item.
    /// </summary>
    public required PartyInfo Owner { get; set; }
}