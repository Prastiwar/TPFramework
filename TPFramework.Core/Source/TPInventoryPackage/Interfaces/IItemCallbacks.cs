/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;

namespace TP.Framework
{
    public interface IItemCallbacks
    {
        Action OnUsed { get; set; }
        Action OnFailUsed { get; set; }

        Action OnMoved { get; set; }
        Action OnFailMoved { get; set; }

        Action OnEquipped { get; set; }
        Action OnUnEquipped { get; set; }
    }
}
