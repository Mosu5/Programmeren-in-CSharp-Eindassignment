using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TomatoLib.ExtensionMethods
{
    public static class ListExtensions
    {
        #region Adding

        public static void Add<T>(this IList<T> originalList, params T[] entriesToAdd)
        {
            AddAll(originalList, entriesToAdd);
        }

        public static void AddAll<T>(this IList<T> originalList, params IList<T>[] listsToAdd)
        {
            foreach (var list in listsToAdd)
            {
                originalList.AddAll(list);
            }
        }

        private static void AddAll<T>(this IList<T> originalList, IList<T> listToAdd)
        {
            foreach (var x1 in listToAdd)
            {
                originalList.Add(x1);
            }
        }

        public static IList<T> AddAllWith<T>(this IList<T> originalList, params IList<T>[] listToAdd)
        {
            originalList.AddAll(listToAdd);
            return originalList;
        }

        #endregion


        #region DuplicateDetection

        public static bool ContainsDuplicateEntry<T>(this IList<T> originalList)
        {
            return originalList.Any(x1 => ContainsDuplicateEntry(originalList, x1));
        }

        private static bool ContainsDuplicateEntry<T>(this IList<T> originalList, T entry)
        {
            return Enumerable.Contains(originalList, entry);
        }

        public static bool ContainsDuplicateEntry<T>(this IList<T> originalList, out int amountOfDuplicateEntries)
        {
            return ContainsDuplicateEntry(originalList, out amountOfDuplicateEntries, out IList<T> outList);
        }

        public static bool ContainsDuplicateEntry<T>(this IList<T> originalList, out int amountOfDuplicateEntries,
            out IList<T> duplicates)
        {
            IList<T> duplicateEntries = new List<T>();

            int currentAmountOfDuplicateEntries = 0;
            foreach (var x1 in originalList)
            {
                if (!duplicateEntries.Contains(x1))
                {
                    currentAmountOfDuplicateEntries += AmountOfDuplicates<T>(originalList, x1);
                    duplicateEntries.Add(x1);
                }
            }

            amountOfDuplicateEntries = currentAmountOfDuplicateEntries;
            duplicates = duplicateEntries;
            return currentAmountOfDuplicateEntries > 0;
        }

        private static int AmountOfDuplicates<T>(this IList<T> originalList, T entry)
        {
            //Matching once means we have 0 duplicates, hence we subtract 1.
            return -1 + originalList.Count(x1 => x1.Equals(entry));
        }

        #endregion


        #region Moving

        public static bool MoveDown<T>(this IList<T> originalList, T elementToMoveDown)
        {
            return MoveDown(originalList, originalList.IndexOf(elementToMoveDown));
        }

        public static bool MoveDown<T>(this IList<T> originalList, int indexSource)
        {
            return Swap(originalList, indexSource, indexSource - 1);
        }

        public static bool MoveUp<T>(this IList<T> originalList, T elementToMoveUp)
        {
            return MoveUp(originalList, originalList.IndexOf(elementToMoveUp));
        }

        public static bool MoveUp<T>(this IList<T> originalList, int indexSource)
        {
            return Swap(originalList, indexSource, indexSource + 1);
        }

        private static bool Swap<T>(this IList<T> originalList, int indexSource, int indexTarget)
        {
            try
            {
                (originalList[indexSource], originalList[indexTarget]) = (originalList[indexTarget], originalList[indexSource]);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        #endregion

        public static string ToElementsString<T>(this IList<T> originalList)
        {
            if (originalList.Count == 0) return "";
            if (originalList.Count == 1) return originalList[0].ToString();
            var stringBuilder = new StringBuilder();
            foreach (var x1 in originalList)
            {
                stringBuilder.Append($"{x1}, ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }
    }
}