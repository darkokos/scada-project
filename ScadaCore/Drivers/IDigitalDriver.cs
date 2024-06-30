namespace ScadaCore.Drivers;

public interface IDigitalDriver {
    bool? Read(int inputOutputAddress);
}