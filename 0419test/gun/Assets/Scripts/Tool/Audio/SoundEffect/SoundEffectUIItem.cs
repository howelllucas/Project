using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Audio
{
    [RequireComponent(typeof(EventTrigger))]
    public class SoundEffectUIItem : MonoBehaviour
    {
        public string soundEffectName;
        public EventTriggerType triggerType = EventTriggerType.PointerDown;

        private void Awake()
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry()
            {
                eventID = triggerType,
                callback = new EventTrigger.TriggerEvent()
            };
            entry.callback.AddListener(PlaySoundEffect);
            trigger.triggers.Add(entry);
        }

        private void PlaySoundEffect(BaseEventData arg0)
        {
            SoundEffectManager.Instance["2d"].PlaySoundEffectOneShot(soundEffectName);
        }
    }
}

