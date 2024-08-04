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
        // U¿yj LeanTween do p³ynnej animacji wartoœci
        LeanTween.value(gameObject, UpdateValue, fromValue, toValue, duration);
    }

    void UpdateValue(float value)
    {
        // Aktualizuj wartoœæ w dowolny sposób
        currentValue = value;

        // Przyk³adowe u¿ycie: zmieñ przezroczystoœæ materia³u na podstawie wartoœci
        // Wymaga komponentu Renderer i materia³u z ustawionym przezroczystoœci¹
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Color color = renderer.material.color;
            color.a = currentValue; // Ustaw kana³ alfa na aktualn¹ wartoœæ
            renderer.material.color = color;
        }
    }
}