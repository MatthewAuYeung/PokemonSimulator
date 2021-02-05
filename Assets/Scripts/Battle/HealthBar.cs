using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image health;
    [SerializeField]
    private Color yellowHealth;
    [SerializeField]
    private Color redHealth;
    private Color greenHealth;

    private float healthvalue;
    [Range(0.0f, 1.0f)]
    public float testhp = 1.0f;

    private void Start()
    {
        greenHealth = health.color;
    }

    private void Update()
    {
        healthvalue = health.transform.localScale.x;

        //SetHP(testhp);

        if(healthvalue > 0.5f)
        {
            health.color = greenHealth;
        }
        if (healthvalue > 0.2f && healthvalue <= 0.5f)
        {
            health.color = yellowHealth;
        }
        if (healthvalue <= 0.2f)
        {
            health.color = redHealth;
        }
    }

    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1.0f);
    }
}
