using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace itfantasy.unii
{
    public delegate float TransitFunction(float ratio);

    public class Transitions
    {

        public static string LINEAR = "linear";
        public static string EASE_IN = "easeIn";
        public static string EASE_OUT = "easeOut";
        public static string EASE_IN_OUT = "easeInOut";
        public static string EASE_OUT_IN = "easeOutIn";
        public static string EASE_IN_BACK = "easeInBack";
        public static string EASE_OUT_BACK = "easeOutBack";
        public static string EASE_IN_OUT_BACK = "easeInOutBack";
        public static string EASE_OUT_IN_BACK = "easeOutInBack";
        public static string EASE_IN_ELASTIC = "easeInElastic";
        public static string EASE_OUT_ELASTIC = "easeOutElastic";
        public static string EASE_IN_OUT_ELASTIC = "easeInOutElastic";
        public static string EASE_OUT_IN_ELASTIC = "easeOutInElastic";
        public static string EASE_IN_BOUNCE = "easeInBounce";
        public static string EASE_OUT_BOUNCE = "easeOutBounce";
        public static string EASE_IN_OUT_BOUNCE = "easeInOutBounce";
        public static string EASE_OUT_IN_BOUNCE = "easeOutInBounce";

        private static Dictionary<string, TransitFunction> transitions;

        public static TransitFunction getTransition(string name)
        {
            if (transitions == null) registerDefaults();
            return transitions[name];
        }

        private static void registerDefaults()
        {
            transitions = new Dictionary<string, TransitFunction>();
            transitions.Add(LINEAR, linear);
            transitions.Add(EASE_IN, easeIn);
            transitions.Add(EASE_OUT, easeOut);
            transitions.Add(EASE_IN_OUT, easeInOut);
            transitions.Add(EASE_OUT_IN, easeOutIn);
            transitions.Add(EASE_IN_BACK, easeInBack);
            transitions.Add(EASE_OUT_BACK, easeOutBack);
            transitions.Add(EASE_IN_OUT_BACK, easeInOutBack);
            transitions.Add(EASE_OUT_IN_BACK, easeOutInBack);
            transitions.Add(EASE_IN_ELASTIC, easeInElastic);
            transitions.Add(EASE_OUT_ELASTIC, easeOutElastic);
            transitions.Add(EASE_IN_OUT_ELASTIC, easeInOutElastic);
            transitions.Add(EASE_OUT_IN_ELASTIC, easeOutInElastic);
            transitions.Add(EASE_IN_BOUNCE, easeInBounce);
            transitions.Add(EASE_OUT_BOUNCE, easeOutBounce);
            transitions.Add(EASE_IN_OUT_BOUNCE, easeInOutBounce);
            transitions.Add(EASE_OUT_IN_BOUNCE, easeOutInBounce);
        }

        private static float linear(float ratio)
        {
            return ratio;
        }

        private static float easeIn(float ratio)
        {
            return ratio * ratio * ratio;
        }

        private static float easeOut(float ratio)
        {
            float invRatio = ratio - 1.0f;
            return invRatio * invRatio * invRatio + 1;
        }

        private static float easeInOut(float ratio)
        {
            return easeCombined(easeIn, easeOut, ratio);
        }

        private static float easeOutIn(float ratio)
        {
            return easeCombined(easeOut, easeIn, ratio);
        }

        private static float easeInBack(float ratio)
        {
            float s = 1.70158f;
            return Mathf.Pow(ratio, 2) * ((s + 1.0f) * ratio - s);
        }

        private static float easeOutBack(float ratio)
        {
            float invRatio = ratio - 1.0f;
            float s = 1.70158f;
            return Mathf.Pow(invRatio, 2) * ((s + 1.0f) * invRatio + s) + 1.0f;
        }

        private static float easeInOutBack(float ratio)
        {
            return easeCombined(easeInBack, easeOutBack, ratio);
        }

        private static float easeOutInBack(float ratio)
        {
            return easeCombined(easeOutBack, easeInBack, ratio);
        }

        private static float easeInElastic(float ratio)
        {
            if (ratio == 0 || ratio == 1) return ratio;
            else
            {
                float p = 0.3f;
                float s = p / 4.0f;
                float invRatio = ratio - 1;
                return -1.0f * Mathf.Pow(2.0f, 10.0f * invRatio) * Mathf.Sin((invRatio - s) * (2.0f * Mathf.PI) / p);
            }
        }

        private static float easeOutElastic(float ratio)
        {
            if (ratio == 0 || ratio == 1) return ratio;
            else
            {
                float p = 0.3f;
                float s = p / 4.0f;
                return Mathf.Pow(2.0f, -10.0f * ratio) * Mathf.Sin((ratio - s) * (2.0f * Mathf.PI) / p) + 1;
            }
        }

        private static float easeInOutElastic(float ratio)
        {
            return easeCombined(easeInElastic, easeOutElastic, ratio);
        }

        private static float easeOutInElastic(float ratio)
        {
            return easeCombined(easeOutElastic, easeInElastic, ratio);
        }

        private static float easeInBounce(float ratio)
        {
            return 1.0f - easeOutBounce(1.0f - ratio);
        }

        private static float easeOutBounce(float ratio)
        {
            float s = 7.5625f;
            float p = 2.75f;
            float l;
            if (ratio < (1.0 / p))
            {
                l = s * Mathf.Pow(ratio, 2);
            }
            else
            {
                if (ratio < (2.0 / p))
                {
                    ratio -= 1.5f / p;
                    l = s * Mathf.Pow(ratio, 2) + 0.75f;
                }
                else
                {
                    if (ratio < 2.5f / p)
                    {
                        ratio -= 2.25f / p;
                        l = s * Mathf.Pow(ratio, 2) + 0.9375f;
                    }
                    else
                    {
                        ratio -= 2.625f / p;
                        l = s * Mathf.Pow(ratio, 2) + 0.984375f;
                    }
                }
            }
            return l;
        }

        private static float easeInOutBounce(float ratio)
        {
            return easeCombined(easeInBounce, easeOutBounce, ratio);
        }

        private static float easeOutInBounce(float ratio)
        {
            return easeCombined(easeOutBounce, easeInBounce, ratio);
        }

        private static float easeCombined(TransitFunction startFunc, TransitFunction endFunc, float ratio)
        {
            if (ratio < 0.5f) return 0.5f * startFunc(ratio * 2.0f);
            else return 0.5f * endFunc((ratio - 0.5f) * 2.0f) + 0.5f;
        }
    }
}
