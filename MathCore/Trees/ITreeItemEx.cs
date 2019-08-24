using System;
using System.Collections.Generic;

namespace MathCore.Trees
{
    /// <summary>������-���������� ���������� �������� ����������� ������</summary>
    public static class ITreeItemEx
    {
        [Serializable]
        public enum OrderWalkType : byte
        {
            CurrentChildNext,
            CurrentNextChild,
            ChildCurrentNext,
            NextCurrentChild,
            ChildNextCurrent,
            NextChildCurrent
        }

        public class TreeLevelItem<T> where T : class, ITreeItem<T>
        {
            public T Item { get; }
            public int Level { get; }
            public TreeLevelItem(T Item, int Level)
            {
                this.Item = Item;
                this.Level = Level;
            }
        }

        private static void DebugWrite(string str, params object[] obj) { Console.Title = string.Format(str, obj); }

        /// <summary>����������� ����� ������</summary>
        /// <typeparam name="T">��� ��������, ����������� ������� � ������������� ��������� �������� ������</typeparam>
        /// <param name="Item">������ � ����������� �������� ������</param>
        /// <returns>�������� ������ ������ ��������</returns>
        public static T GetRootItem<T>(this T Item) where T : class, ITreeItem<T>
        {
            while(Item.Parent != null) Item = Item.Parent;
            return Item;
        }

        /// <summary>����� ��������� ��������� ������� � �������� � �������: �������, ��������, ��������� �� ������</summary>
        /// <typeparam name="T">��� ��������, ����������� ������� � ������������� ��������� �������� ������</typeparam>
        /// <param name="Item">������ � ����������� �������� ������</param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static IEnumerable<TreeLevelItem<T>> OrderWalk<T>
                    (
                        this T Item,
                        OrderWalkType WalkType = OrderWalkType.ChildCurrentNext,
                        int level = 0
                    )
                    where T : class, ITreeItem<T>
        {
            var stack = new Stack<TreeLevelItem<T>>();
            stack.Push(new TreeLevelItem<T>(Item, level));
            Action<T, int> Push = (t, l) =>
                                      {
                                          if(t == null) return;
                                          stack.Push(new TreeLevelItem<T>(t, l));
                                      };

            switch(WalkType)
            {
                case OrderWalkType.CurrentChildNext:
                    do
                    {
                        var s = stack.Pop();
                        var l = s.Level;
                        yield return s;
                        Push(s.Item.Next, l);
                        Push(s.Item.Child, l + 1);
                    } while(stack.Count > 0);
                    break;
                case OrderWalkType.CurrentNextChild:
                    do
                    {
                        var s = stack.Pop();
                        var l = s.Level;
                        yield return s;
                        Push(s.Item.Child, l + 1);
                        Push(s.Item.Next, l);
                    } while(stack.Count > 0);
                    break;
                case OrderWalkType.ChildCurrentNext:
                    throw new NotImplementedException();
                //break;
                case OrderWalkType.NextCurrentChild:
                    throw new NotImplementedException();
                //break;
                case OrderWalkType.ChildNextCurrent:
                    throw new NotImplementedException();
                //break;
                case OrderWalkType.NextChildCurrent:
                    throw new NotImplementedException();
                //break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(WalkType));
            }
        }

        /// <summary>�������� ��� ������������ ��������</summary>
        /// <typeparam name="T">��� ��������, ����������� ������� � ������������� ��������� �������� ������</typeparam>
        /// <param name="Item">������ � ����������� �������� ������</param>
        /// <returns></returns>
        public static T[] GetParents<T>(this T Item) where T : class, ITreeItem<T>
        {
            var stack = new Stack<T>();

            while(Item != null)
            {
                var parent = Item.Parent;
                if(parent != null)
                    stack.Push(parent);
                Item = parent;
            }

            return stack.ToArray();
        }

        /// <summary>�������� ��� �������� ��������� ������ ���������</summary>
        /// <typeparam name="T">��� ��������, ����������� ������� � ������������� ��������� �������� ������</typeparam>
        /// <param name="Item">������ � ����������� �������� ������</param>
        /// <returns></returns>
        public static T[] GetLevelItems<T>(this T Item) where T : class, ITreeItem<T>
        {
            var items = new List<T>();
            if(Item.Parent != null)
            {
                Item = Item.Parent.Child;
                do
                {
                    items.Add(Item);
                    Item = Item.Next;
                } while(Item != null);
            }
            else
            {
                var item = Item.Prev;
                while(item != null)
                {
                    items.Add(item);
                    item = item.Prev;
                }
                if(items.Count != 0)
                    items.Reverse();
                item = Item;
                do
                {
                    items.Add(item);
                    item = item.Next;
                } while(item != null);
            }
            return items.ToArray();
        }

        /// <summary>�������� ��� �������� �������� ���������</summary>
        /// <typeparam name="T">��� ��������, ����������� ������� � ������������� ��������� �������� ������</typeparam>
        /// <param name="Item">������ � ����������� �������� ������</param>
        /// <returns></returns>
        public static T[] GetChilds<T>(this T Item) where T : class, ITreeItem<T>
        {
            Item = Item.Child;
            var items = new List<T>();
            do
            {
                items.Add(Item);
                Item = Item.Next;
            } while(Item != null);
            return items.ToArray();
        }

        //public static int ChildsCount<T>(this T Item) where T : class, ITreeItem<T>
        //{

        //}
    }
}