using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;
    [SerializeField]private GameObject placeParticlepreafab;
    private GameObject placeParticle;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void PlayPlaceParticle(Vector3 position)
    {
        if(placeParticle == null) placeParticle = Instantiate(placeParticlepreafab);
        placeParticle.transform.position = position;
        placeParticle.GetComponent<ParticleSystem>().Play();
    }
}
