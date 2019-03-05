using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Damage
{
    public enum Type
    {
        Physical, Lightning, Cold, Fire, Chaos
    };
    public enum Source
    {
        Melee, Ranged, Siege
    };
    public const float critMultiplier = 2.5f;

    public struct Packet
    {
        public float damage;
        public bool isCritical;
        public bool isStatus;
        public Status.Packet status;
    }

    public struct DOT
    {
        public Type type;
        public float damagePerTick;
        public int duration;
    }

    [System.Serializable]
    public struct Resist
    {
        public float physResist;
        public float lightningResist;
        public float coldResist;
        public float fireResist;
        public float chaosResist;
        public float meleeResist;
        public float rangedResist;
        public float siegeResist;
    }

    public const int totalTypes = 5;

    public static Packet CalculateDamage(Type type, float incDamage, float defense, int critChance, int statusChance, float statusResist, float maxHealth)
    {
        Packet packet = new Packet
        {
            damage = incDamage * (1 - defense),
            isStatus = false
        };
        System.Random rnd = new System.Random();
        int critRoll = rnd.Next(1, 100);
        if (critRoll <= critChance)
        {
            packet.isCritical = true;
            packet.damage *= critMultiplier;
            if (type != Type.Physical && type != Type.Fire)
            {
                packet.isStatus = true;
            }
        } else
        {
            packet.isCritical = false;
        }
        int statusRoll = rnd.Next(1, 100);
        if (statusRoll <= statusChance && type != Type.Physical && type != Type.Fire)
        {
            packet.isStatus = true;
        }
        if (packet.isStatus || type == Type.Cold)
        {
            packet.status = Status.ApplyStatus(type, statusResist, packet.isStatus, maxHealth, packet.damage);
            packet.isStatus = true;
        }
        return packet;
    }

    public static float CalculateDefense(Type type, Source source, Resist resist)
    {
        float typeDefense;
        float sourceDefense;
        switch(type)
        {
            case Type.Physical:
                typeDefense = resist.physResist;
                break;
            case Type.Lightning:
                typeDefense = resist.lightningResist;
                break;
            case Type.Cold:
                typeDefense = resist.coldResist;
                break;
            case Type.Fire:
                typeDefense = resist.fireResist;
                break;
            case Type.Chaos:
                typeDefense = resist.chaosResist;
                break;
            default:
                typeDefense = 0;
                break;
        }
        switch(source)
        {
            case Source.Melee:
                sourceDefense = resist.meleeResist;
                break;
            case Source.Ranged:
                sourceDefense = resist.rangedResist;
                break;
            case Source.Siege:
                sourceDefense = resist.siegeResist;
                break;
            default:
                sourceDefense = 0;
                break;
        }
        return typeDefense * sourceDefense;
    }

    public static DOT CalculateDot(Type _type, float incDamage, float defense, float maxHealth)
    {
        float damage = incDamage * (1 - defense);
        int damagePercent = (int)Mathf.Floor(damage / maxHealth) * 100;
        float durationSeconds = damagePercent * Status.durationPerPercent;
        int frameDuration = (int)Mathf.Floor(durationSeconds * 60);
        return new DOT
        {
            type = _type,
            damagePerTick = damage / durationSeconds,
            duration = frameDuration
        };
    }
}
