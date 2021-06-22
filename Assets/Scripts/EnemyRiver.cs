using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRiver: MonoBehaviour
{
    [SerializeField] protected AnimationClip[] animationClip = null;
    [SerializeField] protected float speed = 0;
    public int hp { get; protected set; }
    [SerializeField] protected float score = 0;
    protected GameManager gameManager = null;
    protected SpriteRenderer spriteRenderer = null;
    protected Animator animator = null;
    protected Collider2D col = null;
    new protected AudioSource audio = null;
    protected bool isDead = false;

    protected virtual void OnEnable()
    {
        audio = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        col.enabled = true;
        isDead = false;
        animator.Play("Enemy_Idle");
        if (gameObject.CompareTag("River")) hp = 3;
        else hp = 4;
        
    }
    protected virtual void Update()
    {
        if (!isDead)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        if (transform.position.x > gameManager.MaxPos.x + 0.1f || transform.position.x < gameManager.MinPos.x - 0.1f)
            gameManager.Despawn(gameObject);
        if (transform.position.y > gameManager.MaxPos.y + 0.1f || transform.position.y < gameManager.MinPos.y - 0.1f)
            gameManager.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.CompareTag("Bullet") || collision.CompareTag("Slice"))
        {
            if(collision.CompareTag("Bullet"))
                gameManager.Despawn(collision.gameObject);
            //audio.Play();
            hp -= collision.CompareTag("Bullet") ? 1 : 2;
            audio.Play();
            if(hp <= 0)
            {
                hp = 0;
                StartCoroutine(Dead());
            }
            animator.Play(animationClip[hp].name);
        }
    }

    public IEnumerator Dead()
    {
        col.enabled = false;
        isDead = true;
        gameManager.AddScore(score);
        //animator.Play("Enemy_Dead");
        for (float i = 1; i <= 2; i += 0.1f)
        {
            spriteRenderer.color = new Color(0, 0, 0, 2 - i);
            transform.localScale = new Vector3(i, i, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        transform.localScale = Vector3.one;
        gameManager.Despawn(gameObject);
    }
}
