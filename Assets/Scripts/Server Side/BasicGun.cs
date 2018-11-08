using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : Gun {

    [SerializeField]
    Transform nozzleTransform;

    [SerializeField]
    GameObject bulletPrefab;

    // seconds between bullets
    float fireRate = 0.3f;
    float sSinceFired = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (sSinceFired < fireRate)
            sSinceFired += Time.deltaTime;
	}

    public override void Fire()
    {
		if (sSinceFired >= fireRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, nozzleTransform.position, nozzleTransform.rotation);
            sSinceFired = 0f;
        }


    }
}
