using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

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

    private Transform target;
    void Start()
    {
        if (Cannon)
        {
            target = enemy.transform;
            CannonShot();
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
        LeanTween.scale(gameObject, new Vector3(2,2,2), 0.6f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(gameObject, new Vector3(0,0,0), 0.1f).setEase(LeanTweenType.easeInOutSine).setDelay(0.61f);
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
    }

    void CannonShot()
    {
        LeanTween.moveX(gameObject, target.position.x, 0.5f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.moveZ(gameObject, target.position.z, 0.5f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.moveY(gameObject, target.position.y+3, 0.25f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.moveY(gameObject, target.position.y, 0.25f).setDelay(0.25f).setEase(LeanTweenType.easeInOutSine);
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
