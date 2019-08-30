using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DST = System.Diagnostics.DebuggerStepThroughAttribute;

// ReSharper disable once CheckNamespace
namespace System.IO
{
    public static class StreamExtensions
    {
        public static byte[] ComputeSHA256(this Stream stream)
        {
            using(var sha256 = new Security.Cryptography.SHA256Managed())
                return sha256.ComputeHash(stream);
        }

        public static byte[] ComputeMD5(this Stream stream)
        {
            using(var md5 = new Security.Cryptography.MD5CryptoServiceProvider())
                return md5.ComputeHash(stream);
        }

        /// <summary>
        /// ������� �������������� ����� ������
        /// </summary>
        /// <param name="DataStream">�������� ����� ������</param>
        /// <param name="BufferSize">������ ������ (�� ��������� 4096 �����)</param>
        /// <returns>�������������� ����� ������</returns>
        [DST]
        public static BufferedStream GetBufferedStream(this Stream DataStream, int BufferSize = 4096) => new BufferedStream(DataStream, BufferSize);

        [DST]
        public static StreamWrapper GetWrapper(this Stream BaseStream) => new StreamWrapper(BaseStream);

        [DST]
        public static T ReadStructure<T>(this Stream stream)
        {
            var size = Marshal.SizeOf(typeof(T));
            var data = new byte[size];
            stream.Read(data, 0, size);
            var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                var ptr = gch.AddrOfPinnedObject();
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            } finally
            {
                gch.Free();
            }
        }

        public static void WriteStructure<T>(this Stream stream, T value) where T : struct
        {
            var size = Marshal.SizeOf(value);
            var buffer = new byte[size]; // ������� ������
            var g_lock = GCHandle.Alloc(buffer, GCHandleType.Pinned); // ������������� � ������
            try
            {
                var p = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); // � ����� ��� �����
                Marshal.StructureToPtr(value, p, true); // ���������� � ������
            } finally
            {
                g_lock.Free(); // ����� ��������
            }
            stream.Write(buffer, 0, size);
        }

        public static byte[] ToArray(this Stream stream)
        {
            var array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            return array;
        }

        public static async Task<byte[]> ToArrayAsync(this Stream stream, CancellationToken cancel = default(CancellationToken))
        {
            var array = new byte[stream.Length];
            await stream.ReadAsync(array, 0, array.Length, cancel).ConfigureAwait(false);
            return array;
        }
    }
}