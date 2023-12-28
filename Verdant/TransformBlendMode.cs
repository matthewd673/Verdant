namespace Verdant;

/// <summary>
/// Determines how TransformStates should be blended.
/// </summary>
public enum TransformBlendMode
{
    // Add the two TransformStates together.
    Add,
    // Multiply the two TransformStates together.
    Multiply,
    // Override the other TransformState.
    Override,
}

