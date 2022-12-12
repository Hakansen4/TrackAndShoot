using System;
using System.Globalization;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.Snapshotting
{
    /// <summary>
    /// A specific orientation in space. Consists of a position, a rotation, and a scale.
    /// Intended to store transform orientations as a whole.
    /// </summary>
    [PublicAPI]
    [Serializable]
    public struct Snapshot : IEquatable<Snapshot>, IFormattable, ICloneable
    {
        [field: SerializeField]
        public Vector3 Position { get; [UsedImplicitly] private set; }

        [field: SerializeField]
        public Quaternion Rotation { get; [UsedImplicitly] private set; }

        [field: SerializeField]
        public Vector3 Scale { get; [UsedImplicitly] private set; }

        /// <summary>
        /// Create a new Snapshot with the given values.
        /// </summary>
        public Snapshot(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        /// <summary>
        /// Create a new Snapshot that is a memberwise clone of <paramref name="source"/>.
        /// </summary>
        public Snapshot(Snapshot source)
            : this(source.Position, source.Rotation, source.Scale) {}

        /// <summary>
        /// Create a new Snapshot with the current values of the given <paramref name="transform"/>.
        /// <para>Data flow direction: Transform -> Snapshot</para>
        /// </summary>
        /// <param name="transform">The transform whose orientation will be snapshot.</param>
        /// <param name="positionGetSpace">If World, gets position. If Self, gets localPosition.</param>
        /// <param name="rotationGetSpace">If World, gets rotation. If Self, gets localRotation.</param>
        /// <param name="scaleGetSpace">If World, gets lossyScale. If Self, gets localScale.</param>
        /// <remarks>
        /// Be wary of the unintended consequences if you are setting <paramref name="scaleGetSpace"/> to World. <see cref="Transform.lossyScale"/> does not have a setter. Therefore all setter utilities are designed to only set <see cref="Transform.localScale"/>.
        /// </remarks>
        public Snapshot(Transform transform, Space positionGetSpace, Space rotationGetSpace, Space scaleGetSpace = Space.Self)
            : this(positionGetSpace switch
                {
                    Space.World => transform.position,
                    Space.Self => transform.localPosition,
                    _ => throw new ArgumentOutOfRangeException(nameof(positionGetSpace), positionGetSpace, null)
                },
                rotationGetSpace switch
                {
                    Space.World => transform.rotation,
                    Space.Self => transform.localRotation,
                    _ => throw new ArgumentOutOfRangeException(nameof(rotationGetSpace), rotationGetSpace, null)
                },
                scaleGetSpace switch
                {
                    Space.World => transform.lossyScale,
                    Space.Self => transform.localScale,
                    _ => throw new ArgumentOutOfRangeException(nameof(scaleGetSpace), scaleGetSpace, null)
                }
            ) {}

        /// <summary>
        /// Create a new Snapshot with the current values of the given <paramref name="transform"/>.
        /// <para>Data flow direction: Transform -> Snapshot</para>
        /// </summary>
        /// <param name="transform">The transform whose orientation will be snapshot.</param>
        /// <param name="getSpace">If World, gets position, rotation, and lossyScale. If Self, gets localPosition, localRotation, and localScale.</param>
        /// <param name="getLocalScale">If true, scale getter disregards <paramref name="getSpace"/> parameter and gets localScale. If false, scale getter respects <paramref name="getSpace"/> parameter.</param>
        /// <remarks>
        /// Be wary of the unintended consequences if you are setting <paramref name="getLocalScale"/> to false. <see cref="Transform.lossyScale"/> does not have a setter. Therefore all setter utilities are designed to only set <see cref="Transform.localScale"/>.
        /// </remarks>
        public Snapshot(Transform transform, Space getSpace, bool getLocalScale = true)
            : this(transform,
                getSpace,
                getSpace,
                getLocalScale ? Space.Self : getSpace
            ) {}

        /// <summary>
        /// Lerps between two snapshots. Uses <see cref="Quaternion.Slerp"/> for rotation.
        /// </summary>
        public static Snapshot Lerp(Snapshot a, Snapshot b, float t)
        {
            var pos = Vector3.Lerp(a.Position, b.Position, t);
            var rot = Quaternion.Slerp(a.Rotation, b.Rotation, t);
            var scale = Vector3.Lerp(a.Scale, b.Scale, t);
            return new Snapshot(pos, rot, scale);
        }

        /// <summary>
        /// Lerps from this snapshot to the <paramref name="other"/> snapshot. Uses <see cref="Quaternion.Slerp"/> for rotation.
        /// </summary>
        public Snapshot LerpTowards(Snapshot other, float t)
        {
            return Lerp(this, other, t);
        }

        /// <summary>
        /// Returns a new Snapshot that is the same with this one but has the given <paramref name="position"/>.
        /// </summary>
        public Snapshot WithPosition(Vector3 position)
        {
            return new Snapshot(position, Rotation, Scale);
        }

        /// <summary>
        /// Returns a new Snapshot that is the same with this one but has the given <paramref name="rotation"/>.
        /// </summary>
        public Snapshot WithRotation(Quaternion rotation)
        {
            return new Snapshot(Position, rotation, Scale);
        }

        /// <summary>
        /// Returns a new Snapshot that is the same with this one but has the given <paramref name="scale"/>.
        /// </summary>
        public Snapshot WithScale(Vector3 scale)
        {
            return new Snapshot(Position, Rotation, scale);
        }

        /// <summary>
        /// Selectively sets values of the given <paramref name="transform"/> from this <see cref="Snapshot"/>.
        /// <para>Data flow direction: Snapshot -> Transform</para>
        /// </summary>
        /// <param name="transform">Target transform whose orientation will be changed.</param>
        /// <param name="setSpace">If World, sets position and rotation. If Self, sets localPosition and localRotation.</param>
        /// <param name="setPosition">If true, sets position or localPosition.</param>
        /// <param name="setRotation">If true, sets rotation or localRotation.</param>
        /// <param name="setLocalScale">If true, sets localScale.</param>
        /// <remarks>
        /// <see cref="Transform.lossyScale"/> does not have a setter, therefore scale setter will disregard the <paramref name="setSpace"/> parameter and always set to localScale.
        /// </remarks>
        public void SetTransform(Transform transform,
            Space setSpace,
            bool setPosition = true,
            bool setRotation = true,
            bool setLocalScale = true)
        {
            SetTransform(transform, setPosition, setSpace, setRotation, setSpace, setLocalScale);
        }

        /// <summary>
        /// Selectively sets values of the given <paramref name="transform"/> from this <see cref="Snapshot"/>.
        /// <para>Data flow direction: Snapshot -> Transform</para>
        /// </summary>
        /// <param name="transform">Target transform whose orientation will be changed.</param>
        /// <param name="setPosition">If true, sets position or localPosition.</param>
        /// <param name="positionSetSpace">If World, sets position. If Self, sets localPosition.</param>
        /// <param name="setRotation">If true, sets rotation or localRotation.</param>
        /// <param name="rotationSetSpace">If World, sets rotation. If Self, sets localRotation.</param>
        /// <param name="setLocalScale">If true, sets localScale.</param>
        /// <remarks>
        /// <see cref="Transform.lossyScale"/> does not have a setter, therefore this method does not have a <see cref="Space"/> parameter for scale.
        /// </remarks>
        public void SetTransform(Transform transform,
            bool setPosition = true,
            Space positionSetSpace = Space.Self,
            bool setRotation = true,
            Space rotationSetSpace = Space.Self,
            bool setLocalScale = true)
        {
            if (setPosition)
            {
                SetTransformPosition(transform, positionSetSpace);
            }

            if (setRotation)
            {
                SetTransformRotation(transform, rotationSetSpace);
            }

            if (setLocalScale)
            {
                SetTransformLocalScale(transform);
            }
        }

        /// <summary>
        /// Set the position or localPosition of the given <paramref name="transform"/> from the <see cref="Position"/> of this <see cref="Snapshot"/>.
        /// <para>Data flow direction: Snapshot -> Transform</para>
        /// </summary>
        /// <param name="transform">Target transform whose position or localPosition will be changed.</param>
        /// <param name="setSpace">If World, sets position. If Self, sets localPosition.</param>
        public void SetTransformPosition(Transform transform, Space setSpace)
        {
            switch (setSpace)
            {
                case Space.World:
                    transform.position = Position;
                    break;

                case Space.Self:
                    transform.localPosition = Position;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Set the rotation or localRotation of the given <paramref name="transform"/> from the <see cref="Rotation"/> of this <see cref="Snapshot"/>.
        /// <para>Data flow direction: Snapshot -> Transform</para>
        /// </summary>
        /// <param name="transform">Target transform whose rotation or localRotation will be changed.</param>
        /// <param name="setSpace">If World, sets rotation. If Self, sets localRotation.</param>
        public void SetTransformRotation(Transform transform, Space setSpace)
        {
            switch (setSpace)
            {
                case Space.World:
                    transform.rotation = Rotation;
                    break;

                case Space.Self:
                    transform.localRotation = Rotation;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Set the <see cref="Transform.localScale"/> of the given <paramref name="transform"/> from the <see cref="Scale"/> of this <see cref="Snapshot"/>.
        /// <para>Data flow direction: Snapshot -> Transform</para>
        /// </summary>
        /// <param name="transform">The target transform whose localScale will be changed.</param>
        /// <remarks>
        /// <see cref="Transform.lossyScale"/> does not have a setter, therefore this method does not have a <see cref="Space"/> parameter.
        /// </remarks>
        public void SetTransformLocalScale(Transform transform)
        {
            transform.localScale = Scale;
        }

        /// <summary>
        /// Returns whether the <paramref name="other"/> Snapshot is APPROXIMATELY equal to this one.
        /// </summary>
        public bool ApproxEquals(Snapshot other)
        {
            return Position == other.Position && Rotation == other.Rotation && Scale == other.Scale;
        }

        /// <summary>
        /// Returns whether the <paramref name="obj"/> is a Snapshot AND it is APPROXIMATELY equal to this one.
        /// </summary>
        public bool ApproxEquals(object obj)
        {
            return obj is Snapshot other && ApproxEquals(other);
        }

        /// <summary>
        /// Returns whether the <paramref name="other"/> Snapshot is EXACTLY equal to this one.
        /// </summary>
        public bool Equals(Snapshot other)
        {
            return Position.Equals(other.Position) && Rotation.Equals(other.Rotation) && Scale.Equals(other.Scale);
        }

        /// <summary>
        /// Returns whether the <paramref name="obj"/> is a Snapshot AND it is EXACTLY equal to this one.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Snapshot other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Rotation.GetHashCode();
                hashCode = (hashCode * 397) ^ Scale.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Returns whether given Snapshots are APPROXIMATELY equal.
        /// For EXACT equality, use <see cref="Equals(EasyClap.Seneca.Common.Snapshotting.Snapshot)"/>.
        /// </summary>
        public static bool operator ==(Snapshot left, Snapshot right)
        {
            return left.ApproxEquals(right);
        }

        /// <summary>
        /// Returns whether given Snapshots are APPROXIMATELY NOT equal.
        /// For EXACT equality, use <see cref="Equals(EasyClap.Seneca.Common.Snapshotting.Snapshot)"/>.
        /// </summary>
        public static bool operator !=(Snapshot left, Snapshot right)
        {
            return !left.ApproxEquals(right);
        }

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public override string ToString() => ToString(null, CultureInfo.InvariantCulture.NumberFormat);

        /// <inheritdoc cref="ToString(string, IFormatProvider)"/>
        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture.NumberFormat);

        /// <summary>
        /// Returns a formatted string for this Snapshot.
        /// </summary>
        /// <param name="format">A numeric format string.</param>
        /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "F1";
            }

            return $"Snapshot[ P: {Position.ToString(format, formatProvider)} | R: {Rotation.ToString(format, formatProvider)} | S: {Scale.ToString(format, formatProvider)} ]";
        }

        /// <summary>
        /// Returns the result of the copy constructor, supplying this instance as the parameter.
        /// Prefer using the copy constructor to avoid boxing.
        /// </summary>
        /// <seealso cref="Snapshot(Snapshot)">The copy constructor</seealso>
        public object Clone()
        {
            return new Snapshot(this);
        }
    }
}
