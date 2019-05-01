using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Jacob Eddy
 * Created: 4/30
 * Last modified 5/1/2018 
 */
public class TargetEnemy : MonoBehaviour {

     GameObject closest;
     Transform enemyLocation;
    public LineRenderer shootingLine;
     ShootEnemy ShootScript;
    
    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    Quaternion baseRotation;

    // Use this for initialization
    void Start () {
        ShootScript = this.GetComponent<ShootEnemy>();
        baseRotation = this.transform.localRotation;
        InvokeRepeating("FindClosestEnemy", 1.0f, 2f);
    }
	
	// Update is called once per frame
	void Update () {

        if (closest != null && closest.tag == "Enemy")
        {
            enemyLocation = closest.transform;
            float dist = Vector3.Distance(this.transform.position, enemyLocation.position);
            if (dist < ShootScript.range)
            {


                transform.LookAt(enemyLocation);
                Vector3 shootAt = transform.InverseTransformPoint(enemyLocation.position);
                shootingLine.SetPosition(1, shootAt);



                // If it's time to fire...
                if (ShootScript.timer >= ShootScript.effectiveTimer)
                {
                    // ... shoot the gun.
                    ShootScript.Shoot(closest);
                   // Debug.Log("shot!");
                }

                // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
                if (ShootScript.timer >= ShootScript.effectiveTimer * ShootScript.effectsDisplayTime)
                {
                    // ... disable the effects.
                    ShootScript.DisableEffects();
                }

            }
        }
        else
        {
            this.transform.localRotation = baseRotation;
            shootingLine.SetPosition(1, shootingLine.GetPosition(0));

        }

        

    }

    void FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest1 = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest1 = go;
                distance = curDistance;
            }
        }
        closest = closest1;
    }

}
