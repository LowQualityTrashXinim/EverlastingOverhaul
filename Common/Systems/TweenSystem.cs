using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Terraria;
using Terraria.Initializers;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
namespace DreamMod.Common.Systems;


public enum TweenEaseType : byte
{

    None = 0,
    //InSine = 1,
    //OutSine = 2,
    //InExpo = 3,
    //OutExpo = 4,
    OutBack = 5,
}

/// <summary>
/// A Tween, when started, use its currentProgress field to get the running value, Also must be updated manually using the Update method, at least for now untill i figure out the best way to update it automatically
/// </summary>
/// <typeparam name="T"></typeparam>
public class Tween<T> where T : struct
{
    public T currentProgress => lerp(start, finish, currentProgressPercentage);
    public T start;
    public T finish;
    private TweenEaseType easeType;
    //public TweenState state;
    public FrameCounter currentDuration;
    public Action<Tween<T>> onFinsihed;
    public delegate T lerpFunction(T value1, T value2, float amount);
    public lerpFunction lerp;
    public bool pingpongEnabled = false;
    public Tween(lerpFunction lerpFunc, bool pingpong = false)
    {

        lerp = lerpFunc;
        pingpongEnabled = pingpong;
    }
    public Tween<T> Start(T start, T finish, TweenEaseType type, int duration)
    {
        SetProperties(start, finish, type, duration, true);
        return this;
    }
    public Tween<T> Start()
    {
        currentDuration.Start();
        return this;
    }

    public Tween<T> SetProperties(T start, T finish, TweenEaseType type, int duration, bool run = false)
    {
        currentDuration = new(duration, run);
        this.start = start;
        this.finish = finish;
        easeType = type;
        currentDuration.onFinsihed += () => onFinsihed?.Invoke(this);
        currentDuration.state = FrameCounterState.Paused;
        return this;
    }

    public float currentProgressPercentage
    {

        get
        {
            float prog = currentDuration.Progress;

            switch (easeType)
            {

                //TODO: fix this stuff

                //    case TweenEaseType.InSine:
                //    currentProgressPercentage = BossRushUtils.InSine(currentDuration / endDuration);
                //    break;
                //case TweenEaseType.OutSine:
                //    currentProgressPercentage = BossRushUtils.OutSine(currentDuration / endDuration);
                //    break;
                //case TweenEaseType.InExpo:
                //    currentProgressPercentage = BossRushUtils.InExpo(currentDuration / endDuration, 11f);
                //    break;
                //case TweenEaseType.OutExpo:
                //    currentProgressPercentage = BossRushUtils.OutExpo(currentDuration / endDuration, 11f);
                //    break;

                case TweenEaseType.OutBack:
                    prog = ModUtils.OutBack(currentDuration.Progress);
                    break;

            }
            if (pingpongEnabled)
                return Terraria.Utils.PingPongFrom01To010(prog);

            return prog;
        }
    }

    public void Pause() => currentDuration.state = FrameCounterState.Paused;
    public void Resume() => currentDuration.state = FrameCounterState.Running;

}

/// <summary>
/// Holds A sequance of Tweens
/// </summary>
/// <typeparam name="T"></typeparam>
public class TweenHandler<T> where T : struct
{
    private List<Tween<T>> tweens = new();
    public List<Tween<T>> Tweens
    {
        get => tweens;

        set
        {   
            tweens = [];
            value.ForEach((t) => tweens.Add(t));
            currentTween = Tweens.FirstOrDefault();
        }
    }
    public Tween<T> currentTween;
    public void PlayTweens()
    {
        
        if(tweens.Count <= 0)
            return;

        foreach (var t in tweens)
        {
            t.onFinsihed += LinkTween;
        }

        currentTween = tweens[0].Start();

    }

    public void LinkTween(Tween<T> finishedTween)
    {
        int nextIndex = tweens.IndexOf(finishedTween) + 1;
        if (nextIndex < tweens.Count)
            currentTween = tweens[nextIndex].Start();
    }
    public void Pause() => currentTween.Pause();
    public void Resume() => currentTween.Resume();

}
