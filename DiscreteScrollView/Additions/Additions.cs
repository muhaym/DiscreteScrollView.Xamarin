using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Com.Yarolegovich.Discretescrollview
{
    partial class InfiniteScrollAdapter
    {
        static IntPtr id_isResetRequired;
        static IntPtr id_onBindViewHolder;
        static IntPtr id_setPosition;
        static IntPtr id_wrapped;
        static IntPtr id_layoutManager;
        static IntPtr id_mapPositionToReal;
        static IntPtr id_getCurrentPosition;
        static IntPtr id_onCreateViewHolder;

        private static int CENTER = int.MaxValue / 2;
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (id_isResetRequired == IntPtr.Zero)
                id_isResetRequired = JNIEnv.GetMethodID(class_ref,"isResetRequired", "()B");
            Console.WriteLine("id_isResetRequired : {0}", id_isResetRequired);
            if(JNIEnv.CallBooleanMethod(class_ref,id_isResetRequired))
            {
                if (id_layoutManager == IntPtr.Zero)
                    id_layoutManager = JNIEnv.GetFieldID(class_ref, "layoutManager", "L");
                Console.WriteLine("id_layoutManager : {0}", id_layoutManager);
                if (id_getCurrentPosition == IntPtr.Zero)
                    id_getCurrentPosition = JNIEnv.GetFieldID(id_layoutManager, "getCurrentPosition", "()I");
                Console.WriteLine("id_getCurrentPosition : {0}", id_getCurrentPosition);
                if (id_mapPositionToReal == IntPtr.Zero)
                    id_mapPositionToReal = JNIEnv.GetFieldID(class_ref, "mapPositionToReal", "()I");
                Console.WriteLine("id_mapPositionToReal : {0}", id_mapPositionToReal);
                var currentposition = JNIEnv.CallIntMethod(id_layoutManager, id_getCurrentPosition);
                Console.WriteLine("currentposition : {0}", currentposition);
                int resetPosition = CENTER + JNIEnv.CallIntMethod(class_ref, id_mapPositionToReal, new JValue(currentposition));
                Console.WriteLine("resetPosition : {0}", resetPosition);
                if (id_setPosition == IntPtr.Zero)
                    id_setPosition = JNIEnv.GetFieldID(class_ref, "setPosition", "(I)V");
                Console.WriteLine("id_setPosition : {0}", id_setPosition);
                JNIEnv.CallVoidMethod(class_ref, id_setPosition,new JValue(id_setPosition));
                return;
            }
            if (id_wrapped == IntPtr.Zero)
                id_wrapped = JNIEnv.GetFieldID(class_ref, "wrapped", "L");
            Console.WriteLine("id_wrapped : {0}", id_wrapped);
            if (id_onBindViewHolder == IntPtr.Zero)
                id_onBindViewHolder = JNIEnv.GetFieldID(id_wrapped, "onBindViewHolder", "(L/I)V");
            Console.WriteLine("id_onBindViewHolder : {0}", id_onBindViewHolder);
            if (id_mapPositionToReal == IntPtr.Zero)
                id_mapPositionToReal = JNIEnv.GetFieldID(class_ref, "mapPositionToReal", "()I");
            Console.WriteLine("id_mapPositionToReal : {0}", id_mapPositionToReal);
            var pos = JNIEnv.CallIntMethod(class_ref, id_mapPositionToReal, new JValue(position));
            Console.WriteLine("pos : {0}", pos);
            JNIEnv.CallVoidMethod(id_wrapped, id_onBindViewHolder, new JValue(pos)); 
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (id_wrapped == IntPtr.Zero)
                id_wrapped = JNIEnv.GetFieldID(class_ref, "wrapped", "L");
            Console.WriteLine("id_wrapped : {0}", id_wrapped);
            if (id_onCreateViewHolder == IntPtr.Zero)
                id_onCreateViewHolder = JNIEnv.GetFieldID(id_wrapped, "OnCreateViewHolder", "(L/I)L");
            Console.WriteLine("id_onCreateViewHolder : {0}", id_onCreateViewHolder);

            var param = new JValue[] { new JValue(parent), new JValue(viewType) };
            var ret = JNIEnv.CallObjectMethod(id_wrapped, id_onCreateViewHolder, param);
            Console.WriteLine("ret : {0}", ret);
            return Java.Lang.Object.GetObject<RecyclerView.ViewHolder>(ret, JniHandleOwnership.TransferLocalRef);          
        }
    }
}
