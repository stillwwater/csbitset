using System;
using System.Diagnostics;

namespace Bitset {
    // Represents a fixed size sequence of 32 bits. Bitsets can be
    // manipulated by standard logic operators converted to and
    // from strings and integers.
    public struct Bitset32 : IBitset, IEquatable<Bitset32> {
        public uint w;

        const int Length = 32;

        public Bitset32(uint w) {
            this.w = w;
        }

        public Bitset32(params bool[] bits) {
            Debug.Assert(bits.Length == Length,
                         "Array length does not match bitset length");
            w = 0;
            for (int i = 0; i < bits.Length; ++i) {
                this[i] = bits[i];
            }
        }

        public Bitset32(byte[] bytes) {
            Debug.Assert(bytes.Length == Length,
                         "Array length does not match bitset length");
            w = 0;
            for (int i = 0; i < bytes.Length; ++i) {
                this[i] = bytes[i] > 0;
            }
        }

        public Bitset32(string s) {
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

        public static bool operator==(Bitset32 a, Bitset32 b) {
            return a.w == b.w;
        }

        public static bool operator!=(Bitset32 a, Bitset32 b) {
            return !(a == b);
        }

        public static Bitset32 operator&(Bitset32 a, Bitset32 b) {
            return new Bitset32(a.w & b.w);
        }

        public static Bitset32 operator|(Bitset32 a, Bitset32 b) {
            return new Bitset32(a.w | b.w);
        }

        public static Bitset32 operator^(Bitset32 a, Bitset32 b) {
            return new Bitset32(a.w ^ b.w);
        }

        public static Bitset32 operator~(Bitset32 a) {
            return new Bitset32(~a.w);
        }

        public static explicit operator Bitset8(Bitset32 s32) {
            return new Bitset8((byte)s32.w);
        }

        public static explicit operator Bitset16(Bitset32 s32) {
            return new Bitset16((ushort)s32.w);
        }

        public static explicit operator Bitset64(Bitset32 s32) {
            return new Bitset64((ulong)s32.w);
        }

        public static explicit operator Bitset128(Bitset32 s32) {
            return new Bitset128((ulong)s32.w, 0ul);
        }

        public static explicit operator Bitset256(Bitset32 s32) {
            return new Bitset256((ulong)s32.w, 0ul, 0ul, 0ul);
        }

        // Checks if the bit at a position is set
        public bool Test(int position) {
            BoundsCheck(position);
            uint mask = 1u << position;
            return (w & mask) == mask;
        }

        // Sets bit to 1 at the given position
        public void Set(int position) {
            BoundsCheck(position);
            w |= (1u << position);
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
            w &= ~(1u << position);
        }

        // Flips the bit at the given position
        public void Flip(int position) {
            BoundsCheck(position);
            w ^= (1u << position);
        }

        // Checks if all bits are set to 1
        public bool All() {
            return w == 0xffffffffu;
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

        public bool Equals(Bitset32 s) {
            return this == s;
        }

        public override bool Equals(object obj) {
            return obj is Bitset32 && (Bitset32)obj == this;
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
                         "Index out of bounds: Bitset32[" + position + "]");
        }
    };
}
