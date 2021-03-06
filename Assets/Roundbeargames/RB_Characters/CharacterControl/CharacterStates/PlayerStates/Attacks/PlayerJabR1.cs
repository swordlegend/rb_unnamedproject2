﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roundbeargames {
    public class PlayerJabR1 : CharacterState {
        public override void InitState () {
            ANIMATION_DATA.DesignatedAnimation = PlayerState.Jab_R_1.ToString ();
        }

        public override void RunFixedUpdate () {

        }

        public override void RunFrameUpdate () {
            if (UpdateAnimation ()) {
                if (DurationTimePassed ()) {
                    characterStateController.ChangeState ((int) PlayerState.HumanoidIdle);
                    attack.DeRegister (characterStateController.controlMechanism.gameObject.name, PlayerState.Jab_R_1.ToString ());
                    return;
                }

                attack.UpdateHit (TouchDetectorType.ATTACK_RIGHT_FIST, ref attack.Target);
            }
        }

        public override void RunLateUpdate () {

        }

        public override void ClearState () {

        }
    }
}