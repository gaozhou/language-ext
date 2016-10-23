﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.TypeClass;
using LanguageExt.TypeClasses;
using LanguageExt.Instances;

namespace LanguageExt
{
    public static partial class Prelude
    {
        /// <summary>
        /// Difference the Ts
        /// </summary>
        /// <param name="lhs">Left-hand side of the operation</param>
        /// <param name="rhs">Right-hand side of the operation</param>
        /// <returns>lhs - rhs</returns>
        [Pure]
        public static Option<T> difference<DIFF, T>(Option<T> lhs, Option<T> rhs) where DIFF : struct, Difference<T> =>
            lhs.Difference<DIFF, T>(rhs);

        /// <summary>
        /// Find the product of the Ts
        [Pure]
        public static Option<T> product<PROD, T>(Option<T> lhs, Option<T> rhs) where PROD : struct, Product<T> =>
            lhs.Product<PROD, T>(rhs);

        /// <summary>
        /// Divide the Ts
        /// </summary>
        /// <param name="lhs">Left-hand side of the operation</param>
        /// <param name="rhs">Right-hand side of the operation</param>
        /// <returns>lhs / rhs</returns>
        [Pure]
        public static Option<T> divide<DIV, T>(Option<T> lhs, Option<T> rhs) where DIV : struct, Divisible<T> =>
            lhs.Divide<DIV, T>(rhs);

        /// <summary>
        /// Add the Ts
        /// </summary>
        /// <param name="lhs">Left-hand side of the operation</param>
        /// <param name="rhs">Right-hand side of the operation</param>
        /// <returns>lhs / rhs</returns>
        [Pure]
        public static Option<T> add<ADD, T>(Option<T> lhs, Option<T> rhs) where ADD : struct, Addition<T> =>
            lhs.Add<ADD, T>(rhs);

        /// <summary>
        /// Check if Option is in a Some state
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">Option</param>
        /// <returns>True if value is in a Some state</returns>
        [Pure]
        public static bool isSome<T>(Option<T> value) =>
            value.IsSome;

        /// <summary>
        /// Check if Option is in a None state
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">Option</param>
        /// <returns>True if value is in a None state</returns>
        [Pure]
        public static bool isNone<T>(Option<T> value) =>
            value.IsNone;

        /// <summary>
        /// Create a Some of T (Option<T>)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">Non-null value to be made optional</param>
        /// <returns>Option<T> in a Some state or throws ValueIsNullException
        /// if isnull(value).</returns>
        [Pure]
        public static Option<T> Some<T>(T value) =>
            isnull(value)
                ? raise<Option<T>>(new ValueIsNullException())
                : default(MOption<T>).Return(value);

        /// <summary>
        /// Create a Some of T from a Nullable<T> (Option<T>)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">Non-null value to be made optional</param>
        /// <returns>Option<T> in a Some state or throws ValueIsNullException
        /// if isnull(value)</returns>
        [Pure]
        public static Option<T> Some<T>(T? value) where T : struct =>
            value.HasValue
                ? default(MOption<T>).Return(value.Value)
                : raise<Option<T>>(new ValueIsNullException());

        /// <summary>
        /// Create an Option
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">Value to be made optional, or null</param>
        /// <returns>If the value is null it will be None else Some(value)</returns>
        [Pure]
        public static Option<T> Optional<T>(T value) =>
            default(MOption<T>).Return(value);

        /// <summary>
        /// Create a lazy Some of T (Option<T>)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="f">A function that returns the value to construct the option with</param>
        /// <param name="memo">True to memoize the value so it's only fetched once</param>
        /// <returns>A lazy Option<T></returns>
        [Pure]
        public static Option<T> Optional<T>(Func<Option<T>> f, bool memo = true) =>
            f == null
                ? raise<Option<T>>(new ArgumentNullException(nameof(f)))
                : new Option<T>(new LazyOption2<T>(f, memo));

        /// <summary>
        /// Create an Option
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">Value to be made optional, or null</param>
        /// <returns>If the value is null it will be None else Some(value)</returns>
        [Pure]
        public static Option<T> Optional<T>(T? value) where T : struct =>
            value.HasValue
                ? default(MOption<T>).Return(value.Value)
                : Option<T>.None;

        /// <summary>
        /// Invokes the action if Option is in the Some state, otherwise nothing happens.
        /// </summary>
        /// <param name="f">Action to invoke if Option is in the Some state</param>
        public static Unit ifSome<T>(Option<T> option, Action<T> Some) => 
            option.IfSome(Some);

        /// <summary>
        /// Returns the result of invoking the None() operation if the optional 
        /// is in a None state, otherwise the bound Some(x) value is returned.
        /// </summary>
        /// <remarks>Will not accept a null return value from the None operation</remarks>
        /// <param name="None">Operation to invoke if the structure is in a None state</param>
        /// <returns>Tesult of invoking the None() operation if the optional 
        /// is in a None state, otherwise the bound Some(x) value is returned.</returns>
        [Pure]
        public static T ifNone<T>(Option<T> option, Func<T> None) =>
            option.IfNone(None);

        /// <summary>
        /// Returns the noneValue if the optional is in a None state, otherwise
        /// the bound Some(x) value is returned.
        /// </summary>
        /// <remarks>Will not accept a null noneValue</remarks>
        /// <param name="noneValue">Value to return if in a None state</param>
        /// <returns>noneValue if the optional is in a None state, otherwise
        /// the bound Some(x) value is returned</returns>
        [Pure]
        public static T ifNone<T>(Option<T> option, T noneValue) =>
            option.IfNone(noneValue);

        /// <summary>
        /// Returns the result of invoking the None() operation if the optional 
        /// is in a None state, otherwise the bound Some(x) value is returned.
        /// </summary>
        /// <remarks>Will allow null the be returned from the None operation</remarks>
        /// <param name="None">Operation to invoke if the structure is in a None state</param>
        /// <returns>Tesult of invoking the None() operation if the optional 
        /// is in a None state, otherwise the bound Some(x) value is returned.</returns>
        [Pure]
        public static T ifNoneUnsafe<T>(Option<T> option, Func<T> None) =>
            option.IfNoneUnsafe(None);

        /// <summary>
        /// Returns the noneValue if the optional is in a None state, otherwise
        /// the bound Some(x) value is returned.
        /// </summary>
        /// <remarks>Will allow noneValue to be null</remarks>
        /// <param name="noneValue">Value to return if in a None state</param>
        /// <returns>noneValue if the optional is in a None state, otherwise
        /// the bound Some(x) value is returned</returns>
        [Pure]
        public static T ifNoneUnsafe<T>(Option<T> option, T noneValue) =>
            option.IfNoneUnsafe(noneValue);

        /// <summary>
        /// Match the two states of the Option and return a non-null R.
        /// </summary>
        /// <typeparam name="B">Return type</typeparam>
        /// <param name="Some">Some match operation. Must not return null.</param>
        /// <param name="None">None match operation. Must not return null.</param>
        /// <returns>A non-null B</returns>
        [Pure]
        public static R match<T, R>(Option<T> option, Func<T, R> Some, Func<R> None) =>
            option.Match(Some, None);

        /// <summary>
        /// Match the two states of the Option and return a B, which can be null.
        /// </summary>
        /// <typeparam name="B">Return type</typeparam>
        /// <param name="Some">Some match operation. May return null.</param>
        /// <param name="None">None match operation. May return null.</param>
        /// <returns>B, or null</returns>
        [Pure]
        public static R matchUnsafe<T, R>(Option<T> option, Func<T, R> Some, Func<R> None) =>
            option.MatchUnsafe(Some, None);

        /// <summary>
        /// Match the two states of the Option
        /// </summary>
        /// <param name="Some">Some match operation</param>
        /// <param name="None">None match operation</param>
        public static Unit match<T>(Option<T> option, Action<T> Some, Action None) =>
            option.Match(Some, None);

        /// <summary>
        /// Apply an Optional argument to an Optional function of arity 1
        /// </summary>
        /// <param name="option">Optional function</param>
        /// <param name="x">Argument to apply</param>
        /// <returns>Returns the result of applying the optional argument to the optional function</returns>
        [Pure]
        public static Option<B> apply<A, B>(Option<Func<A, B>> x, Option<A> y) =>
            apply<MOption<Func<A, B>>, MOption<A>, MOption<B>, Option<Func<A, B>>, Option<A>, Option<B>, A, B>(x, y);

        /// <summary>
        /// Apply two Optional arguments to an Optional function of arity 2
        /// </summary>
        /// <param name="option">Optional function</param>
        /// <param name="x">Argument to apply</param>
        /// <param name="y">Argument to apply</param>
        /// <returns>Returns the result of applying the optional argument to the optional function</returns>
        [Pure]
        public static Option<C> apply<A, B, C>(Option<Func<A, B, C>> x, Option<A> y, Option<B> z) =>
            apply<MOption<Func<A, B, C>>, MOption<A>, MOption<B>, MOption<C>, Option<Func<A, B, C>>, Option<A>, Option<B>, Option<C>, A, B, C>(x, y, z);

        /// <summary>
        /// Apply one Optional arguments to an Optional function of arity 2
        /// </summary>
        /// <param name="option">Optional function</param>
        /// <param name="x">Argument to apply</param>
        /// <returns>Returns the result of applying the optional argument to the optional function</returns>
        [Pure]
        public static Option<Func<B, C>> apply<A, B, C>(Option<Func<A, B, C>> x, Option<A> y) =>
            apply<MOption<Func<A, B, C>>, MOption<A>, MOption<Func<B, C>>, Option<Func<A, B, C>>, Option<A>, Option<Func<B, C>>, A, B, C>(x, y);

        /// <summary>
        /// Apply one Optional arguments to an Optional function of arity 2
        /// </summary>
        /// <param name="option">Optional function</param>
        /// <param name="x">Argument to apply</param>
        /// <returns>Returns the result of applying the optional argument to the optional function</returns>
        [Pure]
        public static Option<Func<B, C>> apply<A, B, C>(Option<Func<A, Func<B, C>>> x, Option<A> y) =>
            apply2<MOption<Func<A, Func<B, C>>>, MOption<A>, MOption<Func<B, C>>, Option<Func<A, Func<B, C>>>, Option<A>, Option<Func<B, C>>, A, B, C>(x, y);

        /// <summary>
        /// Partially apply an Optional argument to a curried Optional function
        /// </summary>
        [Pure]
        public static Option<B> action<A, B>(Option<A> x, Option<B> y) =>
            action<MOption<A>, MOption<B>, Option<A>, Option<B>, A, B>(x, y);

        /// <summary>
        /// <para>
        /// Option types are like lists of 0 or 1 items, and therefore follow the 
        /// same rules when folding.
        /// </para><para>
        /// In the case of lists, 'Fold', when applied to a binary
        /// operator, a starting value(typically the left-identity of the operator),
        /// and a list, reduces the list using the binary operator, from left to
        /// right:
        /// </para><para>
        /// Fold([x1, x2, ..., xn] == x1 `f` (x2 `f` ... (xn `f` z)...)
        /// </para><para>
        /// Note that, since the head of the resulting expression is produced by
        /// an application of the operator to the first element of the list,
        /// 'Fold' can produce a terminating expression from an infinite list.
        /// </para>
        /// </summary>
        /// <typeparam name="S">Aggregate state type</typeparam>
        /// <param name="state">Initial state</param>
        /// <param name="folder">Folder function, applied if Option is in a Some state</param>
        /// <returns>The aggregate state</returns>
        [Pure]
        public static S fold<S, A>(Option<A> option, S state, Func<S, A, S> folder) =>
            option.Fold(state, folder);

        /// <summary>
        /// <para>
        /// Option types are like lists of 0 or 1 items, and therefore follow the 
        /// same rules when folding.
        /// </para><para>
        /// In the case of lists, 'Fold', when applied to a binary
        /// operator, a starting value(typically the left-identity of the operator),
        /// and a list, reduces the list using the binary operator, from left to
        /// right:
        /// </para><para>
        /// Fold([x1, x2, ..., xn] == x1 `f` (x2 `f` ... (xn `f` z)...)
        /// </para><para>
        /// Note that, since the head of the resulting expression is produced by
        /// an application of the operator to the first element of the list,
        /// 'Fold' can produce a terminating expression from an infinite list.
        /// </para>
        /// </summary>
        /// <typeparam name="S">Aggregate state type</typeparam>
        /// <param name="state">Initial state</param>
        /// <param name="Some">Folder function, applied if Option is in a Some state</param>
        /// <param name="None">Folder function, applied if Option is in a None state</param>
        /// <returns>The aggregate state</returns>
        [Pure]
        public static S bifold<S, A>(Option<A> option, S state, Func<S, A, S> Some, Func<S, S> None) =>
            option.BiFold(state, Some, None);

        /// <summary>
        /// <para>
        /// Option types are like lists of 0 or 1 items, and therefore follow the 
        /// same rules when folding.
        /// </para><para>
        /// In the case of lists, 'Fold', when applied to a binary
        /// operator, a starting value(typically the left-identity of the operator),
        /// and a list, reduces the list using the binary operator, from left to
        /// right:
        /// </para><para>
        /// Fold([x1, x2, ..., xn] == x1 `f` (x2 `f` ... (xn `f` z)...)
        /// </para><para>
        /// Note that, since the head of the resulting expression is produced by
        /// an application of the operator to the first element of the list,
        /// 'Fold' can produce a terminating expression from an infinite list.
        /// </para>
        /// </summary>
        /// <typeparam name="S">Aggregate state type</typeparam>
        /// <param name="state">Initial state</param>
        /// <param name="Some">Folder function, applied if Option is in a Some state</param>
        /// <param name="None">Folder function, applied if Option is in a None state</param>
        /// <returns>The aggregate state</returns>
        [Pure]
        public static S bifold<S, A>(Option<A> option, S state, Func<S, A, S> Some, Func<S, Unit, S> None) =>
            option.BiFold(state, Some, None);

        /// <summary>
        /// Apply a predicate to the bound value.  If the Option is in a None state
        /// then True is returned (because the predicate applies for-all values).
        /// If the Option is in a Some state the value is the result of running 
        /// applying the bound value to the predicate supplied.        
        /// </summary>
        /// <param name="pred">Predicate to apply</param>
        /// <returns>If the Option is in a None state then True is returned (because 
        /// the predicate applies for-all values).  If the Option is in a Some state
        /// the value is the result of running applying the bound value to the 
        /// predicate supplied.</returns>
        [Pure]
        public static bool forall<A>(Option<A> option, Func<A, bool> pred) =>
            option.ForAll(pred);

        /// <summary>
        /// Apply a predicate to the bound value.  If the Option is in a None state
        /// then True is returned if invoking None returns True.
        /// If the Option is in a Some state the value is the result of running 
        /// applying the bound value to the Some predicate supplied.        
        /// </summary>
        /// <param name="Some">Predicate to apply if in a Some state</param>
        /// <param name="None">Predicate to apply if in a None state</param>
        /// <returns>If the Option is in a None state then True is returned if 
        /// invoking None returns True. If the Option is in a Some state the value 
        /// is the result of running applying the bound value to the Some predicate 
        /// supplied.</returns>
        [Pure]
        public static bool biforall<A>(Option<A> option, Func<A, bool> Some, Func<Unit, bool> None) =>
            option.BiForAll(Some, None);

        /// <summary>
        /// Apply a predicate to the bound value.  If the Option is in a None state
        /// then True is returned if invoking None returns True.
        /// If the Option is in a Some state the value is the result of running 
        /// applying the bound value to the Some predicate supplied.        
        /// </summary>
        /// <param name="Some">Predicate to apply if in a Some state</param>
        /// <param name="None">Predicate to apply if in a None state</param>
        /// <returns>If the Option is in a None state then True is returned if 
        /// invoking None returns True. If the Option is in a Some state the value 
        /// is the result of running applying the bound value to the Some predicate 
        /// supplied.</returns>
        [Pure]
        public static bool biforall<A>(Option<A> option, Func<A, bool> Some, Func<bool> None) =>
            option.BiForAll(Some, None);

        /// <summary>
        /// <para>
        /// Return the number of bound values in this structure:
        /// </para>
        /// <para>
        ///     None = 0
        /// </para>
        /// <para>
        ///     Some = 1
        /// </para>
        /// </summary>
        /// <returns></returns>
        [Pure]
        public static int count<A>(Option<A> option) =>
            option.Count();

        /// <summary>
        /// Apply a predicate to the bound value.  If the Option is in a None state
        /// then True is returned if invoking None returns True.
        /// If the Option is in a Some state the value is the result of running 
        /// applying the bound value to the Some predicate supplied.        
        /// </summary>
        /// <param name="pred">Predicate to apply</param>
        /// <returns>If the Option is in a None state then True is returned if 
        /// invoking None returns True. If the Option is in a Some state the value 
        /// is the result of running applying the bound value to the Some predicate 
        /// supplied.</returns>
        [Pure]
        public static bool exists<A>(Option<A> option, Func<A, bool> pred) =>
            option.Exists(pred);

        /// <summary>
        /// Apply a predicate to the bound value.  If the Option is in a None state
        /// then True is returned if invoking None returns True.
        /// If the Option is in a Some state the value is the result of running 
        /// applying the bound value to the Some predicate supplied.        
        /// </summary>
        /// <param name="Some">Predicate to apply if in a Some state</param>
        /// <param name="None">Predicate to apply if in a None state</param>
        /// <returns>If the Option is in a None state then True is returned if 
        /// invoking None returns True. If the Option is in a Some state the value 
        /// is the result of running applying the bound value to the Some predicate 
        /// supplied.</returns>
        [Pure]
        public static bool biexists<A>(Option<A> option, Func<A, bool> Some, Func<Unit, bool> None) =>
            option.BiExists(Some, None);

        /// <summary>
        /// Apply a predicate to the bound value.  If the Option is in a None state
        /// then True is returned if invoking None returns True.
        /// If the Option is in a Some state the value is the result of running 
        /// applying the bound value to the Some predicate supplied.        
        /// </summary>
        /// <param name="Some">Predicate to apply if in a Some state</param>
        /// <param name="None">Predicate to apply if in a None state</param>
        /// <returns>If the Option is in a None state then True is returned if 
        /// invoking None returns True. If the Option is in a Some state the value 
        /// is the result of running applying the bound value to the Some predicate 
        /// supplied.</returns>
        [Pure]
        public static bool biexists<A>(Option<A> option, Func<A, bool> Some, Func<bool> None) =>
            option.BiExists(Some, None);

        /// <summary>
        /// Projection from one value to another 
        /// </summary>
        /// <typeparam name="B">Resulting functor value type</typeparam>
        /// <param name="f">Projection function</param>
        /// <returns>Mapped functor</returns>
        [Pure]
        public static Option<B> map<A, B>(Option<A> option, Func<A, B> f) =>
            option.Map(f);

        /// <summary>
        /// Projection from one value to another
        /// </summary>
        /// <typeparam name="B">Resulting functor value type</typeparam>
        /// <param name="Some">Projection function</param>
        /// <param name="None">Projection function</param>
        /// <returns>Mapped functor</returns>
        [Pure]
        public static Option<B> bimap<A, B>(Option<A> option, Func<A, B> Some, Func<B> None) =>
            option.BiMap(Some, None);

        /// <summary>
        /// Projection from one value to another
        /// </summary>
        /// <typeparam name="B">Resulting functor value type</typeparam>
        /// <param name="Some">Projection function</param>
        /// <param name="None">Projection function</param>
        /// <returns>Mapped functor</returns>
        [Pure]
        public static Option<B> bimap<A, B>(Option<A> option, Func<A, B> Some, Func<Unit, B> None) =>
            option.BiMap(Some, None);

        /// <summary>
        /// Partial application map
        /// </summary>
        /// <remarks>TODO: Better documentation of this function</remarks>
        [Pure]
        public static Option<Func<T2, R>> parmap<T1, T2, R>(Option<T1> option, Func<T1, T2, R> mapper) =>
            option.ParMap(mapper);

        /// <summary>
        /// Partial application map
        /// </summary>
        /// <remarks>TODO: Better documentation of this function</remarks>
        [Pure]
        public static Option<Func<T2, Func<T3, R>>> parmap<T1, T2, T3, R>(Option<T1> option, Func<T1, T2, T3, R> mapper) =>
            option.ParMap(mapper);

        /// <summary>
        /// Apply a predicate to the bound value (if in a Some state)
        /// </summary>
        /// <param name="pred">Predicate to apply</param>
        /// <returns>Some(x) if the Option is in a Some state and the predicate
        /// returns True.  None otherwise.</returns>
        [Pure]
        public static Option<T> filter<T>(Option<T> option, Func<T, bool> pred) =>
            option.Filter(pred);

        /// <summary>
        /// Monadic bind operation
        /// </summary>
        [Pure]
        public static Option<R> bind<T, R>(Option<T> option, Func<T, Option<R>> binder) =>
            option.Bind(binder);

        /// <summary>
        /// Match the two states of the list of Options
        /// </summary>
        /// <param name="Some">Some match operation</param>
        /// <param name="None">None match operation</param>
        [Pure]
        public static IEnumerable<R> match<T, R>(IEnumerable<Option<T>> list,
            Func<T, IEnumerable<R>> Some,
            Func<IEnumerable<R>> None
            ) =>
            list.Match(
                None,
                opt     => match(opt, v => Some(v), None),
                (x, xs) => match(x,   v => Some(v), None).Concat(match(xs, Some, None)) // TODO: Flatten recursion
            );

        /// <summary>
        /// Match the two states of the list of Options
        /// </summary>
        /// <param name="Some">Some match operation</param>
        /// <param name="None">None match operation</param>
        [Pure]
        public static IEnumerable<R> match<T, R>(IEnumerable<Option<T>> list,
            Func<T, IEnumerable<R>> Some,
            IEnumerable<R> None
            ) =>
            match(list, Some, () => None);

        /// <summary>
        /// Extracts from a list of 'Option' all the 'Some' elements.
        /// All the 'Some' elements are extracted in order.
        /// </summary>
        [Pure]
        public static IEnumerable<T> somes<T>(IEnumerable<Option<T>> list) =>
            list.Somes();

        /// <summary>
        /// Convert the Option to an immutable list of zero or one items
        /// </summary>
        /// <returns>An immutable list of zero or one items</returns>
        [Pure]
        public static Lst<T> toList<T>(Option<T> option) =>
            option.ToList();

        /// <summary>
        /// Convert the Option to an enumerable of zero or one items
        /// </summary>
        /// <returns>An enumerable of zero or one items</returns>
        [Pure]
        public static T[] toArray<T>(Option<T> option) =>
            option.ToArray();
    }
}
