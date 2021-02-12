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

    private void Start()
    {
        greenHealth = health.color;
    }

    private void Update()
    {
        healthvalue = health.transform.localScale.x;

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

    public IEnumerator SetHpSmooth(float hp)
    {
        float currentHp = health.transform.localScale.x;
        float diff = currentHp - hp;

        while(currentHp - hp > Mathf.Epsilon)
        {
            currentHp -= diff * Time.deltaTime;
            health.transform.localScale = new Vector3(currentHp, 1.0f);
            yield return null;
        }
        health.transform.localScale = new Vector3(hp, 1.0f);
    }
}
