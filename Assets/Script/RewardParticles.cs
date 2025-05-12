using System.Collections;
using UnityEngine;

public class RewardParticles : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
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
    }
}