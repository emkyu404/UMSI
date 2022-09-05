using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveArchitecture", menuName = "Create/WaveArchitecture", order = 1)]
public class WaveArchitecture : ScriptableObject
{

    public int Score = 1;
    public GameObject[] EnemyPack = null;

}
