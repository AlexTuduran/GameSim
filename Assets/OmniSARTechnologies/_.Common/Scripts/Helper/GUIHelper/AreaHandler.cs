//
// Area Handler
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
	public class AreaHandler : MonoBehaviour {
        public const int kInvalidAreaId = -1;

		public int areaId = kInvalidAreaId;
        public Rect worldSpaceBounds;
        public Rect screenSpaceBounds;
		public LineRenderer lineRendererBounds;
		public bool useLineRendererBounds = false;
		public bool showAreaBounds = false;

		private void OnValidate() {
			if (useLineRendererBounds) {
				if (lineRendererBounds) {
					worldSpaceBounds = BoundsToRect(lineRendererBounds.bounds);
				}
			}

			if (showAreaBounds) {
				showAreaBounds = false;
				Debug.DrawLine(worldSpaceBounds.min, worldSpaceBounds.max, Color.red, 3.0f, false);
			}

            screenSpaceBounds.min = WorldSpaceToScreenSpace(worldSpaceBounds.min);
            screenSpaceBounds.max = WorldSpaceToScreenSpace(worldSpaceBounds.max);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect BoundsToRect(Bounds bounds) {
			Rect r = Rect.zero;

			r.min = bounds.min;
			r.max = bounds.max;

			return r;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ScreenSpaceToWorldSpace(Vector2 screenSpace, Camera camera = null) {
			if (!camera) {
				camera = Camera.main;
			}

			if (!camera) {
				return default(Vector3);
			}

			return camera.ScreenToWorldPoint(new Vector3(screenSpace.x, screenSpace.y, 0));
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 WorldSpaceToScreenSpace(Vector3 worldSpace, Camera camera = null) {
			if (!camera) {
				camera = Camera.main;
			}

			if (!camera) {
				return default(Vector2);
			}

			return camera.WorldToScreenPoint(worldSpace);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rect ScreenSpaceRectToWorldSpaceRect(Rect screenSpaceRect) {
			return new Rect(
				ScreenSpaceToWorldSpace(screenSpaceRect.position),
				ScreenSpaceToWorldSpace(screenSpaceRect.size)
			);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rect WorldSpaceRectToScreenSpaceRect(Rect worldSpaceRect) {
			return new Rect(
				WorldSpaceToScreenSpace(worldSpaceRect.position),
				WorldSpaceToScreenSpace(worldSpaceRect.size)
			);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 WorldSpaceToAreaSpace(Vector2 worldSpace) {
			return worldSpace - worldSpaceBounds.position;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 AreaSpaceToWorldSpace(Vector2 areaSpace) {
			return areaSpace + worldSpaceBounds.position;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 AreaSpaceToAreaNormalizedSpace(Vector2 areaSpace) {
			if (Vector2.zero == worldSpaceBounds.size) {
				return Vector2.zero;
			}

			return new Vector2 (
				areaSpace.x / worldSpaceBounds.size.x,
				areaSpace.y / worldSpaceBounds.size.y
			);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 AreaNormalizedSpaceToAreaSpace(Vector2 areaNormalizedSpace) {
            return Vector2.Scale(worldSpaceBounds.size, areaNormalizedSpace);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 WorldSpaceToAreaNormalizedSpace(Vector2 worldSpace) {
            return AreaSpaceToAreaNormalizedSpace(WorldSpaceToAreaSpace(worldSpace));
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 AreaNormalizedSpaceToWorldSpace(Vector2 areaNormalizedSpace) {
            return AreaSpaceToWorldSpace(AreaNormalizedSpaceToAreaSpace(areaNormalizedSpace));
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsWorldSpaceInArea(Vector3 worldSpace) {
			return worldSpaceBounds.Contains(worldSpace);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsScreenSpaceInArea(Vector2 screenSpace) {
			return IsWorldSpaceInArea(ScreenSpaceToWorldSpace(screenSpace));
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int WorldSpaceToAreaId(Vector3 worldSpace) {
			if (IsWorldSpaceInArea(worldSpace)) {
				return areaId;
			}

			return kInvalidAreaId;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ScreenSpaceToAreaId(Vector2 screenSpace) {
			return WorldSpaceToAreaId(ScreenSpaceToWorldSpace(screenSpace));
		}
   }
}