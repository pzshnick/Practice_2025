using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BulletControllerTests
{
    private GameObject bulletObj;
    private GameObject targetObj;
    private BulletController bulletScript;
    private BattleMachineAttack attackScript;
    private GameObject healthbar;

    [SetUp]
    public void Setup()
    {
        bulletObj = new GameObject("Bullet");
        bulletScript = bulletObj.AddComponent<BulletController>();
        bulletScript.speed = 100f;
        bulletScript.damage = 10;

        targetObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        targetObj.tag = "Enemy";

        healthbar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        healthbar.transform.SetParent(targetObj.transform);
        healthbar.transform.localScale = Vector3.one;

        attackScript = targetObj.AddComponent<BattleMachineAttack>();
        attackScript._health = 20;
    }

    [TearDown]
    public void TearDown()
    {
        if (bulletObj != null) Object.DestroyImmediate(bulletObj);
        if (targetObj != null) Object.DestroyImmediate(targetObj);
        if (healthbar != null) Object.DestroyImmediate(healthbar);
    }

    [UnityTest]
    public IEnumerator Bullet_ReducesHealthbarScaleOnHit()
    {
        var bullet = CreateBullet(Vector3.zero, Vector3.forward);
        var enemy = CreateTarget("Enemy", 100);

        float initialScaleX = enemy.transform.GetChild(0).localScale.x;

        bullet.transform.position = enemy.transform.position;

        yield return null;

        float newScaleX = enemy != null && enemy.transform.childCount > 0
            ? enemy.transform.GetChild(0).localScale.x
            : -1f;

        Assert.Less(newScaleX, initialScaleX);
    }


    [UnityTest]
    public IEnumerator Bullet_HitsEnemyAndDestroys_WhenHealthZero()
    {
        var bullet = CreateBullet(Vector3.zero, Vector3.forward);
        var enemy = CreateTarget("Enemy", 0);

        bullet.transform.position = enemy.transform.position;

        yield return null;

        Assert.IsTrue(enemy == null || enemy.Equals(null));
    }


    [UnityTest]
    public IEnumerator Bullet_DoesNotCrash_OnNonTarget()
    {
        GameObject nonTarget = GameObject.CreatePrimitive(PrimitiveType.Cube);
        nonTarget.tag = "Untagged";
        bulletScript.position = nonTarget.transform.position;

        yield return null;

        Assert.IsTrue(nonTarget != null);
        Object.DestroyImmediate(nonTarget);
    }

    private GameObject CreateBullet(Vector3 position, Vector3 target)
    {
        var bulletObj = new GameObject("Bullet");
        var collider = bulletObj.AddComponent<SphereCollider>();
        collider.isTrigger = true;

        var rb = bulletObj.AddComponent<Rigidbody>();
        rb.useGravity = false;

        var bullet = bulletObj.AddComponent<BulletController>();
        bullet.position = target;

        return bulletObj;
    }

    private GameObject CreateTarget(string tag, int health)
    {
        var target = GameObject.CreatePrimitive(PrimitiveType.Cube);
        target.tag = tag;

        var healthbar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        healthbar.transform.SetParent(target.transform);
        healthbar.transform.localPosition = Vector3.zero;
        healthbar.transform.localScale = new Vector3(1f, 1f, 1f);

        var attack = target.AddComponent<BattleMachineAttack>();
        attack._health = health;

        var collider = target.GetComponent<Collider>();
        collider.isTrigger = true;

        var rb = target.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        return target;
    }

}
