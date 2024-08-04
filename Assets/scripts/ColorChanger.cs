using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Color startColor = new Color(1,1,1,25/255);
    private Color endColor = Color.white;
    private float duration = 0.5f;
    public GameObject obj;

    private Material material;
    private float currentValue;

    public void ChangeCol()
    {
        material = obj.GetComponent<Renderer>().material;
        material.color = startColor;

        StartColorTransition(0,1,duration);
    }

    void StartColorTransition(float fromValue, float toValue, float duration)
    {
        // U�yj LeanTween do p�ynnej animacji warto�ci
        LeanTween.value(gameObject, UpdateValue, fromValue, toValue, duration);
    }

    void UpdateValue(float value)
    {
        // Aktualizuj warto�� w dowolny spos�b
        currentValue = value;

        // Przyk�adowe u�ycie: zmie� przezroczysto�� materia�u na podstawie warto�ci
        // Wymaga komponentu Renderer i materia�u z ustawionym przezroczysto�ci�
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Color color = renderer.material.color;
            color.a = currentValue; // Ustaw kana� alfa na aktualn� warto��
            renderer.material.color = color;
        }
    }
}