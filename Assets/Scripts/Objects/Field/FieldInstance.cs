using UnityEngine;

[System.Serializable]
public class FieldInstance
{
    public string id;
    public Vector3 position;
    public PlantInstance currentPlant;
    public FieldType fieldType;

    public FieldInstance(string id, Vector3 pos)
    {
        this.id = id;
        this.position = pos;
        this.fieldType = FieldType.None;
        this.currentPlant = null;
    }
}
