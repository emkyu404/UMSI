using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
  
    private GameObject currentTurretSelected;
    

    private void Awake()
    {
        ClassicTurretSelected();
    }

    public void TeslaTurretSelected()
    {
        currentTurretSelected = Resources.Load<GameObject>("Prefabs/TurretPrefabs/TurretTesla");
    }

    public void ClassicTurretSelected()
    {
        currentTurretSelected = Resources.Load<GameObject>("Prefabs/TurretPrefabs/TurretSample1");
    }

    public void WoodenBoxSelected()
    {
        currentTurretSelected = Resources.Load<GameObject>("Prefabs/TurretPrefabs/WoodenBox");
    }

    public void DeathLaserSelected()
    {
        currentTurretSelected = Resources.Load<GameObject>("Prefabs/TurretPrefabs/TurretDeathLaser");
    }

    public GameObject getSelectedTurret()
    {
        return currentTurretSelected;
    }

}
