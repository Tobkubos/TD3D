using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UpgradeVisualize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RayCastFromCamera rcfc;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rcfc.ts !=null && rcfc.ts.GetLevel() < 3)
        {
            rcfc.Visualize = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rcfc.Visualize = false;
    }
}