using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EasyClap.Seneca.Common.Core;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EasyClap.Seneca.Common.Utility
{
    [PublicAPI]
    public static class Utils
    {
        public static readonly char[] EnglishAlphabetUppercase =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// Loads the singe instance of <typeparamref name="T"/> from the root of any Resources folder.
        /// </summary>
        /// <param name="path">Path to look, relative to the Resources folder. If empty, looks in root of all Resources folders.</param>
        /// <typeparam name="T">What are we searching for?</typeparam>
        /// <returns>The single instance of <typeparamref name="T"/>.</returns>
        /// <exception cref="SenecaCommonException">Throws if found no instance or multiple instances.</exception>
        public static T LoadSingleResource<T>(string path) where T : UnityEngine.Object
        {
            var instances = Resources.LoadAll<T>(path);

            if (instances.Length < 1)
            {
                throw new SenecaCommonException($"No instances of {GetFormattedName<T>()} found! Make sure an instance exists under the root of a Resources folder.");
            }

            if (instances.Length > 1)
            {
                throw new SenecaCommonException($"Multiple instances of {GetFormattedName<T>()} found! Make sure there is only one instance under the roots of all Resources folders combined.");
            }

            return instances[0];
        }

        /// <inheritdoc cref="Extensions.GetFormattedName(System.Type)"/>
        public static string GetFormattedName<T>()
        {
            return typeof(T).GetFormattedName();
        }

        /// <summary>
        /// Returns the streamlined level name, given a level index.
        /// </summary>
        public static string GetLevelName(int levelIndex)
        {
            return $"Level_{levelIndex}";
        }

        /// <summary>
        /// Identity function. Returns the input as-is.
        /// </summary>
        public static T Identity<T>(T x)
        {
            return x;
        }

        /// <summary>
        /// Returns a random uppercase English letter, excluding those in <paramref name="except"/>.
        /// </summary>
        public static char GetRandomUppercaseEnglishLetter(IEnumerable<char> except = null)
        {
            var source = except == null ? EnglishAlphabetUppercase : EnglishAlphabetUppercase.Except(except);
            return source.ToArray().GetRandomItem();
        }

        /// <summary>
        /// Returns a string that consists of random uppercase English letters.
        /// Will not contain any letters given in <paramref name="excludedLetters"/>.
        /// </summary>
        public static string GetRandomUppercaseString(int length, IEnumerable<char> excludedLetters = null)
        {
            var excludedLettersArr = excludedLetters == null
                ? null
                : excludedLetters as char[] ?? excludedLetters.ToArray();

            var str = "";
            for (var i = 0; i < length; i++)
            {
                str += GetRandomUppercaseEnglishLetter(excludedLettersArr);
            }
            return str;
        }

        /// <summary>
        /// Returns a random integer with the guarantee that it will not be <paramref name="except"/>.
        /// </summary>
        public static int GetRandomIntExcept(int minInclusive, int maxExclusive, int except)
        {
            bool Condition(int num) => num != except;
            int Generator() => Random.Range(minInclusive, maxExclusive);
            return ConditionalGenerate(Condition, Generator);
        }

        /// <inheritdoc cref="GetRandomIntExcept(int,int,IList{int})"/>
        public static int GetRandomIntExcept(int minInclusive, int maxExclusive, params int[] except)
            => GetRandomIntExcept(minInclusive, maxExclusive, (IList<int>)except);

        /// <summary>
        /// Returns a random integer with the guarantee that it will not be any of <paramref name="except"/>.
        /// </summary>
        public static int GetRandomIntExcept(int minInclusive, int maxExclusive, IList<int> except)
        {
            bool Condition(int num) => !except.Contains(num);
            int Generator() => Random.Range(minInclusive, maxExclusive);
            return ConditionalGenerate(Condition, Generator);
        }

        /// <summary>
        /// Keeps generating a value until a pass condition is met.
        /// </summary>
        /// <param name="passCondition">The condition. We will keep generating new values as long as this predicate returns true.</param>
        /// <param name="generator">The generator. This function will be used to generate values.</param>
        /// <typeparam name="TResult">Type of the generated value.</typeparam>
        /// <returns>The first generated value that satisfied the condition.</returns>
        public static TResult ConditionalGenerate<TResult>(Predicate<TResult> passCondition, Func<TResult> generator)
        {
            TResult candidate;

            do
            {
                candidate = generator();
            } while (!passCondition(candidate));

            return candidate;
        }

        public static string FormatClampIntegerIfTooClose(this float value, float minDistance = 0.01f)
        {
            var rounded = Mathf.RoundToInt(value);
            if (Mathf.Abs(value - rounded) > minDistance)
            {
                return value.ToString("n2");
            }

            return rounded.ToString();
        }
        
        public static bool IsActiveAndPlaying(this Tween t)
        {
            if (!t.IsActive())
            {
                return false;
            }
            
            return t.IsPlaying();
        }
    }
}
