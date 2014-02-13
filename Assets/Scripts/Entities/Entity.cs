using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{

    protected Transform entity;

    protected AudioManager audioManager;

    // Use this for initialization
    protected virtual void Start()
    {
        entity = transform;

        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }
}
