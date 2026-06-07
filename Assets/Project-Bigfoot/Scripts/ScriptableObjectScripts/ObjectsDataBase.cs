using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ObjectsDataBase", menuName = "Scriptable Objects/ObjectsDataBase")]
public class ObjectsDataBase : ScriptableObject
{
    public Dictionary<int, ObjectsData> objectsGameDataBase = new();

    public ObjectsData GetObjectByID(int id)
    {
        if (objectsGameDataBase.TryGetValue(id, out ObjectsData obj))
        {
            return obj;
        }

        Debug.LogWarning($"No se encontro ningun objeto con el ID: {id}");
        return null;
    }

    public List<ObjectsData> GetObjectsByType(ObjectType type)
    {
        List<ObjectsData> result = new();

        foreach (ObjectsData obj in objectsGameDataBase.Values)
        {
            if (obj.objectType == type)
            {
                result.Add(obj);
            }
        }

        return result;
    }
}