using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Systems
{
    public class FrameCounterSystem : ModSystem
    {
        public List<FrameCounter> timers = new List<FrameCounter>();
        public override void PreUpdateEntities()
        {
            base.PreUpdateEntities();
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].Update();
                if (timers[i].state == FrameCounterState.Finished)
                {
                    timers.RemoveAt(i);
                }
            }

        }
    }

    public enum FrameCounterState : byte
    {
        Finished,
        Running,
        Paused


    }
    public class FrameCounter
    {
        public object? holder = null;
        public FrameCounterState state;
        public int currentFramesPassedOrRemained = 0;
        public int maxOrStartingValue = 60;
        public bool isCounter = false;
        public Action onFinsihed;

        public float Progress => isCounter ? (float)currentFramesPassedOrRemained / maxOrStartingValue : 1f - (float)currentFramesPassedOrRemained / maxOrStartingValue;
        public FrameCounter(int value = 60, bool startAutomatically = true)
        {
            if (value != -1)
                maxOrStartingValue = value;
            else
            {
                this.isCounter = true;
                maxOrStartingValue = 1;
            }

            if (startAutomatically)
                Start();
        }
        public void Start()
        {
            if (isCounter)
                currentFramesPassedOrRemained = 0;
            else
                currentFramesPassedOrRemained = maxOrStartingValue;
            if (state == FrameCounterState.Running)
            {
                return;
            }
            state = FrameCounterState.Running;
            ModContent.GetInstance<FrameCounterSystem>().timers.Add(this);

        }
        public void Update()
        {
            if (state == FrameCounterState.Paused || state == FrameCounterState.Finished)
                return;

            if (isCounter)
            {
                currentFramesPassedOrRemained++;
            }
            else
            {
                if (currentFramesPassedOrRemained <= 0)
                {
                    state = FrameCounterState.Finished;
                    onFinsihed?.Invoke();
                }
                currentFramesPassedOrRemained--;


            }


        }
    }
}
