/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

namespace TP.Framework
{
    public interface IAttributeModifier
    {
        float Value { get; }
        object Source { get; }
        ModifierType Type { get; }
        int Priority { get; }
    }
}
