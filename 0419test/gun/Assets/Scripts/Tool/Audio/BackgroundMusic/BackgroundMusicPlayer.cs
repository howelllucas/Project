using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;

namespace Game.Audio
{
    public class BackgroundMusicPlayer : InnerMonoSingleton<BackgroundMusicPlayer>
    {
        private Dictionary<string, SoundEffectPlayer> curPlaying = new Dictionary<string, SoundEffectPlayer>();

        public void FadeIn(string name, float duration = 0.5f, float delay = 0)
        {
            SoundEffectPlayer player;
            if (!curPlaying.TryGetValue(name, out player))
            {
                player = SoundEffectManager.Instance["bg"].GetSoundEffectPlayer(name, true);
                curPlaying[name] = player;
                player.FadeIn(duration, delay);
            }
            else
            {
                if (!player.IsPlaying)
                    player.FadeIn(duration, delay);
            }
        }

        public void FadeOut(string name, float duration = 0.5f, float delay = 0)
        {
            SoundEffectPlayer player;
            if (curPlaying.TryGetValue(name, out player))
            {
                player.FadeOut(duration, false, delay);
            }
        }

        public void FadeOutAll(float duration = 0.5f, float delay = 0)
        {
            foreach (var item in curPlaying.Values)
            {
                item.FadeOut(duration, false, delay);
            }
        }

    }
}

