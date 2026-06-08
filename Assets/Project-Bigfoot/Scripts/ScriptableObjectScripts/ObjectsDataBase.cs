using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ObjectsDataBase", menuName = "Scriptable Objects/ObjectsDataBase")]
public class ObjectsDataBase : SerializedScriptableObject
{
    public Dictionary<int, ObjectsData> objectsGameDataBase = new();

    public ObjectsData GetObjectByID(int id)
    {
        if (objectsGameDataBase.TryGetValue(id, out ObjectsData obj))
        {
            return obj;
        }

        Debug.Log($"No se encontró ningún objeto con el ID: {id}");
        return null;
    }

    public List<ObjectsData> GetObjectsByType(ObjectType type)
    {
        List<ObjectsData> result = new();

        foreach (ObjectsData obj in objectsGameDataBase.Values)
        {
            if (obj != null && obj.objectType == type)
            {
                result.Add(obj);
            }
        }

        return result;
    }
}