using System.Collections.Generic;
using DatabaseSupport;
using Microsoft.AspNetCore.Mvc;
using Projects;
using TestSupport;
using Xunit;

namespace ProjectsTest
{
    [Collection("Projects")]
    public class ProjectControllerTest
    {
        private readonly DataSourceConfig _dataSourceConfig = new DataSourceConfig();
        private readonly TestDatabaseSupport _support = new TestDatabaseSupport();

        private readonly string _sql = @"
insert into users (id, name) values (4765, 'Jack'), (4766, 'Fred');

insert into accounts (id, owner_id, name) 
values (1673, 4765, 'Jack''s account'), (1674, 4766, 'Fred''s account');

insert into projects (id, account_id, name, active) 
values (55432, 1673, 'Flagship', true), (55431, 1673, 'Hovercraft', false);
";

        static ProjectControllerTest() => TestEnvSupport.SetRegistrationVcap();
        public ProjectControllerTest() => _support.TruncateAllTables();

        [Fact]
        public void TestPost()
        {
            _support.ExecSql(_sql);

            var controller =
                new ProjectController(new ProjectDataGateway(new DatabaseTemplate(_dataSourceConfig)));

            var value = controller.Post(new ProjectInfo(-1, 1673, "aProject", true, ""));
            var actual = (ProjectInfo) ((ObjectResult) value).Value;

            Assert.True(actual.Id > 0);
            Assert.Equal(1673, actual.AccountId);
            Assert.Equal("aProject", actual.Name);
            Assert.Equal(true, actual.Active);
            Assert.Equal("project info", actual.Info);
        }

        [Fact]
        public void TestGet()
        {
            _support.ExecSql(_sql);

            var controller =
                new ProjectController(new ProjectDataGateway(new DatabaseTemplate(_dataSourceConfig)));
            var result = controller.Get(55431);
            var actual = (ProjectInfo) ((ObjectResult) result).Value;

            Assert.Equal(55431, actual.Id);
            Assert.Equal(1673, actual.AccountId);
            Assert.Equal("Hovercraft", actual.Name);
            Assert.Equal(false, actual.Active);
        }

        [Fact]
        public void TestList()
        {
            _support.ExecSql(_sql);

            var controller =
                new ProjectController(new ProjectDataGateway(new DatabaseTemplate(_dataSourceConfig)));
            var result = controller.List(1673);

            // todo - full asserts?
            Assert.Equal(2, ((List<ProjectInfo>) ((ObjectResult) result).Value).Count);
        }
    }
}