namespace Verdant;

/// <summary>
/// Represents the ways that an Entity can update its ZIndex property.
/// </summary>
public enum ZIndexMode
{
    // Assign a ZIndex based on the order it was added to the Manager.
    // This will only work if this is the Entity's ZIndexMode prior to being added to a Manager.
    ByIndex,
    // Manually assign ZIndex (default: 0).
    Manual,
    // Automatically assign ZIndex to the bottom of the Entity's Sprite after each update.
    Bottom,
    // Automatically assign ZIndex to the top of the Entity's Sprite after each update.
    Top,
}
