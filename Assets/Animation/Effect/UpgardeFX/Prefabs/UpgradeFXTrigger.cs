using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFXTrigger : MonoBehaviour
{
    [SerializeField]
    ParticleSystem hold;

    [SerializeField]
    float m_interval = 1f;

    private float m_timer = 0f;

    void Update()
    {
        if (GetComponent<ParticleSystem>().isPlaying)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= m_interval)
            {
                hold.TriggerSubEmitter(0);
                hold.Stop(false);
                m_timer = 0f;
            }
        }
        else
        {
            m_timer = 0f;
        }
    }

    private void OnDisable()
    {
        GetComponent<ParticleSystem>().Stop();
        m_timer = 0f;
    }
}