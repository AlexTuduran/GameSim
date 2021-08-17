//
// Transform Sweep
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
    [AddComponentMenu("OmniSAR Technologies/Common/Helper/Transform/Transform Sweep")]
    //[ExecuteInEditMode]
    public class TransformSweep : MonoBehaviour {
        [Header("Position")]
        public bool sweepPosition = false;
        public bool worldSpacePosition = true;
        public Vector3 positionSweepMagnitude;
        public Vector3 positionSweepPhase;
        public Vector3 positionSweepSpeed;
        public bool resetPositionAfterSweep = false;

        [Header("Rotation - Euler Angles")]
        public bool sweepEulerAngles = false;
        public bool worldSpaceEulerAngles = true;
        public Vector3 eulerAnglesSweepMagnitude;
        public Vector3 eulerAnglesSweepPhase;
        public Vector3 eulerAnglesSweepSpeed;
        public bool resetEulerAnglesAfterSweep = false;

        [Header("Scale")]
        public bool sweepScale = false;
        public Vector3 scaleDefaultMagnitude = Vector3.one;
        public Vector3 scaleMinMagnitude = Vector3.zero;
        public Vector3 scaleMaxMagnitude = Vector3.one;
        public Vector3 scaleSweepPhase;
        public Vector3 scaleSweepSpeed;
        public bool resetScaleAfterSweep = false;

        private float _startTime;
        private Vector3 _initialPosition;
        private Vector3 _initialEulerAngles;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnEnable() {
            _initialPosition = worldSpacePosition ? transform.position : transform.localPosition;
            _initialEulerAngles = worldSpaceEulerAngles ? transform.eulerAngles : transform.localEulerAngles;

            UpdateStartTime();
    	}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float TimeSinceStart() {
            return Time.time - _startTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateStartTime() {
            _startTime = Time.time;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float GenSine(float fq, float phase) {
            return Mathf.Cos((phase + TimeSinceStart() * fq) * Mathf.PI * 2.0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float GenSineUnitSpace(float fq, float phase) {
            return GenSine(fq, phase) * 0.5f + 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetPosition(Vector3 position) {
            if (worldSpacePosition) {
                transform.position = position;
            } else {
                transform.localPosition = position;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetEulerAngles(Vector3 eulerAngles) {
            if (worldSpaceEulerAngles) {
                transform.eulerAngles = eulerAngles;
            } else {
                transform.localEulerAngles = eulerAngles;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetScale(Vector3 scale) {
            transform.localScale = scale;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update() {
            if (!isActiveAndEnabled) {
                return;
            }
            
            if (sweepPosition) {
                SetPosition(_initialPosition + new Vector3(
                    positionSweepMagnitude.x * GenSine(positionSweepSpeed.x, positionSweepPhase.x),
                    positionSweepMagnitude.y * GenSine(positionSweepSpeed.y, positionSweepPhase.y),
                    positionSweepMagnitude.z * GenSine(positionSweepSpeed.z, positionSweepPhase.z)
			    ));
            } else {
                if (resetPositionAfterSweep) {
                    SetPosition(_initialPosition);
                }
            }

            if (sweepEulerAngles) {
                SetEulerAngles(_initialEulerAngles + new Vector3(
                    eulerAnglesSweepMagnitude.x * GenSine(eulerAnglesSweepSpeed.x, eulerAnglesSweepPhase.x),
                    eulerAnglesSweepMagnitude.y * GenSine(eulerAnglesSweepSpeed.y, eulerAnglesSweepPhase.y),
                    eulerAnglesSweepMagnitude.z * GenSine(eulerAnglesSweepSpeed.z, eulerAnglesSweepPhase.z)
				));
            } else {
                if (resetEulerAnglesAfterSweep) {
                    SetEulerAngles(_initialEulerAngles);
                }
			}
				
			if (sweepScale) {
                SetScale(new Vector3(
                    Mathf.Lerp(scaleMinMagnitude.x, scaleMaxMagnitude.x, GenSineUnitSpace(scaleSweepSpeed.x, scaleSweepPhase.x)),
                    Mathf.Lerp(scaleMinMagnitude.y, scaleMaxMagnitude.y, GenSineUnitSpace(scaleSweepSpeed.y, scaleSweepPhase.y)),
                    Mathf.Lerp(scaleMinMagnitude.z, scaleMaxMagnitude.z, GenSineUnitSpace(scaleSweepSpeed.z, scaleSweepPhase.z))
                ));
            } else {
                if (resetScaleAfterSweep) {
                    SetScale(scaleDefaultMagnitude);
                }
            }
    	}
    }
}