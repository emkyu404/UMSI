using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretManager {
    void UpdateTargets();
    IEnumerator TurretAction();
}
