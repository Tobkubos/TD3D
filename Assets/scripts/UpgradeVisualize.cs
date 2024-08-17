using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;

public class UpgradeVisualize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RayCastFromCamera rcfc;
    public void OnPointerEnter(PointerEventData eventData)
    {
        rcfc.Visualize = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rcfc.Visualize = false;
    }
}