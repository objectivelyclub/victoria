using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(victoria.iOS.BlockingCollections_iOS))]

namespace victoria.iOS
{
    class BlockingCollections_iOS : iBlockingCollections
    {
        private BlockingCollection<Action> action;
        private BlockingCollection<string[]> stringarr;
        public BlockingCollections_iOS()
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
