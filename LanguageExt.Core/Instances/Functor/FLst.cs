﻿using System;
using LanguageExt.TypeClasses;
using static LanguageExt.Prelude;
using System.Collections.Generic;

namespace LanguageExt.Instances
{
    public struct FLst<A, B> : 
        Functor<Lst<A>, Lst<B>, A, B>
    {
        public Lst<B> Map(Lst<A> ma, Func<A, B> f) =>
            ma.Map(f);
    }
}
