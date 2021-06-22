using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss : MonoBehaviour
{
    [SerializeField] private int dir = 0;
    [SerializeField] private Transform[] waterSlicePos = null;
    [SerializeField] private GameObject[] waterSlice = null;
    [SerializeField] private SpriteRenderer[] beamSpriteRenderer = null;
    [SerializeField] private Collider2D[] beamCol = null;
    new private AudioSource audio = null;
    private GameManager gameManager = null;
    private SpriteRenderer spriteRenderer = null;
    //private Animator ani = null;
    private float life = 1;
    private bool isPattern = false;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector2(gameManager.MaxPos.x * -dir, 2.5f);
        //ani = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Slice"))
        {
            if (collision.CompareTag("Bullet"))
                gameManager.Despawn(collision.gameObject);
            life -= collision.CompareTag("Bullet") ? 0.02f : 0.04f;
            gameManager.AddScore(10);
            spriteRenderer.color = new Color(life, life, life, 1);
            //hpBar.color = new Color(life, life, life, 1);
            //hpBar.fillAmount = life;
            if (life <= 0)
            {
                StartCoroutine(Dead());
                return;
            }
        }
    }

    private IEnumerator Dead()
    {
        //ani.Play("Boss_Dead");
        for(float i = 1; i <= 2; i += 0.1f)
        {
            spriteRenderer.color = new Color(0, 0, 0, 2 - i);
            transform.localScale = new Vector3(i , i, 1);
            yield return new WaitForSeconds(0.01f);
        }
        //yield return new WaitForSeconds(0.25f / 2);
        life = 1;
        gameManager.ResettingScore(0);
        spriteRenderer.color = new Color(life, life, life, 1);
        transform.localScale = new Vector3(1, 1, 1);
        //hpBar.color = new Color(life, life, life, 1);
        //hpBar.fillAmount = life;
        gameObject.SetActive(false);
        transform.position = new Vector2(gameManager.MaxPos.x * -dir, 2.5f);
        for (int i = 0; i < 2; i++)
            waterSlice[i].SetActive(false);
        isPattern = false;
    }
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if(dir == 1)
                if (transform.position.x <= gameManager.MinPos.x * 0.5f)
                    transform.Translate(Vector3.right * dir * 0.7f * Time.deltaTime);
                else
                {
                    if (!isPattern)
                    {
                        StartCoroutine(WaterSlicePos());
                        StartCoroutine(RandomSlice());
                        isPattern = true;
                    }
                }

            if (dir == -1)
            {
                if (transform.position.x >= gameManager.MaxPos.x * 0.5f)
                    transform.Translate(Vector3.right * dir * 0.7f * Time.deltaTime);
                else
                {
                    if (!isPattern)
                    {
                        StartCoroutine(WaterSlicePos());
                        StartCoroutine(RandomSlice());
                        isPattern = true;
                    }
                }
            }
        }
    }

    private IEnumerator WaterSlicePos()
    {
        while (true)
        {
            for (int i = -90; i < 90; i++)
            {
                waterSlicePos[0].rotation = Quaternion.Euler(0, 0, -i);
                waterSlicePos[1].rotation = Quaternion.Euler(0, 0, i);
                yield return new WaitForSeconds(0.008f);
            }
            for (int i = 90; i > -90; i--)
            {
                waterSlicePos[0].rotation = Quaternion.Euler(0, 0, -i);
                waterSlicePos[1].rotation = Quaternion.Euler(0, 0, i);
                yield return new WaitForSeconds(0.008f);
            }
        }
    }

    private IEnumerator RandomSlice()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 4f));

            waterSlice[0].SetActive(true);
            waterSlice[1].SetActive(true);
            beamCol[0].enabled = false;
            beamCol[1].enabled = false;
            for(int i = 0; i < 3; i++)
            {
                beamSpriteRenderer[0].color = new Color(1, 1, 1, 0.2f);
                beamSpriteRenderer[1].color = new Color(1, 1, 1, 0.2f);
                yield return new WaitForSeconds(0.15f);
                beamSpriteRenderer[0].color = new Color(1, 1, 1, 0f);
                beamSpriteRenderer[1].color = new Color(1, 1, 1, 0f);
                yield return new WaitForSeconds(0.15f);
            }
            beamCol[0].enabled = true;
            beamCol[1].enabled = true;
            beamSpriteRenderer[0].color = new Color(1, 1, 1, 1f);
            beamSpriteRenderer[1].color = new Color(1, 1, 1, 1f);
            waterSlice[0].SetActive(false);
            waterSlice[1].SetActive(false);
            yield return new WaitForSeconds(0.2f);
            audio.Play();
            waterSlice[0].SetActive(true);
            waterSlice[1].SetActive(true);
            yield return new WaitForSeconds(1f);
            audio.Pause();
            waterSlice[0].SetActive(false);
            waterSlice[1].SetActive(false);
        }
    }
}
