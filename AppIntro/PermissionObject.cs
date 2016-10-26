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
    public class PermissionObject
    {
        private string[] permission { get; set; }
        private int position { get; set; }

        public PermissionObject(string[] permission, int position)
        {
            this.permission = permission;
            this.position = position;
        }

        public string[] GetPermission()
        {
            return permission;
        }

        public int GetPosition()
        {
            return position;
        }
    }
}