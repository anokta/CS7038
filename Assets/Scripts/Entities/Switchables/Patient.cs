using UnityEngine;
using System.Collections;

public class Patient : Switchable
{
    private bool treated;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        treated = false;
    }

    public bool IsTreated()
    {
        return treated;
    }

    public override void Switch()
    {
        if (!treated)
        {
            audioManager.PlaySFX("Treated");
        }

        treated = true;
    }
}
