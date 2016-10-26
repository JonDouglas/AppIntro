using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AppIntro
{
    public interface IndicatorController
    {
        View NewInstance(Context context);
        void Initialize(int slideCount);
        void SelectPosition(int index);
        void SetSelectedIndicatorColor(int color);
        void SetUnselectedIndicatorColor(int color);
    }
}