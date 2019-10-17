using System.ComponentModel;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
    /// <summary>����� ������� ���������� ��� ������������ �������</summary>
    public static class EventHandlerTyped3Extension
    {
        /// <summary>������-���������� ��������� �������</summary>
        /// <param name="Handler">���������� �������</param>
        /// <param name="Sender">�������� �������</param>
        /// <param name="e">�������� �������</param>
        [DST]
        public static void Start<TSender, TEventArgs1, TEventArgs2, TEventArgs3>(this EventHandler<TSender, TEventArgs1, TEventArgs2, TEventArgs3> Handler, TSender Sender, EventArgs<TEventArgs1, TEventArgs2, TEventArgs3> e)
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
        public static void StartAsync<TS, TEventArgs1, TEventArgs2, TEventArgs3>(this EventHandler<TS, TEventArgs1, TEventArgs2, TEventArgs3> Handler, TS Sender, EventArgs<TEventArgs1, TEventArgs2, TEventArgs3> e,
                                                    AsyncCallback CallBack = null, object State = null) => Handler?.BeginInvoke(Sender, e, CallBack, State);

        /// <summary>������� ��������� �������</summary>
        /// <param name="Handler">���������� �������</param>
        /// <param name="Sender">�������� �������</param>
        [DST]
        public static void FastStart<TSender, TEventArgs1, TEventArgs2, TEventArgs3>(this EventHandler<TSender, TEventArgs1, TEventArgs2, TEventArgs3> Handler, TSender Sender) => Handler?.Invoke(Sender, default);

        /// <summary>������� ��������� �������</summary>
        /// <param name="Handler">���������� �������</param>
        /// <param name="Sender">�������� �������</param>
        /// <param name="e">��������� �������</param>
        [DST]
        public static void FastStart<TSender, TEventArgs1, TEventArgs2, TEventArgs3>(this EventHandler<TSender, TEventArgs1, TEventArgs2, TEventArgs3> Handler, TSender Sender, EventArgs<TEventArgs1, TEventArgs2, TEventArgs3> e) => Handler?.Invoke(Sender, e);

        ///// <summary>������� ��������� �������</summary>
        ///// <param name="Handler">���������� �������</param>
        ///// <param name="Sender">�������� �������</param>
        ///// <typeparam name="TEventArgs">��� ��������� �������</typeparam>
        ///// <param name="e">��������� �������</param>
        //[DST]
        //public static void FastStart<TEventArgs>(this EventHandler<TEventArgs> Handler, object Sender, TEventArgs e)
        //    where TEventArgs : EventArgs
        //{
        //    var lv_Handler = Handler;
        //    if(lv_Handler != null)
        //        lv_Handler.Invoke(Sender, e);
        //}

        ///// <summary>������-���������� ��������� �������</summary>
        ///// <param name="Handler">���������� �������</param>
        ///// <param name="Sender">�������� �������</param>
        ///// <typeparam name="TEventArgs">��� ��������� �������</typeparam>
        ///// <param name="e">��������� �������</param>
        //[DST]
        //public static void Start<TEventArgs>(this EventHandler<TEventArgs> Handler, object Sender, TEventArgs e)
        //    where TEventArgs : EventArgs
        //{
        //    var lv_Handler = Handler;
        //    if(lv_Handler is null) return;
        //    var invocations = lv_Handler.GetInvocationList();
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
        //    where TEventArgs : EventArgs
        //{
        //    var lv_Handler = Handler;
        //    if(lv_Handler != null)
        //        lv_Handler.BeginInvoke(Sender, e, CallBack, State);
        //}

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
        //    var lv_Handler = Handler;
        //    if(lv_Handler is null) return new TResult[0];

        //    return lv_Handler
        //                .GetInvocationList()
        //                .Select(I => (TResult)(I.Target is ISynchronizeInvoke && ((ISynchronizeInvoke)I.Target).InvokeRequired
        //                                                   ? ((ISynchronizeInvoke)I.Target)
        //                                                                 .Invoke(I, new object[] { Sender, Args })
        //                                                   : I.DynamicInvoke(Sender, Args))).ToArray();
        //}
    }
}