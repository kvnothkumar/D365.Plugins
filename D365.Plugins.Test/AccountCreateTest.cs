using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using D365.Plugins.Test.Utilities;
using System.Collections.Specialized;

namespace D365.Plugins.Test
{
    [TestFixture]
    public class AccountCreateTest
    {
        XrmRealContext context;
        IOrganizationService organizationService;
        Dictionary<String, Object> parameters = new Dictionary<string, object>();
        List<KeyValuePair<string, Guid>> deleteObjects = new List<KeyValuePair<string, Guid>>();

        [OneTimeSetUp]
        public void Initialize()
        {
            //Initialize the Context with Admin Account
            context = new XrmRealContext("Xrm");

            //To Run the Plugin with User Context, Add a new ConnectionString and Pass the connectionStringName
            //context = new XrmRealContext("Xrm_Officer");
            //context = new XrmRealContext("Xrm_Manager");

            organizationService = context.GetOrganizationService();

            //Use this method to arrange the required data.
            Entity entity = new Entity("account");
            entity["name"] = "Fourth Coffee Pvt. Ltd";
            Guid accountId = organizationService.Create(entity);

            parameters.Add("Account", entity);
            parameters.Add("AccountId", accountId);

            deleteObjects.Add("account", accountId);
        }


        [Test, Order(1)]
        public void CheckTask()
        {
            //Arrange
            Entity target = parameters["Account"] as Entity;

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", target);

            ParameterCollection outputParameters = new ParameterCollection();
            outputParameters.Add("id", parameters["AccountId"].ToString());

            //Act
            this.context.ExecutePluginWith<AccountCreate>(inputParameters, outputParameters, null, null);


            //Assert
            Entity task = QueryHelper.GetEntity(organizationService, "task", "regardingobjectid", ((Guid)parameters["AccountId"]), "subject", "createdon", "actualend");

            Assert.IsNotNull(task);
            Assert.IsTrue(task.GetAttributeValue<String>("subject").StartsWith("Follow-Up with Account"));
            Assert.IsTrue(task.GetAttributeValue<DateTime>("createdon").Date == DateTime.Today.Date);
        }       

        [OneTimeTearDown]
        public void Delete()
        {
            foreach (var item in deleteObjects)
            {
                organizationService.Delete(item.Key, item.Value);
            }
        }
    }
}
