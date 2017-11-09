using Accounts;
using DatabaseSupport;
using Microsoft.AspNetCore.Mvc;
using TestSupport;
using Users;
using Xunit;

namespace AccountsTest
{
    [Collection("Accounts")]
    public class RegistrationControllerTest
    {
        private readonly DataSourceConfig _dataSourceConfig = new DataSourceConfig();
        private readonly TestDatabaseSupport _support = new TestDatabaseSupport();

        static RegistrationControllerTest() => TestEnvSupport.SetRegistrationVcap();
        public RegistrationControllerTest() => _support.TruncateAllTables(); 

        [Fact]
        public void TestPost()
        {
            _support.ExecSql("insert into users (id, name) values (4765, 'Jack'), (4766, 'Fred');");
            _support.ExecSql(@"insert into accounts (id, owner_id, name) 
            values (1673, 4765, 'Jack''s account'), (1674, 4766, 'Fred''s account');");

            var userDataGateway = new UserDataGateway(new DatabaseTemplate(_dataSourceConfig));
            var accountDataGateway = new AccountDataGateway(new DatabaseTemplate(_dataSourceConfig));
            var service = new RegistrationService(userDataGateway, accountDataGateway);

            var controller = new RegisationController(service);
            var value = controller.Post(new UserInfo(-1, "aUser", ""));
            var actual = (UserInfo) ((ObjectResult) value).Value;

            Assert.True(actual.Id > 0);
            Assert.Equal("aUser", actual.Name);
            Assert.Equal("registration info", actual.Info);
        }
    }
}