using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.Utility
{
    /// <summary>
    /// Includes some utilities for selecting the most fitting item from a collection based on custom criteria.
    /// </summary>
    public static class SelectionUtilities
    {
        /// <summary>
        /// Used by <see cref="SelectionUtilities.SelectFittest{TItem,TByproduct}"/> for calculating the comparison criteria.
        /// </summary>
        public delegate TCriteria CriteriaCalculator<in TItem, out TCriteria>(TItem item);

        /// <summary>
        /// Used by <see cref="SelectionUtilities.SelectFittest{TItem,TByproduct}"/> as the criteria comparator.
        /// </summary>
        public delegate bool CriteriaComparer<in TCriteria>(TCriteria leaderByproduct, TCriteria contenderByproduct);

        // TODO: make an optimized version for IList that runs a for loop
        /// <summary>
        /// Selects the most fitting item in the given enumerable via custom criteria.
        /// Does NOT mutate <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The enumerable we are selecting from.</param>
        /// <param name="criteriaCalculator">The criteria calculator. Take in an item, return a criteria to be used in comparison.</param>
        /// <param name="criteriaComparer">The criteria comparator. Return true if the leader wins. Return false if the contender wins.</param>
        /// <returns>The most fitting item.</returns>
        /// <exception cref="InvalidOperationException">Throws if the <paramref name="source"/> is empty.</exception>
        public static TItem SelectFittest<TItem, TByproduct>([NotNull] this IEnumerable<TItem> source,
            [NotNull] CriteriaCalculator<TItem, TByproduct> criteriaCalculator,
            [NotNull] CriteriaComparer<TByproduct> criteriaComparer)
        {
            using var sourceIterator = source.GetEnumerator();

            if (!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            var leader = sourceIterator.Current;
            var leaderCriteria = criteriaCalculator(leader);

            while (sourceIterator.MoveNext())
            {
                var contender = sourceIterator.Current;
                var contenderCriteria = criteriaCalculator(contender);

                var didLeaderWin = criteriaComparer(leaderCriteria, contenderCriteria);
                if (didLeaderWin)
                {
                    continue;
                }

                leader = contender;
                leaderCriteria = contenderCriteria;
            }

            return leader;
        }
    }
}
