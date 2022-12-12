using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.Utility
{
    /// <summary>
    /// Provides some LINQ-alike extension methods for Unity-specific purposes.
    /// </summary>
    [PublicAPI]
    public static class LinqToUnity
    {
        /// <summary>
        /// Selects the component <typeparamref name="T"/> from each member of <paramref name="source"/> enumerable.
        /// Does NOT mutate <paramref name="source"/>.
        /// </summary>
        /// <param name="source">Enumerable to select from.</param>
        /// <typeparam name="T">Type of the component to select.</typeparam>
        /// <returns>The selected components.</returns>
        public static IEnumerable<T> SelectComponent<T>(this IEnumerable<Component> source) where T : Component
        {
            return source.Select(x => x.GetComponent<T>());
        }

        /// <inheritdoc cref="SelectComponent{T}(System.Collections.Generic.IEnumerable{UnityEngine.Component})"/>
        public static IEnumerable<T> SelectComponent<T>(this IEnumerable<GameObject> source) where T : Component
        {
            return source.Select(x => x.GetComponent<T>());
        }

        /// <inheritdoc cref="SelectComponent{T}(System.Collections.Generic.IEnumerable{UnityEngine.Component})"/>
        public static IEnumerable<T> SelectComponent<T>(this IEnumerable<RaycastHit> source) where T : Component
        {
            return source.Select(x => x.transform.GetComponent<T>());
        }

        /// <summary>
        /// Returns the first component of type <typeparamref name="T"/> found in members of <paramref name="source"/>.
        /// Throws if none found.
        /// Does NOT mutate <paramref name="source"/>.
        /// </summary>
        /// <param name="source">Enumerable to select from.</param>
        /// <typeparam name="T">Type of the component to select.</typeparam>
        /// <returns>The first component found.</returns>
        public static T FirstComponent<T>(this IEnumerable<Component> source) where T : Component
        {
            return source.SelectComponent<T>().WhereNotNull().First();
        }

        /// <inheritdoc cref="FirstComponent{T}(System.Collections.Generic.IEnumerable{UnityEngine.Component})"/>
        public static T FirstComponent<T>(this IEnumerable<GameObject> source) where T : Component
        {
            return source.SelectComponent<T>().WhereNotNull().First();
        }

        /// <inheritdoc cref="FirstComponent{T}(System.Collections.Generic.IEnumerable{UnityEngine.Component})"/>
        public static T FirstComponent<T>(this IEnumerable<RaycastHit> source) where T : Component
        {
            return source.SelectComponent<T>().WhereNotNull().First();
        }

        /// <summary>
        /// Returns the first component of type <typeparamref name="T"/> found in members of <paramref name="source"/>.
        /// Returns <c>null</c> if none found.
        /// Does NOT mutate <paramref name="source"/>.
        /// </summary>
        /// <param name="source">Enumerable to select from.</param>
        /// <typeparam name="T">Type of the component to select.</typeparam>
        /// <returns>The first component found or <c>null</c>.</returns>
        public static T FirstComponentOrDefault<T>(this IEnumerable<Component> source) where T : Component
        {
            return source.SelectComponent<T>().WhereNotNull().FirstOrDefault();
        }

        /// <inheritdoc cref="FirstComponentOrDefault{T}(System.Collections.Generic.IEnumerable{UnityEngine.Component})"/>
        public static T FirstComponentOrDefault<T>(this IEnumerable<GameObject> source) where T : Component
        {
            return source.SelectComponent<T>().WhereNotNull().FirstOrDefault();
        }

        /// <inheritdoc cref="FirstComponentOrDefault{T}(System.Collections.Generic.IEnumerable{UnityEngine.Component})"/>
        public static T FirstComponentOrDefault<T>(this IEnumerable<RaycastHit> source) where T : Component
        {
            return source.SelectComponent<T>().WhereNotNull().FirstOrDefault();
        }

        /// <summary>
        /// Returns the item in <paramref name="source"/> that is closest to <see cref="reference"/>.
        /// Does NOT mutate <paramref name="source"/>.
        /// </summary>
        public static Vector3 SelectClosest(this IEnumerable<Vector3> source, Vector3 reference)
        {
            float Calculator(Vector3 item) => Vector3.SqrMagnitude(reference - item);
            static bool Comparer(float leaderDist, float contenderDist) => leaderDist < contenderDist;
            return source.SelectFittest(Calculator, Comparer);
        }

        /// <inheritdoc cref="SelectClosest(System.Collections.Generic.IEnumerable{UnityEngine.Vector3},UnityEngine.Vector3)"/>
        public static Transform SelectClosest(this IEnumerable<Transform> source, Vector3 reference)
        {
            float Calculator(Transform it) => Vector3.SqrMagnitude(reference - it.position);
            static bool Comparer(float leaderDist, float contenderDist) => leaderDist < contenderDist;
            return source.SelectFittest(Calculator, Comparer);
        }

        /// <inheritdoc cref="SelectClosest(System.Collections.Generic.IEnumerable{UnityEngine.Vector3},UnityEngine.Vector3)"/>
        public static GameObject SelectClosest(this IEnumerable<GameObject> source, Vector3 reference)
        {
            float Calculator(GameObject it) => Vector3.SqrMagnitude(reference - it.transform.position);
            static bool Comparer(float leaderDist, float contenderDist) => leaderDist < contenderDist;
            return source.SelectFittest(Calculator, Comparer);
        }

        /// <summary>
        /// Returns the item in <paramref name="source"/> that is farthest from <see cref="reference"/>.
        /// Does NOT mutate <paramref name="source"/>.
        /// </summary>
        public static Vector3 SelectFarthest(this IEnumerable<Vector3> source, Vector3 reference)
        {
            float Calculator(Vector3 it) => Vector3.SqrMagnitude(reference - it);
            static bool Comparer(float leaderCriteria, float contenderCriteria) => leaderCriteria > contenderCriteria;
            return source.SelectFittest(Calculator, Comparer);
        }

        /// <inheritdoc cref="SelectFarthest(System.Collections.Generic.IEnumerable{UnityEngine.Vector3},UnityEngine.Vector3)"/>
        public static Transform SelectFarthest(this IEnumerable<Transform> source, Vector3 reference)
        {
            float Calculator(Transform it) => Vector3.SqrMagnitude(reference - it.position);
            static bool Comparer(float leaderDist, float contenderDist) => leaderDist > contenderDist;
            return source.SelectFittest(Calculator, Comparer);
        }

        /// <inheritdoc cref="SelectFarthest(System.Collections.Generic.IEnumerable{UnityEngine.Vector3},UnityEngine.Vector3)"/>
        public static GameObject SelectFarthest(this IEnumerable<GameObject> source, Vector3 reference)
        {
            float Calculator(GameObject it) => Vector3.SqrMagnitude(reference - it.transform.position);
            static bool Comparer(float leaderDist, float contenderDist) => leaderDist > contenderDist;
            return source.SelectFittest(Calculator, Comparer);
        }
    }
}
