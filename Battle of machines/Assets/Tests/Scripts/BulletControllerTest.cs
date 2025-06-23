using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DefaultNamespace
{
    public class BulletControllerTest
    {
        private GameObject bulletObj;
        private BulletController bullet;
        private GameObject targetObj;
        private BattleMachineAttack attack;

        [SetUp]
        public void SetUp()
        {
            bulletObj = new GameObject();
            bullet = bulletObj.AddComponent<BulletController>();
            targetObj = new GameObject();
            attack = targetObj.AddComponent<BattleMachineAttack>();
            attack._health = 20;
            var healthBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            healthBar.transform.SetParent(targetObj.transform);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(bulletObj);
            Object.DestroyImmediate(targetObj);
        }

        [Test]
        public void Bullet_Moves_Towards_Position()
        {
            bullet.position = new Vector3(10, 0, 0);
            bulletObj.transform.position = Vector3.zero;
            bullet.speed = 10f;

            float initialDistance = Vector3.Distance(bulletObj.transform.position, bullet.position);
            bullet.Update();
            float newDistance = Vector3.Distance(bulletObj.transform.position, bullet.position);

            Assert.Less(newDistance, initialDistance);
        }

        [Test]
        public void Bullet_Damages_Target_OnTriggerEnter()
        {
            targetObj.tag = "Enemy";
            var collider = targetObj.AddComponent<BoxCollider>();
            bullet.damage = 5;

            bullet.OnTriggerEnter(collider);

            Assert.AreEqual(15, attack._health);
        }

        [Test]
        public void Bullet_Destroys_Target_When_Health_Zero()
        {
            targetObj.tag = "Enemy";
            var collider = targetObj.AddComponent<BoxCollider>();
            bullet.damage = 20;
            StartGame.enemies = 1;

            bullet.OnTriggerEnter(collider);

            Assert.AreEqual(0, StartGame.enemies);
        }
    }

    // Dummy class for testing
    public class BattleMachineAttack : MonoBehaviour
    {
        public int _health;
    }

    // Dummy class for testing
    public static class StartGame
    {
        public static int enemies = 0;
        public static int machines = 0;
    }
}