using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform[] bulletPos = null;
    [SerializeField] private Transform[] RandomBulletPos = null;
    [SerializeField] private GameObject[] bulletPref = null;
    [SerializeField] private Image hpBar = null;
    [SerializeField] private Canvas status = null;
    [SerializeField] private Transform playerPos = null;
    private float life = 1;
    private SpriteRenderer spriteRenderer = null;
    private GameManager gameManager = null;
    private int dir = 1;
    private bool isPattern = false;
    private IEnumerator spinBigBullet = null;
    private IEnumerator fire = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        spinBigBullet = SpinBigBullet();
        fire = Fire();
    }

    private void Update()
    {
        if(gameObject.activeInHierarchy)

        if(transform.position.y >= 2)
            transform.Translate(Vector3.down * 0.7f * Time.deltaTime);
        else
        {
            if (!isPattern)
                {
                    StartCoroutine(Pattern());
                    status.enabled = true;
                    isPattern = true;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.CompareTag("Slice"))
        {
            if(collision.CompareTag("Bullet"))
                gameManager.Despawn(collision.gameObject);
            life -= collision.CompareTag("Bullet") ? 0.005f : 0.01f;
            gameManager.AddScore(10);
            spriteRenderer.color = new Color(life, life, life, 1);
            hpBar.color = new Color(life, life, life, 1);
            hpBar.fillAmount = life;
            if (life <= 0)
                StartCoroutine(Dead());
        }
    }

    private IEnumerator BulletDir()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            dir *= -1;
        }
    }

    private IEnumerator Dead()
    {
        for (float i = 1; i <= 2; i += 0.1f)
        {
            spriteRenderer.color = new Color(0, 0, 0, 2 - i);
            transform.localScale = new Vector3(i, i, 1);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.05f);
        //gameManager.isBossSpawned = false;
        gameManager.ResettingScore(1);
        life = 1;
        isPattern = false;
        spriteRenderer.color = new Color(life, life, life, 1);
        transform.localScale = Vector3.one;
        hpBar.color = new Color(life, life, life, 1);
        hpBar.fillAmount = life;
        gameObject.SetActive(false);
        transform.position = new Vector2(0,5);

        playerPos.rotation = Quaternion.Euler(0, 0, 0);
    }

    private IEnumerator Pattern()
    {
        StartCoroutine(RandomBigBullet());
        yield return new WaitForSeconds(4);
        StartCoroutine(spinBigBullet);
        yield return new WaitForSeconds(6);
        StopCoroutine(spinBigBullet);
        yield return new WaitForSeconds(1);
        StartCoroutine(RandomBigBullet());
        yield return new WaitForSeconds(4);
        StartCoroutine(spinBigBullet);
        StartCoroutine(BulletDir());
        yield return new WaitForSeconds(11);
        StartCoroutine(fire);
    }

    private IEnumerator SpinBigBullet()
    {

        while (true)
        {
            for (int i = 0; i < 25; i++)
            {
                gameManager.Pooling(bulletPref[1], bulletPos[2].position + new Vector3(0, 0, 1), bulletPos[2].rotation);
                bulletPos[2].Rotate(Vector3.forward * 14.4f);
            }
            bulletPos[3].Rotate(Vector3.forward * 8 * dir);
            yield return new WaitForSeconds(0.30f);
        }
    }
    private IEnumerator RandomBigBullet()
    {
        int pos;
        for(int i = 0; i < 8; i++)
        {
            pos = Random.Range(0, 4);
            for(int j = 0; j < 20; j++)
            {
                gameManager.Pooling(bulletPref[1], RandomBulletPos[pos].position + new Vector3(0, 0, 1), RandomBulletPos[pos].rotation);
                RandomBulletPos[pos].Rotate(Vector3.forward * 18);
            }
            yield return new WaitForSeconds(0.7f);
        }
    }
    private IEnumerator Fire()
    {
        while (true)
        {
            for (int i = 0; i < gameManager.bossCount * 3; i++)
            {
                gameManager.Pooling(bulletPref[0], bulletPos[0].position + new Vector3(0, 0, 1), bulletPos[0].rotation);
                bulletPos[0].Rotate(Vector3.forward * 360 / (gameManager.bossCount * 3));
            }
            bulletPos[1].Rotate(Vector3.forward * 25);
            yield return new WaitForSeconds(0.1f);
        }
    }

    //private GameObject InstantiateOrPool(int count , int pos)
    //{
    //    GameObject result = null;
    //    int i = 0;
    //    if (gameManager.poolingManager.transform.childCount > 0)
    //    {
    //        while (true)
    //        {
    //            result = gameManager.poolingManager.transform.GetChild(i).gameObject;
    //            i++;
    //            if (result.layer == bulletPref[count].layer)
    //                if (result.CompareTag(bulletPref[count].tag))
    //                    break;
    //            if (i >= gameManager.poolingManager.transform.childCount)
    //            {
    //                result = Instantiate(bulletPref[count], bulletPos[pos]);
    //                break;
    //            }
    //        }
    //        result.transform.position = bulletPos[pos].position + Vector3.forward;
    //        result.transform.rotation = bulletPos[pos].rotation;
    //        result.transform.SetParent(null);
    //        result.SetActive(true);
    //    }
    //    else
    //    {
    //        result = Instantiate(bulletPref[count], bulletPos[pos].position + Vector3.forward, bulletPos[pos].rotation);
    //        result.transform.position = bulletPos[pos].position + Vector3.forward;
    //        result.transform.SetParent(null);
    //    }
    //    return result;
    //}
}
