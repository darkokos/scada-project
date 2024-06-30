namespace ScadaCore.Drivers;

public interface IAnalogDriver {
    decimal? Read(int inputAddress);
}