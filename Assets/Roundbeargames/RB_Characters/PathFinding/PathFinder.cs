﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace roundbeargames {
    public class PathFinder : MonoBehaviour {
        public GameObject WayPointPrefab;
        public bool ShowDebugRaycast;
        public Vector3 GreenLineOffset;

        [Button (ButtonSizes.Medium)]
        private void ToggleRender () {
            MeshRenderer[] renderers = this.gameObject.GetComponentsInChildren<MeshRenderer> ();

            if (RenderersOn) {
                RenderersOn = false;
            } else {
                RenderersOn = true;
            }

            foreach (MeshRenderer r in renderers) {
                if (RenderersOn) {
                    r.enabled = false;
                } else {
                    r.enabled = true;
                }
            }
        }
        private bool RenderersOn;
        public List<WayPoint> Unvisited;
        public List<WayPoint> ResultPath;

        [HorizontalGroup ("Split", 0.5f)]
        [Button (ButtonSizes.Large), GUIColor (0.4f, 0.8f, 1)]
        private void CreateWayPointFarLeft () {
            GetLatestWayPoint ().CreateWayPointLeft ();
        }

        [VerticalGroup ("Split/right")]
        [Button (ButtonSizes.Large), GUIColor (0, 1, 0)]
        private void CreateWayPointFarRight () {
            GetLatestWayPoint ().CreateWayPointRight ();
        }

        public WayPoint GetLatestWayPoint () {
            WayPoint[] wArray = this.gameObject.GetComponentsInChildren<WayPoint> ();
            return wArray[wArray.Length - 1];
        }

        private void ResetUnvisited () {
            Unvisited.Clear ();
            WayPoint[] all = this.gameObject.GetComponentsInChildren<WayPoint> ();
            foreach (WayPoint w in all) {
                w.PreviousPoint = null;
                w.KnownDistance = 1000;

                Unvisited.Add (w);

                if (w.pathFinder == null) {
                    w.pathFinder = this;
                }
            }
        }

        public List<WayPoint> FindPath (WayPoint start, WayPoint end) {
            ResetUnvisited ();
            ResultPath.Clear ();

            start.KnownDistance = 0;
            CalcDistance (start);

            AddResult (start, end);

            return ResultPath;
        }

        private void AddResult (WayPoint start, WayPoint end) {
            ResultPath.Add (end);
            if (start != end) {
                AddResult (start, end.PreviousPoint);
            }
        }

        private void CalcDistance (WayPoint point) {
            //Debug.Log ("--- checking " + point.gameObject.name + " ---");
            point.CalcNeighborDistance ();
            Unvisited.Remove (point);

            foreach (WayPoint n in point.Neighbors) {
                if (Unvisited.Contains (n)) {
                    CalcDistance (n);
                }
            }
        }
    }
}