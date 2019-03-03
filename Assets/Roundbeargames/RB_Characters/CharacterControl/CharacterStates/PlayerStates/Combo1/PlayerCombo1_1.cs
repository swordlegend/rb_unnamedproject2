﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roundbeargames {
	public class PlayerCombo1_1 : CharacterState {
		public override void InitState () {
			ANIMATION_DATA.DesignatedAnimation = PlayerState.PlayerCombo1_1.ToString ();

			float turn = move.GetTurn ();
			move.InstMoveForward (0.3f, turn);
		}

		public override void RunFixedUpdate () {

		}

		public override void RunFrameUpdate () {
			if (UpdateAnimation ()) {
				//Debug.Log (ANIMATION_DATA.PlayTime);
				if (DurationTimePassed ()) {
					characterStateController.ChangeState ((int) PlayerState.HumanoidIdle);
					return;
				}

				if (ANIMATION_DATA.PlayTime > 0.558f) {
					if (ATTACK_DATA.AttackA) {
						characterStateController.ChangeState ((int) PlayerState.PlayerCombo1_2);
						return;
					}
				}

				attack.UpdateHit (TouchDetectorType.ATTACK_RIGHT_FIST, ref attack.Target);
			}
		}

		public override void RunLateUpdate () {

		}

		public override void ClearState () {
			attack.DeRegister (characterStateController.controlMechanism.gameObject.name, PlayerState.PlayerCombo1_1.ToString ());
		}
	}
}