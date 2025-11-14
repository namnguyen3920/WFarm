using UnityEngine;

public class ShopClicker : MonoBehaviour, IClickable
{
    [SerializeField] private RectTransform shopCanvas;

    private void OnEnable()
    {
        InputManager.OnObjectClicked += HandleClickShop;
        InputManager.OnClickedGround += HandleHideShop;
    }
    private void OnDisable()
    {
        InputManager.OnObjectClicked -= HandleClickShop;
        InputManager.OnClickedGround -= HandleHideShop;
    }
    public void OnClick()
    {
        HandleClickShop(this);
    }
    private void HandleClickShop(IClickable clicker)
    {
        UIManager.d_Instance.ShowUI(shopCanvas);
    }
    private void HandleHideShop()
    {
        UIManager.d_Instance.HideUI(shopCanvas);
    }
}
