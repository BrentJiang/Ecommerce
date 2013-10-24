using System.Collections.Generic;
using MrCMS.Web.Apps.Ecommerce.Entities.Orders;
using MrCMS.Web.Apps.Ecommerce.Models;
using System.Linq;

namespace MrCMS.Web.Apps.Ecommerce.Services.Analytics.Orders
{
    public interface IGroupOrdersService
    {
        void AddOrdersGroupedByShippmentType(IEnumerable<IGrouping<SalesChannel, Order>> baseData,
            ref List<IList<KeyValuePair<string, decimal>>> results, SalesChannel salesChannel);
    }

    public class GroupOrdersService : IGroupOrdersService
    {
        public void AddOrdersGroupedByShippmentType(IEnumerable<IGrouping<SalesChannel, Order>> baseData,
            ref List<IList<KeyValuePair<string, decimal>>> results, SalesChannel salesChannel)
        {
            var item = baseData.ToList().SingleOrDefault(x => x.Key == salesChannel);
            if (item != null)
                results.Add(item.Where(x=>x.ShippingMethod!=null).GroupBy(c => c.ShippingMethod.Name)
                                    .Select(k => new { k.Key, Sum = k.Count() })
                                    .ToDictionary(t => t.Key, t => decimal.Parse(t.Sum.ToString()))
                                    .ToList());
            else
                results.Add(new List<KeyValuePair<string, decimal>>());
        }
    }
}