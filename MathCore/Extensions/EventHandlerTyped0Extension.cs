using System.ComponentModel;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>����� ������� ���������� ��� ������������ �������</summary>
    public static class EventHandlerTyped1Extension
    {
        /// <summary>������-���������� ��������� �������</summary>
        /// <param name="Handler">���������� �������</param>
        /// <param name="Sender">�������� �������</param>
        /// <param name="e">�������� �������</param>
        //[DST]
        public static void Start<TS, TE>(this EventHandler<TS, TE> Handler, TS Sender, EventArgs<TE> e)
        {
            var handler = Handler;
            if(handler is null) return;
            var invocations = handler.GetInvocationList();
            foreach (var invocation in invocations)
            {
                if(invocation.Target is ISynchronizeInvoke invoke && invoke.InvokeRequired)
                    invoke.Invoke(invocation, new object[] { Sender, e });
                else
                    invocation.DynamicInvoke(Sender, e);
            }
        }

        /// <summary>������-���������� ����������� ��������� �������</summary>
        /// <param name="Handler">���������� �������</param>
        /// <param name="Sender">�������� �������</param>
        /// <param name="e">�������� �������</param>
        /// <param name="CallBack">����� ���������� ��������� �������</param>
        /// <param name="State">������-���������, ������������ � ����� ���������� ��������� �������</param>
        [DST]
        public static void StartAsync<TS, TE>(this EventHandler<TS, TE> Handler, TS Sender, EventArgs<TE> e, AsyncCallback CallBack = null, object State = null) => Handler?.BeginInvoke(Sender, e, CallBack, State);

        /// <summary>������� ��������� �������</summary>
        /// <param name="Handler">���������� �������</param>
        /// <param name="Sender">�������� �������</param>
        [DST]
        public static void FastStart<TSender, TEventArgs>(this EventHandler<TSender, TEventArgs> Handler, TSender Sender) => Handler?.Invoke(Sender, default);

        /// <summary>������� ��������� �������</summary>
        /// <param name="Handler">���������� �������</param>
        /// <param name="Sender">�������� �������</param>
        /// <param name="e">��������� �������</param>
        [DST]
        public static void FastStart<TSender, TEventArgs>(this EventHandler<TSender, TEventArgs> Handler, TSender Sender, EventArgs<TEventArgs> e) => Handler?.Invoke(Sender, e);

        ///// <summary>������� ��������� �������</summary>
        ///// <param name="Handler">���������� �������</param>
        ///// <param name="Sender">�������� �������</param>
        ///// <typeparam name="TEventArgs">��� ��������� �������</typeparam>
        ///// <param name="e">��������� �������</param>
        //[DST]
        //public static void FastStart<TEventArgs>(this EventHandler<TEventArgs> Handler, object Sender, TEventArgs e)
        //    where TEventArgs : EventArgs =>
        //    Handler?.Invoke(Sender, e);

        ///// <summary>������-���������� ��������� �������</summary>
        ///// <param name="Handler">���������� �������</param>
        ///// <param name="Sender">�������� �������</param>
        ///// <typeparam name="TEventArgs">��� ��������� �������</typeparam>
        ///// <param name="e">��������� �������</param>
        //[DST]
        //public static void Start<TEventArgs>(this EventHandler<TEventArgs> Handler, object Sender, TEventArgs e)
        //    where TEventArgs : EventArgs
        //{
        //    var handler = Handler;
        //    if(handler is null) return;
        //    var invocations = handler.GetInvocationList();
        //    for(var i = 0; i < invocations.Length; i++)
        //    {
        //        var I = invocations[i];
        //        if(I.Target is ISynchronizeInvoke && ((ISynchronizeInvoke)I.Target).InvokeRequired)
        //            ((ISynchronizeInvoke)I.Target).Invoke(I, new[] { Sender, e });
        //        else
        //            I.DynamicInvoke(Sender, e);
        //    }
        //}

        ///// <summary>������-���������� ���������� ��������� �������</summary>
        ///// <param name="Handler">���������� �������</param>
        ///// <param name="Sender">�������� �������</param>
        ///// <typeparam name="TEventArgs">��� ��������� �������</typeparam>
        ///// <param name="e">��������� �������</param>
        ///// <param name="CallBack">����� ���������� ��������� �������</param>
        ///// <param name="State">������-���������, ������������ � ����� ���������� ��������� �������</param>
        //[DST]
        //public static void StartAsync<TEventArgs>(this EventHandler<TEventArgs> Handler,
        //    object Sender, TEventArgs e, AsyncCallback CallBack = null, object @State = null)
        //    where TEventArgs : EventArgs => Handler?.BeginInvoke(Sender, e, CallBack, State); 

        ///// <summary>������-���������� ��������� �������</summary>
        ///// <param name="Handler">���������� �������</param>
        ///// <param name="Sender">�������� �������</param>
        ///// <typeparam name="TArgs">��� ��������� �������</typeparam>
        ///// <param name="Args">��������� �������</param>
        ///// <typeparam name="TResult">��� ���������� ��������� �������</typeparam>
        ///// <typeparam name="TSender">��� ��������� �������</typeparam>
        ///// <returns>������ ����������� ��������� �������</returns>
        //[DST]
        //public static TResult[] Start<TResult, TSender, TArgs>(this EventHandler<TResult, TSender, TArgs> Handler,
        //                                                       TSender Sender, TArgs Args)
        //{
        //    var handler = Handler;
        //    if(handler is null) return new TResult[0];

        //    return handler
        //                .GetInvocationList()
        //                .Select(I => (TResult)(I.Target is ISynchronizeInvoke && ((ISynchronizeInvoke)I.Target).InvokeRequired
        //                                                   ? ((ISynchronizeInvoke)I.Target)
        //                                                                 .Invoke(I, new object[] { Sender, Args })
        //                                                   : I.DynamicInvoke(Sender, Args))).ToArray();
        //}
    }
}