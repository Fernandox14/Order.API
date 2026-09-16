namespace Order.Dominio.Enums;

[AttributeUsage(AttributeTargets.Field)]
public class EnumTextAttribute : Attribute
{
    public string Text { get; }

    public EnumTextAttribute(string text)
    {
        Text = text;
    }
}
