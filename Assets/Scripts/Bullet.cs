using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] protected float speed = 0;
    protected GameManager gameManager = null;
    protected Animator animator = null;

    protected virtual void OnEnable()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (transform.position.x > gameManager.MaxPos.x + 0.1f || transform.position.x < gameManager.MinPos.x - 0.1f)
            gameManager.Despawn(gameObject);
        if (transform.position.y > gameManager.MaxPos.y + 0.1f || transform.position.y < gameManager.MinPos.y - 0.1f)
            gameManager.Despawn(gameObject);
    }
}
