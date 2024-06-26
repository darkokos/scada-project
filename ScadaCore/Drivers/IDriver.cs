namespace ScadaCore.Drivers;

public interface IDriver {
    decimal? Read(int inputAddress);
}