using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaTurret : MonoBehaviour, ITurretManager
{

    [SerializeField]
    private GameObject electricityEffect;

    [SerializeField]
    private GameObject electricityImpact;

    public Animator animator;
    public RuntimeAnimatorController[] allAnimator;

    [Header("Detection")]
    private string gameObjectName;
    private TurretDetection td;
    private DIRECTION looksAt = DIRECTION.IDLE;

    [Header("Attributes")]
    private bool isCharging;

    [SerializeField]
    private GameObject lightningOrb;



    private string enemyTag = "Enemy";
    // Start is called before the first frame update

    void Awake()
    {
        td = GetComponentInChildren<TurretDetection>();
        CircleCollider2D circleCollider = transform.GetComponent<CircleCollider2D>();
        circleCollider.radius = gameObject.GetComponent<TurretAttributes>().getRange();
        gameObjectName = gameObject.name.Replace("(Clone)", "");
        string path = "Animations/TurretAnimations/" + gameObjectName;
        lightningOrb.SetActive(false);
        transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
        animator = GetComponentInChildren<Animator>();
        allAnimator = Resources.LoadAll<RuntimeAnimatorController>(path);
    }
    private void Update()
    {
        if (td.enemyInRange() || td.GetTargets().Count > 0)
        {
            UpdateTargets();
            if (!isCharging)
            {
                StartCoroutine(TurretAction());
            }
        }
    }

    public IEnumerator TurretAction()
    {
        RuntimeAnimatorController newAnimator;
        isCharging = true;
        for(int i = 1; i <= 3; ++i)
        {
            newAnimator = findAnimator(gameObjectName + "_Charging"+i);
            animator.runtimeAnimatorController = newAnimator;
            yield return new WaitForSeconds(1f);
        }

        newAnimator = findAnimator(gameObjectName + "_Release");
        animator.runtimeAnimatorController = newAnimator;
        lightningOrb.SetActive(true);
        yield return new WaitForSeconds(1f);
        bool enemyDetected = (td.GetTargets().Count > 0);
        while (!enemyDetected)
        {
            yield return new WaitForSeconds(1f);
            enemyDetected = (td.GetTargets().Count > 0);
        }
        foreach (GameObject target in td.GetTargets())
        {
            ElectricityEffect(target.transform);
            target.GetComponent<EnemyAttributes>().TakeDamage(gameObject.GetComponent<TurretAttributes>().getTurretDamage());
        }
        lightningOrb.SetActive(false);

        newAnimator = findAnimator(gameObjectName + "_Waiting");
        animator.runtimeAnimatorController = newAnimator;
        yield return new WaitForSeconds(1f);
        isCharging = false;
        yield break;
    }

    public void UpdateTargets()
    {
        td.UpdateTargets();
    }


    private void ElectricityEffect(Transform target)
    {
        Vector3 dir = target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir);
        GameObject lightningEffect = Instantiate(electricityEffect, transform.position, rotation);
        GameObject lightningImpact = Instantiate(electricityImpact, target.transform.position, Quaternion.identity);
        Destroy(lightningEffect, 1f);
        Destroy(lightningImpact, 1f);
    }





    private RuntimeAnimatorController findAnimator(string name)
    {
        foreach (RuntimeAnimatorController a in allAnimator)
        {
            if (a.name == name)
            {
                return a;
            }
        }
        return null;
    }
}
