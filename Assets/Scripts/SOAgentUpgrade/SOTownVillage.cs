using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOTownVillage", menuName = "SOTownVillage")]
public class SOTownVillage : ScriptableObject
{
    public int townLevel;
    public DefaultTownVillage DefaultTownVillage;

    public void DefaultTown()
    {
        townLevel = DefaultTownVillage.defaultTownLevel;
    }
}
[Serializable]
public struct DefaultTownVillage
{
    public int defaultTownLevel;
}
