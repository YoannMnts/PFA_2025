using System.Collections;
using UnityEngine;

public class RewardParticles : SoundObject
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    public override void Start()
    {
        base.Start();
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    public void Reward(int count)
    {
        StartCoroutine(EmmitParticles(count));
    }
    IEnumerator EmmitParticles(int number)
    {
        ParticleSystem.Burst burst = new ParticleSystem.Burst();
        burst.count = number;
        ps.emission.SetBurst(0,burst);
        yield return new WaitForSeconds(1f);
        ps.Play();
        yield return new WaitForSeconds(0.2f);
        int currentPCount = number;
        while (ps.isPlaying)
        {
            if ( ps.particleCount<currentPCount )
            {
                int nb = currentPCount- ps.particleCount;
                currentPCount = ps.particleCount;
                for (int i = 0; i < nb; i++)
                {
                    Debug.Log("sqq");
                    PlaySound(clips[0], SoundType.Effects);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            yield return null;
        }
    }
}