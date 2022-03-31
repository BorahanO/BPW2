using UnityEditor;
using UnityEngine;
/*
Implements original Quake light flickering in Unity.
Inspired by this blog post
https://80.lv/articles/valve-reused-the-code-for-flickering-lights-in-alyx-22-years-later/
Can be used as component on a Light, or you can call the static methods to animate anything you want.
Implements original integer based light flickers, which are great for harsh flickers, but blends still animate at 10fps, as originally designed.
Also implemented an interpolated version which linear blends the values.
Additional extra parameters for manual time step and speed.
Quake source code license is GPL, but I only used the 'lightstyles' array, so it's a matter of debate if it applies.
My code can be considered free public domain, so use your own judgement on what rights apply. ¯\_(ツ)_/¯
*/
public class QuakeLightFlicker : MonoBehaviour
{
	public FlickerType flickerType = FlickerType._10_FLUORESCENT_FLICKER;
	public bool interpolated;
	private Light light;

	void Awake()
	{
		light = GetComponent<Light>();
		startIntensity = light.intensity;
	}

	private float startIntensity;

	void Update()
	{
		if (interpolated)
		{
			light.intensity = startIntensity * SampleFlickerInterpolated(flickerType);
		}
		else
		{
			light.intensity = startIntensity * SampleFlicker(flickerType);
		}
	}

	//  v v v  static methods  v v v

	public enum FlickerType
	{
		_0_NORMAL,
		_1_FLICKER,
		_2_SLOW_STRONG_PULSE,
		_3_CANDLE,
		_4_FAST_STROBE,
		_5_GENTLE_PULSE_1,
		_6_FLICKER,
		_7_CANDLE,
		_8_CANDLE,
		_9_SLOW_STROBE,
		_10_FLUORESCENT_FLICKER,
		_11_SLOW_PULSE_NOT_FADE_TO_BLACK,
	}

	// 'm' is normal light, 'a' is no light, 'z' is double bright
	private static string[] lightstyles = {
		"m",
		"mmnmmommommnonmmonqnmmo",
		"abcdefghijklmnopqrstuvwxyzyxwvutsrqponmlkjihgfedcba",
		"mmmmmaaaaammmmmaaaaaabcdefgabcdefg",
		"mamamamamama",
		"jklmnopqrstuvwxyzyxwvutsrqponmlkj",
		"nmonqnmomnmomomno",
		"mmmaaaabcdefgmmmmaaaammmaamm",
		"mmmaaammmaaammmabcdefaaaammmmabcdefmmmaaaa",
		"aaaaaaaazzzzzzzz",
		"mmamammmmammamamaaamammma",
		"abcdefghijklmnopqrrqponmlkjihgfedcba",
	};

	public static float SampleFlicker(FlickerType type, float speed = 1f / 10f)
	{ // speed defaults to 10fps
		return SampleFlickerAtTime(Time.time, type, speed);
	}

	public static float SampleFlickerAtTime(float time, FlickerType type, float speed = 1f / 10f)
	{
		// implementation in quake r_light.c;
		// k = i % cl_lightstyle[j].length;
		// k = cl_lightstyle[j].map[k] - 'a';
		// k = k*22;
		// d_lightstylevalue[j] = k;

		time /= speed;
		string style = lightstyles[(int)type];
		float value = style[((int)time) % style.Length] - 'a';
		value /= 'm' - 'a'; // using 'm' as pivot point as original integer math does not result in 1.0
		return value;
	}

	//  v v v  interpolated implementation  v v v

	private static AnimationCurve[] lightstyle_curves = { };

	static void InitCurves()
	{
		lightstyle_curves = new AnimationCurve[lightstyles.Length];
		for (int i = 0; i < lightstyles.Length; i++)
		{
			AnimationCurve c = new AnimationCurve();
			// c.postWrapMode = c.preWrapMode = WrapMode.Loop; // does not interpolate loop, so useless 
			for (int v = 0; v < lightstyles[i].Length; v++)
			{
				c.AddKey((float)v, (float)(lightstyles[i][v] - 'a') / (float)('m' - 'a'));
			}
			c.AddKey((float)lightstyles[i].Length, (float)(lightstyles[i][0] - 'a') / (float)('m' - 'a')); // AnimationCurve does not interpolate between ends, so re-add first frame

			for (int v = 0; v < c.length; v++)
			{ // set linear to avoid overshoot
				//AnimationUtility.SetKeyLeftTangentMode(c, v, AnimationUtility.TangentMode.Linear);
				//AnimationUtility.SetKeyRightTangentMode(c, v, AnimationUtility.TangentMode.Linear);
			}
			lightstyle_curves[i] = c;
		}
	}

	public static float SampleFlickerInterpolated(FlickerType type, float speed = 1f / 10f)
	{ // speed defaults to 10fps
		return SampleFlickerInterpolatedAtTime(Time.time, type, speed);
	}

	public static float SampleFlickerInterpolatedAtTime(float time, FlickerType type, float speed = 1f / 10f)
	{
		if (lightstyle_curves.Length == 0)
		{
			InitCurves();
		}
		AnimationCurve lightstyle = lightstyle_curves[(int)type];
		return lightstyle.Evaluate((time / speed) % lightstyle[lightstyle.length - 1].time);
	}
}