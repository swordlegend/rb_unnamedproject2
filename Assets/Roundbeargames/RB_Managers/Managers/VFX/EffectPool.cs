﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SimpleEffectType {
    SPARK,
    FLARE,
    BLOOD,
    GROUND_SHOCK,
    DISTORTION,
    GROUND_SMOKE,
    MOTION_SPEED_WHITE,
    MOTION_STRAIGHT_ATTACK,
    NITRONIC,
    GROUND_IMPACT_DUST,
}

namespace roundbeargames {
    public abstract class EffectPool : MonoBehaviour {
        [SerializeField] SimpleEffectType simpleEffectType;
        [SerializeField] GameObject EffectPrefab;
        [SerializeField] List<GameObject> Showing;
        [SerializeField] List<GameObject> Pool;
        [SerializeField] float Duration;

        public GameObject ShowEffect (Vector3 pos) {
            GameObject effect;
            if (Pool.Count == 0) {
                effect = Instantiate (EffectPrefab) as GameObject;
            } else {
                effect = Pool[0];
                Pool.RemoveAt (0);
            }
            //effect.transform.localPosition = Vector3.zero;
            effect.transform.position = new Vector3 (pos.x, pos.y, pos.z);

            effect.SetActive (true);
            Showing.Add (effect);

            if (Duration != 0f) {
                StartCoroutine (_TurnOff (Duration, effect));
            }

            return effect;
        }

        IEnumerator _TurnOff (float seconds, GameObject obj) {
            yield return new WaitForSeconds (seconds);
            Showing.Remove (obj);
            Pool.Add (obj);

            ParticleSystem ps = obj.GetComponent<ParticleSystem> ();
            if (ps != null) {
                ps.Stop ();
            }

            ParticleSystem[] arr = obj.GetComponentsInChildren<ParticleSystem> ();
            foreach (ParticleSystem p in arr) {
                p.Stop ();
            }

            obj.SetActive (false);
        }

    }
}