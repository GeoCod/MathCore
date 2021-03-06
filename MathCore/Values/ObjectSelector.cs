﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathCore.Annotations;
// ReSharper disable UnusedMember.Global

namespace MathCore.Values
{
    /// <summary>Система псевдопараллельного потокового доступа к значению ряда источников объектов</summary>
    /// <typeparam name="T">Тип объектов в источниках</typeparam>
    /// <example>
    /// Примером использования может быть способ последовательного выбора данных из ряда потоков чтения 
    /// с выбором последовательности данных, выводимых в общий поток.
    /// void Test()
    /// {
    ///    //Используем уничтожаемую группу объектов
    ///   using(var readers = new DisposableGroup&lt;StreamReader&gt;(//Объекты чтения из потока
    ///           Directory.GetFiles(@"c:\", "*.txt").Select(f => new StreamReader(f))))  //для всех файлов C:\*.txt
    ///   {
    ///     var rnd = new Random();                                         //Генератор случайных чисел
    ///     var o_selector = new ObjectSelector&lt;string&gt;(                     //Создаём объект выбора строк
    ///                 SS => rnd.Next(readers.Count),                      //Очередная строка из случайного файла
    ///                 () => readers.All(r => !r.EndOfStream),             //Признак возможности чтения - ни один из потоков не закончен
    ///                 readers.Select(r => (Func&lt;string&gt;)(r.ReadLine)));   //Инициализатор - метод, возвращающий строку из файла
    ///     while(o_selector.CanRead) Console.Write(o_selector.Value);        //Читаем всё построчно в консоль
    ///   }
    /// }
    /// </example>
    public sealed class ObjectSelector<T> : IValueRead<T>
    {
        /* ------------------------------------------------------------------------------------------ */

        /// <summary>Метод выбора одного из значений ряда источников объектов</summary>
        private readonly Func<T[], int> _Selector;
        /// <summary>Массив "ленивых" значений, используемых в качестве генераторов объектов </summary>
        private readonly LazyValue<T>[] _Values;
        /// <summary>Метод, определяющий возможность чтения данных из источников</summary>
        private readonly Func<bool> _CanRead;

        /* ------------------------------------------------------------------------------------------ */

        /// <summary>Очередное значение из вектора генераторов значений</summary>
        public T Value
        {
            get
            {
                //Получить массив значений от всех "ленивых" значений объектов
                var values = new T[_Values.Length].Initialize(_Values, (i, vv) => vv[i].Value);
                //Выбрать индекс интересующего объекта
                var index = _Selector(values);
                //Выбрать объект из массива значений
                var value = values[index];
                //Сбросить состояния выбранного "ленивого" значения
                _Values[index].Reset();
                return value;
            }
        }

        /// <summary>Признак возможности чтения объекта</summary>
        public bool CanRead => _CanRead();

        /* ------------------------------------------------------------------------------------------ */


        /// <summary>Новый генератор последовательности объектов из источника параллельных значений</summary>
        /// <param name="Selector">Метод выбора значения</param>
        /// <param name="CanRead">Метод определения возможности чтения значения</param>
        /// <param name="Generator">Массив генераторов объектов "ленивых" значений</param>
        public ObjectSelector(Func<T[], int> Selector, Func<bool> CanRead, [NotNull] params Func<T>[] Generator)
        {
            //Создать массив "ленивых" значений
            _Values = new LazyValue<T>[Generator.Length].Initialize(Generator, (i, g) => new LazyValue<T>(g[i]));
            _Selector = Selector;
            _CanRead = CanRead;
        }

        /// <summary>Новый генератор последовательности объектов из источника параллельных значений</summary>
        /// <param name="Selector">Метод выбора значения</param>
        /// <param name="CanRead">Метод определения возможности чтения значения</param>
        /// <param name="GeneratorsEnum">Массив генераторов объектов "ленивых" значений</param>
        public ObjectSelector(
            Func<T[], int> Selector,
            Func<bool> CanRead,
            [NotNull] IEnumerable<Func<T>> GeneratorsEnum)
            : this(Selector, CanRead, GeneratorsEnum.ToArray())
        { }

        //private void Test()
        //{
        //    //Используем уничтожаемую группу объектов
        //    using var readers = new DisposableGroup<StreamReader>(//Объекты чтения из потока
        //        Directory.GetFiles(@"c:\", "*.txt").Select(f => new StreamReader(f)));
        //    var rnd = new Random();                                         //Генератор случайных чисел
        //    var o_selector = new ObjectSelector<string>(                     //Создаём объект выбора строк
        //        SS => rnd.Next(readers.Count),                      //Очередная строка из случайного файла
        //        () => readers.All(r => !r.EndOfStream),             //Признак возможности чтения - ни один из потоков не закончен
        //        readers.Select(r => (Func<string>)(r.ReadLine)));   //Инициализатор - метод, возвращающий строку из файла
        //    while (o_selector.CanRead) Console.Write(o_selector.Value);        //Читаем всё построчно в консоль
        //}

        /* ------------------------------------------------------------------------------------------ */
    }
}