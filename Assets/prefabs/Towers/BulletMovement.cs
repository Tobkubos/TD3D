using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BulletMovement : MonoBehaviour
{ 
    [SerializeField] float BulletSpeed;
    public float damage;
    public int Elementaldamage;
    public float DamageOverTime;
    public string Type;
	public Transform enemy;
	public TowerStats ts;
	private Vector3 LastDir;
    private Vector3 direction;
    public bool isFollowing;
    public bool Cannon;

    private Collider[] enemies;

    private Transform target;
    public Material invisibleMat;

    public ParticleSystem boom;
    void Start()
    {
        if (Cannon)
        {
            target = enemy.transform;
            GetComponent<AudioSource>().Play();
            StartCoroutine(CannonShot());
        }
        else
        {
            direction = (enemy.position - transform.position).normalized;
            direction.y = ts.gameObject.transform.position.y;
        }

        if (!Cannon)
        {
            Destroy(gameObject, 1);
        }
     }
    public IEnumerator CannonDamage()
    {
        LeanTween.scale(gameObject, new Vector3(2,2,2), 0.3f).setEase(LeanTweenType.easeOutSine);
        LeanTween.scale(gameObject, new Vector3(0,0,0), 0.1f).setEase(LeanTweenType.easeInOutSine).setDelay(0.31f);

        enemies = null;
        enemies = Physics.OverlapBox(this.transform.position, new Vector3(1f, 1, 1f), Quaternion.identity);

        foreach (Collider c in enemies)
        {
            if (c.CompareTag("enemy"))
            {
                float offset = Random.Range(damage * 0.5f, damage * 1.5f) - damage;
                c.GetComponent<EnemyInfo>().DealDamage(damage + offset,ts);
            }
            
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(boom.gameObject, 2);
        Destroy(gameObject);
    }

    private IEnumerator CannonShot()
    {
        float time = Vector3.Distance(transform.position, target.position);
        float t1 = time / 4;
        float t2 = time / 8;
        LeanTween.moveX(gameObject, target.position.x, t1).setEase(LeanTweenType.easeInOutSine);
        LeanTween.moveZ(gameObject, target.position.z, t1).setEase(LeanTweenType.easeInOutSine);
        LeanTween.moveY(gameObject, target.position.y + time/2, t2).setEase(LeanTweenType.easeInOutSine);
        LeanTween.moveY(gameObject, target.position.y, t2).setDelay(t2).setEase(LeanTweenType.easeInOutSine);
        yield return new WaitForSeconds(t1);
        gameObject.transform.rotation = Quaternion.identity;
        GetComponent<MeshRenderer>().material = invisibleMat;
        boom.Play();
        boom.transform.parent = null;
        StartCoroutine(CannonDamage());
    }


    void FixedUpdate()
    {
        if (isFollowing && !Cannon)
        {
            if (enemy != null)
            {
                direction = (enemy.position - transform.position).normalized;

                transform.position += direction * BulletSpeed * Time.deltaTime;
                //transform.position = Vector3.Lerp(transform.position, transform.position + direction * BulletSpeed * Time.deltaTime, 0.5f);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                LastDir = direction;
            }
            else
            {
                transform.position += LastDir * BulletSpeed * Time.deltaTime;
            }
        }
    }
}
