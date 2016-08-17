﻿using Sliders;
using System.Collections;
using UnityEngine;

namespace Sliders.Cam
{
    public class CamShake : MonoBehaviour
    {
        public static CamShake _instance;
        public Camera cam;

        [Header("Shake Settings")]
        public float maxChargingShake = 5F;
        public float deathShakeAmount = 10f;
        public float deathShakeDuration = 0.2f;

        private void Awake()
        {
            _instance = this;
            Debug.Log("awake");
        }

        public static void Shake(float duration, float amount)
        {
            _instance.StopAllCoroutines();
            _instance.StartCoroutine(_instance.cShakeCoroutine(duration, amount));
        }

        public static void DeathShake()
        {
            _instance.StopAllCoroutines();
            _instance.StartCoroutine(_instance.cShakeCoroutine(_instance.deathShakeDuration, _instance.deathShakeAmount));
        }

        public static void VelocityShake(MonoBehaviour behaviour)
        {
            if (behaviour != null && behaviour.GetComponent<Rigidbody2D>() != null)
            {
                _instance.StopAllCoroutines();
                _instance.StartCoroutine(_instance.cVelocityShake(behaviour.GetComponent<Rigidbody2D>()));
            }
            else
                Debug.LogError("[VelocityShake] cant find rigidbody2D on gameobject");
        }

        public IEnumerator cShakeCoroutine(float duration, float amount)
        {
            float endTime = Time.time + duration;
            Vector3 originalPos = Vector3.zero;
            Vector3 newPos = Vector3.zero;

            while (Time.time < endTime)
            {
                originalPos = cam.transform.position;
                newPos = originalPos + Random.insideUnitSphere * amount;
                newPos.z = originalPos.z;
                cam.transform.position = newPos;
                duration -= Time.deltaTime;

                yield return null;
            }

            cam.transform.position = originalPos;
        }

        //creates increasing shake depending on the rigidbody's velocity
        public IEnumerator cVelocityShake(Rigidbody2D rb)
        {
            float maxVelocity = Player._instance.maxChargeVelocity;
            Vector2 velocity;
            Vector3 originalPos = Vector3.zero;
            Vector3 newPos = Vector3.zero;

            float amount;

            while (true)
            {
                originalPos = cam.transform.position;
                velocity = rb.velocity;
                velocity.x = System.Math.Abs(velocity.x);

                if (velocity.x > (maxVelocity - Constants.velocityThreshhold))
                {
                    velocity.x = maxVelocity;
                }

                amount = Mathf.SmoothStep(0, maxChargingShake, Mathf.InverseLerp(0, maxVelocity, velocity.magnitude));
                newPos = originalPos + Random.insideUnitSphere * amount;
                newPos.z = 0;
                cam.transform.position = newPos;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}