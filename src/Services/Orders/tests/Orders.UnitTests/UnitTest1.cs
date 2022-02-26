namespace Orders.UnitTests;

public class UnitTest1
{
    [Test]
    public void Test1()
    {
        // Baskets.Application.IntegrationEvents.EventHandler.OrderStartedHandler
        var fullname = "Baskets.Application.IntegrationEvents.EventHandler.OrderStartedHandler";
        var newName =  fullname.Split('.');
        var first =  newName.First();
       var last =  newName.Last();

        if (last.EndsWith("Handler"))
        {
           last =  last.Replace("Handler","").Trim();
        }
        var res = first + last;
    }

    [Test]
    public void Test2()
    {
        // EventBus.IntegrationEvents.Orders.OrderStarted

        var fullname = "EventBus.IntegrationEvents.Orders.OrderStarted";
        var newName = fullname.Split('.');
        var length = newName.Length - 1;
        var first = newName[length -1 ];
        var last = newName[length];

        if (!last.EndsWith("Msg"))
        {
            last = last + "Msg";
        }

        var res = first + last;
    }

    [Test]
    public void SperateString()
    {
        string ids = "p01,p02,p03,p04";

        var sp = ids.Split(',').ToList();

        var sp1 = string.Join(',',"a","b","c");
    }

}