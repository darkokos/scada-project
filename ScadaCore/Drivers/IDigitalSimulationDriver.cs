namespace ScadaCore.Drivers;

public interface IDigitalSimulationDriver {
    bool? Read(int inputOutputAddress);
}