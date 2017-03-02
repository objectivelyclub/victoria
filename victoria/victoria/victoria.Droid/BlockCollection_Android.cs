using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using victoria.Droid;
using victoria;

[assembly: Dependency(typeof(victoria.Droid.BlockingCollections_Android))]

namespace victoria.Droid
{
    public class BlockingCollections_Android: iBlockingCollections
    {
        private BlockingCollection<Action> action;
        private BlockingCollection<string[]> stringarr;
        public BlockingCollections_Android()
        {
            action = new BlockingCollection<Action>();
            stringarr = new BlockingCollection<string[]>();
        }


        public Action actTake()
        {
            return action.Take();
        }

        public void Add(Action a)
        {
            action.Add(a);
        }

        public void Add(string[] s)
        {
            stringarr.Add(s);
        }

        public string[] strTake()
        {
            return stringarr.Take();
        }
    }
}