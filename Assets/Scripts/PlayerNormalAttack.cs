using BayatGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttack : MonoBehaviour
{
    public Player player;
    [SerializeField]
    protected LayerMask enemyLayer;

    [Header("Parameters")]
    [SerializeField]
    protected float hitForce = 200f;
    public bool attacking = false;
    public float releaseDamageTime;
    public float castTime;
    private float releaseDamageTimeEnd;
    private float castTimeEnd;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            attacking = true;
            releaseDamageTimeEnd = Time.time + releaseDamageTime;
            castTimeEnd = Time.time + castTime;
            player.animator.SetTrigger("Attack");
            player.animator.SetFloat("AttackId", (float)Random.Range(0, 3));
        }
        if(attacking && Time.time > releaseDamageTimeEnd)
        {
            ApplyDemage();
            releaseDamageTimeEnd = Mathf.Infinity;
        }
        if(attacking && Time.time > castTimeEnd)
        {
            attacking = false;
        }
    }

    public void ApplyDemage()
    {
        RaycastHit2D[] hitInfos = Physics2D.RaycastAll(this.transform.position, Vector2.right * this.transform.localScale, 3f, enemyLayer);
        for (int i = 0; i < hitInfos.Length; i++)
        {
            Health health = hitInfos[i].collider.GetComponent<Health>();
            if (health != null)
            {
                hitInfos[i].rigidbody.AddForce(new Vector2(this.transform.localScale.x * hitForce, 0));
                health.TakeDamage(50);
                health.ApplyEffects(hitInfos[i].point);
            }
        }


    }
}
