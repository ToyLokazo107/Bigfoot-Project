using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ObjectsData", menuName = "Scriptable Objects/ObjectsData")]
public class ObjectsData : ScriptableObject
{
    [FoldoutGroup("Object Data")]
    public int ID;

    [FoldoutGroup("Object Data")]
    public string objectName;

    [FoldoutGroup("Object Data")]
    public ObjectType objectType;

    [FoldoutGroup("Object References")]
    [PreviewField(150)]
    public Sprite Icon;

    [FoldoutGroup("Object References")]
    public GameObject objectPrefab;

    [FoldoutGroup("Settings")]
    [TextArea(3, 10)]
    public string Description;
}
