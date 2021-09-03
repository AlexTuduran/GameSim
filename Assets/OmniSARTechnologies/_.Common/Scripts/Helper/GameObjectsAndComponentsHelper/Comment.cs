//
// Comment
//
// Author     : Alex Tuduran
// Copyright  : OmniSAR Technologies
// Web        : https://www.omnisar.com

using UnityEngine;

namespace OmniSARTechnologies.Helper {
    [System.Serializable]
    public class CommentReference {
        public GameObject reference;

        [TextArea]
        public string referenceComment = "Comment about this reference.";
    }

    public class Comment : MonoBehaviour {
        [TextArea(minLines: 2, maxLines: 64)]
        public string comment = "Your comment here.";

        public CommentReference[] references;
        public GameObject[] commentlessReferences;
    }
}