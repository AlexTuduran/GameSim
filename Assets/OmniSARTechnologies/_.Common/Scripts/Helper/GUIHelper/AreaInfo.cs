//
// Area Info
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniSARTechnologies.Helper {
	[System.Serializable]
	public struct AreaInfo {
		public int areaId;
        public Rect screenSpaceBounds;
        public Rect worldSpaceBounds;
		public Vector2 mousePositionScreenSpace;
        public Vector2 mousePositionWorldSpace;
        public Vector2 mousePositionAreaSpace;
        public Vector2 mousePositionAreaNormalizedSpace;

		public void Reset() {
			areaId 			                    = AreaHandler.kInvalidAreaId;
            mousePositionScreenSpace 		    = Vector2.zero;
            mousePositionWorldSpace	            = Vector2.zero;
            mousePositionAreaSpace              = Vector2.zero;
            mousePositionAreaNormalizedSpace    = Vector2.zero;
            screenSpaceBounds                   = Rect.zero;
            worldSpaceBounds                    = Rect.zero;
		}

		public override string ToString() {
            return string.Format(
                "(ID={0} SS={1} WS={2} AS={3} ANS={4} SSB={5} WSB={6})",
                areaId,
                mousePositionScreenSpace,
                mousePositionWorldSpace,
                mousePositionAreaSpace,
                mousePositionAreaNormalizedSpace,
                screenSpaceBounds,
                worldSpaceBounds
            );
		}
	}
}
