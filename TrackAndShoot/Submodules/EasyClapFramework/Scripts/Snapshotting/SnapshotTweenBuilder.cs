using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyClap.Seneca.Common.Snapshotting
{
    /// <summary>
    /// Builder for a <see cref="DG.Tweening.Sequence"/> which can selectively tween values of a <see cref="Transform"/> to a <see cref="Snapshot"/>.
    /// </summary>
    /// <remarks>
    /// <para>The API is designed as a builder with fluent interface. You can chain setter methods to quickly set configuration selectively.</para>
    /// <para>When you are done configuring, call <see cref="SnapshotTweenBuilder.Build"/> to get the <see cref="DG.Tweening.Sequence"/>.</para>
    /// <para>Properties not present as constructor arguments all have meaningful default values, so don't feel the need to set all of them.</para>
    /// <para><see cref="Transform.lossyScale"/> does not have a setter. Therefore scale setters always set to <see cref="Transform.localScale"/> and doesn't take a <see cref="Space"/> parameter.</para>
    /// <para>Although not recommended, you can build multiple sequences from the same builder. Just keep in mind that none of the calls to the setters will affect a <see cref="DG.Tweening.Sequence"/> already returned from the <see cref="Build"/> method.</para>
    /// <para>Individual tweens in the sequence are chained with <see cref="TweenSettingsExtensions.Join"/>, which means they will all play at the same time.</para>
    /// </remarks>
    [PublicAPI]
    public struct SnapshotTweenBuilder
    {
        /// <summary>
        /// The target transform whose values will be tweened.
        /// </summary>
        public Transform Target { get; private set; }

        /// <summary>
        /// This snapshot's values will be the end values of the tweens.
        /// </summary>
        public Snapshot Snapshot { get; private set; }

        /// <summary>
        /// Configuration of the tweens.
        /// </summary>
        public SnapshotTweenConfig Config { get; private set; }

        /// <param name="target">The target transform whose values will be tweened.</param>
        /// <param name="snapshot">This snapshot's values will be the end values of the tweens.</param>
        /// <inheritdoc cref="SnapshotTweenBuilder"/>
        public SnapshotTweenBuilder(Transform target, Snapshot snapshot)
        {
            Target = target;
            Snapshot = snapshot;
            Config = new SnapshotTweenConfig();
        }

        /// <param name="target">The target transform whose values will be tweened.</param>
        /// <param name="snapshot">This snapshot's values will be the end values of the tweens.</param>
        /// <param name="duration">Duration of the tweens.</param>
        /// <inheritdoc cref="SnapshotTweenBuilder"/>
        public SnapshotTweenBuilder(Transform target, Snapshot snapshot, float duration)
            : this(target, snapshot)
        {
            Config.SetDuration(duration);
        }

        /// <param name="target">The target transform whose values will be tweened.</param>
        /// <param name="snapshot">This snapshot's values will be the end values of the tweens.</param>
        /// <param name="config">Configuration of the tweens.</param>
        /// <inheritdoc cref="SnapshotTweenBuilder"/>
        public SnapshotTweenBuilder(Transform target, Snapshot snapshot, SnapshotTweenConfig config)
        {
            Target = target;
            Snapshot = snapshot;
            Config = config;
        }

        /// <summary>
        /// Sets <see cref="Config"/>: <inheritdoc cref="Config"/>
        /// </summary>
        public SnapshotTweenBuilder SetConfig(SnapshotTweenConfig value) { Config = value; return this; }

        /// <summary>
        /// Sets <see cref="Target"/>: <inheritdoc cref="Target"/>
        /// </summary>
        public SnapshotTweenBuilder SetTarget(Transform value) { Target = value; return this; }

        /// <summary>
        /// Sets <see cref="Snapshot"/>: <inheritdoc cref="Snapshot"/>
        /// </summary>
        public SnapshotTweenBuilder SetSnapshot(Snapshot value) { Snapshot = value; return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetDuration"/>
        public SnapshotTweenBuilder SetDuration(float value) { Config.SetDuration(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetPositionTweenEnabled"/>
        public SnapshotTweenBuilder SetPositionTweenEnabled(bool value) { Config.SetPositionTweenEnabled(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetPositionSpace"/>
        public SnapshotTweenBuilder SetPositionSpace(Space value) { Config.SetPositionSpace(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetPositionEase"/>
        public SnapshotTweenBuilder SetPositionEase(Ease value) { Config.SetPositionEase(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetPositionSnapping"/>
        public SnapshotTweenBuilder SetPositionSnapping(bool value) { Config.SetPositionSnapping(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetRotationTweenEnabled"/>
        public SnapshotTweenBuilder SetRotationTweenEnabled(bool value) { Config.SetRotationTweenEnabled(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetRotationSpace"/>
        public SnapshotTweenBuilder SetRotationSpace(Space value) { Config.SetRotationSpace(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetRotationEase"/>
        public SnapshotTweenBuilder SetRotationEase(Ease value) { Config.SetRotationEase(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetLocalScaleTweenEnabled"/>
        public SnapshotTweenBuilder SetLocalScaleTweenEnabled(bool value) { Config.SetLocalScaleTweenEnabled(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetLocalScaleEase"/>
        public SnapshotTweenBuilder SetLocalScaleEase(Ease value) { Config.SetLocalScaleEase(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetAllEase"/>
        public SnapshotTweenBuilder SetAllEase(Ease value) { Config.SetAllEase(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetAllSpace"/>
        public SnapshotTweenBuilder SetAllSpace(Space value) { Config.SetAllSpace(value); return this; }

        /// <inheritdoc cref="SnapshotTweenConfig.SetAllTweensEnabled"/>
        public SnapshotTweenBuilder SetAllTweensEnabled(bool value) { Config.SetAllTweensEnabled(value); return this; }

        /// <summary>
        /// Builds the <see cref="DG.Tweening.Sequence"/> with the current configuration.
        /// <see cref="SnapshotTweenBuilder"/> cannot make changes to the sequence after it is built.
        /// Make sure you call this method AFTER you are done with the configuration.
        /// </summary>
        /// <returns>A <see cref="DG.Tweening.Sequence"/> that is configured with the current state of this builder.</returns>
        public Sequence Build()
        {
            var seq = DOTween.Sequence().SetTarget(Target);

            if (Config.PositionTweenEnabled)
            {
                seq.Join(Target.DOOrientPosition(Snapshot, Config.PositionSpace, Config.Duration, Config.PositionSnapping).SetEase(Config.PositionEase));
            }

            if (Config.RotationTweenEnabled)
            {
                seq.Join(Target.DOOrientRotation(Snapshot, Config.RotationSpace, Config.Duration).SetEase(Config.RotationEase));
            }

            if (Config.LocalScaleTweenEnabled)
            {
                seq.Join(Target.DOOrientScale(Snapshot, Config.Duration).SetEase(Config.LocalScaleEase));
            }

            return seq;
        }
    }
}
