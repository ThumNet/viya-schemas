namespace Sample;

[AttributeUsage(AttributeTargets.Property)]
public class RefFromAttribute : Attribute
{
    public string RefValue { get; }

    public RefFromAttribute(string refValue)
    {
        RefValue = refValue;
    }
}