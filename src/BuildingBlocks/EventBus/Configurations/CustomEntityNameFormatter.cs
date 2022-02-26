using MassTransit.Definition;
using MassTransit.Topology;

namespace EventBus.Configurations;

public class CustomEntityNameFormatter : IEntityNameFormatter
{
    private readonly IEndpointNameFormatter _formatter;

    public CustomEntityNameFormatter(IEntityNameFormatter entityNameFormatter)
    {
        _formatter = KebabCaseEndpointNameFormatter.Instance;
    }

    public string FormatEntityName<T>()
    {
        var name = typeof(T).FullName;
        name = MyEntityName(name);
        name = _formatter.SanitizeName(name);
        return name;
    }

    private string MyEntityName(string fullname)
    {
        var newName = fullname.Split('.');
        var length = newName.Length - 1;
        var first = newName[length - 1];
        var last = newName[length];

        if (!last.EndsWith("Msg"))
        {
            last = last + "Msg";
        }
        var res = first + last;
        return res;
    }
}
