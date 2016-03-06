﻿namespace MoreDotNet.Extentions.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public static class ListExtentions
    {
        public static T BinarySearch<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key)
        where TKey : IComparable<TKey>
        {
            int min = 0;
            int max = list.Count;
            while (min < max)
            {
                int mid = (max + min) / 2;
                T midItem = list[mid];
                TKey midKey = keySelector(midItem);
                int comp = midKey.CompareTo(key);
                if (comp < 0)
                {
                    min = mid + 1;
                }
                else if (comp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    return midItem;
                }
            }

            if (min == max &&
                keySelector(list[min]).CompareTo(key) == 0)
            {
                return list[min];
            }

            throw new InvalidOperationException("Item not found");
        }

        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(this IList<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = prop.PropertyType.GetCoreType();
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Performs an insertion sort on this list.
        /// </summary>
        /// <typeparam name="T">The type of the list supplied.</typeparam>
        /// <param name="list">the list to sort.</param>
        /// <param name="comparison">the method for comparison of two elements.</param>
        /// <returns></returns>
        public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
        {
            for (int i = 2; i < list.Count; i++)
            {
                for (int j = i; j > 1 && comparison(list[j], list[j - 1]) < 0; j--)
                {
                    T tempItem = list[j];
                    list.RemoveAt(j);
                    list.Insert(j - 1, tempItem);
                }
            }
        }
    }
}