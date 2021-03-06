﻿using MathCore.Annotations;

using DST = System.Diagnostics.DebuggerStepThroughAttribute;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    /// <summary>Методы расширения для интерфейса <see cref="IList{T}"/></summary>
    public static class IListExtensions
    {
        /// <summary>Ссылка на список пуста, либо список не содержит элементов</summary>
        /// <param name="list">Проверяемый список</param>
        /// <returns>Истина, если не задана ссылка на список, либо список пуст</returns>
        public static bool IsNullOrEmpty([CanBeNull] this IList list) => list is null || list.Count == 0;

        ///<summary>Метод расширения для инициализации списка</summary>
        ///<param name="list">Инициализируемый объект</param>
        ///<param name="Count">Требуемое число элементов</param>
        ///<param name="Initializator">Метод инициализации</param>
        ///<param name="ClearBefore">Очищать предварительно (по умолчанию)</param>
        ///<typeparam name="T">Тип элементов списка</typeparam>
        ///<returns>Инициализированный список</returns>
        [DST, CanBeNull]
        public static IList<T> Initialize<T>
        (
            [CanBeNull] this IList<T> list,
            int Count,
            [NotNull] Func<int, T> Initializator,
            bool ClearBefore = true
        )
        {
            switch (list)
            {
                case null: return null;
                case List<T> l:
                {
                    if (ClearBefore) l.Clear();
                    for (var i = 0; i < Count; i++) l.Add(Initializator(i));
                    break;
                }
                default:
                {
                    if (ClearBefore) list.Clear();
                    for (var i = 0; i < Count; i++) list.Add(Initializator(i));
                    break;
                }
            }

            return list;
        }

        ///<summary>Метод расширения для инициализации списка</summary>
        ///<param name="list">Инициализируемый объект</param>
        ///<param name="Count">Требуемое число элементов</param>
        ///<param name="parameter">Параметр инициализации</param>
        ///<param name="Initializator">Метод инициализации</param>
        ///<param name="ClearBefore">Очищать предварительно (по умолчанию)</param>
        ///<typeparam name="T">Тип элементов списка</typeparam>
        ///<typeparam name="TParameter">Тип параметра инициализации</typeparam>
        ///<returns>Инициализированный список</returns>
        [DST, CanBeNull]
        public static IList<T> Initialize<T, TParameter>
        (
            [CanBeNull] this IList<T> list,
            int Count,
            [CanBeNull] in TParameter parameter,
            [NotNull] Func<int, TParameter, T> Initializator,
            bool ClearBefore = true
        )
        {
            if (list is null) return null;
            if (list is List<T> l)
            {
                if (ClearBefore) l.Clear();
                for (var i = 0; i < Count; i++)
                    l.Add(Initializator(i, parameter));
            }
            else
            {
                if (ClearBefore) list.Clear();
                for (var i = 0; i < Count; i++)
                    list.Add(Initializator(i, parameter));
            }
            return list;
        }

        /// <summary>Перемешать список</summary>
        /// <param name="list">Перемешиваемый список</param>
        /// <typeparam name="T">Тип элементов списка</typeparam>
        /// <returns>Перемешанный исходный список</returns>
        [NotNull]
        public static IList<T> Mix<T>([NotNull] this IList<T> list)
        {
            var rnd = new Random();

            if (list is List<T> l)
            {
                var length = l.Count - 1;
                var temp = l[0];
                var index = 0;
                for (var i = 1; i <= length; i++)
                    l[index] = l[index = rnd.Next(length)];
                l[index] = temp;
            }
            else
            {
                var length = list.Count - 1;
                var temp = list[0];
                var index = 0;
                for (var i = 1; i <= length; i++)
                    list[index] = list[index = rnd.Next(length)];
                list[index] = temp;
            }
            return list;
        }
    }
}