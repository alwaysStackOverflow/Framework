using System;
using System.Buffers;
using System.IO;

namespace GameFramework.Network
{
	public class MemoryBuffer : MemoryStream, IBufferWriter<byte>
	{
		private readonly int _offset = 0;

		public MemoryBuffer() { }

		public MemoryBuffer(int capacity) : base(capacity) { }

		public MemoryBuffer(byte[] buffer) : base(buffer) { }

		public MemoryBuffer(byte[] buffer, int offset, int count) : base(buffer, offset, count) 
		{
			_offset = offset;
		}

		public ReadOnlyMemory<byte> WrittenMemory => GetBuffer().AsMemory(_offset, (int)Position);

		public ReadOnlySpan<byte> WrittenSpan => GetBuffer().AsSpan(_offset, (int)Position);

		public void Advance(int count)
		{
			long newLength = Position + count;
			if(newLength > Length)
			{
				SetLength(newLength);
			}
			Position = newLength;
		}

		public Memory<byte> GetMemory(int sizeHint = 0)
		{
			if(Length - Position < sizeHint)
			{
				SetLength(Position + sizeHint);
			}
			return GetBuffer().AsMemory((int)Position + _offset, (int)(Length - Position));
		}

		public Span<byte> GetSpan(int sizeHint = 0)
		{
			if (Length - Position < sizeHint)
			{
				SetLength(Position + sizeHint);
			}
			return GetBuffer().AsSpan((int)Position + _offset, (int)(Length - Position));
		}
	}
}
