using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthHUD : MonoBehaviour
{
    [SerializeField] private Health target;
    [SerializeField] private Slider bar;
    [SerializeField] private TMP_Text label;

    [System.Obsolete]
    void Awake() {
        if (!target) target = FindObjectOfType<Health>();
        if (!target) return;
        target.OnHealthChanged += OnHealthChanged;
        OnHealthChanged(target.CurrentHealth, target.MaxHealth); // draw once on start
    }

    void OnDestroy() {
        if (target) target.OnHealthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(float current, float max) {
        if (bar) { bar.maxValue = max; bar.value = current; }
        if (label) label.text = $"{current:0}/{max:0}";
    }
}