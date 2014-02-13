using UnityEngine;
using System.Collections;

public class Sanitizer : Collectible
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Collect()
    {
        audioManager.PlaySFX("Collect");

        base.Collect();
    }
}
