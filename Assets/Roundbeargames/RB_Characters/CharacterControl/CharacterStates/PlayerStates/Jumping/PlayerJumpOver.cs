﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace roundbeargames
{
    public class PlayerJumpOver : CharacterState
    {
        public override void InitState()
        {
            ANIMATION_DATA.DesignatedAnimation = PlayerState.JumpOver.ToString();
            IsDiving = false;
            StopAnimationTriggered = false;
            CONTROL_MECHANISM.RIGIDBODY.useGravity = false;
            CONTROL_MECHANISM.RIGIDBODY.AddForce(Vector3.up * 17.5f);
            CONTROL_MECHANISM.TriggerColliderControl(DynamicColliderType.JumpOver);
        }

        public override void RunFixedUpdate()
        {
            if (ANIMATION_DATA.AnimationNameMatches)
            {
                CheckDive();
                CommitDive();
                DashBeforeRoll();
                Roll();

                if (ANIMATION_DATA.PlayTime > FallTime)
                {
                    if (!move.IsGoingToLand())
                    {
                        if (CONTROL_MECHANISM.IsFacingForward())
                        {
                            MOVEMENT_DATA.AirMomentum = 3f;
                        }
                        else
                        {
                            MOVEMENT_DATA.AirMomentum = -3f;
                        }

                        //if (IsDiving) {
                        //	if (CONTROL_MECHANISM.IsFalling()){}
                        //}
                    }

                    if (CONTROL_MECHANISM.IsFalling())
                    {
                        characterStateController.ChangeState((int)PlayerState.FallALoop);
                        return;
                    }
                }
            }
            else
            {
                move.MoveForward(MOVEMENT_DATA.RunSpeed * 1.05f, CHARACTER_TRANSFORM.rotation.eulerAngles.y);
            }
        }

        public override void RunFrameUpdate()
        {
            if (UpdateAnimation())
            {
                //Debug.Log (ANIMATION_DATA.PlayTime);

                if (DurationTimePassed())
                {
                    if (MOVEMENT_DATA.MoveDown)
                    {
                        characterStateController.ChangeState((int)PlayerState.CrouchIdle);
                        return;
                    }

                    switch (move.GetMoveTransition())
                    {
                        case MoveTransitionStates.RUN:
                            if (move.MoveDirectionMatches())
                            {
                                characterStateController.ChangeState((int)PlayerState.HumanoidRun);
                            }
                            else
                            {
                                characterStateController.ChangeState((int)PlayerState.RunningTurn);
                            }
                            return;
                        case MoveTransitionStates.WALK:
                            if (move.MoveDirectionMatches())
                            {
                                characterStateController.ChangeState((int)PlayerState.HumanoidWalk);
                            }
                            else
                            {
                                characterStateController.ChangeState((int)PlayerState.RunningTurn);
                            }
                            return;
                        case MoveTransitionStates.JUMP:
                            if (MOVEMENT_DATA.Run)
                            {
                                characterStateController.ChangeState((int)PlayerState.RunningJump);
                            }
                            else
                            {
                                characterStateController.ChangeState((int)PlayerState.JumpingUp);
                            }
                            return;
                        case MoveTransitionStates.NONE:
                            if (MOVEMENT_DATA.MoveDown)
                            {
                                characterStateController.ChangeState((int)PlayerState.CrouchIdle);
                                return;
                            }
                            else
                            {
                                characterStateController.ChangeState((int)PlayerState.HumanoidIdle);
                                return;
                            }
                    }
                }
            }
        }

        public override void RunLateUpdate()
        {

        }

        public override void ClearState()
        {
            CONTROL_MECHANISM.RIGIDBODY.useGravity = true;
            IsDiving = false;
            DiveChecked = false;
            StopAnimationTriggered = false;
            CONTROL_MECHANISM.TriggerColliderControl(DynamicColliderType.DEFAULT);
        }

        public float DiveCheckTime;
        public float DiveTime;
        public float RollTime;
        public float FallTime;
        public bool IsDiving;
        public bool DiveChecked;
        public bool StopAnimationTriggered;

        private void CheckDive()
        {
            if (DiveChecked)
            {
                return;
            }

            if (ANIMATION_DATA.PlayTime >= DiveCheckTime)
            {
                DiveChecked = true;
                if (!move.IsGoingToLand())
                {
                    CONTROL_MECHANISM.RIGIDBODY.useGravity = true;
                    IsDiving = true;
                }
            }
        }

        private void DashBeforeRoll()
        {
            if (ANIMATION_DATA.PlayTime > RollTime)
            {
                return;
            }

            move.MoveForward(MOVEMENT_DATA.RunSpeed * 1.025f, CHARACTER_TRANSFORM.rotation.eulerAngles.y);
        }

        private void CommitDive()
        {
            if (ANIMATION_DATA.PlayTime < DiveTime)
            {
                return;
            }

            if (ANIMATION_DATA.PlayTime > RollTime)
            {
                return;
            }

            CONTROL_MECHANISM.RIGIDBODY.useGravity = true;
            CONTROL_MECHANISM.TriggerColliderControl(DynamicColliderType.DEFAULT);

            if (IsDiving)
            {
                if (!StopAnimationTriggered)
                {
                    StopAnimationTriggered = true;
                    ANIMATION_DATA.StopAnimation();
                }
                UpdateFallTilt();
            }
        }

        private void Roll()
        {
            //check for ground collision
            if (!ANIMATION_DATA.AnimationIsPlaying())
            {
                if (move.IsGoingToLand() || MOVEMENT_DATA.IsGrounded)
                {
                    CONTROL_MECHANISM.ClearVelocity();
                    ANIMATION_DATA.PlayAnimation();
                }
            }

            //resume normal play
            if (ANIMATION_DATA.PlayTime >= RollTime)
            {
                SetDefaultTilt();
                move.MoveForward(MOVEMENT_DATA.RunSpeed * 0.9f, CHARACTER_TRANSFORM.rotation.eulerAngles.y);
                CONTROL_MECHANISM.TriggerColliderControl(DynamicColliderType.DEFAULT);
            }
        }

        private void UpdateFallTilt()
        {
            float forwardLook = -1f;
            if (!CONTROL_MECHANISM.IsFacingForward())
            {
                forwardLook = 1f;
            }
            if (ANIMATION_DATA.characterAnimator.transform.eulerAngles.x >= 12.5f * forwardLook)
            {
                ANIMATION_DATA.characterAnimator.transform.Rotate(Vector3.forward * Time.deltaTime * 15f * forwardLook, Space.World);
            }
        }

        private void SetDefaultTilt()
        {
            if (CONTROL_MECHANISM.IsFacingForward())
            {
                ANIMATION_DATA.characterAnimator.transform.eulerAngles = new Vector3(0, 90f, 0);
            }
            else
            {
                ANIMATION_DATA.characterAnimator.transform.eulerAngles = new Vector3(0, -90f, 0);
            }
        }
    }
}