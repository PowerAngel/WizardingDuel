using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{
    private string name;
    private int power;
    private int damage;
    private Material material;

    public Spell(string name, int power, int damage, Material material)
    {
        this.name = name;
        this.power = power;
        this.damage = damage;
        this.material = material;
    }

    public string getName()
    {
        return name;
    }

    public int getPower()
    {
        return power;
    }

    public int getDamage()
    {
        return damage;
    }

    public Material getMaterial()
    {
        return material;
    }

}
