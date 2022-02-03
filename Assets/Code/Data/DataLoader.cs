using System.IO;
using UnityEngine;


public abstract class DataLoader
{
    protected virtual T Load<T>(string resourcesPath) where T : Object
    {
        return Resources.Load<T>(Path.ChangeExtension(resourcesPath, null));
    }
}