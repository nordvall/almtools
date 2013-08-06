using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Build.Client;

namespace ALMTools.TFS
{
    public class BuildNumberChecker
    {
        private IBuildServer _buildServer;

        public BuildNumberChecker(IBuildServer buildServer)
        {
            if (buildServer == null)
            {
                throw new ArgumentNullException("buildServer");
            }

            _buildServer = buildServer;
        }

        public string GetUniqueBuildNumber(IBuildDefinition buildDefinition, string suggestedName)
        {
            IBuildDetail[] previousBuilds = GetPreviousBuildsWithSameName(buildDefinition, suggestedName);

            if (previousBuilds == null || previousBuilds.Length == 0)
            {
                return suggestedName;
            }
            else
            {
                string newBuildNumber = string.Format("{0}-{1}", suggestedName, previousBuilds.Count());
                return newBuildNumber;
            }
        }

        private IBuildDetail[] GetPreviousBuildsWithSameName(IBuildDefinition buildDefinition, string suggestedName)
        {
            IBuildDetailSpec buildDetailSpec = _buildServer.CreateBuildDetailSpec(buildDefinition);
            buildDetailSpec.QueryOptions = QueryOptions.None;
            buildDetailSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending;
            buildDetailSpec.QueryDeletedOption = QueryDeletedOption.IncludeDeleted;
            buildDetailSpec.MaxBuildsPerDefinition = 100;
            buildDetailSpec.BuildNumber = string.Concat(suggestedName, "*");

            IBuildQueryResult queryResult = _buildServer.QueryBuilds(buildDetailSpec);

            return queryResult.Builds;
        }
    }
}
