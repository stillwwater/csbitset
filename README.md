# C# Bitset

A library for doing operations on 8, 16, 32, 64, 128 and 256 bit sets with a similar API to `std::bitset` from the C++ standard library. Unlike `System.Collections.BitArray`, bit sets do not heap allocate.

For sizes greater than 256 bits, the standard `BitArray` is likely better suited as copying larger bitset structs would become expensive.
