using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashBullet : Bullet
{
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(SetSpeed(10));
    }
    public IEnumerator SetSpeed(int velocity)
    {
        speed = 2.5f;
        yield return new WaitForSeconds(0.2f);
        speed = velocity;
        animator.Play("SliceIdle");
    }
}
