using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySky : EnemyRiver
{

    [SerializeField] private GameObject bulletPref = null;
    [SerializeField] private Transform[] bulletPos = null;
    private IEnumerator fire = null;
    protected override void OnEnable()
    {
        base.OnEnable();
        fire = Fire();
        StartCoroutine(fire);

    }

    protected override void Update()
    {
        if (!isDead)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        if (transform.position.x > gameManager.MaxPos.x + 0.1f || transform.position.x < gameManager.MinPos.x - 0.1f)
            gameManager.Despawn(gameObject);
        if (transform.position.y > gameManager.MaxPos.y + 0.1f || transform.position.y < gameManager.MinPos.y - 0.1f)
            gameManager.Despawn(gameObject);
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
