using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using itfantasy.unii.utils;

namespace itfantasy.unii
{
    public class Tween
    {
        object target;
        string transition;
        List<string> properties;
        List<float> startValues;
        List<float> endValues;

        float currentTime;
        float totalTime;
        bool roundToInt;

        public float delay;
        public Tween Delay(float value)
        {
            currentTime = currentTime + delay - value;
            delay = value;
            return this;
        }

        public bool RoundToInt
        {
            get { return roundToInt; }
            set { roundToInt = value; }
        }

        private Action<object> completed;
        public Tween Callback(Action<object> complete)
        {
            this.completed = complete;
            return this;
        }

        public bool ing = false;

        public Tween(object target, float time, string transition)
        {
            Reset(target, time, transition);
        }

        public Tween Reset(object target, float time, string transition)
        {
            this.target = target;
            currentTime = 0;
            totalTime = Mathf.Max(0.0001f, time);
            delay = 0;
            this.transition = transition;
            roundToInt = false;
            if (properties != null) properties.Clear();
            else properties = new List<string>();
            if (startValues != null) startValues.Clear();
            else startValues = new List<float>();
            if (endValues != null) endValues.Clear();
            else endValues = new List<float>();
            return this;
        }

        public Tween Reset()
        {
            currentTime = 0;
            if (properties != null) properties.Clear();
            else properties = new List<string>();
            if (startValues != null) startValues.Clear();
            else startValues = new List<float>();
            if (endValues != null) endValues.Clear();
            else endValues = new List<float>();
            return this;
        }

        public void Animate(String property, float targetValue)
        {
            properties.Add(property);
            startValues.Add(float.NaN);
            endValues.Add(targetValue);
            ing = true;
        }

        public void Animate(String property, float startValue, float targetValue)
        {
            properties.Add(property);
            startValues.Add(startValue);
            endValues.Add(targetValue);
            ing = true;
        }

        public void MoveTo(float x, float y)
        {
            Animate("x", x);
            Animate("y", y);
        }

        public void MoveTo(float x, float y, float z)
        {
            Animate("x", x);
            Animate("y", y);
            Animate("z", z);
        }

        public void ScaleTo(float factor)
        {
            Animate("scaleX", factor);
            Animate("scaleY", factor);
        }

        public void ScaleTo(float x, float y)
        {
            Animate("scaleX", x);
            Animate("scaleY", y);
        }

        public void FadeTo(float alpha)
        {
            Animate("alpha", alpha);
        }

        public void FadeIn()
        {
            Animate("alpha", 1);
        }

        public void FadeOut()
        {
            Animate("alpha", 0);
        }

        public void Update()
        {
            float previousTime = currentTime;
            currentTime += Time.deltaTime;
            if (currentTime < 0 || previousTime >= totalTime)
                return;
            float ratio = Mathf.Min(totalTime, currentTime) / totalTime;
            int numAnimatedProperties = startValues.Count;

            for (int i = 0; i < numAnimatedProperties; ++i)
            {
                if (float.IsNaN(startValues[i]))
                {
                    startValues[i] = float.Parse(Comm.GetPropertyValue(target, properties[i]).ToString());
                }

                float startValue = startValues[i];
                float endValue = endValues[i];
                float delta = endValue - startValue;

                TransitFunction transitionFunc = Transitions.getTransition(transition);
                float temp = transitionFunc(ratio) * delta;
                float currentValue = startValue + temp;
                
                if (roundToInt) currentValue = Mathf.Round(currentValue);
                Comm.SetPropertyValue(target, properties[i], currentValue.ToString());
            }

            if (previousTime < totalTime && currentTime >= totalTime)
            {
                if (completed != null)
                {
                    completed.Invoke(target);
                }
                ing = false;
            }
        }

        public void Dispose()
        {
            properties.Clear(); properties = null;
            startValues.Clear(); startValues = null;
            endValues.Clear(); endValues = null;
        }
    }
}
