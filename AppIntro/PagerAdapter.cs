using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using Object = Java.Lang.Object;

namespace AppIntro
{
    public class PagerAdapter : FragmentPagerAdapter
    {
        private List<Fragment> fragments;
        private Dictionary<int, Fragment> retainedFragments;

        public PagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public PagerAdapter(FragmentManager fm) : base(fm)
        {
        }

        public PagerAdapter(FragmentManager fm, List<Fragment> fragments) : base(fm)
        {
            this.fragments = fragments;
            this.retainedFragments = new Dictionary<int, Fragment>();
        }

        public override Fragment GetItem(int position)
        {
            if (retainedFragments.ContainsKey(position))
            {
                return retainedFragments[position];
            }

            return fragments[position];
        }

        public override Object InstantiateItem(ViewGroup container, int position)
        {
            Fragment fragment = (Fragment) base.InstantiateItem(container, position);
            retainedFragments.Add(position, fragment);

            return fragment;
        }

        public List<Fragment> GetFragments()
        {
            return fragments;
        }

        public List<Fragment> GetRetainedFragments()
        {
            return retainedFragments.Values.ToList();
        }

        public override void DestroyItem(ViewGroup container, int position, Object @object)
        {
            if (retainedFragments.ContainsKey(position))
            {
                retainedFragments.Remove(position);
            }
            base.DestroyItem(container, position, @object);
        }

        public override int Count => this.fragments.Count;
    }
}