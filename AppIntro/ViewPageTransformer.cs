using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace AppIntro
{
    public class ViewPageTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private static float MIN_SCALE_DEPTH = 0.75f;
        private static float MIN_SCALE_ZOOM = 0.85f;
        private static float MIN_ALPHA_ZOOM = 0.5f;
        private static float SCALE_FACTOR_SLIDE = 0.85f;
        private static float MIN_ALPHA_SLIDE = 0.35f;
        private TransformType _transformType;

        public ViewPageTransformer(TransformType transformType)
        {
            _transformType = transformType;
        }

        public void TransformPage(View page, float position)
        {
            float alpha = 1;
            float scale = 1;
            float translationX = 0;

            switch (_transformType)
            {
                case TransformType.FLOW:
                    page.RotationY = position*-30f;
                    return;
                case TransformType.SLIDE_OVER:
                    if (position < 0 && position > -1)
                    {
                        scale = Math.Abs(Math.Abs(position) - 1)*(1.0f - SCALE_FACTOR_SLIDE) + SCALE_FACTOR_SLIDE;
                        alpha = Math.Max(MIN_ALPHA_SLIDE, 1 - Math.Abs(position));
                        int pageWidth = page.Width;
                        float translateValue = position*-pageWidth;
                        if (translateValue > -pageWidth)
                        {
                            translationX = translateValue;
                        }
                        else
                        {
                            translationX = 0;
                        }
                    }
                    else
                    {
                        alpha = 1;
                        scale = 1;
                        translationX = 0;
                    }
                    break;
                case TransformType.DEPTH:
                    if (position > 0 && position < 1)
                    {
                        alpha = (1 - position);
                        scale = MIN_SCALE_DEPTH + (1 - MIN_SCALE_DEPTH)*(1 - Math.Abs(position));
                        translationX = (page.Width*-position);
                    }
                    else
                    {
                        alpha = 1;
                        scale = 1;
                        translationX = 0;
                    }
                    break;
                case TransformType.ZOOM:
                    if (position >= -1 && position <= 1)
                    {
                        scale = Math.Max(MIN_SCALE_ZOOM, 1 - Math.Abs(position));
                        alpha = MIN_ALPHA_ZOOM + (scale - MIN_SCALE_ZOOM)/(1 - MIN_SCALE_ZOOM)*(1 - MIN_ALPHA_ZOOM);
                        float vMargin = page.Height*(1 - scale)/2;
                        float hMargin = page.Width*(1 - scale)/2;
                        if (position < 0)
                        {
                            translationX = (hMargin - vMargin/2);
                        }
                        else
                        {
                            translationX = (-hMargin + vMargin/2);
                        }
                    }
                    else
                    {
                        alpha = 1;
                        scale = 1;
                        translationX = 0;
                    }
                    break;
                case TransformType.FADE:
                    if (position <= -1.0F || position >= 1.0F)
                    {
                        page.Alpha = 0.0F;
                        page.Clickable = false;
                    }
                    else if(position == 0.0F)
                    {
                        page.Alpha = 1.0F;
                        page.Clickable = true;
                    }
                    else
                    {
                        page.Alpha = 1.0F - Math.Abs(position);
                    }
                    break;
                default:
                    return;
            }

            page.Alpha = alpha;
            page.TranslationX = translationX;
            page.ScaleX = scale;
            page.ScaleY = scale;
        }
    }

    public enum TransformType
    {
        FLOW,
        DEPTH,
        ZOOM,
        SLIDE_OVER,
        FADE
    }
}