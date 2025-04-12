using UnityEngine;

public class FirePitAndPot : CookInterationObj
{
    public ParticleSystem FireParticle;

    private void Awake()
    {
        InteractDescription = "Fry";
    }

    private new void Start()
    {
        base.Start();

        //FireParticle.Stop();
    }

    void Update()
    {
        
    }
}
