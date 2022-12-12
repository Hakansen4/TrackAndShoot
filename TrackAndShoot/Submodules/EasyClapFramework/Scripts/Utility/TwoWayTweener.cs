using System;
using DG.Tweening;
using EasyClap.Seneca.Common.Snapshotting;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyClap.Seneca.Common.Utility
{
    /// <summary>
    /// A component that can tween its transform between two orientations (<see cref="Snapshot"/>s).
    /// </summary>
    [PublicAPI]
    public class TwoWayTweener : MonoBehaviour
    {
        // ReSharper disable InconsistentNaming
        private const string __Inspector_GroupName_Toolbox = "Two-Way Tweener Toolbox";
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Indicates which state a <see cref="TwoWayTweener"/> is either currently in or tweening towards.
        /// </summary>
        public enum State
        {
            First,
            Second
        }

        /// <summary>
        /// Snapshot of the first orientation.
        /// </summary>
        [field: SerializeField]
        public Snapshot FirstSnapshot { get; [UsedImplicitly] private set; }

        /// <summary>
        /// Snapshot of the second orientation.
        /// </summary>
        [field: SerializeField]
        public Snapshot SecondSnapshot { get; [UsedImplicitly] private set; }

        /// <summary>
        /// Configuration for the tween towards the first orientation.
        /// </summary>
        [field: SerializeField]
        public SnapshotTweenConfig ToFirstTweenConfig { get; [UsedImplicitly] private set; }

        /// <summary>
        /// Configuration for the tween towards the second orientation.
        /// </summary>
        [field: SerializeField]
        public SnapshotTweenConfig ToSecondTweenConfig { get; [UsedImplicitly] private set; }

        /// <summary>
        /// The orientation that this tweener is either currently in, or tweening towards.
        /// </summary>
        [field: ShowInInspector, DisplayAsString, FoldoutGroup(__Inspector_GroupName_Toolbox)]
        public State CurrentState { get; private set; } = State.First;

        /// <summary>
        /// The current or last active tween this tweener has created.
        /// Check <see cref="IsCurrentlyTweening"/> to distinguish whether this tween is currently active.
        /// </summary>
        public Tween ActiveTween { get; private set; } = null;

        /// <summary>
        /// True if this tweener is currently tweening. False if it is idle.
        /// </summary>
        [field: ShowInInspector, DisplayAsString, FoldoutGroup(__Inspector_GroupName_Toolbox)]
        public bool IsCurrentlyTweening { get; private set; } = false;

        /// <summary>
        /// Tweens to the next state (to first if in second, and to second if in first). Kills the <see cref="ActiveTween"/> first.
        /// </summary>
        /// <param name="completeActiveTween">If true, <see cref="ActiveTween"/> will be completed (teleported) before being killed.</param>
        /// <param name="callback">An optional callback that will be invoked when the tween is completed or killed.</param>
        [Button, FoldoutGroup(__Inspector_GroupName_Toolbox)]
        public void TweenToNext(bool completeActiveTween = false, TweenCallback callback = null)
        {
            switch (CurrentState)
            {
                case State.First:
                    TweenToSecond(completeActiveTween, callback);
                    break;
                case State.Second:
                    TweenToFirst(completeActiveTween, callback);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Tweens to the first state. Kills the <see cref="ActiveTween"/> first.
        /// </summary>
        /// <inheritdoc cref="TweenToNext"/>
        [Button, FoldoutGroup(__Inspector_GroupName_Toolbox)]
        public void TweenToFirst(bool completeActiveTween = false, TweenCallback callback = null)
        {
            IsCurrentlyTweening = true;
            CurrentState = State.First;

            ActiveTween?.Kill(completeActiveTween);
            ActiveTween = transform.DOOrient(FirstSnapshot, ToFirstTweenConfig)
                .Build()
                .AppendCallback(() => IsCurrentlyTweening = false)
                .AppendCallback(callback)
                .Play();
        }

        /// <summary>
        /// Tweens to the second state. Kills the <see cref="ActiveTween"/> first.
        /// </summary>
        /// <inheritdoc cref="TweenToNext"/>
        [Button, FoldoutGroup(__Inspector_GroupName_Toolbox)]
        public void TweenToSecond(bool completeActiveTween = false, TweenCallback callback = null)
        {
            IsCurrentlyTweening = true;
            CurrentState = State.Second;

            ActiveTween?.Kill(completeActiveTween);
            ActiveTween = transform.DOOrient(SecondSnapshot, ToSecondTweenConfig)
                .Build()
                .AppendCallback(() => IsCurrentlyTweening = false)
                .AppendCallback(callback)
                .Play();
        }
    }
}
