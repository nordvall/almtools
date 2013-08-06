using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ALMTools.TFS.Tests
{
    [TestClass]
    public class BuildNumberCheckerTests
    {
        private const string _buildDefinitionName = "BuildDefinition";

        [TestMethod]
        public void EnsureUniqueBuildNumber_WhenNoMatchingBuildsExistOnServer_SuggestedNameIsReturned()
        {
            IBuildServer buildServer = Substitute.For<IBuildServer>();
            var checker = new BuildNumberChecker(buildServer);

            IBuildDefinition buildDefinition = Substitute.For<IBuildDefinition>();
            string suggestion = string.Concat(_buildDefinitionName, "_1.0.0.0");

            string result = checker.GetUniqueBuildNumber(buildDefinition, suggestion);

            Assert.AreEqual(suggestion, result);
        }

        [TestMethod]
        public void EnsureUniqueBuildNumber_WhenSingleMatchingBuildsExistOnServer_SuggestedNameGetsSuffix()
        {
            string suggestion = string.Concat(_buildDefinitionName, "_1.0.0.0");

            IBuildDetail buildDetail = Substitute.For<IBuildDetail>();
            buildDetail.BuildNumber.Returns(suggestion);
            
            IBuildQueryResult queryResult = Substitute.For<IBuildQueryResult>();
            queryResult.Builds.Returns(new IBuildDetail[] { buildDetail });

            IBuildServer buildServer = Substitute.For<IBuildServer>();
            buildServer.QueryBuilds(Arg.Any<IBuildDetailSpec>()).Returns(queryResult);
            
            var checker = new BuildNumberChecker(buildServer);

            IBuildDefinition buildDefinition = Substitute.For<IBuildDefinition>();
            string result = checker.GetUniqueBuildNumber(buildDefinition, suggestion);

            Assert.AreEqual(suggestion + "-1", result);
        }

    }
}
