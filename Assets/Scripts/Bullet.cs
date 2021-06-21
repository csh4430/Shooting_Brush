using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 0;
    private GameManager gameManager = null;
    private Animator animator = null;

    void Start()
    {
        Turn();
    }
    private void Turn()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }
    public IEnumerator SetSpeed(int velocity)
    {
        Turn();
        speed = 2.5f;
        yield return new WaitForSeconds(0.2f);
        speed = velocity;
        animator.Play("SliceIdle");
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
