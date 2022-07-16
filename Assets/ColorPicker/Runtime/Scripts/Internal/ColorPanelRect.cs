using System;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ColorPicker.Runtime.Scripts.Internal
{
    public class ColorPanelRect : MonoBehaviour
    {
        [SerializeField]
        private Material originalMaterial;
        
        public RectTransform RectTransform { get; private set; }
        public Material Material { get; private set; }
        public IObservable<PointerEventData> OnPointerClick;
        public IObservable<PointerEventData> OnPointerDrag;
        
        private ObservableEventTrigger eventTrigger;
        
        
        private void Awake()
        {
            eventTrigger = gameObject.AddComponent<ObservableEventTrigger>();
            GetComponent<RawImage>().material = Material = new Material(originalMaterial);

            RectTransform = GetComponent<RectTransform>();
            OnPointerClick = eventTrigger.OnPointerClickAsObservable();
            OnPointerDrag = eventTrigger.OnDragAsObservable();
        }
    }
}
