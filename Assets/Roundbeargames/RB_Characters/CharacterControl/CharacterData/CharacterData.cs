﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace roundbeargames
{

    public class CharacterData : SerializedMonoBehaviour
    {
        public CharacterMovementData characterMovementData;
        public CharacterAnimationData characterAnimationData;
        public CharacterAttackData characterAttackData;
        public ControlMechanism controlMechanism;
        public HitRegister hitRegister;
        public Dictionary<TouchDetectorType, TouchDetector> TouchDetectors;
        public List<Touchable> Touchables;

        public TouchDetector GetTouchDetector(TouchDetectorType touchDetectorType)
        {
            if (TouchDetectors.ContainsKey(touchDetectorType))
            {
                return TouchDetectors[touchDetectorType];
            }
            else
            {
                return null;
            }
        }

        public List<Touchable> GetTouchable(TouchableType touchableType)
        {
            List<Touchable> tList = new List<Touchable>();
            foreach (Touchable t in Touchables)
            {
                if (t.touchableType == touchableType)
                {
                    tList.Add(t);
                }
            }

            return tList;
        }

        public bool CanMoveThrough(TouchDetectorType touchDetectorType)
        {
            if (controlMechanism.controlType == ControlType.ENEMY)
            {
                return true;
            }

            if (TouchDetectors.ContainsKey(touchDetectorType))
            {
                TouchDetector t = TouchDetectors[touchDetectorType];
                if (t.IsTouching)
                {
                    foreach (GameObject obj in t.GeneralObjects)
                    {
                        if (obj.name.Contains("Enemy"))
                        {
                            if (controlMechanism.characterStateController.CurrentState.GetType() != typeof(PlayerRunningSlide))
                            {
                                AIControl ai = obj.GetComponent<AIControl>();
                                if (!ai.IsDead())
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool IsTouchingPlayer(TouchDetectorType touchDetectorType)
        {
            if (TouchDetectors.ContainsKey(touchDetectorType))
            {
                TouchDetector t = TouchDetectors[touchDetectorType];
                if (t.IsTouching)
                {
                    foreach (GameObject obj in t.GeneralObjects)
                    {
                        if (obj.name.Contains("Player"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsTouchingGeneralObject(TouchDetectorType touchDetectorType)
        {
            if (TouchDetectors.ContainsKey(touchDetectorType))
            {
                TouchDetector t = TouchDetectors[touchDetectorType];
                if (t.IsTouching)
                {
                    if (t.GeneralObjects.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void FindDatas()
        {
            if (controlMechanism == null)
            {
                controlMechanism = this.gameObject.GetComponentInParent<ControlMechanism>();
            }

            if (characterMovementData == null)
            {
                characterMovementData = this.gameObject.GetComponentInChildren<CharacterMovementData>();
            }

            if (characterAnimationData == null)
            {
                characterAnimationData = this.gameObject.GetComponentInChildren<CharacterAnimationData>();
            }

            if (characterAttackData == null)
            {
                characterAttackData = this.gameObject.GetComponentInChildren<CharacterAttackData>();
            }

            if (hitRegister == null)
            {
                hitRegister = this.gameObject.GetComponentInChildren<HitRegister>();
            }

            TouchDetector[] detectors = controlMechanism.gameObject.GetComponentsInChildren<TouchDetector>();

            foreach (TouchDetector d in detectors)
            {
                if (!TouchDetectors.ContainsKey(d.touchDetectorType))
                {
                    TouchDetectors.Add(d.touchDetectorType, d);
                }
            }

            Touchable[] touchables = controlMechanism.gameObject.GetComponentsInChildren<Touchable>();

            foreach (Touchable t in touchables)
            {
                if (!Touchables.Contains(t))
                {
                    Touchables.Add(t);
                }
            }
        }
    }
}