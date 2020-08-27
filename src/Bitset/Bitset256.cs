using System;
using System.Diagnostics;

namespace Bitset {
    // Represents a fixed size sequence of 256 bits. Bitsets can be
    // manipulated by standard logic operators converted to and
    // from strings and integers.
    public unsafe struct Bitset256 : IBitset, IEquatable<Bitset256> {
        public fixed ulong w[WordsLength];

        const int Length = 256;
        const int BitsPerWord = 64;
        const int WordShift = 6;
        const int WordsLength = Length / BitsPerWord;

        public Bitset256(ulong w0, ulong w1 = 0ul,
                         ulong w2 = 0ul, ulong w3 = 0ul) {
            w[0] = w0;
            w[1] = w1;
            w[2] = w2;
            w[3] = w3;
        }

        public Bitset256(params bool[] bits) {
            Debug.Assert(bits.Length == Length,
                         "Array length does not match bitset length");
            w[0] = 0ul;
            w[1] = 0ul;
            w[2] = 0ul;
            w[3] = 0ul;
            for (int i = 0; i < bits.Length; ++i) {
                this[i] = bits[i];
            }
        }

        public Bitset256(byte[] bytes) {
            Debug.Assert(bytes.Length == Length,
                         "Array length does not match bitset length");
            w[0] = 0ul;
            w[1] = 0ul;
            w[2] = 0ul;
            w[3] = 0ul;
            for (int i = 0; i < bytes.Length; ++i) {
                this[i] = bytes[i] > 0;
            }
        }

        public Bitset256(string s) {
            Debug.Assert(s.Length == Length,
                         "String length does not match bitset length");
            w[0] = 0ul;
            w[1] = 0ul;
            w[2] = 0ul;
            w[3] = 0ul;
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

        public Bitset128 High {
            get => new Bitset128(w[2], w[3]);
            set {
                w[2] = value.w[0];
                w[3] = value.w[1];
            }
        }

        public Bitset128 Low {
            get => new Bitset128(w[0], w[1]);
            set {
                w[0] = value.w[0];
                w[1] = value.w[1];
            }
        }

        public static bool operator==(Bitset256 a, Bitset256 b) {
            return a.w[0] == b.w[0]
                && a.w[1] == b.w[1]
                && a.w[2] == b.w[2]
                && a.w[3] == b.w[3];
        }

        public static bool operator!=(Bitset256 a, Bitset256 b) {
            return !(a == b);
        }

        public static Bitset256 operator&(Bitset256 a, Bitset256 b) {
            return new Bitset256(a.w[0] & b.w[0],
                                 a.w[1] & b.w[1],
                                 a.w[2] & b.w[2],
                                 a.w[3] & b.w[3]);
        }

        public static Bitset256 operator|(Bitset256 a, Bitset256 b) {
            return new Bitset256(a.w[0] | b.w[0],
                                 a.w[1] | b.w[1],
                                 a.w[2] | b.w[2],
                                 a.w[3] | b.w[3]);
        }

        public static Bitset256 operator^(Bitset256 a, Bitset256 b) {
            return new Bitset256(a.w[0] ^ b.w[0],
                                 a.w[1] ^ b.w[1],
                                 a.w[2] ^ b.w[2],
                                 a.w[3] ^ b.w[3]);
        }

        public static Bitset256 operator~(Bitset256 a) {
            return new Bitset256(~a.w[0], ~a.w[1], ~a.w[2], ~a.w[3]);
        }

        public static explicit operator Bitset8(Bitset256 s256) {
            return new Bitset8((byte)s256.w[0]);
        }

        public static explicit operator Bitset16(Bitset256 s256) {
            return new Bitset16((ushort)s256.w[0]);
        }

        public static explicit operator Bitset32(Bitset256 s256) {
            return new Bitset32((uint)s256.w[0]);
        }

        public static explicit operator Bitset64(Bitset256 s256) {
            return new Bitset64(s256.w[0]);
        }

        public static explicit operator Bitset128(Bitset256 s256) {
            return new Bitset128(s256.w[0], s256.w[1]);
        }

        // Checks if the bit at a position is set
        public bool Test(int position) {
            BoundsCheck(position);
            ulong mask = 1u << WhichBit(position);
            return (w[WhichWord(position)] & mask) == mask;
        }

        // Sets bit to 1 at the given position
        public void Set(int position) {
            BoundsCheck(position);
            w[WhichWord(position)] |= 1ul << WhichBit(position);
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
            w[WhichWord(position)] &= ~(1ul << WhichBit(position));
        }

        // Flips the bit at the given position
        public void Flip(int position) {
            BoundsCheck(position);
            w[WhichWord(position)] ^= 1ul << WhichBit(position);
        }

        // Checks if all bits are set to 1
        public bool All() {
            for (int i = 0; i < WordsLength; ++i) {
                if (w[i] != 0xfffffffffffffffful) {
                    return false;
                }
            }
            return true;
        }

        // Checks if any bit is set to 1
        public bool Any() {
            for (int i = 0; i < WordsLength; ++i) {
                if (w[i] != 0ul) {
                    return true;
                }
            }
            return false;
        }

        // Checks if none of the bits are set to 1
        public bool None() {
            for (int i = 0; i < WordsLength; ++i) {
                if (w[i] != 0ul) {
                    return false;
                }
            }
            return true;
        }

        // Converts bits to an unsigned int
        public uint ToUInt32() {
            Debug.Assert((w[0] & 0xffffffff00000000ul) == 0
                         && CanConvertToUInt64(),
                         "Cannot convert to Uint32");
            return (uint)w[0];
        }

        // Converts bits to an unsigned long
        public ulong ToUInt64() {
            Debug.Assert(CanConvertToUInt64(), "Cannot convert to UInt64");
            return w[0];
        }

        // Converts each bit to one byte in a byte array
        public byte[] ToByteArray() {
            var bytes = new byte[Length];
            for (int i = 0; i < Length; ++i) {
                bytes[i] = Convert.ToByte(this[i]);
            }
            return bytes;
        }

        public bool Equals(Bitset256 s) {
            return this == s;
        }

        public override bool Equals(object obj) {
            return obj is Bitset256 && (Bitset256)obj == this;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 23 + w[0].GetHashCode();
                hash = hash * 23 + w[1].GetHashCode();
                hash = hash * 23 + w[2].GetHashCode();
                hash = hash * 23 + w[3].GetHashCode();
                return hash;
            }
        }

        public override string ToString() {
            return Convert.ToString((long)w[3], 2).PadLeft(BitsPerWord, '0')
                 + Convert.ToString((long)w[2], 2).PadLeft(BitsPerWord, '0')
                 + Convert.ToString((long)w[1], 2).PadLeft(BitsPerWord, '0')
                 + Convert.ToString((long)w[0], 2).PadLeft(BitsPerWord, '0');
        }

        bool CanConvertToUInt64() {
            for (int i = 1; i < WordsLength; ++i) {
                if (w[i] != 0ul) {
                    return false;
                }
            }
            return true;
        }

        int WhichWord(int position) {
            return position >> WordShift;
        }

        int WhichBit(int position) {
            return position & (BitsPerWord - 1);
        }

        [Conditional("DEBUG")]
        void BoundsCheck(int position) {
            Debug.Assert(position >= 0 && position < Length,
                         "Index out of bounds: Bitset256[" + position + "]");
        }
    };
}
