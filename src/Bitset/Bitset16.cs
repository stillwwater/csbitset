using System;
using System.Diagnostics;

namespace Bitset {
    // Represents a fixed size sequence of 16 bits. Bitsets can be
    // manipulated by standard logic operators converted to and
    // from strings and integers.
    public struct Bitset16 : IBitset, IEquatable<Bitset16> {
        public ushort w;

        const int Length = 16;

        public Bitset16(ushort w) {
            this.w = w;
        }

        public Bitset16(params bool[] bits) {
            Debug.Assert(bits.Length == Length,
                         "Array length does not match bitset length");
            w = 0;
            for (int i = 0; i < bits.Length; ++i) {
                this[i] = bits[i];
            }
        }

        public Bitset16(byte[] bytes) {
            Debug.Assert(bytes.Length == Length,
                         "Array length does not match bitset length");
            w = 0;
            for (int i = 0; i < bytes.Length; ++i) {
                this[i] = bytes[i] > 0;
            }
        }

        public Bitset16(string s) {
            Debug.Assert(s.Length == Length,
                         "String length does not match bitset length");
            w = 0;
            for (int i = 0; i < s.Length; ++i) {
                char c = s[i];
                Debug.Assert(c == '0' || c == '1');
                this[i] = c == '1';
            }
        }

        // Returns the total number of bits in the bitset
        public int Count => Length;

        public bool this[int position] {
            get => Test(position);
            set => Set(position, value);
        }

        public static bool operator==(Bitset16 a, Bitset16 b) {
            return a.w == b.w;
        }

        public static bool operator!=(Bitset16 a, Bitset16 b) {
            return !(a == b);
        }

        public static Bitset16 operator&(Bitset16 a, Bitset16 b) {
            return new Bitset16((ushort)(a.w & b.w));
        }

        public static Bitset16 operator|(Bitset16 a, Bitset16 b) {
            return new Bitset16((ushort)(a.w | b.w));
        }

        public static Bitset16 operator^(Bitset16 a, Bitset16 b) {
            return new Bitset16((ushort)(a.w ^ b.w));
        }

        public static Bitset16 operator~(Bitset16 a) {
            return new Bitset16((ushort)(~a.w));
        }

        public static explicit operator Bitset8(Bitset16 s16) {
            return new Bitset8((byte)s16.w);
        }

        public static explicit operator Bitset32(Bitset16 s16) {
            return new Bitset32((uint)s16.w);
        }

        public static explicit operator Bitset64(Bitset16 s16) {
            return new Bitset64((ulong)s16.w);
        }

        public static explicit operator Bitset128(Bitset16 s16) {
            return new Bitset128((ulong)s16.w, 0ul);
        }

        public static explicit operator Bitset256(Bitset16 s16) {
            return new Bitset256((ulong)s16.w, 0ul, 0ul, 0ul);
        }

        // Checks if the bit at a position is set
        public bool Test(int position) {
            BoundsCheck(position);
            ushort mask = (ushort)(1 << position);
            return (w & mask) == mask;
        }

        // Sets bit to 1 at the given position
        public void Set(int position) {
            BoundsCheck(position);
            w |= (ushort)(1 << position);
        }

        // Sets bit at a position to a given value
        public void Set(int position, bool value) {
            if (value)
                Set(position);
            else
                Reset(position);
        }

        // Resets bit to 0 at the given position
        public void Reset(int position) {
            BoundsCheck(position);
            w &= (ushort)(~(1 << position));
        }

        // Flips the bit at the given position
        public void Flip(int position) {
            BoundsCheck(position);
            w ^= (ushort)(1 << position);
        }

        // Checks if all bits are set to 1
        public bool All() {
            return w == 0xffff;
        }

        // Checks if any bit is set to 1
        public bool Any() {
            return w != 0u;
        }

        // Checks if none of the bits are set to 1
        public bool None() {
            return w == 0u;
        }

        // Converts bits to an unsigned int
        public uint ToUInt32() {
            return w;
        }

        // Converts bits to an unsigned long
        public ulong ToUInt64() {
            return (ulong)w;
        }

        // Converts each bit to one byte in a byte array
        public byte[] ToByteArray() {
            var bytes = new byte[Length];
            for (int i = 0; i < Length; ++i) {
                bytes[i] = Convert.ToByte(this[i]);
            }
            return bytes;
        }

        public bool Equals(Bitset16 s) {
            return this == s;
        }

        public override bool Equals(object obj) {
            return obj is Bitset16 && (Bitset16)obj == this;
        }

        public override int GetHashCode() {
            return w.GetHashCode();
        }

        public override string ToString() {
            return Convert.ToString(w, 2).PadLeft(Length, '0');
        }

        [Conditional("DEBUG")]
        void BoundsCheck(int position) {
            Debug.Assert(position >= 0 && position < Length,
                         "Index out of bounds: Bitset16[" + position + "]");
        }
    };
}
