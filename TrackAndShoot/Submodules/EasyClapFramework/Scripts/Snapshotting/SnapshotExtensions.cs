using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using EasyClap.Seneca.Common.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.Snapshotting
{
    [PublicAPI]
    public static class SnapshotExtensions
    {
        /// <inheritdoc cref="Snapshot(Transform, Space, Space, Space)" />
        public static Snapshot TakeSnapshot(this Transform transform, Space positionGetSpace, Space rotationGetSpace, Space scaleGetSpace = Space.Self)
        {
            return new Snapshot(transform, positionGetSpace, rotationGetSpace, scaleGetSpace);
        }

        /// <inheritdoc cref="Snapshot(Transform, Space, bool)"/>
        public static Snapshot TakeSnapshot(this Transform transform, Space getSpace, bool getLocalScale = true)
        {
            return new Snapshot(transform, getSpace, getLocalScale);
        }

        /// <inheritdoc cref="Snapshotting.Snapshot.SetTransform(Transform, Space, bool, bool, bool)"/>
        public static void SetFromSnapshot(this Transform transform,
            Snapshot snapshot,
            Space setSpace = Space.Self,
            bool setPosition = true,
            bool setRotation = true,
            bool setLocalScale = true)
        {
            snapshot.SetTransform(transform, setSpace, setPosition, setRotation, setLocalScale);
        }

        /// <inheritdoc cref="Snapshotting.Snapshot.SetTransform(Transform, bool, Space, bool, Space, bool)"/>
        public static void SetFromSnapshot(this Transform transform,
            Snapshot snapshot,
            bool setPosition = true,
            Space positionSetSpace = Space.Self,
            bool setRotation = true,
            Space rotationSetSpace = Space.Self,
            bool setLocalScale = true)
        {
            snapshot.SetTransform(transform, setPosition, positionSetSpace, setRotation, rotationSetSpace, setLocalScale);
        }

        /// <inheritdoc cref="Snapshotting.Snapshot.SetTransformPosition(Transform, Space)"/>
        public static void SetPositionFromSnapshot(this Transform transform, Snapshot snapshot, Space setSpace)
        {
            snapshot.SetTransformPosition(transform, setSpace);
        }

        /// <inheritdoc cref="Snapshotting.Snapshot.SetTransformRotation(Transform, Space)"/>
        public static void SetRotationFromSnapshot(this Transform transform, Snapshot snapshot, Space setSpace)
        {
            snapshot.SetTransformRotation(transform, setSpace);
        }

        /// <inheritdoc cref="Snapshotting.Snapshot.SetTransformLocalScale(Transform)"/>
        public static void SetLocalScaleFromSnapshot(this Transform transform, Snapshot snapshot)
        {
            snapshot.SetTransformLocalScale(transform);
        }

        /// <summary>
        /// Lerps the position or localPosition of this <paramref name="transform"/> towards the position of the <paramref name="target"/> snapshot.
        /// Mutates the transform AND returns the result.
        /// </summary>
        /// <param name="transform">Transform whose position or localPosition will be changed.</param>
        /// <param name="target">Target snapshot.</param>
        /// <param name="t">Lerp time.</param>
        /// <param name="space">If World, position will be changed. If Self, localPosition will be changed.</param>
        public static Vector3 LerpPosition(this Transform transform, Snapshot target, float t, Space space)
        {
            return space switch
            {
                Space.World => transform.LerpPosition(target.Position, t),
                Space.Self => transform.LerpLocalPosition(target.Position, t),
                _ => throw new ArgumentOutOfRangeException(nameof(space), space, null)
            };
        }

        /// <summary>
        /// Slerps the rotation or localRotation of this <paramref name="transform"/> towards the rotation of the <paramref name="target"/> snapshot.
        /// Mutates the transform AND returns the result. Uses <see cref="Quaternion.Slerp"/>.
        /// </summary>
        /// <param name="transform">Transform whose rotation or localRotation will be changed.</param>
        /// <param name="target">Target snapshot.</param>
        /// <param name="t">Slerp time.</param>
        /// <param name="space">If World, rotation will be changed. If Self, localRotation will be changed.</param>
        public static Quaternion SlerpRotation(this Transform transform, Snapshot target, float t, Space space)
        {
            return space switch
            {
                Space.World => transform.SlerpRotation(target.Rotation, t),
                Space.Self => transform.SlerpLocalRotation(target.Rotation, t),
                _ => throw new ArgumentOutOfRangeException(nameof(space), space, null)
            };
        }

        /// <summary>
        /// Lerps the localScale of this <paramref name="transform"/> towards the scale of the <paramref name="target"/> snapshot.
        /// Mutates the transform AND returns the result.
        /// </summary>
        /// <param name="transform">Transform whose localScale will be changed.</param>
        /// <param name="target">Target snapshot.</param>
        /// <param name="t">Lerp time.</param>
        public static Vector3 LerpLocalScale(this Transform transform, Snapshot target, float t)
        {
            return transform.LerpLocalScale(target.Scale, t);
        }

        /// <summary>
        /// Lerps the orientation of this <paramref name="transform"/> towards the <paramref name="target"/> snapshot.
        /// Mutates the transform. Uses <see cref="Quaternion.Slerp"/> for rotation.
        /// </summary>
        /// <param name="transform">Transform whose orientation will be changed.</param>
        /// <param name="target">Target snapshot.</param>
        /// <param name="t">Lerp time.</param>
        /// <param name="space">If World: position and rotation will be changed. If Self: localPosition and localRotation will be changed. Scale sets localScale either way.</param>
        /// <param name="lerpPosition">If true, lerps position or localPosition.</param>
        /// <param name="slerpRotation">If true slerps rotation or localRotation.</param>
        /// <param name="lerpLocalScale">If true, lerps localScale.</param>
        /// <remarks>
        /// <see cref="Transform.lossyScale"/> does not have a setter. Therefore scale setter ignores <paramref name="space"/> parameter and always sets to <see cref="Transform.localScale"/>.
        /// </remarks>
        public static void Lerp(this Transform transform,
            Snapshot target,
            float t,
            Space space,
            bool lerpPosition = true,
            bool slerpRotation = true,
            bool lerpLocalScale = true)
        {
            transform.Lerp(target, t, lerpPosition, space, slerpRotation, space, lerpLocalScale);
        }

        /// <summary>
        /// Lerps the orientation of this <paramref name="transform"/> towards the <paramref name="target"/> snapshot.
        /// Mutates the transform. Uses <see cref="Quaternion.Slerp"/> for rotation.
        /// </summary>
        /// <param name="transform">Transform whose orientation will be changed.</param>
        /// <param name="target">Target snapshot.</param>
        /// <param name="t">Lerp time.</param>
        /// <param name="lerpPosition">If true, lerps position or localPosition.</param>
        /// <param name="positionSpace">If World: position will be changed. If Self: localPosition will be changed.</param>
        /// <param name="slerpRotation">If true slerps rotation or localRotation.</param>
        /// <param name="rotationSpace">If World: rotation will be changed. If Self: localRotation will be changed.</param>
        /// <param name="lerpLocalScale">If true, lerps localScale.</param>
        /// <remarks>
        /// <see cref="Transform.lossyScale"/> does not have a setter. Therefore scale setter does not take a <see cref="Space"/> parameter and always sets to <see cref="Transform.localScale"/>.
        /// </remarks>
        public static void Lerp(this Transform transform,
            Snapshot target,
            float t,
            bool lerpPosition = true,
            Space positionSpace = Space.Self,
            bool slerpRotation = true,
            Space rotationSpace = Space.Self,
            bool lerpLocalScale = true)
        {
            if (lerpPosition)
            {
                transform.LerpPosition(target, t, positionSpace);
            }

            if (slerpRotation)
            {
                transform.SlerpRotation(target, t, rotationSpace);
            }

            if (lerpLocalScale)
            {
                transform.LerpLocalScale(target, t);
            }
        }

        /// <summary>
        /// Tweens a Transform's position to the position of the given snapshot.
        /// </summary>
        /// <param name="target">The target transform whose position will be changed.</param>
        /// <param name="snapshot">This snapshot's position will be the end value to reach.</param>
        /// <param name="duration">The duration of the tween.</param>
        /// <param name="snapping">If true the tween will smoothly snap all values to integers.</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(this Transform target,
            Snapshot snapshot,
            float duration,
            bool snapping = false)
        {
            return target.DOMove(snapshot.Position, duration, snapping);
        }

        /// <summary>
        /// Tweens a Transform's localPosition to the position of the given snapshot.
        /// </summary>
        /// <param name="target">The target transform whose localPosition will be changed.</param>
        /// <param name="snapshot">This snapshot's position will be the end value to reach.</param>
        /// <param name="duration">The duration of the tween.</param>
        /// <param name="snapping">If true the tween will smoothly snap all values to integers.</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMove(this Transform target,
            Snapshot snapshot,
            float duration,
            bool snapping = false)
        {
            return target.DOLocalMove(snapshot.Position, duration, snapping);
        }

        /// <summary>
        /// Tweens a Transform's position or localPosition to the position of the given snapshot.
        /// </summary>
        /// <param name="target">The target transform whose position or localPosition will be changed.</param>
        /// <param name="snapshot">This snapshot's position will be the end value to reach.</param>
        /// <param name="space">If World: then tweens position. If Self: then tweens localPosition.</param>
        /// <param name="duration">The duration of the tween.</param>
        /// <param name="snapping">If true the tween will smoothly snap all values to integers.</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOOrientPosition(this Transform target,
            Snapshot snapshot,
            Space space,
            float duration,
            bool snapping = false)
        {
            return space switch
            {
                Space.World => target.DOMove(snapshot, duration, snapping),
                Space.Self => target.DOLocalMove(snapshot, duration, snapping),
                _ => throw new ArgumentOutOfRangeException(nameof(space), space, null)
            };
        }

        /// <summary>
        /// Tweens a Transform's rotation to the rotation of the given snapshot.
        /// </summary>
        /// <param name="target">The target transform whose rotation will be changed.</param>
        /// <param name="snapshot">This snapshot's rotation will be the end value to reach.</param>
        /// <param name="duration">The duration of the tween.</param>
        public static TweenerCore<Quaternion, Quaternion, NoOptions> DORotateQuaternion(this Transform target,
            Snapshot snapshot,
            float duration)
        {
            return target.DORotateQuaternion(snapshot.Rotation, duration);
        }

        /// <summary>
        /// Tweens a Transform's localRotation to the rotation of the given snapshot.
        /// </summary>
        /// <param name="target">The target transform whose localRotation will be changed.</param>
        /// <param name="snapshot">This snapshot's rotation will be the end value to reach.</param>
        /// <param name="duration">The duration of the tween.</param>
        public static TweenerCore<Quaternion, Quaternion, NoOptions> DOLocalRotateQuaternion(this Transform target,
            Snapshot snapshot,
            float duration)
        {
            return target.DOLocalRotateQuaternion(snapshot.Rotation, duration);
        }

        /// <summary>
        /// Tweens a Transform's rotation or localRotation to the rotation of the given snapshot.
        /// </summary>
        /// <param name="target">The target transform whose rotation or localRotation will be changed.</param>
        /// <param name="snapshot">This snapshot's rotation will be the end value to reach.</param>
        /// <param name="space">If World: then tweens rotation. If Self: then tweens localRotation.</param>
        /// <param name="duration">The duration of the tween.</param>
        public static TweenerCore<Quaternion, Quaternion, NoOptions> DOOrientRotation(this Transform target,
            Snapshot snapshot,
            Space space,
            float duration)
        {
            return space switch
            {
                Space.World => target.DORotateQuaternion(snapshot, duration),
                Space.Self => target.DOLocalRotateQuaternion(snapshot, duration),
                _ => throw new ArgumentOutOfRangeException(nameof(space), space, null)
            };
        }

        /// <summary>
        /// Tweens a Transform's localScale to the scale of the given snapshot.
        /// </summary>
        /// <param name="target">The target transform whose localScale will be changed.</param>
        /// <param name="snapshot">This snapshot's scale will be the end value to reach.</param>
        /// <param name="duration">The duration of the tween.</param>
        /// <remarks><see cref="Transform.lossyScale"/> does not have a setter. Therefore scale setter does not take a <see cref="Space"/> parameter and always sets to <see cref="Transform.localScale"/>.</remarks>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this Transform target, Snapshot snapshot, float duration)
        {
            return target.DOScale(snapshot.Scale, duration);
        }

        /// <inheritdoc cref="DOScale(Transform, Snapshot, float)"/>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOOrientScale(this Transform target, Snapshot snapshot, float duration)
        {
            return target.DOScale(snapshot, duration);
        }

        /// <inheritdoc cref="SnapshotTweenBuilder(Transform, Snapshot)"/>
        public static SnapshotTweenBuilder DOOrient(this Transform target, Snapshot snapshot)
        {
            return new SnapshotTweenBuilder(target, snapshot);
        }

        /// <inheritdoc cref="SnapshotTweenBuilder(Transform, Snapshot, float)"/>
        public static SnapshotTweenBuilder DOOrient(this Transform target, Snapshot snapshot, float duration)
        {
            return new SnapshotTweenBuilder(target, snapshot, duration);
        }

        /// <inheritdoc cref="SnapshotTweenBuilder(Transform, Snapshot, SnapshotTweenConfig)"/>
        public static SnapshotTweenBuilder DOOrient(this Transform target, Snapshot snapshot, SnapshotTweenConfig config)
        {
            return new SnapshotTweenBuilder(target, snapshot, config);
        }
    }
}
