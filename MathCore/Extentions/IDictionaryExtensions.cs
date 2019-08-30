using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using MathCore.Annotations;

// ReSharper disable UnusedMethodReturnValue.Global

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    /// <summary>����� �������-���������� ��� ���������� <see cref="IDictionary{T,V}"/></summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class IDictionaryExtensions
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> item, out TKey key, out TValue value)
        {
            key = item.Key;
            value = item.Value;
        }

        /// <summary>����� ���������� �������� � ������� ������� ��������</summary>
        /// <param name="dictionary">������� ������� <see cref="IList{TValue}"/> �������� ���� <typeparamref name="TValue"/></param>
        /// <param name="key">���� ������� ���� <typeparamref name="TKey"/></param>
        /// <param name="value">�������� ������ ���� <typeparamref name="TValue"/></param>
        /// <typeparam name="TKey">��� ����� ������� <paramref name="key"/></typeparam>
        /// <typeparam name="TValue">��� �������� ������ �������� <paramref name="value"/></typeparam>
        public static void AddValue<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, IList<TValue>> dictionary,
            [NotNull] TKey key,
            TValue value
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(key != null);
            Contract.Requires(value != null);
            dictionary.GetValueOrAddNew(key, () => new List<TValue>()).Add(value);
        }

        /// <summary>����� ���������� �������� � ������� ������� ��������</summary>
        /// <param name="dictionary">������� ������� <see cref="IList{TValue}"/> �������� ���� <typeparamref name="TValue"/></param>
        /// <param name="obj">������-���� ������� ���� <typeparamref name="TObject"/></param>
        /// <param name="KeySelector">����� ����������� ����� ���� <typeparamref name="TKey"/> ������� �� ������� ���� <typeparamref name="TObject"/></param>
        /// <param name="ValueSelector">����� ����������� �������� ���� <typeparamref name="TValue"/> �� ������� ���� <typeparamref name="TObject"/></param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TObject">��� �������� �������</typeparam>
        /// <typeparam name="TValue">��� �������� ������</typeparam>
        public static void AddValue<TKey, TObject, TValue>
        (
            [NotNull] this IDictionary<TKey, IList<TValue>> dictionary,
            TObject obj,
            [NotNull] Func<TObject, TKey> KeySelector,
            [NotNull] Func<TObject, TValue> ValueSelector
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(KeySelector != null);
            Contract.Requires(ValueSelector != null);
            dictionary.AddValue(KeySelector(obj), ValueSelector(obj));
        }

        /// <summary>����� ���������� �������� � ������� ������� ��������</summary>
        /// <param name="dictionary">������� ������� <see cref="IList{TValue}"/> �������� ���� <typeparamref name="TValue"/></param>
        /// <param name="value">��������, ������������ � �������</param>
        /// <param name="KeySelector">����� ���������� ����� �� ���������� ��������</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� ������</typeparam>
        public static void AddValue<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, IList<TValue>> dictionary,
            TValue value,
            [NotNull] Func<TValue, TKey> KeySelector
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(KeySelector != null);
            dictionary.AddValue(KeySelector(value), value);
        }

        /// <summary>���������� �������� � �������</summary>
        /// <param name="dictionary">������� � ������� ���� �������� ��������</param>
        /// <param name="collection">��������� ����������� ��������</param>
        /// <param name="converter">����� ����������� ����� ������� ��� ������� �� ��������� ���������</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        public static void AddValues<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] IEnumerable<TValue> collection,
            [NotNull] Func<TValue, TKey> converter
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(collection != null);
            Contract.Requires(converter != null);
            collection.AddToDictionary(dictionary, converter);
        }

        /// <summary>�������� �������� �� ������� � ������ ��� �������, ��� �������� �����</summary>
        /// <param name="dictionary">��������������� �������</param>
        /// <param name="key">���� ��������, ������� ���� ��������</param>
        /// <param name="creator">����� ��������� ������ ��������, ���������� � ������� ��� ���������� � �� ���������� �����</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <returns>���������� �� ������� �� ���������� ����� ��������, ���� ��������� ����� � ���������� �������� ��������� �������</returns>
        public static TValue GetValueOrAddNew<TKey, TValue>
        (
            [NotNull] this Dictionary<TKey, TValue> dictionary,
            [NotNull] TKey key,
            [NotNull] Func<TValue> creator
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(key != null);
            Contract.Requires(creator != null);
            if(!dictionary.TryGetValue(key, out var value))
                dictionary.Add(key, value = creator());
            return value;
        }

        /// <summary>�������� �������� �� ������� � ������ ��� �������, ��� �������� �����</summary>
        /// <param name="dictionary">��������������� �������</param>
        /// <param name="key">���� ��������, ������� ���� ��������</param>
        /// <param name="creator">����� ��������� ������ �������� �� ���������� �����, ���������� � ������� ��� ���������� � �� ���������� �����</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <returns>���������� �� ������� �� ���������� ����� ��������, ���� ��������� ����� � ���������� �������� ��������� �������</returns>
        public static TValue GetValueOrAddNew<TKey, TValue>
        (
            [NotNull] this Dictionary<TKey, TValue> dictionary,
            [NotNull] TKey key,
            [NotNull] Func<TKey, TValue> creator
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(key != null);
            Contract.Requires(creator != null);
            if(!dictionary.TryGetValue(key, out var value))
                dictionary.Add(key, value = creator(key));
            return value;
        }

        /// <summary>�������� �������� �� ������� � ������ ��� �������, ��� �������� �����</summary>
        /// <param name="dictionary">��������������� �������</param>
        /// <param name="key">���� ��������, ������� ���� ��������</param>
        /// <param name="creator">����� ��������� ������ ��������, ���������� � ������� ��� ���������� � �� ���������� �����</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <returns>���������� �� ������� �� ���������� ����� ��������, ���� ��������� ����� � ���������� �������� ��������� �������</returns>
        public static TValue GetValueOrAddNew<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] TKey key,
            [NotNull] Func<TValue> creator
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(key != null);
            Contract.Requires(creator != null);
            if(!dictionary.TryGetValue(key, out var value))
                dictionary.Add(key, value = creator());
            return value;
        }

        /// <summary>�������� �������� �� ������� � ������ ��� �������, ��� �������� �����</summary>
        /// <param name="dictionary">��������������� �������</param>
        /// <param name="key">���� ��������, ������� ���� ��������</param>
        /// <param name="creator">����� ��������� ������ ��������, ���������� � ������� ��� ���������� � �� ���������� �����</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <returns>���������� �� ������� �� ���������� ����� ��������, ���� ��������� ����� � ���������� �������� ��������� �������</returns>
        public static TValue GetValueOrAddNew<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] TKey key,
            [NotNull] Func<TKey, TValue> creator
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(key != null);
            Contract.Requires(creator != null);
            if(!dictionary.TryGetValue(key, out var value))
                dictionary.Add(key, value = creator(key));
            return value;
        }

        /// <summary>�������� �������� �� ������� � ������ ��� �������, ��� �������� �����</summary>
        /// <param name="dictionary">��������������� �������</param>
        /// <param name="key">����, �������� ��� �������� ��������� ��������</param>
        /// <param name="DefaultValue">�������� ��-���������, ������� ����� ��������� � ������� � ��������� ������, ���� �� �����������</param>
        /// <typeparam name="TKey">��� �����</typeparam>
        /// <typeparam name="TValue">��� ��������</typeparam>
        /// <returns>�������� ������� ��� ���������� �����, ���� ��������� �������� ��-���������</returns>
        public static TValue GetValue<TKey, TValue>
        (
            [NotNull] this Dictionary<TKey, TValue> dictionary,
            [NotNull] TKey key,
            TValue DefaultValue = default
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(key != null);
            return dictionary.TryGetValue(key, out var v) ? v : DefaultValue;
        }

        /// <summary>�������� �������� �� ������� � ������ ��� �������, ��� �������� �����</summary>
        /// <param name="dictionary">��������������� �������</param>
        /// <param name="key">����, �������� ��� �������� ��������� ��������</param>
        /// <param name="DefaultValue">�������� ��-���������, ������� ����� ��������� � ������� � ��������� ������, ���� �� �����������</param>
        /// <typeparam name="TKey">��� �����</typeparam>
        /// <typeparam name="TValue">��� ��������</typeparam>
        /// <returns>�������� ������� ��� ���������� �����, ���� ��������� �������� ��-���������</returns>
        public static TValue GetValue<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] TKey key,
            TValue DefaultValue = default
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(key != null);
            return dictionary.TryGetValue(key, out var value) ? value : DefaultValue;
        }

        /// <summary>�������� �������� �� ������� � ������ ��� �������, ��� �������� �����</summary>
        /// <param name="dictionary">��������������� �������</param>
        /// <param name="name">�������� �������, �������� ��� �������� ��������� ��������</param>
        /// <typeparam name="TValue">��� ��������</typeparam>
        /// <returns>�������� ������� ��� ���������� �����</returns>
        public static TValue GetValue<TValue>([NotNull] this Dictionary<string, object> dictionary, [NotNull] string name)
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(name != null);
            return dictionary.TryGetValue(name, out var value) ? (TValue)value : default;
        }

        /// <summary>������������� ������� ��������� ������� ��� ���������� ����� ��������</summary>
        /// <param name="dictionary">���������������� �������</param>
        /// <param name="count">���������� ����������� ���������</param>
        /// <param name="initializer">����� ��������� ����� ���������</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <returns>������������������ �������</returns>
        [NotNull]
        public static IDictionary<TKey, TValue> Initialize<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            int count,
            [NotNull] Func<KeyValuePair<TKey, TValue>> initializer
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(count >= 0);
            Contract.Requires(initializer != null);
            Contract.Ensures(ReferenceEquals(Contract.Result<IDictionary<TKey, TValue>>(), dictionary));

            for(var i = 0; i < count; i++)
                dictionary.Add(initializer());

            return dictionary;
        }

        /// <summary>������������� ������� ��������� ������� ��� ���������� ����� ��������</summary>
        /// <param name="dictionary">���������������� �������</param>
        /// <param name="count">���������� ����������� ���������</param>
        /// <param name="parameter">�������� �������������</param>
        /// <param name="initializer">����� ��������� ����� ���������</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <typeparam name="TParameter">��� ���������</typeparam>
        /// <returns>������������������ �������</returns>
        [NotNull]
        public static IDictionary<TKey, TValue> Initialize<TKey, TValue, TParameter>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            int count,
            [CanBeNull] TParameter parameter,
            [NotNull] Func<TParameter, KeyValuePair<TKey, TValue>> initializer
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(count >= 0);
            Contract.Requires(initializer != null);
            Contract.Ensures(ReferenceEquals(Contract.Result<IDictionary<TKey, TValue>>(), dictionary));

            for(var i = 0; i < count; i++)
                dictionary.Add(initializer(parameter));

            return dictionary;
        }

        /// <summary>������������� ������� ��������� ������� ��� ���������� ����� ��������</summary>
        /// <param name="dictionary">���������������� �������</param>
        /// <param name="count">���������� ����������� ���������</param>
        /// <param name="initializer">����� ��������� ����� ���������</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <returns>������������������ �������</returns>
        [NotNull]
        public static IDictionary<TKey, TValue> Initialize<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            int count,
            [NotNull] Func<int, KeyValuePair<TKey, TValue>> initializer
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(count >= 0);
            Contract.Requires(initializer != null);
            Contract.Ensures(ReferenceEquals(Contract.Result<IDictionary<TKey, TValue>>(), dictionary));

            for(var i = 0; i < count; i++)
                dictionary.Add(initializer(i));

            return dictionary;
        }

        /// <summary>������������� ������� ��������� ������� ��� ���������� ����� ��������</summary>
        /// <param name="dictionary">���������������� �������</param>
        /// <param name="count">���������� ����������� ���������</param>
        /// <param name="parameter">�������� ���������������</param>
        /// <param name="initializer">����� ��������� ����� ���������</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <typeparam name="TParameter">��� ��������� �������������</typeparam>
        /// <returns>������������������ �������</returns>
        [NotNull]
        public static IDictionary<TKey, TValue> Initialize<TKey, TValue, TParameter>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            int count,
            [NotNull] TParameter parameter,
            [NotNull] Func<int, TParameter, KeyValuePair<TKey, TValue>> initializer
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(count >= 0);
            Contract.Requires(initializer != null);
            Contract.Ensures(ReferenceEquals(Contract.Result<IDictionary<TKey, TValue>>(), dictionary));

            for(var i = 0; i < count; i++)
                dictionary.Add(initializer(i, parameter));

            return dictionary;
        }

        /// <summary>������������� ������� ��������� ������� ��� ���������� ����� ��������</summary>
        /// <param name="dictionary">���������������� �������</param>
        /// <param name="keys">��������� ������</param>
        /// <param name="initializer">����� ��������� ����� ���������</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <returns>������������������ �������</returns>
        [NotNull]
        public static IDictionary<TKey, TValue> Initialize<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] IEnumerable<TKey> keys,
            [NotNull] Func<TKey, TValue> initializer
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(keys != null);
            Contract.Requires(Contract.ForAll(keys, k => k != null));
            Contract.Requires(initializer != null);
            Contract.Ensures(ReferenceEquals(Contract.Result<IDictionary<TKey, TValue>>(), dictionary));

            foreach(var key in keys)
                dictionary.Add(key, initializer(key));

            return dictionary;
        }

        /// <summary>������������� ������� ��������� ������� ��� ���������� ����� ��������</summary>
        /// <param name="dictionary">���������������� �������</param>
        /// <param name="keys">��������� ������</param>
        /// <param name="parameter">�������� ���������������</param>
        /// <param name="initializer">����� ��������� ����� ���������</param>
        /// <typeparam name="TKey">��� ����� �������</typeparam>
        /// <typeparam name="TValue">��� �������� �������</typeparam>
        /// <typeparam name="TParameter">��� ��������� �������������</typeparam>
        /// <returns>������������������ �������</returns>
        [NotNull]
        public static IDictionary<TKey, TValue> Initialize<TKey, TValue, TParameter>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] IEnumerable<TKey> keys,
            [CanBeNull] TParameter parameter,
            [NotNull] Func<TKey, TParameter, TValue> initializer
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(keys != null);
            Contract.Requires(Contract.ForAll(keys, k => k != null));
            Contract.Requires(initializer != null);
            Contract.Ensures(ReferenceEquals(Contract.Result<IDictionary<TKey, TValue>>(), dictionary));

            foreach (var key in keys)
                dictionary.Add(key, initializer(key, parameter));

            return dictionary;
        }

        /// <summary>�������� �� ������� ���������, ��������������� ���������</summary>
        /// <param name="dictionary">��������������� �������</param>
        /// <param name="selector">����� ������ ���������</param>
        /// <typeparam name="TKey">��� �����</typeparam>
        /// <typeparam name="TValue">��� ��������</typeparam>
        /// <returns>������ �������� ��� ����-��������</returns>
        [NotNull]
        public static KeyValuePair<TKey, TValue>[] RemoveWhere<TKey, TValue>
        (
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] Func<KeyValuePair<TKey, TValue>, bool> selector
        )
        {
            Contract.Requires(dictionary != null);
            Contract.Requires(selector != null);
            Contract.Ensures(Contract.Result<KeyValuePair<TKey, TValue>[]>() != null);
            Contract.Ensures(Contract.Result<KeyValuePair<TKey, TValue>[]>().Length >= 0);
            var to_remove = dictionary.Where(selector).ToArray();
            foreach(var remove in to_remove)
                dictionary.Remove(remove.Key);
            return to_remove;
        }

        public static TValue[] RemoveItems<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            var result = new List<TValue>(dictionary.Count);
            foreach(var key in keys)
                if(dictionary.TryGetValue(key, out TValue value))
                {
                    result.Add(value);
                    dictionary.Remove(key);
                }
            return result.ToArray();
        }
    }
}