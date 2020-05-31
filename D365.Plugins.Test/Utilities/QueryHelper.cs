using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365.Plugins.Test.Utilities
{
    public static class QueryHelper
    {
        public static Entity GetEntity(IOrganizationService organizationService, String entityName, String conditionAtributeName, object conditionAttributeValue, params String[] columns)
        {
            QueryExpression query = new QueryExpression(entityName);
            query.ColumnSet = new ColumnSet(columns);
            query.Criteria.AddCondition(conditionAtributeName, ConditionOperator.Equal, conditionAttributeValue);

            return organizationService.RetrieveMultiple(query).Entities.FirstOrDefault();
        }

    }
}
