
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOAgentUpgrade", menuName = "Agent/SOGiantUpgrade")]
public class SOGiantUpgrade : ScriptableObject
{
   public float speed;
   public DefaultGiant DefaultGiant;
   
   public void DefaultData()
   {
       speed = DefaultGiant.speed;
   }
}
[Serializable]
public struct DefaultGiant
{
    public float speed;

}
