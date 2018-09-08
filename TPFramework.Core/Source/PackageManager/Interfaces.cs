/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

namespace TP.Framework.Internal
{
    public interface ITPDefineManager
    {
        void ToggleDefine(string define);
        void SetDefine(string define, bool enabled);

        bool IsDefined(string define);
    }
}
