using UnityEngine;

public class Dragao : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 4f;
    private Vector2 movimento;

    [Header("Ataque")]
    private bool playerInAttackRange;
    public int attackDamage = 6;
    public float attackDamageInterval = 1f; // tempo entre danos

    [Header("Vida")]
    public int maxHealth = 150;
    private int currentHealth;
    private bool morreu = false;

    [Header("Referências")]
    public Transform player;
    private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float deathDelay = 2f;  

    [Header("Drop")]
    public GameObject itemDrop;
    [Range(0f, 1f)] public float chanceDrop = 1f;
    [SerializeField] private float dropDelay = 1f;  

    private float attackDamageTimer;
    private PlayerHealth playerHealth;
    private bool playerInRange;
    private Vector2 direcao;
    private float tempoUltimoAtaque;
    private Vector3 posicaoInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicaoInicial = transform.position;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (morreu) return;

        attackDamageTimer += Time.deltaTime;

        if (playerInRange && player != null && playerHealth != null){
            float distance = Vector2.Distance(transform.position, player.position);

            if (playerInAttackRange)
            {
                movimento = Vector2.zero;
                // animator.SetBool("isWalking", false);
                OlharParaPlayer();

                if (attackDamageTimer >= attackDamageInterval)
                {
                    // animator.SetTrigger("attack");
                    player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
                    attackDamageTimer = 0f;
                }

                return;
            }
            else
                PerseguirPlayer();
        }
        else
        {
            VoltarParaOrigem();
        }
    }

    private void FixedUpdate(){   
        Vector2 movimentoFinal = movimento;

        if (movimentoFinal != Vector2.zero)
        {
            if (Mathf.Abs(movimentoFinal.x) > Mathf.Abs(movimentoFinal.y))
                movimentoFinal = new Vector2(Mathf.Sign(movimentoFinal.x), 0f);
            else
                movimentoFinal = new Vector2(0f, Mathf.Sign(movimentoFinal.y));
        }

        rb.MovePosition(rb.position + movimentoFinal * velocidade * Time.fixedDeltaTime);
    }

    private void AtualizarFlip(float deltaX){
        if (playerInAttackRange) return;

        if (deltaX > 0.05f)
            spriteRenderer.flipX = true;
        else if (deltaX < -0.05f)
            spriteRenderer.flipX = false;
    }

    void PerseguirPlayer(){
        Vector2 diferenca = player.position - transform.position;

        if (Mathf.Abs(diferenca.x) > Mathf.Abs(diferenca.y))
        {
            movimento = new Vector2(Mathf.Sign(diferenca.x), 0f);
            AtualizarFlip(movimento.x);
        }
        else
        {
            movimento = new Vector2(0f, Mathf.Sign(diferenca.y));
        }

        // animator.SetBool("isWalking", true);
    }

    void VoltarParaOrigem(){
        float distance = Vector2.Distance(transform.position, posicaoInicial);

        if (distance > 0.05f)
        {
            Vector2 direcao = ((Vector2)posicaoInicial - rb.position).normalized;
            movimento = direcao;

            // animator.SetBool("isWalking", true);
            AtualizarFlip(direcao.x);
        }
        else
        {
            rb.MovePosition((Vector2)posicaoInicial);
            movimento = Vector2.zero;
            // animator.SetBool("isWalking", false);
        }
    }

    void OlharParaPlayer(){
        Vector2 direcaoOlhar = (player.position - transform.position).normalized;
        AtualizarFlip(direcaoOlhar.x);
    }

    public void Morrer(){
        morreu = true;

        // travar movimento
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public void EntrarRangeDeteccao(Collider2D other)
    {
        player = other.transform;
        playerHealth = other.GetComponent<PlayerHealth>();
        playerInRange = true;
        attackDamageTimer = 0f;
    }

    public void SairRangeDeteccao(Collider2D other)
    {
        if (other.transform == player){
            playerInRange = false;
            playerInAttackRange = false;
            attackDamageTimer = 0f;
            movimento = Vector2.zero;
        }
    }

    public void EntrarRangeAtaque()
    {
        playerInAttackRange = true;
    }

    public void SairRangeAtaque()
    {
        playerInAttackRange = false;
    }

    public void TakeDamage(int damage)
    {
        // animator.SetTrigger("hurt");
        currentHealth -= damage;
        Debug.Log("Inimigo levou dano. Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // animator.SetTrigger("die");
        Debug.Log("Inimigo morreu");

        GetComponent<Collider2D>().enabled = false;

        Dragao dragao = GetComponent<Dragao>();
            if (dragao != null)
                dragao.Morrer();

        TentarDrop();
        Destroy(gameObject, deathDelay);
    }

    void TentarDrop()
    {
        if (itemDrop == null) return;

        float chance = Random.value;

        if (chance <= chanceDrop)
        {
            Invoke(nameof(SpawnDrop), dropDelay);
        }
    }
 
    void SpawnDrop()
    {
        Instantiate(itemDrop, transform.position, Quaternion.identity);
    }
}