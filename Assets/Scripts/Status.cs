using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Status
{
    public enum Type
    {
        Shock, Chill, Freeze, Curse
    };

    public struct Packet
    {
        public Type type;
        public float duration;
    }

    public const float shockDefenseMultiplier = 1.5f;
    public const float chillMovespeedMultiplier = 0.5f;
    public const float chillAttackspeedMultiplier = 0.5f;
    public const float freezeMovespeedMultiplier = 0f;
    public const float freezeAttackspeedMultiplier = 0f;
    public const float curseDamageMultiplier = 0.5f;
    public const float durationPerPercent = 0.1f;

    public static Packet ApplyStatus(Damage.Type type, float statusResist, bool isStatus = true, float maxHealth = 1, float damage = 0)
    {
        Packet packet = new Packet();
        switch (type)
        {
            case Damage.Type.Lightning:
                packet.type = Type.Shock;
                break;
            case Damage.Type.Cold:
                if (isStatus)
                {
                    packet.type = Type.Freeze;
                } else
                {
                    packet.type = Type.Chill;
                }
                break;
            case Damage.Type.Chaos:
                packet.type = Type.Curse;
                break;
        }
        int damagePercent = (int)Mathf.Floor(damage / maxHealth) * 100;
        if (damagePercent > 50)
            damagePercent = 50;
        else if (damagePercent < 5)
            damagePercent = 5;
        float durationSeconds = damagePercent * durationPerPercent;
        //packet.duration = (int)Mathf.Floor(durationSeconds * 60);
        packet.duration = durationSeconds;
        return packet;
    }
}
