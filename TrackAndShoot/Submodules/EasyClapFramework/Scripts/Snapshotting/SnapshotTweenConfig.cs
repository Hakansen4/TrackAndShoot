using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.Snapshotting
{
    /// <summary>
    /// Contains configuration for a <see cref="DG.Tweening.Sequence"/> which can selectively tween values of a <see cref="Transform"/> to a <see cref="Snapshot"/>.
    /// </summary>
    /// <remarks>
    /// <para>The API is designed with fluent interface. You can chain setter methods to quickly set configuration selectively.</para>
    /// <para>All properties have meaningful default values, so don't feel the need to set all of them.</para>
    /// <para><see cref="Transform.lossyScale"/> does not have a setter. Therefore scale setters always set to <see cref="Transform.localScale"/> and doesn't take a <see cref="Space"/> parameter.</para>
    /// </remarks>
    [PublicAPI]
    [System.Serializable]
    public class SnapshotTweenConfig
    {
        // ReSharper disable InconsistentNaming
        private const string __Inspector_GroupName_Position = "Position";
        private const string __Inspector_GroupName_Rotation = "Rotation";
        private const string __Inspector_GroupName_LocalScale = "Local Scale";
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Duration of the tweens.
        /// </summary>
        [field: SerializeField]
        [field: SuffixLabel("seconds")]
        public float Duration { get; [UsedImplicitly] private set; } = 1f;

        /// <summary>
        /// If true, position or localPosition will be tweened.
        /// </summary>
        [field: SerializeField]
        [field: TitleGroup(__Inspector_GroupName_Position)]
        public bool PositionTweenEnabled { get; [UsedImplicitly] private set; } = true;

        /// <summary>
        /// If World: then tweens position. If Self: then tweens localPosition.
        /// </summary>
        [field: SerializeField]
        [field: TitleGroup(__Inspector_GroupName_Position)]
        public Space PositionSpace { get; [UsedImplicitly] private set; } = Space.Self;

        /// <summary>
        /// Ease of the position or localPosition tween.
        /// </summary>
        [field: SerializeField]
        [field: TitleGroup(__Inspector_GroupName_Position)]
        public Ease PositionEase { get; [UsedImplicitly] private set; } = Ease.Linear;

        /// <summary>
        /// If true, sets snapping option to true on the position or localPosition tween.
        /// </summary>
        [field: SerializeField]
        [field: TitleGroup(__Inspector_GroupName_Position)]
        public bool PositionSnapping { get; [UsedImplicitly] private set; } = false;

        /// <summary>
        /// If true, rotation or localRotation will be tweened.
        /// </summary>
        [field: SerializeField]
        [field: TitleGroup(__Inspector_GroupName_Rotation)]
        public bool RotationTweenEnabled { get; [UsedImplicitly] private set; } = true;

        /// <summary>
        /// If World: then tweens rotation. If Self: then tweens localRotation.
        /// </summary>
        [field: SerializeField]
        [field: TitleGroup(__Inspector_GroupName_Rotation)]
        public Space RotationSpace { get; [UsedImplicitly] private set; } = Space.Self;

        /// <summary>
        /// Ease of the rotation or localRotation tween.
        /// </summary>
        [field: SerializeField]
        [field: TitleGroup(__Inspector_GroupName_Rotation)]
        public Ease RotationEase { get; [UsedImplicitly] private set; } = Ease.Linear;

        /// <summary>
        /// If true, localScale will be tweened.
        /// </summary>
        /// <remarks>
        /// It is not possible to tween <see cref="Transform.lossyScale"/> since it does not have a setter.
        /// </remarks>
        [field: SerializeField]
        [field: TitleGroup(__Inspector_GroupName_LocalScale)]
        public bool LocalScaleTweenEnabled { get; [UsedImplicitly] private set; } = true;

        /// <summary>
        /// Ease of the localScale tween.
        /// </summary>
        /// <remarks><inheritdoc cref="LocalScaleTweenEnabled"/></remarks>
        [field: SerializeField]
        [field: TitleGroup(__Inspector_GroupName_LocalScale)]
        public Ease LocalScaleEase { get; [UsedImplicitly] private set; } = Ease.Linear;

        /// <summary>
        /// Sets <see cref="Duration"/>: <inheritdoc cref="Duration"/>
        /// </summary>
        public SnapshotTweenConfig SetDuration(float value) { Duration = value; return this; }

        /// <summary>
        /// Sets <see cref="PositionTweenEnabled"/>: <inheritdoc cref="PositionTweenEnabled"/>
        /// </summary>
        public SnapshotTweenConfig SetPositionTweenEnabled(bool value) { PositionTweenEnabled = value; return this; }

        /// <summary>
        /// Sets <see cref="PositionSpace"/>: <inheritdoc cref="PositionSpace"/>
        /// </summary>
        public SnapshotTweenConfig SetPositionSpace(Space value) { PositionSpace = value; return this; }

        /// <summary>
        /// Sets <see cref="PositionEase"/>: <inheritdoc cref="PositionEase"/>
        /// </summary>
        public SnapshotTweenConfig SetPositionEase(Ease value) { PositionEase = value; return this; }

        /// <summary>
        /// Sets <see cref="PositionSnapping"/>: <inheritdoc cref="PositionSnapping"/>
        /// </summary>
        public SnapshotTweenConfig SetPositionSnapping(bool value) { PositionSnapping = value; return this; }

        /// <summary>
        /// Sets <see cref="RotationTweenEnabled"/>: <inheritdoc cref="RotationTweenEnabled"/>
        /// </summary>
        public SnapshotTweenConfig SetRotationTweenEnabled(bool value) { RotationTweenEnabled = value; return this; }

        /// <summary>
        /// Sets <see cref="RotationSpace"/>: <inheritdoc cref="RotationSpace"/>
        /// </summary>
        public SnapshotTweenConfig SetRotationSpace(Space value) { RotationSpace = value; return this; }

        /// <summary>
        /// Sets <see cref="RotationEase"/>: <inheritdoc cref="RotationEase"/>
        /// </summary>
        public SnapshotTweenConfig SetRotationEase(Ease value) { RotationEase = value; return this; }

        /// <summary>
        /// Sets <see cref="LocalScaleTweenEnabled"/>: <inheritdoc cref="LocalScaleTweenEnabled"/>
        /// </summary>
        /// <remarks><inheritdoc cref="LocalScaleTweenEnabled"/></remarks>
        public SnapshotTweenConfig SetLocalScaleTweenEnabled(bool value) { LocalScaleTweenEnabled = value; return this; }

        /// <summary>
        /// Sets <see cref="LocalScaleEase"/>: <inheritdoc cref="LocalScaleEase"/>
        /// </summary>
        /// <remarks><inheritdoc cref="LocalScaleEase"/></remarks>
        public SnapshotTweenConfig SetLocalScaleEase(Ease value) { LocalScaleEase = value; return this; }

        /// <summary>
        /// Sets all of:
        /// <list type="number">
        ///   <item><see cref="PositionEase"/>: <inheritdoc cref="PositionEase"/></item>
        ///   <item><see cref="RotationEase"/>: <inheritdoc cref="RotationEase"/></item>
        ///   <item><see cref="LocalScaleEase"/>: <inheritdoc cref="LocalScaleEase"/></item>
        /// </list>
        /// </summary>
        /// <remarks><inheritdoc cref="LocalScaleEase"/></remarks>
        public SnapshotTweenConfig SetAllEase(Ease value)
        {
            SetPositionEase(value);
            SetRotationEase(value);
            SetLocalScaleEase(value);
            return this;
        }

        /// <summary>
        /// Sets all of:
        /// <list type="number">
        ///   <item><see cref="PositionSpace"/>: <inheritdoc cref="PositionSpace"/></item>
        ///   <item><see cref="RotationSpace"/>: <inheritdoc cref="RotationSpace"/></item>
        /// </list>
        /// </summary>
        /// <remarks><inheritdoc cref="LocalScaleEase"/></remarks>
        public SnapshotTweenConfig SetAllSpace(Space value)
        {
            SetPositionSpace(value);
            SetRotationSpace(value);
            return this;
        }

        /// <summary>
        /// Sets all of:
        /// <list type="number">
        ///   <item><see cref="PositionTweenEnabled"/>: <inheritdoc cref="PositionTweenEnabled"/></item>
        ///   <item><see cref="RotationTweenEnabled"/>: <inheritdoc cref="RotationTweenEnabled"/></item>
        ///   <item><see cref="LocalScaleTweenEnabled"/>: <inheritdoc cref="LocalScaleTweenEnabled"/></item>
        /// </list>
        /// </summary>
        /// <remarks><inheritdoc cref="LocalScaleEase"/></remarks>
        public SnapshotTweenConfig SetAllTweensEnabled(bool value)
        {
            SetPositionTweenEnabled(value);
            SetRotationTweenEnabled(value);
            SetLocalScaleTweenEnabled(value);
            return this;
        }
    }
}
