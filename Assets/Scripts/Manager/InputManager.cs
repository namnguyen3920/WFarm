using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static event Action<IClickable> OnObjectClicked;
    public static event Action OnClickedGround;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                IClickable clickable = hit.collider.GetComponentInParent<IClickable>();

                if (clickable != null)
                {
                    clickable.OnClick();
                    return;
                }
            }
            OnClickedGround?.Invoke();
        }
        
    }
}
