namespace Dishcovery.Model;

public class Ingredient
{
    public float Measurement { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public Ingredient(string name, float measurement, string type)
    {
        Name = name;
        Measurement = measurement;
        Type = type;
    }
}
