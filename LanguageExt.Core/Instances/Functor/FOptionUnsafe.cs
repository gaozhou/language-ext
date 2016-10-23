﻿using System;
using LanguageExt.TypeClasses;
using static LanguageExt.Prelude;

namespace LanguageExt.Instances
{
    public struct FOptionUnsafe<A, B> : 
        Functor<OptionUnsafe<A>, OptionUnsafe<B>, A, B>,
        BiFunctor<OptionUnsafe<A>, OptionUnsafe<B>, Unit, A, B>
    {
        public OptionUnsafe<B> BiMap(OptionUnsafe<A> ma, Func<Unit, B> fa, Func<A, B> fb) =>
            ma.IsNone
                ? fa == null
                    ? OptionUnsafe<B>.None
                    : fa(unit)
                : fb == null
                    ? OptionUnsafe<B>.None
                    : fb(ma.Value);

        public OptionUnsafe<B> Map(OptionUnsafe<A> ma, Func<A, B> f) =>
            ma.IsSome
                ? OptionUnsafe<B>.Some(f(ma.Value))
                : OptionUnsafe<B>.None;
    }
}
