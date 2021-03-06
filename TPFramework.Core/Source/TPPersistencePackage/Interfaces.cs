﻿/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;

namespace TP.Framework
{
    public interface IPersistSystem
    {
        HashSet<Type> SupportedTypes { get; }

        void SaveObject<T>(T source);
        void SaveObjects(params object[] objects);

        T LoadObject<T>(T source);
        void LoadObjects(params object[] objects);

        bool IsTypeSupported(Type type);
    }
}
