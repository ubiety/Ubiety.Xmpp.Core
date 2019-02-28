#module "nuget:?package=Cake.DotNetTool.Module&version=0.1.0"
#tool "nuget:?package=coveralls.io&version=1.4.2"
#tool "dotnet:?package=dotnet-sonarscanner&version=4.6.0"
#addin "nuget:?package=Cake.Git&version=0.19.0"
#addin "nuget:?package=Nuget.Core&version=2.14.0"
#addin "nuget:?package=Cake.Coveralls&version=0.9.0"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactDir = new DirectoryPath("./artifacts/");
var testDir = new DirectoryPath("./test");
var testProjectDir = new DirectoryPath("Ubiety.Xmpp.Test");
var testProject = new FilePath("Ubiety.Xmpp.Test.csproj");
var solution = "./Ubiety.Xmpp.Core.sln";
var coverageResults = new FilePath("coverage.xml");
var currentBranch = Argument<string>("currentBranch", GitBranchCurrent("./").FriendlyName);
var isReleaseBuild = string.Equals(currentBranch, "master", StringComparison.OrdinalIgnoreCase);
var fullTestDir = testDir.Combine(testProjectDir);
var testProjectPath = fullTestDir.CombineWithFilePath(testProject);
var coverallsToken = Argument<string>("coverallsToken", null);
var nugetKey = Argument<string>("nugetKey", null);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
 
Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
.Does(() => {
    if (DirectoryExists(artifactDir))
    {
        DeleteDirectory(
            artifactDir,
            new DeleteDirectorySettings
            {
                Recursive = true,
                Force = true
            });
    }

    CreateDirectory(artifactDir);
    DotNetCoreClean(solution);
});

Task("Restore")
.Does(() => {
    DotNetCoreRestore(solution);
});

Task("Build")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.Does(() => {
    DotNetCoreBuild(
        solution,
        new DotNetCoreBuildSettings
        {
            Configuration = configuration
        }
    );
});

Task("Test")
.Does(() => {
    var settings = new DotNetCoreTestSettings
    {
        ArgumentCustomization = args => args
            .Append("/p:CollectCoverage=true")
            .Append("/p:CoverletOutputFormat=opencover")
            .Append($"/p:CoverletOutput=./{coverageResults}")
            .Append("/p:Exclude=\"[xunit.*]*\"")
    };

    DotNetCoreTest(testProjectPath.FullPath, settings);
    MoveFile(fullTestDir.CombineWithFilePath(coverageResults), artifactDir.CombineWithFilePath(coverageResults));
});

Task("SonarBegin")
.Does(() => {
   DotNetCoreTool("sonarscanner", new DotNetCoreToolSettings {
       ArgumentCustomization = args => args
            .Append("begin")
            .Append("/k:\"ubiety_Ubiety.Xmpp.Core\"")
            .Append("/o:\"ubiety\"")
            .Append("/d:sonar.host.url=\"https://sonarcloud.io\"")
            .Append("/d:sonar.login=\"29ba6d2bcdb3c1ad1c78785797374e166749941c\"")
   });
});

Task("SonarEnd")
.Does(() => {
    DotNetCoreTool("sonarscanner", new DotNetCoreToolSettings {
        ArgumentCustomization = args => args
            .Append("end")
            .Append("/d:sonar.login=\"29ba6d2bcdb3c1ad1c78785797374e166749941c\"")
    });
});

Task("UploadCoverage")
.IsDependentOn("Test")
.Does(() => {
    CoverallsIo(artifactDir.CombineWithFilePath(coverageResults), new CoverallsIoSettings {
        RepoToken = coverallsToken
    });
});

Task("Sonar")
.IsDependentOn("SonarBegin")
.IsDependentOn("BuildAndTest")
.IsDependentOn("SonarEnd");

Task("BuildAndTest")
.IsDependentOn("Build")
.IsDependentOn("Test");

Task("Default")
.Does(() => {
   Information("Hello Cake!");
});

RunTarget(target);
