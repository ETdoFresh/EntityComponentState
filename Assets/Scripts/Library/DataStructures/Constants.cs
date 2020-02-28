using System.Text;

namespace EntityComponentState
{
    public static class Constants
    {
        public const byte SIMULATION_RATE = 30;
        public const byte TICK_RATE = 5;
        public static readonly byte[] DELIMITER = Encoding.UTF8.GetBytes("<EOF>");
        public const string STATE_FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\state.bin";
        public const string DELTASTATE_FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\deltaState.bin";
        public const string STATEHISTORY_FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\stateHistory.bin";
        public const string DELTASTATEHISTORY_FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\deltaStateHistory.bin";
        public const string TICK_FILE = @"C:\Users\etgarcia\Desktop\EntityComponentState\tick.bin";
    }
}