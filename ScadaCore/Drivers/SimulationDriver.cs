namespace ScadaCore.Drivers;

public class SimulationDriver : IDriver {
    private static decimal Sine() {
        return (decimal) (100 * Math.Sin((double) DateTime.Now.Second / 60 * Math.PI));
    }

    private static decimal Cosine() {
        return (decimal) (100 * Math.Cos((double) DateTime.Now.Second / 60 * Math.PI));
    }

    private static decimal Ramp() {
        return (decimal) 100 * DateTime.Now.Second / 60;
    }
    
    public decimal? Read(int inputAddress) {
        return (inputAddress % 3) switch {
            0 => Sine(),
            1 => Cosine(),
            2 => Ramp(),
            _ => null
        };
    }
}