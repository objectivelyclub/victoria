﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace victoria
{
    public interface iBlockingCollections
    {
        void Add(string[] s);
        void Add(Action a);

        string[] strTake();
        Action actTake();

    }
}