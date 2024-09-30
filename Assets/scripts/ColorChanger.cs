using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Color startColor = new Color(1,1,1,50f/255f);
    private Color endColor = new Color(0.9f, 0.9f, 0.9f, 1f);
    private float duration = 0.5f;
    public GameObject obj;

    private Material material;
    private float currentValue;

    public void ChangeCol()
    {
        // Pobranie materia³u
        material = obj.GetComponent<Renderer>().material;

        // Ustawienie pocz¹tkowego koloru
        material.color = startColor;

        // Zainicjowanie interpolacji kolorów za pomoc¹ LeanTween
        StartColorTransition(startColor, endColor, duration);
    }

    // Funkcja przejœcia kolorów
    void StartColorTransition(Color fromColor, Color toColor, float duration)
    {
        LeanTween.value(gameObject, UpdateColor, fromColor, toColor, duration);
    }

    // Funkcja aktualizacji koloru w czasie animacji
    void UpdateColor(Color color)
    {
        if (material != null)
        {
            material.color = color;
        }
    }
}