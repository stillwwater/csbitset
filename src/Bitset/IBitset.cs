namespace Bitset {
    public interface IBitset {
        // Returns the total number of bits in the bitset
        int Count { get; }
        bool this[int position] { get; set; }

        // Checks if the bit at a position is set
        bool Test(int position);

        // Sets bit to 1 at the given position
        void Set(int position);

        // Sets bit at a position to a given value
        void Set(int position, bool value);

        // Resets bit to 0 at the given position
        void Reset(int position);

        // Flips the bit at the given position
        void Flip(int position);

        // Checks if all bits are set to 1
        bool All();

        // Checks if any bit is set to 1
        bool Any();

        // Checks if none of the bits are set to 1
        bool None();

        // Converts bits to an unsigned int
        uint ToUInt32();

        // Converts bits to an unsigned long
        ulong ToUInt64();

        // Converts each bit to one byte in a byte array
        byte[] ToByteArray();
    };
}
