namespace Api.Chaos;

public class ChaosSettings
{
    public ExceptionSettings ExceptionSettings { get; set; }
    public LatencySettings LatencySettings { get; set; }
    public ResultSettings ResultSettings { get; set; }
    
    public BehaviorSettings BehaviorSettings { get; set; }
}

public class ExceptionSettings
{
    public bool Enabled { get; set; }
    public double InjectionRate { get; set; } = 0;
}

public class LatencySettings
{
    public bool Enabled { get; set; }
    public double InjectionRate { get; set; } = 0;
    public int MsLatency { get; set; } = 0;
    public int SecondsLatency { get; set; } = 0;
}

public class ResultSettings
{
    public bool Enabled { get; set; }
    public double InjectionRate { get; set; } = 0;
}

public class BehaviorSettings
{
    public bool Enabled { get; set; }
    public double InjectionRate { get; set; } = 0;
}