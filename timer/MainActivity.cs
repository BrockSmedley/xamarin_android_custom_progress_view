using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.Animations;
using Android.Util;
using MyTimer;


namespace timer {
	[Activity(Label = "timer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {
		Button button_short, button_long;
		TextView textView_timer;
		int intervalInMillis = 1000; //1 sec
		int timeInMillis, timeLeftInMillis;
		bool timerStarted;
		float progress;

		CountDown countdown;
		ProgressCircleView progressCircle;
		
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);


			//instantiate views
			button_short = FindViewById<Button>(Resource.Id.button_short);
			button_long = FindViewById<Button>(Resource.Id.button_long);
			textView_timer = FindViewById<TextView>(Resource.Id.textView_timer);
			progressCircle = FindViewById<ProgressCircleView>(Resource.Id.progressCircle);

			timerStarted = false;//timers start non-running

			// Get our button from the layout resource
			// and attach an event to it

			//TODO: remove duplicatable code
			button_short.Click += delegate {
				runTimer(5);
			};

			button_long.Click += delegate {
				runTimer(10);
			};
		}

		//this should be in CountDown.cs -- move this method & associated variables
		private void runTimer(int timeInSecs) {
			timeLeftInMillis = getTimeLeftInMillis(timeInSecs, intervalInMillis);//display this

			//start a new timer
			if (!timerStarted) {
				timeInMillis = getTimeInMillis(timeInSecs);

				countdown = new CountDown(timeInMillis, intervalInMillis);
				countdown.Start();

				//happens every interval in milliseconds
				countdown.Tick += delegate {
					int outputTime = timeLeftInMillis / 1000;
					
					timeLeftInMillis -= intervalInMillis;
					
					string outputText;
					
					if (outputTime == 0) {
						outputText = "Done";
						timerStarted = false;
						countdown.Cancel();
						progress = 100f;
					}
					else {
						outputText = outputTime.ToString();
						timerStarted = true;

						progress = (100f * ((float)(timeInMillis - timeLeftInMillis) / (float)timeInMillis) - intervalInMillis/100f);
						Log.Debug("runTimer_progress", progress.ToString());
					}

					textView_timer.SetText(outputText, TextView.BufferType.Normal);

					progressCircle.StartTimerAnimation(timeInSecs, (float)progress);
				};
			}
			else { //reset the timer
				timerStarted = false;
				countdown.Cancel();
				runTimer(timeInSecs);	
			}	
		}

		private int getTimeInMillis(int timeInSecs) {
			return (timeInSecs * intervalInMillis) + (2 * intervalInMillis);//add 2 intervals to be able to count to 0 from 5 instead of 4 to 1, for example
		}
		private int getTimeLeftInMillis(int timeInSecs, int intervalInMillis){
			return timeInSecs * intervalInMillis;
		}

	}
}

