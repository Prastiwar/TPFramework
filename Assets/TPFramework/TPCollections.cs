/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TPFramework.Core;

namespace TPFramework.Unity
{
    internal static class TPCollections { } // marker to find this script

    //[Serializable]
    //public class SharedObjectCollection
    //{
    //    public readonly Dictionary<int, GameObject> SharedObjects;

    //    public SharedObjectCollection(int capacity = 10)
    //    {
    //        SharedObjects = new Dictionary<int, GameObject>(capacity);
    //    }

    //    /// <summary> Returns shared object if exists, if no, instantiate it and return </summary>
    //    [MethodImpl((MethodImplOptions)0x100)] // agressive inline
    //    public GameObject ShareObject(GameObject gameObject, Transform parent = null)
    //    {
    //        int id = gameObject.GetInstanceID();
    //        if (!SharedObjects.ContainsKey(id))
    //            return SharedObjects[id] = UnityEngine.Object.Instantiate(gameObject, parent);
    //        return SharedObjects[id];
    //    }
    //}

    [Serializable]
    public class SharedGameObjectCollection : SharedObjectCollection<GameObject>
    {
        //public readonly Dictionary<int, GameObject> SharedObjects;

        //public SharedObjectCollection(int capacity = 10)
        //{
        //    SharedObjects = new Dictionary<int, GameObject>(capacity);
        //}

        ///// <summary> Returns shared object if exists, if no, instantiate it and return </summary>
        //[MethodImpl((MethodImplOptions)0x100)] // agressive inline
        //public GameObject ShareObject(GameObject gameObject, Transform parent = null)
        //{
        //    int id = gameObject.GetInstanceID();
        //    if (!SharedObjects.ContainsKey(id))
        //        return SharedObjects[id] = UnityEngine.Object.Instantiate(gameObject, parent);
        //    return SharedObjects[id];
        //}
    }
}
