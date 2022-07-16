using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ColorPicker.Scripts
{
    public class ColorPanelRect : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }
        public Material Material { get; set; }
        public IObservable<PointerEventData> OnPointerClick;
        public IObservable<PointerEventData> OnPointerDrag;

        private ObservableEventTrigger eventTrigger;
        
        private void Awake()
        {
            eventTrigger = gameObject.AddComponent<ObservableEventTrigger>();
            Material = GetComponent<RawImage>().material;

            RectTransform = GetComponent<RectTransform>();
            OnPointerClick = eventTrigger.OnPointerClickAsObservable();
            OnPointerDrag = eventTrigger.OnDragAsObservable();
        }
    }
}
