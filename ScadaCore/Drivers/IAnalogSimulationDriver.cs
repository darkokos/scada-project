namespace ScadaCore.Drivers;

public interface IAnalogSimulationDriver {
    decimal? Read(int inputAddress);
}