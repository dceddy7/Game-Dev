using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public GameObject bullet;
    public float cooldownTime = 0.5F;
    public Transform shootPoint;

    public bool canShoot = true;

    public IEnumerator Fire()
    {
        GameObject bulletClone = Instantiate(bullet, shootPoint.position, transform.rotation);
        Rigidbody2D rigid = bulletClone.GetComponent<Rigidbody2D>();
        rigid.velocity = this.transform.right * bulletSpeed;
        //Debug.Log("help have reached first");
        canShoot = false;
        //wait for some time
        yield return new WaitForSeconds(cooldownTime);
        //Debug.Log("have reached second");
        canShoot = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canShoot)
        {
            StartCoroutine(Fire());
        } else
        {
            //Debug.Log("help "  + canShoot);
        }
    }

}
