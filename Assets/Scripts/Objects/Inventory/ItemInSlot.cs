using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemAmount;

    public SeedData seed;
    public Field currentField;

    private void OnEnable()
    {
        currentField = GetComponentInParent<Field>();
        itemAmount = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void DisplayItemAttributes(SeedData seedData, float amount)
    {
        if (seedData != null)
        {
            itemIcon.sprite = seedData.itemSprite;
            itemAmount.text = amount.ToString();
        }
        this.seed = seedData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.d_Instance.PlayItemClickSound(this.transform.position);
        currentField.Plant(seed);
    }

}
