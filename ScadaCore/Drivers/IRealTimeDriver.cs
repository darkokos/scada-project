namespace ScadaCore.Drivers;

public interface IRealTimeDriver {
    public void RegisterWriter(int inputAddress, string publicKey);
}