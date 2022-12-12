using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyClap.Seneca.Common.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EasyClap.Seneca.Common.Utility
{
    [PublicAPI]
    public static class Extensions
    {
        /// <summary>
        /// Keep Y and Z, set X to given value.
        /// </summary>
        public static Vector3 WithX(this Vector3 v3, float x)
        {
            return new Vector3(x, v3.y, v3.z);
        }

        /// <summary>
        /// Keep X and Z, set Y to given value.
        /// </summary>
        public static Vector3 WithY(this Vector3 v3, float y)
        {
            return new Vector3(v3.x, y, v3.z);
        }

        /// <summary>
        /// Keep X and Y, set Z to given value.
        /// </summary>
        public static Vector3 WithZ(this Vector3 v3, float z)
        {
            return new Vector3(v3.x, v3.y, z);
        }

        /// <summary>
        /// Keep X, set Y and Z to 0.
        /// </summary>
        public static Vector3 OnlyX(this Vector3 v3)
        {
            return new Vector3(v3.x, 0, 0);
        }

        /// <summary>
        /// Keep Y, set X and Z to 0.
        /// </summary>
        public static Vector3 OnlyY(this Vector3 v3)
        {
            return new Vector3(0, v3.y, 0);
        }

        /// <summary>
        /// Keep Z, set X and Y to 0.
        /// </summary>
        public static Vector3 OnlyZ(this Vector3 v3)
        {
            return new Vector3(0, 0, v3.z);
        }

        /// <summary>
        /// Set <c>localPosition.x</c>.
        /// </summary>
        public static void SetLocalPosX(this Transform transform, float x)
        {
            transform.localPosition = transform.localPosition.WithX(x);
        }

        /// <summary>
        /// Set <c>localPosition.y</c>.
        /// </summary>
        public static void SetLocalPosY(this Transform transform, float y)
        {
            transform.localPosition = transform.localPosition.WithY(y);
        }

        /// <summary>
        /// Set <c>localPosition.z</c>.
        /// </summary>
        public static void SetLocalPosZ(this Transform transform, float z)
        {
            transform.localPosition = transform.localPosition.WithZ(z);
        }

        /// <summary>
        /// Set <c>position.x</c>.
        /// </summary>
        public static void SetWorldPosX(this Transform transform, float x)
        {
            transform.position = transform.position.WithX(x);
        }

        /// <summary>
        /// Set <c>position.y</c>.
        /// </summary>
        public static void SetWorldPosY(this Transform transform, float y)
        {
            transform.position = transform.position.WithY(y);
        }

        /// <summary>
        /// Set <c>position.z</c>.
        /// </summary>
        public static void SetWorldPosZ(this Transform transform, float z)
        {
            transform.position = transform.position.WithZ(z);
        }

        /// <summary>
        /// Trim the list such that we are left with only <paramref name="keepLength"/> items from the start.
        /// </summary>
        public static void TrimEnd<T>(this IList<T> list, int keepLength)
        {
            for (var i = keepLength; i < list.Count; i++)
            {
                list.RemoveAt(i);
            }
        }

        /// <summary>
        /// Remove and return the last item.
        /// </summary>
        public static T PopLast<T>(this IList<T> list)
        {
            var item = list.Last();
            list.RemoveAt(list.Count - 1);
            return item;
        }

        /// <summary>
        /// Returns the type name. If this is a generic type, appends the list of generic type arguments between angle
        /// brackets.
        /// </summary>
        /// <param name="type">The type.</param>
        public static string GetFormattedName(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var genericArguments = type.GetGenericArguments()
                .Select(x => x.GetFormattedName())
                .Aggregate((x1, x2) => $"{x1}, {x2}");

            return $"{type.Name.Before("`")}<{genericArguments}>";
        }

        /// <summary>
        /// Returns a new string that only includes what comes before the first instance of <paramref name="phrase"/>.
        /// </summary>
        public static string Before(this string original, string phrase)
        {
            var index = original.IndexOf(phrase, StringComparison.Ordinal);
            return index >= 0 ? original.Substring(0, index) : original;
        }

        /// <summary>
        /// Returns a formatted string that ensures a "+" or "-" sign is prepended to the number.
        /// </summary>
        public static string ToNumberWithSign(this float value)
        {
            return $"{(value > 0 ? "+" : "")}{value}";
        }

        /// <summary>
        /// Returns a formatted string that ensures a "+" or "-" sign is prepended to the number.
        /// </summary>
        public static string ToNumberWithSign(this int value)
        {
            return $"{(value > 0 ? "+" : "")}{value}";
        }

        /// <summary>
        /// Returns a formatted string that ensures a "+" or "-" sign is prepended to the number.
        /// </summary>
        public static string ToNumberWithSign(this double value)
        {
            return $"{(value > 0 ? "+" : "")}{value}";
        }

        /// <summary>
        /// Clamp both components of the given Vector2 between [-1, 1] range.
        /// </summary>
        public static Vector2 ClampNeg1Pos1(this Vector2 v2)
        {
            return v2.Clamp(-1, 1);
        }

        /// <summary>
        /// Clamp both components of the given Vector2 between [min, max] range.
        /// </summary>
        public static Vector2 Clamp(this Vector2 v2, float min, float max)
        {
            return new Vector2(Mathf.Clamp(v2.x, min, max), Mathf.Clamp(v2.y, min, max));
        }

        /// <summary>
        /// Returns a new number that is this <paramref name="value"/> moved towards zero by the given
        /// <paramref name="delta"/>. The result will never overshoot zero.
        /// </summary>
        public static float MoveTowardsZero(this float value, float delta)
        {
            // clamp the delta so we never go beyond 0
            var clampedDelta = Mathf.Clamp(delta, 0f, Mathf.Abs(value));

            // if the value is negative, we should add the delta; otherwise subtract it
            var result = value < 0f ? value + clampedDelta : value - clampedDelta;

            // if the result is very close to 0, return 0; otherwise return the result
            return Mathf.Approximately(result, 0f) ? 0f : result;
        }

        /// <summary>
        /// Move <c>localPosition.x</c> of this transform towards zero by the given <paramref name="delta"/>. The
        /// result will never overshoot zero.
        /// </summary>
        public static void MoveLocalXTowardsZero(this Transform transform, float delta)
        {
            transform.SetLocalPosX(transform.localPosition.x.MoveTowardsZero(delta));
        }

        /// <summary>
        /// Move <c>localPosition.y</c> of this transform towards zero by the given <paramref name="delta"/>. The
        /// result will never overshoot zero.
        /// </summary>
        public static void MoveLocalYTowardsZero(this Transform transform, float delta)
        {
            transform.SetLocalPosY(transform.localPosition.y.MoveTowardsZero(delta));
        }

        /// <summary>
        /// Move <c>localPosition.z</c> of this transform towards zero by the given <paramref name="delta"/>. The
        /// result will never overshoot zero.
        /// </summary>
        public static void MoveLocalZTowardsZero(this Transform transform, float delta)
        {
            transform.SetLocalPosZ(transform.localPosition.z.MoveTowardsZero(delta));
        }

        /// <summary>
        /// Lerps <c>transform.position</c>.
        /// Assigns the result to <c>transform.position</c> and returns the result.
        /// </summary>
        public static Vector3 LerpPosition(this Transform transform, Vector3 target, float t)
        {
            return transform.position = Vector3.Lerp(transform.position, target, t);
        }

        /// <summary>
        /// Lerps <c>transform.localPosition</c>.
        /// Assigns the result to <c>transform.localPosition</c> and returns the result.
        /// </summary>
        public static Vector3 LerpLocalPosition(this Transform transform, Vector3 target, float t)
        {
            return transform.localPosition = Vector3.Lerp(transform.localPosition, target, t);
        }

        /// <summary>
        /// Slerps <c>transform.rotation</c>.
        /// Assigns the result to <c>transform.rotation</c> and returns the result.
        /// </summary>
        public static Quaternion SlerpRotation(this Transform transform, Quaternion target, float t)
        {
            return transform.rotation = Quaternion.Slerp(transform.rotation, target, t);
        }

        /// <summary>
        /// Slerps <c>transform.localRotation</c>.
        /// Assigns the result to <c>transform.localRotation</c> and returns the result.
        /// </summary>
        public static Quaternion SlerpLocalRotation(this Transform transform, Quaternion target, float t)
        {
            return transform.localRotation = Quaternion.Slerp(transform.localRotation, target, t);
        }

        /// <summary>
        /// Lerps <c>transform.localScale</c>.
        /// Assigns the result to <c>transform.localScale</c> and returns the result.
        /// </summary>
        public static Vector3 LerpLocalScale(this Transform transform, Vector3 target, float t)
        {
            return transform.localScale = Vector3.Lerp(transform.localScale, target, t);
        }

        /// <summary>
        /// Sets <c>transform.position</c> to <c>Vector3.zero</c> and <c>transform.rotation</c> to <c>Quaternion.identity</c>.
        /// </summary>
        public static void ResetWorldPosRot(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Sets <c>transform.localPosition</c> to <c>Vector3.zero</c> and <c>transform.localRotation</c> to <c>Quaternion.identity</c>.
        /// </summary>
        public static void ResetLocalPosRot(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// If the given <paramref name="currentIndex"/> is the last index, returns 0; otherwise returns the next index.
        /// </summary>
        /// <remarks>
        /// If the given <paramref name="currentIndex"/> is out of range, it gets wrapped over.
        /// <para/>
        /// So, for negative indexes:<br/>
        /// 0 goes to 1 <br/>
        /// -1 goes to 0 <br/>
        /// -2 goes to the last index <br/>
        /// -3 goes to the last index - 1 <br/>
        /// <para/>
        /// Similarly, for out-of-range positive indexes: <br/>
        /// length goes to 0 <br/>
        /// length + 1 goes to 1 <br/>
        /// length + 2 goes to the 2 <br/>
        /// length + 3 goes to the 3 <br/>
        /// <para/>
        /// You can call <see cref="ContainsIndex{T}"/> separately to check if the index is contained in the list.
        /// </remarks>
        public static int GetNextIndexCircular<T>(this IList<T> list, int currentIndex)
        {
            return list.WrapIndex(currentIndex + 1);
        }

        /// <summary>
        /// If the given <paramref name="currentItem"/> is the last item, or if it is not contained in <paramref name="list"/>, returns 0; otherwise returns the next index.
        /// </summary>
        /// <remarks>
        /// You can call <see cref="IList{T}.Contains"/> separately to check if the item is contained in the list.
        /// </remarks>
        public static int GetNextIndexCircular<T>(this IList<T> list, T currentItem)
        {
            var currentIndex = list.IndexOf(currentItem);
            return list.GetNextIndexCircular(currentIndex);
        }

        /// <summary>
        /// If the given <paramref name="currentIndex"/> is the last index, returns the first item; otherwise returns the next item.
        /// </summary>
        /// <remarks><inheritdoc cref="GetNextIndexCircular{T}(System.Collections.Generic.IList{T},int)"/></remarks>
        public static T GetNextItemCircular<T>(this IList<T> list, int currentIndex)
        {
            var nextIndexCircular = list.GetNextIndexCircular(currentIndex);
            return list[nextIndexCircular];
        }

        /// <summary>
        /// If the given <paramref name="currentItem"/> is the last item, returns the first item; otherwise returns the next item.
        /// </summary>
        /// <remarks><inheritdoc cref="GetNextIndexCircular{T}(System.Collections.Generic.IList{T},T)"/></remarks>
        public static T GetNextItemCircular<T>(this IList<T> list, T currentItem)
        {
            var nextIndexCircular = list.GetNextIndexCircular(currentItem);
            return list[nextIndexCircular];
        }

        /// <summary>
        /// Returns whether <paramref name="first"/> and <paramref name="second"/> has the same elements and the same elements only.
        /// </summary>
        public static bool HasSameElementsWith<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return first.Order().SequenceEqual(second.Order());
        }

        /// <summary>
        /// Returns the ordered version of the <paramref name="source"/> using an identity function as the comparator.
        /// Does NOT mutate the original.
        /// </summary>
        public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(Utils.Identity);
        }

        /// <summary>
        /// Returns a new color that has the same RGB values with <paramref name="source"/>, but with the given <paramref name="alpha"/>.
        /// Does NOT mutate the original.
        /// </summary>
        public static Color WithAlpha(this Color source, float alpha)
        {
            return new Color(source.r, source.g, source.b, alpha);
        }

        /// <summary>
        /// Sets the alpha of the given <paramref name="image"/>'s color.
        /// </summary>
        public static void SetColorAlpha(this Image image, float alpha)
        {
            image.color = image.color.WithAlpha(alpha);
        }

        /// <summary>
        /// This is not intended to be used directly. It is here to provide deconstruction support for <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }

        /// <summary>
        /// Returns a new number that is this <paramref name="value"/> moved towards <paramref name="target"/> by the given
        /// <paramref name="delta"/>. The result will never overshoot <paramref name="target"/>.
        /// </summary>
        public static float MoveTowards(this float value, float target, float delta)
        {
            var valueAfterDelta = value - target; // origin shift to zero
            return valueAfterDelta.MoveTowardsZero(delta) + target; // move towards zero, origin shift to original
        }

        /// <summary>
        /// Filters <c>null</c> references out from <paramref name="source"/>.
        /// Does NOT mutate <paramref name="source"/>.
        /// </summary>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
        {
            return source.Where(x => x != null);
        }

        /// <summary>
        /// Returns a random item from the given list.
        /// </summary>
        public static T GetRandomItem<T>(this IList<T> list)
        {
            return list[list.GetRandomIndex()];
        }

        /// <summary>
        /// Returns a random index from the given list.
        /// Uses <c>UnityEngine.Random</c>.
        /// </summary>
        public static int GetRandomIndex<T>(this IList<T> list)
        {
            return UnityEngine.Random.Range(0, list.Count);
        }

        /// <summary>
        /// Creates an enumerable that consists of this <paramref name="item"/> repeated <paramref name="count"/> times.
        /// </summary>
        public static IEnumerable<T> Repeat<T>(this T item, int count)
        {
            return Enumerable.Repeat(item, count);
        }

        /// <summary>
        /// Returns whether the given <paramref name="rawLayer"/> is contained in this <paramref name="layerMask"/>.
        /// </summary>
        /// <param name="layerMask">The layer mask.</param>
        /// <param name="rawLayer">The raw (ie. not bit-shifted) layer.</param>
        /// <seealso cref="ContainsLayerShifted"/>
        /// <remarks>
        /// Bit shifting is this operation: <c>1 &lt;&lt; layer</c>. If you have no idea what this means or does, you
        /// probably have a raw (ie. not bit-shifted) layer.
        /// </remarks>
        public static bool ContainsLayerRaw(this LayerMask layerMask, int rawLayer)
        {
            var shiftedLayer = 1 << rawLayer;
            return layerMask.ContainsLayerShifted(shiftedLayer);
        }

        /// <summary>
        /// Returns whether the given <paramref name="shiftedLayer"/> is contained in this <paramref name="layerMask"/>.
        /// </summary>
        /// <param name="layerMask">The layer mask.</param>
        /// <param name="shiftedLayer">The bit-shifted layer.</param>
        /// <seealso cref="ContainsLayerRaw"/>
        /// <remarks><inheritdoc cref="ContainsLayerRaw"/></remarks>
        public static bool ContainsLayerShifted(this LayerMask layerMask, int shiftedLayer)
        {
            return (layerMask & shiftedLayer) == shiftedLayer;
        }


        /// <summary>
        /// Converts <paramref name="value"/> from [<paramref name="curMin"/>, <paramref name="curMax"/>] range to
        /// [<paramref name="newMin"/>, <paramref name="newMax"/>] range.
        /// </summary>
        public static float ConvertRange(this float value, float curMin, float curMax, float newMin, float newMax)
        {
            return (value - curMin) * (newMax - newMin) / (curMax - curMin) + newMin;
        }

        /// <summary>
        /// Converts <paramref name="value"/> from [0, 1] range to [<paramref name="newMin"/>, <paramref name="newMax"/>] range.
        /// </summary>
        public static float ConvertRangeFrom01(this float value, float newMin, float newMax)
        {
            return value * (newMax - newMin) + newMin;
        }

        /// <summary>
        /// Converts <paramref name="value"/> from [<paramref name="curMin"/>, <paramref name="curMax"/>] range to [0, 1] range.
        /// </summary>
        public static float ConvertRangeTo01(this float value, float curMin, float curMax)
        {
            return (value - curMin) / (curMax - curMin);
        }

        /// <inheritdoc cref="ConvertRangeTo01"/>
        /// <remarks>This is an alias for <see cref="ConvertRangeTo01"/>.</remarks>
        public static float NormalizeRange(this float value, float curMin, float curMax)
        {
            return value.ConvertRangeTo01(curMin, curMax);
        }

        public static string FormatTimer(this float value)
        {
            value = Mathf.Max(0, value);
            return $"{Mathf.Floor(value / 60):00}:{Mathf.Floor(value % 60):00}";
        }

        /// <summary>
        /// WARNING: THIS METHOD IS VERY EXPENSIVE. USE IT SPARINGLY.
        /// Returns a string that lists the names and values of all fields and properties of the object.
        /// </summary>
        public static string ToObjectSummaryString(this object obj)
        {
            var type = obj.GetType();
            var sb = new StringBuilder();

            var props = type.GetProperties();
            foreach (var p in props)
            {
                sb.AppendLine($"{p.Name}: {p.GetValue(obj, null)}");
            }

            var fields = type.GetFields();
            foreach (var f in fields)
            {
                sb.AppendLine($"{f.Name}: {f.GetValue(obj)}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Wrapper around <see cref="String.Join(string, System.Collections.Generic.IEnumerable{string})"><c>string.Join</c></see>.
        /// Joins the <paramref name="source"/> sequence into a single string, with the given <paramref name="separator"/> between each item.
        /// </summary>
        public static string JoinToString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// General-purpose surrogate for the null conditional operators for <see cref="UnityEngine.Object">UnityEngine.Object</see>.
        /// This method can be expensive in hot code paths because it takes in a general-purpose delegate. Try to use more specialized versions as much as possible.
        /// </summary>
        /// <param name="unityObject">The <see cref="UnityEngine.Object">UnityEngine.Object</see> that we operate on but are not sure if null or not. (Left-hand side of the null conditional operator)</param>
        /// <param name="getter">The getter function that does the thing we want. (Right-hand side of the null conditional operator)</param>
        /// <param name="defaultVal">The default value to return if the <paramref name="unityObject"/> is null. Defaults to null for reference types, and the default value for value types.</param>
        /// <returns>If <paramref name="unityObject"/> is null, then <paramref name="defaultVal"/>; otherwise the result of <paramref name="getter"/>.</returns>
        /// <remarks>
        /// <see cref="UnityEngine.Object">UnityEngine.Object</see> has a custom equality implementation for a couple of very good reasons. However, this makes it impossible to properly use null conditional operators. See <a href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-">the Microsoft documentation on null conditional operators</a> and <a href="https://blog.unity.com/technology/custom-operator-should-we-keep-it">the Unity Blog post on this topic</a>.
        /// </remarks>
        [CanBeNull]
        public static TOut Maybe<TIn, TOut>([CanBeNull] this TIn unityObject, [NotNull] Func<TIn, TOut> getter, [CanBeNull] TOut defaultVal = default)
            where TIn : UnityEngine.Object
        {
            return unityObject ? getter(unityObject) : defaultVal;
        }

        /// <summary>
        /// Surrogate for this null conditional expression: <c>component?.gameObject</c>
        /// </summary>
        /// <returns>If <paramref name="component"/> is null then null; otherwise its gameObject.</returns>
        /// <remarks><inheritdoc cref="Maybe{TIn,TOut}"/></remarks>
        [CanBeNull]
        public static GameObject MaybeGameObject([CanBeNull] this Component component)
        {
            return component ? component.gameObject : null;
        }

        /// <summary>
        /// Surrogate for this null conditional expression: <c>component?.transform</c>
        /// </summary>
        /// <returns>If <paramref name="component"/> is null then null; otherwise its transform.</returns>
        /// <remarks><inheritdoc cref="Maybe{TIn,TOut}"/></remarks>
        [CanBeNull]
        public static Transform MaybeTransform([CanBeNull] this Component component)
        {
            return component ? component.transform : null;
        }

        /// <summary>
        /// Surrogate for this null conditional expression: <c>gameObject?.transform</c>
        /// </summary>
        /// <returns>If <paramref name="gameObject"/> is null then null; otherwise its transform.</returns>
        /// <remarks><inheritdoc cref="Maybe{TIn,TOut}"/></remarks>
        [CanBeNull]
        public static Transform MaybeTransform([CanBeNull] this GameObject gameObject)
        {
            return gameObject ? gameObject.transform : null;
        }

        /// <summary>
        /// Surrogate for this null conditional expression: <c>gameObject?.GetComponent&lt;TComp&gt;()</c>
        /// </summary>
        /// <returns>If <paramref name="gameObject"/> is null then null; otherwise the result of <see cref="GameObject.GetComponent{T}"/>.</returns>
        /// <remarks><inheritdoc cref="Maybe{TIn,TOut}"/></remarks>
        /// <seealso cref="GameObject.GetComponent{T}"/>
        [CanBeNull]
        public static TComp MaybeGetComponent<TComp>([CanBeNull] this GameObject gameObject) where TComp : Component
        {
            return gameObject ? gameObject.GetComponent<TComp>() : null;
        }

        /// <summary>
        /// Surrogate for this null conditional expression: <c>component?.GetComponent&lt;TComp&gt;()</c>
        /// </summary>
        /// <returns>If <paramref name="component"/> is null then null; otherwise the result of <see cref="Component.GetComponent{T}"/>.</returns>
        /// <remarks><inheritdoc cref="Maybe{TIn,TOut}"/></remarks>
        /// <seealso cref="Component.GetComponent{T}"/>
        [CanBeNull]
        public static TComp MaybeGetComponent<TComp>([CanBeNull] this Component component) where TComp : Component
        {
            return component ? component.GetComponent<TComp>() : null;
        }


        /// <summary>
        /// Surrogate for this null conditional expression: <c>gameObject?.GetComponent(componentType)</c>
        /// </summary>
        /// <returns>If <paramref name="gameObject"/> is null then null; otherwise the result of <see cref="GameObject.GetComponent(Type)"/>.</returns>
        /// <remarks><inheritdoc cref="Maybe{TIn,TOut}"/></remarks>
        /// <seealso cref="GameObject.GetComponent(Type)"/>
        [CanBeNull]
        public static Component MaybeGetComponent([CanBeNull] this GameObject gameObject, [NotNull] Type type)
        {
            return gameObject ? gameObject.GetComponent(type) : null;
        }

        /// <summary>
        /// Surrogate for this null conditional expression: <c>component?.GetComponent(componentType)</c>
        /// </summary>
        /// <returns>If <paramref name="component"/> is null then null; otherwise the result of <see cref="Component.GetComponent(Type)"/>.</returns>
        /// <remarks><inheritdoc cref="Maybe{TIn,TOut}"/></remarks>
        /// <seealso cref="Component.GetComponent(Type)"/>
        [CanBeNull]
        public static Component MaybeGetComponent([CanBeNull] this Component component, [NotNull] Type type)
        {
            return component ? component.GetComponent(type) : null;
        }

        // TODO: maybe generalize this to accept closed generics and non-generics as well
        /// <summary>
        /// Searches upwards to find a closed generic type that matches a given open generic type in the hierarchy of a given type.
        /// Starting type is also included in the search.
        /// </summary>
        /// <param name="type">The search will start here and go upwards (towards ancestors).</param>
        /// <param name="targetOpenGenericType">The open generic type we are searching for.</param>
        /// <param name="foundClosedGenericType">The found closed generic type, or null.</param>
        /// <returns>True if a match is found, false otherwise.</returns>
        /// <remarks>
        /// A generic type is considered open if at least one of its generic parameters is not given. For example:
        /// <list type="bullet">
        ///     <item>For this definition: <c>Foo&lt;T&gt;</c></item>
        ///     <item>This is an open generic type: <c>Foo&lt;&gt;</c></item>
        ///     <item>This is a closed generic type: <c>Foo&lt;int&gt;</c></item>
        /// </list>
        /// </remarks>
        public static bool TryGetClosedGenericAncestorType(this Type type, Type targetOpenGenericType, out Type foundClosedGenericType)
        {
            for (var candidate = type; candidate != null; candidate = candidate.BaseType)
            {
                if (candidate.IsGenericType && candidate.GetGenericTypeDefinition() == targetOpenGenericType)
                {
                    foundClosedGenericType = candidate;
                    return true;
                }
            }

            foundClosedGenericType = null;
            return false;
        }

        /// <summary>
        /// Wraps the given index along the length of this enumerable.
        /// This ensures the index stays within <c>[0, length-1]</c> range.
        /// </summary>
        /// <remarks>
        /// Calls <see cref="ModuloNonNegative(int,int)"/>, number being index, and mod being length.
        /// </remarks>
        public static int WrapIndex<T>(this IEnumerable<T> source, int index)
        {
            return index.ModuloNonNegative(source.Count());
        }

        /// <summary>
        /// Takes modulo of the given number. Output is non-negative, with a range of <c>[0, Abs(mod))</c>.
        /// </summary>
        /// <param name="num">Left-hand side operand. Can be any value.</param>
        /// <param name="mod">Right-hand side operand. Can be any value except 0.</param>
        /// <exception cref="SenecaCommonException">Throws if <paramref name="mod"/> is 0.</exception>
        /// <remarks>
        /// Output range of the built-in <c>%</c> operator is <c>(-mod, +mod)</c>.
        /// </remarks>
        public static float ModuloNonNegative(this float num, float mod)
        {
            if (mod == 0)
            {
                throw SenecaCommonException.FromArgumentOutOfRange(nameof(mod), mod, "Right-hand side operand a modulo operation cannot be 0.");
            }

            return (num % mod + Mathf.Abs(mod)) % mod;
        }

        /// <inheritdoc cref="ModuloNonNegative(float,float)"/>
        public static int ModuloNonNegative(this int num, int mod)
        {
            if (mod == 0)
            {
                throw SenecaCommonException.FromArgumentOutOfRange(nameof(mod), mod, "Right-hand side operand a modulo operation cannot be 0.");
            }

            return (num % mod + Mathf.Abs(mod)) % mod;
        }

        /// <summary>
        /// Returns the index of the first entry in <paramref name="source"/> that equals <paramref name="item"/>.
        /// Returns -1 if none found.
        /// </summary>
        public static int IndexOf<T>(this IReadOnlyList<T> source, T item, IEqualityComparer<T> equalityComparer = null)
        {
            equalityComparer ??= EqualityComparer<T>.Default;

            for (var i = 0; i < source.Count; i++)
            {
                if(equalityComparer.Equals(source[i], item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <inheritdoc cref="GetNextIndexCircular{T}(System.Collections.Generic.IList{T},int)"/>
        public static int GetNextIndexCircular<T>(this IReadOnlyList<T> source, int currentIndex)
        {
            return source.WrapIndex(currentIndex + 1);
        }

        /// <inheritdoc cref="GetNextIndexCircular{T}(System.Collections.Generic.IList{T},T)"/>
        public static int GetNextIndexCircular<T>(this IReadOnlyList<T> source, T currentItem, IEqualityComparer<T> equalityComparer = null)
        {
            var currentIndex = source.IndexOf(currentItem, equalityComparer);
            return source.GetNextIndexCircular(currentIndex);
        }

        /// <inheritdoc cref="GetNextItemCircular{T}(System.Collections.Generic.IList{T},int)"/>
        public static T GetNextItemCircular<T>(this IReadOnlyList<T> source, int currentIndex)
        {
            var nextIndexCircular = source.GetNextIndexCircular(currentIndex);
            return source[nextIndexCircular];
        }

        /// <inheritdoc cref="GetNextItemCircular{T}(System.Collections.Generic.IList{T},T)"/>
        public static T GetNextItemCircular<T>(this IReadOnlyList<T> source, T currentItem, IEqualityComparer<T> equalityComparer = null)
        {
            var nextIndexCircular = source.GetNextIndexCircular(currentItem, equalityComparer);
            return source[nextIndexCircular];
        }

        /// <summary>
        /// Starts the given <paramref name="routine"/> in <paramref name="host"/>.
        /// </summary>
        public static Coroutine StartCoroutineIn(this IEnumerator routine, MonoBehaviour host)
        {
            return host.StartCoroutine(routine);
        }
        
        public static void StopCoroutineIn(this Coroutine coroutine, MonoBehaviour host)
        {
            host.StopCoroutine(coroutine);
        }

        /// <summary>
        /// Starts the given <paramref name="routine"/> in the singleton instance of <see cref="GlobalMonoBehaviourSurrogate"/>.
        /// </summary>
        public static Coroutine StartCoroutineInGlobalSurrogate(this IEnumerator routine)
        {
            return routine.StartCoroutineIn(GlobalMonoBehaviourSurrogate.Instance);
        }
        
        /// <summary>
        /// Stops the given <paramref name="routine"/> in the singleton instance of <see cref="GlobalMonoBehaviourSurrogate"/>.
        /// </summary>
        public static void StopCoroutineInGlobalSurrogate(this Coroutine coroutine)
        {
            coroutine.StopCoroutineIn(GlobalMonoBehaviourSurrogate.Instance);
        }

        /// <summary>
        /// Starts the given <paramref name="routine"/> in the singleton instance of <see cref="SceneMonoBehaviourSurrogate"/>.
        /// </summary>
        public static Coroutine StartCoroutineInSceneSurrogate(this IEnumerator routine)
        {
            return routine.StartCoroutineIn(SceneMonoBehaviourSurrogate.Instance);
        }
        
        /// <summary>
        /// Stops the given <paramref name="routine"/> in the singleton instance of <see cref="SceneMonoBehaviourSurrogate"/>.
        /// </summary>
        public static void StopCoroutineInSceneSurrogate(this Coroutine coroutine)
        { 
            coroutine.StopCoroutineIn(SceneMonoBehaviourSurrogate.Instance);
        }

        /// <summary>
        /// Returns whether <see cref="suspiciousIndex"/> is in range of this <see cref="source"/> enumerable.
        /// </summary>
        /// <returns>True if index is in range; false otherwise.</returns>
        public static bool ContainsIndex<T>(this IEnumerable<T> source, int suspiciousIndex)
        {
            var sourceCount = source.Count();
            return sourceCount > 0 // source must not be empty
                   && suspiciousIndex >= 0 // index greater than or equal to 0
                   && suspiciousIndex < sourceCount; // index less than count
        }

        public static List<Transform> GetChildsWithTag(this Transform parent, string tag)
        {
            var childs = new List<Transform>();
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.CompareTag(tag))
                {
                    childs.Add(child);
                }
                
                childs.AddRange(child.GetChildsWithTag(tag));
            }

            return childs;
        }
        
        public static List<Transform> GetChildsWithLayer(this Transform parent, int layer)
        {
            var childs = new List<Transform>();
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.gameObject.layer == layer)
                {
                    childs.Add(child);
                }
                
                childs.AddRange(child.GetChildsWithLayer(layer));
            }

            return childs;
        }
        
        public static List<T> GetComponentsInChildrenWithTag<T>(this Component component, string tag) where T : Component
        {
            var components = new List<T>(component.GetComponentsInChildren<T>());
            for (int i = 0; i < components.Count; i++)
            {
                if (!components[i].CompareTag(tag))
                {
                    components.RemoveAtFast(i);
                    i--;
                }
            }

            return components;
        }
        
        public static List<T> GetComponentsInChildrenWithLayer<T>(this Component component, int layer) where T : Component
        {
            var components = new List<T>(component.GetComponentsInChildren<T>());
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].gameObject.layer != layer)
                {
                    components.RemoveAtFast(i);
                    i--;
                }
            }

            return components;
        }
    }
}
