/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPS_Counter : MonoBehaviour 
{
 
	// Attach this to a Text to make a frames/second indicator.
	//
	// It calculates frames/second over each updateInterval,
	// so the display does not keep changing wildly.
	//
	// It is also fairly accurate at very low FPS counts (<10).
	// We do this not by simply counting frames per interval, but
	// by accumulating FPS for each frame. This way we end up with
	// correct overall FPS even if the interval renders something like
	// 5.5 frames.

	public  float updateInterval = 0.5F;

	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	private Text FPS_Text;

	void Start()
	{
	timeleft = updateInterval;  
	FPS_Text = transform.GetComponent<Text>();
	}

	void Update()
	{
	timeleft -= Time.deltaTime;
	accum += Time.timeScale/Time.deltaTime;
	++frames;

	// Interval ended - update GUI text and start new interval
	if( timeleft <= 0.0 )
	{
		    // display two fractional digits (f2 format)
		float fps = accum/frames;
		string format = System.String.Format("{0:F2} FPS",fps);
				FPS_Text.text = format;

		if(fps < 25)
					FPS_Text.color = Color.yellow;
		else 
			if(fps < 15)
						FPS_Text.color = Color.red;
			else
					FPS_Text.color = Color.green;

		    timeleft = updateInterval;
		    accum = 0.0F;
		    frames = 0;
	}
	}
}
