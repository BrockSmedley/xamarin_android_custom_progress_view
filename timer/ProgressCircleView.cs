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
	class ProgressCircleView : View {
		Context _context;
		Canvas _canvas;
		float progressPercentage, interval;

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
		}

		protected override void OnDraw(Android.Graphics.Canvas canvas) {
			//base.OnDraw(canvas);

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

		void DrawArc(Canvas canvas) {
			float sweepAngle = progressPercentage * 360;

			RectF oval = new RectF(10, 10, Width - 10, Height - 10);
			canvas.DrawArc(oval, -90, sweepAngle, false, ProgressPaint);
		}

		public void UpdateArc(float progressPercentage) {
			Invalidate();
			
			//animate this value
			this.progressPercentage = progressPercentage;
		}
	}
}