using NUnit.Framework;
using Bitset;

namespace Bitset.Tests {
    public class Tests {
        [Test]
        public void TestBitset8() {
            Bitset8 a = default;
            Bitset8 b = default;

            a.Set(0);
            b.Set(1);
            Assert.AreEqual(1, a.ToUInt64());
            Assert.AreEqual(2, b.ToUInt32());
            Assert.AreEqual(3, (a | b).ToUInt64());
            Assert.AreEqual(3, (a ^ b).ToUInt64());
            Assert.AreEqual(0, (a & b).ToUInt64());

            var c = ~a;
            Assert.False(c[0]);
            Assert.True(c.Any());

            c |= a;
            Assert.True(c.All());
            Assert.True(c[7]);
            Assert.AreEqual(c, c);
            Assert.AreNotEqual(c, a);

            c = ~c;
            Assert.True(c.None());
        }

        [Test]
        public void TestBitset16() {
            Bitset16 a = default;
            Bitset16 b = default;

            a.Set(0);
            b.Set(1);
            Assert.AreEqual(1, a.ToUInt64());
            Assert.AreEqual(2, b.ToUInt32());
            Assert.AreEqual(3, (a | b).ToUInt64());
            Assert.AreEqual(3, (a ^ b).ToUInt64());
            Assert.AreEqual(0, (a & b).ToUInt64());

            var c = ~a;
            Assert.False(c[0]);
            Assert.True(c.Any());

            c |= a;
            Assert.True(c.All());
            Assert.True(c[15]);
            Assert.AreEqual(c, c);
            Assert.AreNotEqual(c, a);

            c = ~c;
            Assert.True(c.None());
        }

        [Test]
        public void TestBitset32() {
            Bitset32 a = default;
            Bitset32 b = default;

            a.Set(0);
            b.Set(1);
            Assert.AreEqual(1, a.ToUInt64());
            Assert.AreEqual(2, b.ToUInt32());
            Assert.AreEqual(3, (a | b).ToUInt64());
            Assert.AreEqual(3, (a ^ b).ToUInt64());
            Assert.AreEqual(0, (a & b).ToUInt64());

            var c = ~a;
            Assert.False(c[0]);
            Assert.True(c.Any());

            c |= a;
            Assert.True(c.All());
            Assert.True(c[31]);
            Assert.AreEqual(c, c);
            Assert.AreNotEqual(c, a);

            c = ~c;
            Assert.True(c.None());
        }

        [Test]
        public void TestBitset64() {
            Bitset64 a = default;
            Bitset64 b = default;

            a.Set(0);
            b.Set(1);
            Assert.AreEqual(1, a.ToUInt64());
            Assert.AreEqual(2, b.ToUInt32());
            Assert.AreEqual(3, (a | b).ToUInt64());
            Assert.AreEqual(3, (a ^ b).ToUInt64());
            Assert.AreEqual(0, (a & b).ToUInt64());

            var c = ~a;
            Assert.False(c[0]);
            Assert.True(c.Any());

            c |= a;
            Assert.True(c.All());
            Assert.True(c[63]);
            Assert.AreEqual(c, c);
            Assert.AreNotEqual(c, a);

            c = ~c;
            Assert.True(c.None());
        }

        [Test]
        public unsafe void TestBitset128() {
            Bitset128 a = default;
            Bitset128 b = default;

            a.Set(0);
            b.Set(1);
            Assert.AreEqual(1, a.ToUInt64());
            Assert.AreEqual(2, b.ToUInt32());
            Assert.AreEqual(3, (a | b).ToUInt64());
            Assert.AreEqual(3, (a ^ b).ToUInt64());
            Assert.AreEqual(0, (a & b).ToUInt64());

            var c = ~a;
            Assert.False(c[0]);
            Assert.True(c.Any());

            c |= a;
            Assert.True(c.All());
            Assert.True(c[127]);
            Assert.AreEqual(c, c);
            Assert.AreNotEqual(c, a);

            c = ~c;
            Assert.True(c.None());

            a.Set(127);
            Assert.AreEqual(a.w[1], 0x8000000000000000);

            Bitset128 d = default;
            d.Set(1);
            d.Reset(1);
            d.Flip(0);
            Assert.AreEqual(1, d.Low);
            Assert.AreEqual(0, d.High);
        }

        [Test]
        public unsafe void TestBitset256() {
            Bitset256 a = default;
            Bitset256 b = default;

            a.Set(0);
            b.Set(1);
            Assert.AreEqual(1, a.ToUInt64());
            Assert.AreEqual(2, b.ToUInt32());
            Assert.AreEqual(3, (a | b).ToUInt64());
            Assert.AreEqual(3, (a ^ b).ToUInt64());
            Assert.AreEqual(0, (a & b).ToUInt64());

            var c = ~a;
            Assert.False(c[0]);
            Assert.True(c.Any());

            c |= a;
            Assert.True(c.All());
            Assert.True(c[255]);
            Assert.AreEqual(c, c);
            Assert.AreNotEqual(c, a);

            c = ~c;
            Assert.True(c.None());

            a.Set(255);
            Assert.AreEqual(a.w[3], 0x8000000000000000);

            var one = new Bitset128(1);
            var zero = new Bitset128(0);
            Bitset256 d = default;
            d.Set(1);
            d.Reset(1);
            d.Set(128);
            Assert.AreEqual(one, d.High);
            Assert.AreEqual(zero, d.Low);
        }
    };
}
