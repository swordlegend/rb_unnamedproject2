﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roundbeargames
{
    public class PlayerWalk : CharacterState
    {
        public override void InitState()
        {
            ANIMATION_DATA.DesignatedAnimation = PlayerState.HumanoidWalk.ToString();

            if (!move.CanInstantTurn(characterStateController.PrevState.GetType()))
            {

            }
            else
            {
                MOVEMENT_DATA.Turn = move.GetTurn();
                if (MOVEMENT_DATA.Turn != CHARACTER_TRANSFORM.rotation.eulerAngles.y)
                {
                    CHARACTER_TRANSFORM.rotation = Quaternion.Euler(0, MOVEMENT_DATA.Turn, 0);
                }
            }
        }

        public override void RunFixedUpdate()
        {
            MOVEMENT_DATA.Turn = move.GetTurn();

            if (ANIMATION_DATA.AnimationNameMatches)
            {
                if (CONTROL_MECHANISM.IsFalling())
                {
                    characterStateController.ChangeState((int)PlayerState.FallALoop);
                    return;
                }

                if (UpdateWalk())
                {
                    return;
                }
            }
            else
            {
                if (characterStateController.PrevState.GetType() != typeof(PlayerCrouchSneakLeft))
                {
                    move.MoveForward(MOVEMENT_DATA.CrouchSpeed, CHARACTER_TRANSFORM.rotation.eulerAngles.y);
                }
                else
                {
                    move.MoveForward(MOVEMENT_DATA.WalkSpeed * 0.8f, CHARACTER_TRANSFORM.rotation.eulerAngles.y);
                }
            }
        }

        public override void RunFrameUpdate()
        {
            if (UpdateAnimation())
            {

            }
        }

        public override void RunLateUpdate()
        {

        }

        public override void ClearState()
        {

        }

        bool UpdateWalk()
        {
            switch (move.GetMoveTransition())
            {
                case MoveTransitionStates.RUN:
                    characterStateController.ChangeState((int)PlayerState.HumanoidRun);
                    return true;
                case MoveTransitionStates.WALK:
                    if (MOVEMENT_DATA.MoveDown)
                    {
                        characterStateController.ChangeState((int)PlayerState.CrouchedSneakingLeft);
                    }
                    float turn = move.GetTurn();
                    move.MoveForward(MOVEMENT_DATA.WalkSpeed, turn);
                    return true;
                case MoveTransitionStates.JUMP:
                    if (Mathf.Abs(CONTROL_MECHANISM.RIGIDBODY.velocity.y) < 0.025f)
                    {
                        characterStateController.ChangeState((int)PlayerState.JumpingUp);
                    }
                    return true;
                case MoveTransitionStates.NONE:
                    if (MOVEMENT_DATA.MoveDown)
                    {
                        characterStateController.ChangeState((int)PlayerState.CrouchIdle);
                    }
                    else
                    {
                        characterStateController.ChangeState((int)PlayerState.HumanoidIdle);
                    }

                    return true;
            }

            return false;
        }
    }
}