using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Android.Util;

namespace timer {
	class ProgressCircleView : View, ValueAnimator.IAnimatorUpdateListener {
		Context _context;
		Canvas _canvas;
		ValueAnimator animation;
		float progressPercentage, intervalPercentage, additionalPercentage;
		int updates = 0;

		public Paint ProgressPaint { get; set; }

		public ProgressCircleView(Context context)
			: base(context) {
				Init(context);
		}
		public ProgressCircleView(Context context, IAttributeSet attrs)
			: base(context, attrs) {
				Init(context);
		}
		public ProgressCircleView(Context context, IAttributeSet attrs, int defStyleAttr) 
			: base(context, attrs, defStyleAttr){
				Init(context);
		}

		void Init(Context context){
			this._context = context;

			ProgressPaint = new Paint();
			ProgressPaint.Color = Color.Red;

			ProgressPaint.AntiAlias = true;
			ProgressPaint.SetStyle(Paint.Style.Stroke);
			ProgressPaint.StrokeWidth = 10;

			animation = ValueAnimator.OfInt(0, 100);//"animate" this value from 0-100
			animation.SetDuration(1000);
			animation.AddUpdateListener(this);
			animation.SetInterpolator(new DecelerateInterpolator());

			progressPercentage = 0;
			updates = 0;
		}

		protected override void OnDraw(Android.Graphics.Canvas canvas) {
			base.OnDraw(canvas);
			DrawArc(canvas);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
			//base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

			int desiredWidth = 400;
			int desiredHeight = 400;

			var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
			var heightMode = MeasureSpec.GetMode(heightMeasureSpec);

			var widthSize = MeasureSpec.GetSize(widthMeasureSpec);
			var heightSize = MeasureSpec.GetSize(heightMeasureSpec);

			int width, height;

			//Measure Width
			if (widthMode == MeasureSpecMode.Exactly) {
				//Must be this size
				width = widthSize;
			}
			else if (widthMode == MeasureSpecMode.AtMost) {
				//Can't be bigger than...
				width = Math.Min(desiredWidth, widthSize);
			}
			else {
				//Be whatever you want
				width = desiredWidth;
			}

			//Measure Height
			if (heightMode == MeasureSpecMode.Exactly) {
				//Must be this size
				height = heightSize;
			}
			else if (heightMode == MeasureSpecMode.AtMost) {
				//Can't be bigger than...
				height = Math.Min(desiredHeight, heightSize);
			}
			else {
				//Be whatever you want
				height = desiredHeight;
			}

			SetMeasuredDimension(width, height);
		}

		public void OnAnimationUpdate(ValueAnimator animator) {
			UpdateArc(((float)animator.AnimatedValue));

			Log.Debug("progressPercentage_OnAnimationUpdate", progressPercentage.ToString());
			Log.Debug("animator.AnimatedValue_OnAnimationUpdate", animator.AnimatedValue.ToString());
			Log.Debug("interval_OnAnimationUpdate", intervalPercentage.ToString());
		}

		void DrawArc(Canvas canvas) {
			RectF oval = new RectF(10, 10, Width - 10, Height - 10);

			float startStatic = -90f;
			float endStatic = (intervalPercentage/100f) * 360f * (updates-1);

			float startDynamic = endStatic + startStatic;
			float endDynamic = ((additionalPercentage) / 100f) * (intervalPercentage / 100f) * 360f;

			//runs instantly
			canvas.DrawArc(oval, startStatic, endStatic, false, ProgressPaint);
			//runs incrementally by animation
			canvas.DrawArc(oval, startDynamic, endDynamic, false, ProgressPaint);

			Log.Debug("endStatic_DrawArc", endStatic.ToString());
			Log.Debug("startDynamic_DrawArc", startDynamic.ToString());
			Log.Debug("endDynamic_DrawArc", endDynamic.ToString());
		}

		public void UpdateArc(float additionalPercentage) {
			this.additionalPercentage = additionalPercentage;

			Invalidate();

			Log.Debug("additionalPercentage_UpdateArc", additionalPercentage.ToString());
		}

		public void StartTimerAnimation(float durationInSeconds, float progressPercentage) {
			this.progressPercentage = progressPercentage;
			this.intervalPercentage = 100f / durationInSeconds;
			additionalPercentage = 0;

			animation.Start();

			updates++;

			Log.Debug("progressPercentage_StartTimerAnimation", progressPercentage.ToString());
		}

		
	}
}