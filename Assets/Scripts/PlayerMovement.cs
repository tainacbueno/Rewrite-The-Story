using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer map;
    [SerializeField] private float padding = 0.2f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 lastDirection = Vector2.up;
    private bool facingLeft = true;
    public float xPosLastFrame;

    [Header("Vida")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Ataque")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    public float attackCooldown = 0.5f; // segundos
    private float lastAttackTime = -999f;

    [Header("Coleta")]
    public int tinta = 0;
    public static Action<int> OnTintaChanged;

    [Header("Leitores")]
    public int leitores = 0;
    public static Action<int> OnLeitoresChanged;

    [Header("Quebra Roteiro")]
    public int custoQuebraRoteiro = 10;

    [Header("Porta Secreta")]
    [SerializeField] private GameObject portaSecreta;
    [SerializeField] private string cenaPorta = "Final";

    void Awake(){
        animator.SetFloat("lastX", 0);
        animator.SetFloat("lastY", 1); // UP
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", 0);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        string cenaAtual =
            SceneManager.GetActiveScene().name;

        // =========================
        // VILAREJO = RESET
        // =========================

        if (cenaAtual == "Vilarejo")
        {
            // vida máxima
            currentHealth = maxHealth;

            // zera tinta
            tinta = 0;

            // salva
            PlayerPrefs.SetInt(
                "PlayerHealth",
                currentHealth
            );

            PlayerPrefs.SetInt(
                "PlayerTinta",
                tinta
            );
        }
        else
        {
            // =========================
            // CARREGAR VIDA E TINTA
            // =========================

            currentHealth = PlayerPrefs.GetInt(
                "PlayerHealth",
                maxHealth
            );

            tinta = PlayerPrefs.GetInt(
                "PlayerTinta",
                0
            );
        }

        // =========================
        // CARREGAR LEITORES
        // =========================

        leitores = PlayerPrefs.GetInt(
            "PlayerLeitores",
            0
        );

        OnTintaChanged?.Invoke(tinta);
        OnLeitoresChanged?.Invoke(leitores);
        VerificarPortaSecreta();
    }

    void Update()
    {
        HandleMovement();
        ClampMovement();

        if ((input.x < 0 && !facingLeft) || (input.x > 0 && facingLeft))
            FlipCharacterX();

        VerificarQuebraRoteiro();
        VerificarPortaSecreta();
    }

    void FixedUpdate()
    {        
        rb.linearVelocity = input * speed;
    }

    // =========================
    // MOVIMENTO
    // =========================

    private void HandleMovement()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input = input.normalized;

        bool isWalking = input != Vector2.zero;

        animator.SetBool("isWalking", isWalking);
        animator.SetFloat("moveX", input.x);
        animator.SetFloat("moveY", input.y);

        if (isWalking){
            lastDirection = input;

            animator.SetFloat("lastX", lastDirection.x);
            animator.SetFloat("lastY", lastDirection.y);

            UpdateAttackPoint();
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown){
            lastAttackTime = Time.time;
            
            animator.SetTrigger("attack");
            Attack();
        }
    }

    private void FlipCharacterX(){
        float input = Input.GetAxis("Horizontal");

        if (input > 0 && transform.position.x > xPosLastFrame){
            // We are moving right
            spriteRenderer.flipX = false;
        }
        else if (input < 0 && transform.position.x < xPosLastFrame){
            // We are moving left
            spriteRenderer.flipX = true;
        }

        xPosLastFrame = transform.position.x;
    } 

    private void ClampMovement()
    {
        if (map == null) return;

        Bounds bounds = map.bounds;

        float minX = bounds.min.x + padding;
        float maxX = bounds.max.x - padding;

        float minY = bounds.min.y + padding;
        float maxY = bounds.max.y - padding;

        float x = Mathf.Clamp(transform.position.x, minX, maxX);
        float y = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector2(x, y);
    }

    // =========================
    // ATAQUE
    // =========================

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Lobo>()?.TakeDamage(attackDamage);
            enemy.GetComponent<Ogro>()?.TakeDamage(attackDamage);
            enemy.GetComponent<Dragao>()?.TakeDamage(attackDamage);
        }
    }

    private void UpdateAttackPoint()
    {
        float distance = 0.5f;
        attackPoint.localPosition = lastDirection * distance;
    }

    // =========================
    // COLETA (TintaNarrativa)
    // =========================

    public void Coletar(int valor)
    {
        tinta += valor;
        PlayerPrefs.SetInt("PlayerTinta", tinta);
        OnTintaChanged?.Invoke(tinta);
        Debug.Log("Tinta: " + tinta);
    }

    
    public void RemoveTinta(int valor)
    {
        tinta -= valor;
        if (tinta < 0) tinta = 0;

        OnTintaChanged?.Invoke(tinta);
    }

    // =========================
    // PORTA SECRETA
    // =========================

    private void VerificarPortaSecreta()
    {
        string cenaAtual =
            SceneManager.GetActiveScene().name;

        if (
            cenaAtual == "Vilarejo" &&
            leitores >= 10000 &&
            portaSecreta != null
        )
        {
            portaSecreta.SetActive(true);
        }
    }

    // =========================
    // QUEBRA ROTEIRO
    // =========================

    private void VerificarQuebraRoteiro()
    {
        // apertou P
        if (Input.GetKeyDown(KeyCode.P))
        {
            string cenaAtual =
                SceneManager.GetActiveScene().name;

            string proximaCena = "";

            // =========================
            // QUAL CENA VAI ABRIR
            // =========================

            if (cenaAtual == "Floresta")
            {
                proximaCena = "Caverna1";
            }
            else if (cenaAtual == "Castelo")
            {
                proximaCena = "Caverna2";
            }
            else if (cenaAtual == "Dragao (Batalha)")
            {
                proximaCena = "Caverna3";
            }

            // se não houver destino
            if (proximaCena == "")
                return;

            // =========================
            // VERIFICA TINTA
            // =========================

            if (tinta >= custoQuebraRoteiro)
            {
                // desconta tinta
                tinta -= custoQuebraRoteiro;
                PlayerPrefs.SetInt("PlayerTinta", tinta);
                OnTintaChanged?.Invoke(tinta);

                // ganha leitores
                leitores += 1000;
                PlayerPrefs.SetInt("PlayerLeitores", leitores);
                OnLeitoresChanged?.Invoke(leitores);

                Debug.Log(
                    "Quebra Roteiro ativada!"
                );

                // troca cena
                SceneManager.LoadScene(
                    proximaCena
                );
            }
            else
            {
                Debug.Log(
                    "Tinta Narrativa insuficiente!"
                );
            }
        }
    }

    // =========================
    // DEBUG
    // =========================

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    
    [ContextMenu("Dar 10000 de Tinta")]
    private void DebugDarTinta()
    {
        tinta = 10000;
        PlayerPrefs.SetInt("PlayerTinta", tinta);
        OnTintaChanged?.Invoke(tinta);
    }

}