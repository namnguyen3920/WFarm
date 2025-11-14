using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldManager : Singleton_Mono_Method<FieldManager>
{
    [SerializeField] public Field selectingField = null;
    [SerializeField] private List<Field> allFields = new List<Field>();


    [SerializeField] Field fieldPrefab;
    [SerializeField] Transform fieldSpawner;
    [SerializeField] List<Transform> fieldSlots = new List<Transform>();
    public int MaxFields => fieldSlots.Count;   

    private void OnEnable()
    {
        GameSettingsManager.OnStarterDataLoaded += HandleSpawnStarterField;
        InputManager.OnClickedGround += HandleFieldDeSelected;
    }
    private void OnDisable()
    {
        GameSettingsManager.OnStarterDataLoaded -= HandleSpawnStarterField;
        InputManager.OnClickedGround -= HandleFieldDeSelected;
    }
    private void HandleSpawnStarterField(Dictionary<string, string> settings)
    {
        foreach (Transform slot in fieldSpawner) fieldSlots.Add(slot);
        int fieldCount = int.Parse(settings["StartFields"]);
        Debug.Log($"Field Count {fieldCount}");
        for (int i = 0; i < fieldCount; i++)
        {
            SpawnField(i);
        }
    }
    public Field SpawnField(int index)
    {
        if (index < 0 || index >= fieldSlots.Count)
        {
            return null;
        }

        Transform fieldSlot = fieldSlots[index];

        GameObject fieldPrefab = Instantiate(this.fieldPrefab.gameObject, fieldSlot.position, fieldSlot.rotation, fieldSlot);
        Field field = fieldPrefab.GetComponent<Field>();

        //allFields.Add(field);

        return field;
    }
    public Field OnSpawnMoreField()
    {
        if (allFields.Count >= MaxFields)
        {
            return null;
        }

        return SpawnField(allFields.Count);
    }
    public void SelectingField(Field field)
    {
        if (selectingField != null && selectingField != field)
        {
            selectingField.HideFieldUI();
            selectingField.isSelecting = false;
        }
        if (selectingField == field)
        {
            field.ShowFieldUI();
            return;
        }
        field.ShowFieldUI();
        selectingField = field;
        selectingField.isSelecting = true;
    }
    private void HandleFieldDeSelected()
    {
        if (selectingField != null)
        {
            selectingField.HideFieldUI();
            selectingField = null;
        }
    }
    public List<Field> GetAllFields()
    {
        return allFields;
    }
    public void RegisterField(Field field)
    {
        if (!allFields.Contains(field))
            allFields.Add(field);
    }
}
