using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AppIntro.Utils
{
    public class CustomFontCache
    {
        //private static final String TAG = LogHelper.makeLogTag(CustomFontCache.class);

        private static Dictionary<string, Typeface> _cache = new Dictionary<string, Typeface>();

        public static Typeface Get(string tfn, Context ctx)
        {
            Typeface tf = _cache[tfn];
            if (tf == null)
            {
                try
                {
                    tf = Typeface.CreateFromAsset(ctx.Assets, tfn);
                    if (tf != null)
                    {
                        _cache.Add(tfn, tf);
                    }
                }
                catch (Exception ex)
                {
                    if (("").Equals(tfn))
                    {
                        //LogHelper.w(TAG, e, "Empty path");
                    }
                    else
                    {
                        //LogHelper.w(TAG, e, tfn);
                    }
                    return null;
                }
            }
            else
            {
                return tf;
            }

            return null;
        }
    }
}