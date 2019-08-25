using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MathCore.Graphs
{
    public class TreeListNode<TValue> : IList<TreeListNode<TValue>>, IList<TValue>
    {
        private TreeListNode<TValue> _Prev;
        private TreeListNode<TValue> _Next;

        private TreeListNode<TValue> _Child;

        public TreeListNode<TValue> Prev
        {
            get => _Prev;
            set
            {
                var last = _Prev;
                if (last is { })
                {
                    if (ReferenceEquals(last.Next, this)) last._Next = null;
                    else if (ReferenceEquals(last.Child, this)) last._Child = null;
                }
                _Prev = value;
            }
        }

        public TreeListNode<TValue> Next
        {
            get => _Next;
            set
            {
                var last = _Next;
                if (last is { }) last.Prev = null;
                _Next = value;
                value.Prev = this;
            }
        }

        public TreeListNode<TValue> Child
        {
            get => _Child;
            set
            {
                var last = _Child;
                if (last is { }) last.Prev = null;
                _Child = value;
                value.Prev = this;
            }
        }

        public TValue Value { get; set; }

        public int Length => this[n => n.Next].Count();

        /// <summary>
        /// ���������� ������ ��������� �������� ��������� <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// ������ <paramref name="item"/> ���� �� ������ � ������; � ��������� ������ ��� �������� ����� -1.
        /// </returns>
        /// <param name="item">������, ������� ��������� ����� � <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(TreeListNode<TValue> item)
        {
            var index = 0;
            bool find;
            for (var node = this; !(find = ReferenceEquals(node, item)) && node != null; node = node.Next)
                index++;
            return find ? index : -1;
        }

        /// <summary>���������� ������ ��������� �������� ��������� <see cref="T:System.Collections.Generic.IList`1"/>.</summary>
        /// <returns>������ <paramref name="item"/> ���� �� ������ � ������; � ��������� ������ ��� �������� ����� -1.</returns>
        /// <param name="item">������, ������� ��������� ����� � <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(TValue item)
        {
            var index = 0;
            var find = false;
            if (item is { })
                for (var node = this; node != null && !(find = Equals(item, node.Value)); node = node.Next)
                    index++;
            else
                for (var node = this; node != null && !(find = node.Value == null); node = node.Next)
                    index++;
            return find ? index : -1;
        }

        /// <summary>��������� ������� � ������ <see cref="T:System.Collections.Generic.IList`1"/> �� ���������� �������.</summary>
        /// <param name="index">������ (� ����), �� �������� ������� �������� �������� <paramref name="item"/>.</param><param name="item">������, ����������� � <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException">�������� ��������� <paramref name="index"/> �� �������� ���������� �������� � <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">������ <see cref="T:System.Collections.Generic.IList`1"/> �������� ������ ��� ������.</exception>
        public void Insert(int index, TreeListNode<TValue> item)
        {
            if (index == 0 || item == null) return;
            var prev = this[index - 1] ?? Last;
            var next = prev.Next;
            prev.Next = item;
            item.Next = next;
        }

        /// <summary>��������� ������� � ������ <see cref="T:System.Collections.Generic.IList`1"/> �� ���������� �������.</summary>
        /// <param name="index">������ (� ����), �� �������� ������� �������� �������� <paramref name="item"/>.</param><param name="item">������, ����������� � <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException">�������� ��������� <paramref name="index"/> �� �������� ���������� �������� � <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">������ <see cref="T:System.Collections.Generic.IList`1"/> �������� ������ ��� ������.</exception>
        public void Insert(int index, TValue item) => Insert(index, new TreeListNode<TValue>(item));

        /// <summary>������� ������� <see cref="T:System.Collections.Generic.IList`1"/> �� ���������� �������.</summary>
        /// <param name="index">������ (� ����) ���������� ��������.</param><exception cref="T:System.ArgumentOutOfRangeException">�������� ��������� <paramref name="index"/> �� �������� ���������� �������� � <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">������ <see cref="T:System.Collections.Generic.IList`1"/> �������� ������ ��� ������.</exception>
        public void RemoveAt(int index)
        {
            //Contract.Requires(index >= 0);
            if (index == 0) return;
            var node = this[index];
            var prev = node.Prev;
            var next = node.Next;
            prev.Next = next;
        }

        void IList<TreeListNode<TValue>>.RemoveAt(int index) => RemoveAt(index);

        public TreeListNode<TValue> this[int i]
        {
            get
            {
                //Contract.Requires(i >= 0);
                var node = this[n => n.Next].FirstOrDefault(n => i-- == 0);
                if (node == null) throw new IndexOutOfRangeException();
                return node;
            }
            set
            {
                i--;
                var node = this[n => n.Next].FirstOrDefault(n => i-- == 0);
                if (node == null) throw new IndexOutOfRangeException();
                var next = node.Next;
                node.Next = value;
                value.Next = next;
            }
        }

        /// <summary>�������� ��� ������ ������� �� ���������� �������.</summary>
        /// <returns>������� � ��������� ��������.</returns>
        /// <param name="index">������ (� ����) ��������, ������� ���������� �������� ��� ������.</param><exception cref="T:System.ArgumentOutOfRangeException">�������� ��������� <paramref name="index"/> �� �������� ���������� �������� � <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">�������� ������, � ������ <see cref="T:System.Collections.Generic.IList`1"/> �������� ������ ��� ������.</exception>
        TValue IList<TValue>.this[int index]
        {
            get => this[index].Value;
            set => this[index].Value = value;
        }

        public TreeListNode<TValue> this[params int[] index]
        {
            get
            {
                if (index == null || index.Length == 1 && index[0] == 0) return this;
                var result = this;
                for (var i = 0; result != null && i < index.Length; i++)
                {
                    result = result[index[i]];
                    if (i < index.Length - 1)
                        result = result?.Child;
                }
                return result;
            }
        }

        public bool IsFirst => _Prev == null || ReferenceEquals(_Prev.Child, this);
        public bool IsLast => _Next is null;
        public bool IsRoot => _Prev is null;
        public bool IsChild => !IsRoot && ReferenceEquals(_Prev.Child, this);

        public TreeListNode<TValue> Root => this[n => n.Prev].First(n => n.IsRoot);
        public TreeListNode<TValue> First => this[n => n.Prev].First(n => n.IsFirst);
        public TreeListNode<TValue> Last => this[n => n.Next].First(n => n.IsLast);

        public delegate TreeListNode<TValue> NodeSelector(TreeListNode<TValue> Prev, TreeListNode<TValue> Next, TreeListNode<TValue> Chield);

        public IEnumerable<TreeListNode<TValue>> this[NodeSelector Selector]
        {
            get
            {
                for (var node = this; node != null; node = Selector(_Prev, _Next, _Child))
                    yield return node;
            }
        }

        public IEnumerable<TreeListNode<TValue>> this[Func<TreeListNode<TValue>, TreeListNode<TValue>> Selector]
        {
            get
            {
                for (var node = this; node != null; node = Selector(node))
                    yield return node;
            }
        }

        public TreeListNode() { }
        public TreeListNode(TValue Value) => this.Value = Value;

        public TreeListNode(IEnumerable<TValue> Collection)
        {
            var first = true;
            var node = this;
            foreach (var item in Collection)
                if (first)
                {
                    Value = item;
                    first = false;
                }
                else
                    node = node.Add(item);
        }

        public TreeListNode<TValue> Add(TValue Value)
        {
            var node = new TreeListNode<TValue>(Value);
            Add(node);
            return node;
        }

        public void Add(TreeListNode<TValue> Node)
        {
            var node = this;
            var next = node.Next;
            while (next != null)
            {
                node = next;
                next = node.Next;
            }
            node.Next = Node;
        }

        /// <summary>��������� ������� � ��������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <param name="item">������, ����������� � ��������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">��������� <see cref="T:System.Collections.Generic.ICollection`1"/> �������� ������ ��� ������.</exception>
        void ICollection<TValue>.Add(TValue item) => Add(new TreeListNode<TValue>(item));

        /// <summary>������� ��� �������� �� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <exception cref="T:System.NotSupportedException">��������� <see cref="T:System.Collections.Generic.ICollection`1"/> �������� ������ ��� ������.</exception>
        public void Clear() => Next = null;

        public void ClearChild() => Child = null;

        public void ClearFull() { Clear(); ClearChild(); }

        /// <summary>����������, �������� �� ��������� <see cref="T:System.Collections.Generic.ICollection`1"/> ��������� ��������.</summary>
        /// <returns>�������� true, ���� ������ <paramref name="item"/> ������ � <see cref="T:System.Collections.Generic.ICollection`1"/>; � ��������� ������ � �������� false.</returns>
        /// <param name="item">������, ������� ��������� ����� � <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(TValue item) => item == null
            ? ((IEnumerable<TreeListNode<TValue>>)this).Any(n => n is null)
            : ((IEnumerable<TreeListNode<TValue>>)this).Any(n => item.Equals(n.Value));

        /// <summary>
        /// �������� �������� <see cref="T:System.Collections.Generic.ICollection`1"/> � ������ <see cref="T:System.Array"/>, ������� � ���������� ������� <see cref="T:System.Array"/>.
        /// </summary>
        /// <param name="array">���������� ������ <see cref="T:System.Array"/>, � ������� ���������� �������� �� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>. ���������� � ������� <see cref="T:System.Array"/> ������ ���������� � ����.</param><param name="index">�������� ������� (� ����) � ������� <paramref name="array"/>, � �������� ���������� �����������.</param><exception cref="T:System.ArgumentNullException">�������� <paramref name="array"/> ����� �������� null.</exception><exception cref="T:System.ArgumentOutOfRangeException">�������� ��������� <paramref name="index"/> ������ 0.</exception><exception cref="T:System.ArgumentException">������ <paramref name="array"/> �������� �����������.-���-
        ///                 �������� ������� ������� <paramref name="index"/> ������ ��� ����� ����� ������� <paramref name="array"/>.-���-���������� ��������� � �������� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/> ��������� ������ ���������� �����, ������� � ������� <paramref name="index"/> � �� ����� ������� ���������� <paramref name="array"/>.-���-��� <paramref name="TValue"/> �� ����� ���� ������������� �������� � ���� ������� ���������� <paramref name="array"/>.</exception>
        public void CopyTo(TValue[] array, int index)
        {
            var node = this;
            for (var i = index; i < array.Length && node != null; i++, node = node.Next)
                array[i] = node.Value;
        }

        /// <summary>������� ������ ��������� ���������� ������� �� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <returns>
        /// �������� true, ���� ������ <paramref name="item"/> ������� ������ �� <see cref="T:System.Collections.Generic.ICollection`1"/>, � ��������� ������ � �������� false. ���� ����� ����� ���������� �������� false, ���� �������� <paramref name="item"/> �� ������ � �������� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">������, ������� ���������� ������� �� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">��������� <see cref="T:System.Collections.Generic.ICollection`1"/> �������� ������ ��� ������.</exception>
        public bool Remove(TValue item)
        {
            if (ReferenceEquals(item, Value)) return false;
            var node = item == null
                ? ((IEnumerable<TreeListNode<TValue>>)this).FirstOrDefault(n => n.Value == null)
                : ((IEnumerable<TreeListNode<TValue>>)this).FirstOrDefault(n => item.Equals(n.Value));
            if (node == null) return false;
            if (node.IsChild) node.Prev.Child = node.Next; else node.Prev.Next = node.Next;
            return true;
        }

        /// <summary>�������� ����� ���������, ������������ � ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <returns>����� ���������, ������������ � ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        int ICollection<TValue>.Count => Length;

        /// <summary>����������, �������� �� ��������� <see cref="T:System.Collections.Generic.ICollection`1"/> ��������� ��������.</summary>
        /// <returns>
        /// �������� true, ���� ������ <paramref name="item"/> ������ � <see cref="T:System.Collections.Generic.ICollection`1"/>; � ��������� ������ � �������� false.
        /// </returns>
        /// <param name="item">������, ������� ��������� ����� � <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(TreeListNode<TValue> item) => ((IEnumerable<TreeListNode<TValue>>)this).Any(n => ReferenceEquals(n, item));

        /// <summary>
        /// �������� �������� <see cref="T:System.Collections.Generic.ICollection`1"/> � ������ <see cref="T:System.Array"/>, ������� � ���������� ������� <see cref="T:System.Array"/>.
        /// </summary>
        /// <param name="array">���������� ������ <see cref="T:System.Array"/>, � ������� ���������� �������� �� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>. ���������� � ������� <see cref="T:System.Array"/> ������ ���������� � ����.</param><param name="index">�������� ������� (� ����) � ������� <paramref name="array"/>, � �������� ���������� �����������.</param><exception cref="T:System.ArgumentNullException">�������� <paramref name="array"/> ����� �������� null.</exception><exception cref="T:System.ArgumentOutOfRangeException">�������� ��������� <paramref name="index"/> ������ 0.</exception><exception cref="T:System.ArgumentException">������ <paramref name="array"/> �������� �����������.-���-
        ///                 �������� ������� ������� <paramref name="index"/> ������ ��� ����� ����� ������� <paramref name="array"/>.-���-���������� ��������� � �������� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/> ��������� ������ ���������� �����, ������� � ������� <paramref name="index"/> � �� ����� ������� ���������� <paramref name="array"/>.-���-��� <paramref name="T"/> �� ����� ���� ������������� �������� � ���� ������� ���������� <paramref name="array"/>.</exception>
        public void CopyTo(TreeListNode<TValue>[] array, int index)
        {
            var node = this;
            for (var i = index; i < array.Length && node != null; i++, node = node.Next)
                array[i] = node;
        }

        /// <summary>������� ������ ��������� ���������� ������� �� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <returns>
        /// �������� true, ���� ������ <paramref name="item"/> ������� ������ �� <see cref="T:System.Collections.Generic.ICollection`1"/>, � ��������� ������ � �������� false. ���� ����� ����� ���������� �������� false, ���� �������� <paramref name="item"/> �� ������ � �������� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">������, ������� ���������� ������� �� ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">��������� <see cref="T:System.Collections.Generic.ICollection`1"/> �������� ������ ��� ������.</exception>
        public bool Remove(TreeListNode<TValue> item)
        {
            if (ReferenceEquals(item, this)) return false;
            var node = ((IEnumerable<TreeListNode<TValue>>)this).FirstOrDefault(n => ReferenceEquals(n, item));
            if (node == null) return false;
            if (node.IsChild) node.Prev.Child = node.Next; else node.Prev.Next = node.Next; return true;
        }

        /// <summary>�������� ����� ���������, ������������ � ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <returns>����� ���������, ������������ � ���������� <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        int ICollection<TreeListNode<TValue>>.Count => Length;

        /// <summary>�������� ��������, �����������, �������� �� ��������� <see cref="T:System.Collections.Generic.ICollection`1"/> ������ ��� ������.</summary>
        /// <returns>�������� true, ���� ��������� <see cref="T:System.Collections.Generic.ICollection`1"/> �������� ������ ��� ������, � ��������� ������ � �������� false.</returns>
        public bool IsReadOnly => false;

        public void Add(IEnumerable<TValue> collection) => collection.Aggregate(this, (current, value) => current.Add(value));

        public void AddChild(TreeListNode<TValue> Node) { if (Child == null) Child = Node; else Child.Add(Node); }
        public TreeListNode<TValue> AddChild(TValue value)
        {
            var node = new TreeListNode<TValue>(value);
            AddChild(node);
            return node;
        }

        public void AddChild(IEnumerable<TValue> collection) => collection.Aggregate<TValue, TreeListNode<TValue>>(null, (current, item) => current == null ? AddChild(item) : current.Add(item));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<TreeListNode<TValue>> GetEnumerator()
        {
            for (var node = this; node != null; node = node.Next)
                yield return node;
        }

        /// <summary>���������� �������������, ����������� ������� ��������� � ���������</summary>
        /// <returns>��������� <see cref="T:System.Collections.Generic.IEnumerator`1"/>, ������� ����� �������������� ��� �������� ��������� ���������.</returns>
        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => ((IEnumerable<TreeListNode<TValue>>)this).Select(n => n.Value).GetEnumerator();

        public override string ToString() => $"{Value}{(_Child is null ? "" : $"{{{_Child}}}")}{(_Next is null ? "" : $",{_Next}")}";

        public static implicit operator TValue(TreeListNode<TValue> Node) => Node.Value;
        public static implicit operator TreeListNode<TValue>(TValue Value) => new TreeListNode<TValue>(Value);
    }
}