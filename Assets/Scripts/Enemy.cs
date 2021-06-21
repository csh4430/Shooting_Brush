using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bulletPref = null;
    [SerializeField] private Transform[] bulletPos = null;
    [SerializeField] private AnimationClip[] animationClip = null;
    [SerializeField] private float speed = 0;
    public int hp { get; private set; }
    [SerializeField] private float score = 0;
    private GameManager gameManager = null;
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;
    private Collider2D col = null;
    new private AudioSource audio = null;
    private IEnumerator fire = null;
    private bool isDead = false;
    [SerializeField] private int mode = 0;
    void Start()
    {
        Turn();
    }

    public void Turn()
    {
        audio = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        fire = Fire();
        col.enabled = true;
        isDead = false;
        //mode = count;
        //speed = velocity;
        //score = num;
        animator.Play("Enemy_Idle");
        if (mode == 1)
        {
            StartCoroutine(fire);
            hp = 4;
        }
        else
            hp = 3;
    }
    int i = 0;
    void Update()
    {
        if (!isDead)
        {
            if(mode == 0)
                transform.Translate(Vector2.down * speed * Time.deltaTime);
            if(mode == 1)
                transform.Translate(Vector2.left * speed * Time.deltaTime);
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
        StopCoroutine(fire);
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            for (int i = 0; i < 22; i++)
            {
                gameManager.Pooling(bulletPref, bulletPos[0].position, bulletPos[0].rotation);
                bulletPos[0].Rotate(Vector3.forward * 360 / 22);
            }
            yield return new WaitForSeconds(0.7f);
            for (int i = 0; i < 12; i++)
            {
                gameManager.Pooling(bulletPref, bulletPos[0].position, bulletPos[0].rotation);
                bulletPos[0].Rotate(Vector3.forward * 360 / 12);
            }
            yield return new WaitForSeconds(1.2f);
        }
    }
}
