using UnityEngine;

public class InventoryClicker : MonoBehaviour, IClickable
{
    [SerializeField] private RectTransform inventoryCanvas;

    
    private void OnEnable()
    {
        InputManager.OnObjectClicked += HandleClickSilo;
        InputManager.OnClickedGround += HandleHideSilo;
    }
    private void OnDisable()
    {
        InputManager.OnObjectClicked -= HandleClickSilo;
        InputManager.OnClickedGround -= HandleHideSilo;
    }
    public void OnClick()
    {
        HandleClickSilo(this);
    }
    private void HandleClickSilo(IClickable clicker)
    {
        UIManager.d_Instance.ShowUI(inventoryCanvas);
    }
    private void HandleHideSilo()
    {
        UIManager.d_Instance.HideUI(inventoryCanvas);
    }
}
