//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright © 2013 - 2015 Ninjutsu Games LTD.
//----------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace NJG
{
    [AddComponentMenu("NJG MiniMap/Interaction/Button Zoom Map")]
    public class ButtonZoom : MonoBehaviour, IPointerClickHandler
    {
        public Map map;
        public bool zoomIn;
        public float amount = 0.5f;

        void Start()
        {
            if (map == null) map = GetComponentInParent<Map>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (map == null)
            {
                Debug.LogError("There is no Map instance set.", gameObject);
                return;
            }

            if (zoomIn) map.ZoomIn(amount);
            else map.ZoomOut(amount);
        }
    }
}
