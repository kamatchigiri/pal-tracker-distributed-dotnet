using DatabaseSupport;
using Microsoft.AspNetCore.Mvc;
using TestSupport;
using Users;
using Xunit;

namespace UsersTest
{
    [Collection("Users")]
    public class UserControllerTest
    {
        private readonly DataSourceConfig _dataSourceConfig = new DataSourceConfig();
        private readonly TestDatabaseSupport _support = new TestDatabaseSupport();

        static UserControllerTest() => TestEnvSupport.SetRegistrationVcap();
        public UserControllerTest() => _support.TruncateAllTables();

        [Fact]
        public void TestGet()
        {
            _support.ExecSql("insert into users (id, name) values (4765, 'Jack'), (4766, 'Fred');");

            var controller =
                new UserController(new UserDataGateway(new DatabaseTemplate(_dataSourceConfig)));
            var result = controller.Get(4765);
            var info = ((UserInfo) ((ObjectResult) result).Value);

            Assert.Equal(4765, info.Id);
            Assert.Equal("Jack", info.Name);
            Assert.Equal("user info", info.Info);
        }
    }
}