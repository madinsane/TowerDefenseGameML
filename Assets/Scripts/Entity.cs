using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Unit unit;

    // Start is called before the first frame update
    protected void Start()
    {
        unit.InitHealth();
        if (unit.regenPercent > 0)
        {
            StartCoroutine(Regeneration());
        }
    }

    IEnumerator Regeneration()
    {
        float regenAmount;
        while(unit.Health > 0)
        {
            if (unit.Health < unit.maxHealth)
            {
                regenAmount = (unit.regenPercent / 100) * unit.maxHealth;
                unit.Health += regenAmount;
                unit.healthBar.fillAmount = (unit.Health / unit.maxHealth);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        unit.Health -= amount;
        unit.healthBar.fillAmount = (unit.Health / unit.maxHealth);

        if (unit.Health <= 0f)
        {
            Kill();
        }
    }

    public void TakeStatus(Status.Packet packet)
    {
        if (unit.StatusAffected)
            return;
        int startAttackRate = unit.attackRate;
        Damage.Resist startResist = unit.resists;
        float startSpeed = unit.speed;
        float startDamage = unit.damage; 
        switch (packet.type)
        {
            case Status.Type.Shock:
                unit.resists.physResist /= Status.shockDefenseMultiplier;
                unit.resists.lightningResist /= Status.shockDefenseMultiplier;
                unit.resists.coldResist /= Status.shockDefenseMultiplier;
                unit.resists.fireResist /= Status.shockDefenseMultiplier;
                unit.resists.chaosResist /= Status.shockDefenseMultiplier;
                break;
            case Status.Type.Chill:
                unit.speed *= Status.chillMovespeedMultiplier;
                unit.attackRate = Mathf.FloorToInt(unit.attackRate / Status.chillAttackspeedMultiplier);
                break;
            case Status.Type.Freeze:
                unit.speed *= Status.freezeMovespeedMultiplier;
                unit.attackRate = Mathf.FloorToInt(unit.attackRate / Status.freezeAttackspeedMultiplier);
                break;
            case Status.Type.Curse:
                unit.damage *= Status.curseDamageMultiplier;
                break;
        }
        unit.StatusAffected = true;
        StartCoroutine(StatusDelay(packet.duration, startAttackRate, startResist, startSpeed, startDamage));
    }

    IEnumerator StatusDelay(float duration, int startAttackRate, Damage.Resist startResist, float startSpeed, float startDamage)
    {
        yield return new WaitForSeconds(duration);
        unit.attackRate = startAttackRate;
        unit.resists = startResist;
        unit.speed = startSpeed;
        unit.damage = startDamage;
        unit.StatusAffected = false;
    }

    public void TakeDOT(Damage.DOT dot)
    {
        StartCoroutine(DOTTick(dot));
    }

    IEnumerator DOTTick(Damage.DOT dot)
    {
        for (int i = 0; i < (dot.duration / 0.1f); i++)
        {
            unit.healthBar.color = Color.magenta;
            TakeDamage(dot.damagePerTick);
            yield return new WaitForSeconds(0.1f);

        }
        unit.healthBar.color = new Color(0, 255, 5);
    }

    public virtual void Kill()
    {
        //StatManager.Gold += unit.value;
        Destroy(gameObject);
    }
}
