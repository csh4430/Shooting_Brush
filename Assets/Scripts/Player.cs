using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject[] bulletPref = null;
    [SerializeField] private Transform bulletPos = null;
    [SerializeField] private Text scoreText = null;
    [SerializeField] private float speed = 0;
    [SerializeField] private RuntimeAnimatorController[] aniCon = null;
    [SerializeField] private GameObject boss = null;
    [SerializeField] private AudioClip[] playerClips = null;
    private Animator animator = null;
    private GameManager gameManager = null;
    private SpriteRenderer spriteRenderer = null;
    new private AudioSource audio = null;
    private IEnumerator fire = null;
    private bool isDead = false;
    private bool isSlicing = false;
    private bool isFiring = false;
    private bool isTurning = false;
    private float lastPressed = 0;
    private int mode = 0;
    private Vector2 mousePos = Vector2.zero;
    private Vector2 targetPos = Vector2.zero;
    private Vector2 dis = Vector2.zero;
    private Vector3 len = Vector2.zero;

    void Start()
    {
        dis = new Vector2(0, -4);
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();

        fire = Fire();
        StartCoroutine(fire);
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (boss.activeInHierarchy)
        {
            len = boss.transform.position - transform.position;
            float z = Mathf.Atan2(len.y, len.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, z - 90);
        }

        if(!gameManager.isMenu)
        if (DoubleClicked() && !isSlicing && !isTurning)
        {
            StartCoroutine(Slash());
        }

        if (Input.GetMouseButton(1))
        {
            if (!isTurning && !boss.activeInHierarchy)
            {
                StartCoroutine(Turn());
            }
        }
    }
    private void Move()
    {
        if(Input.GetMouseButton(0))
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition/*Input.GetTouch(0).position*/);

        if (/*Input.GetTouch(0).phase == TouchPhase.Began*/ Input.GetMouseButtonDown(0))
        {
            dis = (Vector2)transform.position - (mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition/*Input.GetTouch(0).position*/));
        }

        //if(Input.GetTouch(0).phase == TouchPhase.Ended)
        //{
        //    dis = (Vector2)transform.position - (mousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position));
        //}

        targetPos = mousePos + dis;
        targetPos.x = Mathf.Clamp(targetPos.x, gameManager.MinPos.x, gameManager.MaxPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, gameManager.MinPos.y, gameManager.MaxPos.y);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
    }
    private IEnumerator Turn()
    {
        StopCoroutine(fire);
        isFiring = false;
        isTurning = true;
        for(int i = 0; i < 36; i++)
        {
            transform.Rotate(Vector3.forward * 10);
            yield return new WaitForSeconds(0.01f);
        }
        isTurning = false;
        if (Input.GetMouseButton(1)) yield break;
        if (!isFiring)
            StartCoroutine(fire);
        isFiring = true;
    }
    private bool DoubleClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float com = Time.time - lastPressed;

            lastPressed = Time.time;

            if (com <= 0.2f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    private IEnumerator Fire()
    {
        isFiring = true;
        while (true)
        {
            animator.Play("Fire");
            if (!isSlicing || !isDead)
            {
                if (mode == 0)
                {
                    yield return new WaitForSeconds(0.42f);

                    animator.Play("Idle");

                    for (int i = 0; i < 3; i++)
                    {

                        audio.clip = playerClips[0];
                        audio.Play();
                        gameManager.Pooling(bulletPref[0], bulletPos.position, bulletPos.rotation);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else if (mode == 1)
                {
                    audio.clip = playerClips[0];
                    audio.Play();
                    gameManager.Pooling(bulletPref[0], bulletPos.position, bulletPos.rotation);
                    yield return new WaitForSeconds(0.15f);
                    animator.Play("Idle");
                }
            }
        }
    }

    private IEnumerator Slash()
    {
        if (isDead)
            yield break;
        isSlicing = true;
        StopCoroutine(fire);
        isFiring = false;
        audio.clip = playerClips[1];
        audio.Play();
        GameObject aa = gameManager.Pooling(bulletPref[1], bulletPos.position, bulletPos.rotation);
        aa.GetComponent<Animator>().Play("Drawing");
        animator.Play("Slice");
        yield return new WaitForSeconds(mode == 0 ? 0.7f : 0.3f);
        animator.Play("Idle");
        if(!isFiring)
            StartCoroutine(fire);
        isSlicing = false;
        isFiring = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            StartCoroutine(UseItem(collision.tag));
            gameManager.Despawn(collision.gameObject);
            return;
        }
        StartCoroutine(Damaged(collision, 0));
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (isDead) return;
        StartCoroutine(Damaged(collision, 0.5f));
    }
    private IEnumerator Dead()
    {
        isDead = true;
        isFiring = false;
        animator.Play("Dead");
        audio.clip = playerClips[2];
        audio.Play();
        StopCoroutine(fire);
        scoreText.color = new Color(0.7f, 0, 0, 1);
        for(int i = 0; i < 5; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
        scoreText.color = new Color(0, 0, 0, 1);
        if (!isFiring)
            StartCoroutine(fire);
        isDead = false;
        isFiring = true;
    }

    private IEnumerator UseItem(string item)
    {
        if(item == "InkStone")
        {
            mode = 1;
            animator.runtimeAnimatorController = aniCon[1];
            speed = 2.5f;
            yield return new WaitForSeconds(5f);
            mode = 0;
            speed = 100;
            animator.runtimeAnimatorController = aniCon[0];
            dis = (Vector2)transform.position - mousePos;

        }
    }

    private IEnumerator Damaged(Collider2D collision, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (collision.CompareTag("Bullet"))
            gameManager.Despawn(collision.gameObject);
        if (isDead) yield break;
        gameManager.Dead();
        StartCoroutine(Dead());
    }
}
