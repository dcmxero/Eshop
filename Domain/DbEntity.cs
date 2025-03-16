namespace Domain;

/// <summary>
/// Represents the base class for entities with an ID in the system.
/// </summary>
public abstract class DbEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }
}
