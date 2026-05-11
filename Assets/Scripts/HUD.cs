using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private Slider healthbar;
    [SerializeField] private TextMeshProUGUI tintacounter;
    [SerializeField] private TextMeshProUGUI leitorescounter;
    [SerializeField] private GameObject icon;
    [SerializeField] private int custoQuebraRoteiro = 10;

    private void Start()
    {
        PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();

        healthbar.minValue = 0;
        healthbar.maxValue = playerHealth.maxHealth;
        healthbar.value = playerHealth.currentHealth;

        icon.SetActive(false);
    }

    private void Update()
    {        
        if (icon.activeSelf)
        {
            float scale = 1f + Mathf.Sin(Time.time * 5f) * 0.05f;
            icon.transform.localScale = Vector3.one * scale;
        }
    }

    private void UpdateHealthbar(int currentHealth)
    {
        healthbar.value = currentHealth;
    }

    private void UpdateTinta(int value)
    {
        tintacounter.text = value.ToString();
    }

    private void UpdateLeitores(int value)
    {
        leitorescounter.text = value.ToString();
    }
    
    private void UpdateIcon(int tintaAtual)
    {
        icon.SetActive(tintaAtual >= custoQuebraRoteiro);
    }


    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeDamage += UpdateHealthbar;
        Player.OnTintaChanged += UpdateTinta;
        Player.OnTintaChanged += UpdateIcon;
        Player.OnLeitoresChanged += UpdateLeitores;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeDamage -= UpdateHealthbar;
        Player.OnTintaChanged -= UpdateTinta;
        Player.OnTintaChanged -= UpdateIcon;
        Player.OnLeitoresChanged -= UpdateLeitores;
    }
}
