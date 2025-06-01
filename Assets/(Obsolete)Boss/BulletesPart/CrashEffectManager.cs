using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashEffectManager : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> effects;
    List<EffectPool> effectsPools = new List<EffectPool>();
    [SerializeField] float explodeScale = 0.2f;
    [SerializeField] float normalOffset = 0.7f;
    public static CrashEffectManager Instance { get ; private set; }
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    ParticleSystem particle = Instantiate(effects);
        //    particle.gameObject.SetActive(false);
        //    pool.Enqueue(particle);
        //}
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    private ParticleSystem InstantiateParticleEffects(Vector2 position,string fxName)
    {
        ParticleSystem fx;
        EffectPool pool = effectsPools.Find(x => x.PoolName == fxName);
        if (pool is null)
        {
            pool = new EffectPool();
            pool.PoolName = fxName;
            pool.Pool.Enqueue(Instantiate(effects.Find(x => x.name == fxName)));
            effectsPools.Add(pool);
        }else if (pool.Pool.Count==0)
        {
            pool.Pool.Enqueue(Instantiate(effects.Find(x => x.name == fxName)));
        }
        fx = pool.Pool.Dequeue();

        fx.transform.position = position;

        return fx;
    }

    public async void PlayCollideByParticleEffect(Vector2 point, Vector2 normal, float normalPulse,string fxname)
    {
        ParticleSystem particles = InstantiateParticleEffects(point, fxname);
        particles.gameObject.SetActive(true);
        particles.transform.position = point + normalOffset * normal;
        particles.transform.up = normal;
        ModifyScale(particles,normalPulse);
        particles.Play();
        await System.Threading.Tasks.Task.Delay((int)(particles.main.startLifetime.constant*1000));
        ReturnInPool(particles,fxname);
    }

    public async void PlayEffect(Vector2 point, string fxname)
    {
        Debug.Log("effect : " + fxname);
        ParticleSystem particles = InstantiateParticleEffects(point, fxname);
        particles.gameObject.SetActive(true);
        particles.transform.position = point ;
        particles.Play();
        await System.Threading.Tasks.Task.Delay((int)(particles.main.startLifetime.constant * 1000));
        ReturnInPool(particles,fxname);
    }

    private void ModifyScale(ParticleSystem particles,float normalPulse)
    {
        Debug.Log("pulse : "+ normalPulse);
        float scalepara = normalPulse / 10f;
        scalepara = Mathf.Clamp(scalepara,0.3f,2f);
        particles.transform.localScale = Vector2.one * scalepara * explodeScale;
    }

    void ReturnInPool(ParticleSystem particle,string poolName)
    {
        particle.gameObject.SetActive(false);
        effectsPools.Find(x => x.PoolName == poolName).Pool.Enqueue(particle);
    }

    public class EffectPool
    {
        public string PoolName;
        public Queue<ParticleSystem> Pool = new Queue<ParticleSystem>();
    }
}
