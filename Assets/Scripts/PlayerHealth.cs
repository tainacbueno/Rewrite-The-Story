using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int health = 10;
    [SerializeField] private float deathDelay = 5f;
    [SerializeField] private Rigidbody2D rb;

    public int currentHealth { get; private set; }
    public int maxHealth { get; private set; }
    public bool morreu = false;
    
    public static Action<int> OnPlayerTakeDamage;
    public static Action OnPlayerDeath;

    void Awake()
    {
        currentHealth = health;
        maxHealth = health;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }


    public void TakeDamage(int damage)
    {
        if (morreu) return;

        animator.SetTrigger("hurt");
        currentHealth -= damage;
        Debug.Log("Player levou dano. Vida atual: " + currentHealth);
        OnPlayerTakeDamage?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            morreu = true;
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("die");
        Debug.Log("Player morreu");

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        StartCoroutine(MorrerERedirecionar());
    }

    private IEnumerator MorrerERedirecionar()
    {
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene("GameOver");
    }
}