#tool "nuget:?package=coveralls.io&version=1.4.2"
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
            .Append("/p:CoverletOutput=./" + coverageResults)
            .Append("/p:Exclude=\"[xunit.*]*\"")
    };

    DotNetCoreTest(testProjectPath.FullPath, settings);
    MoveFile(fullTestDir.CombineWithFilePath(coverageResults), artifactDir.CombineWithFilePath(coverageResults));
});

Task("BuildAndTest")
.IsDependentOn("Build")
.IsDependentOn("Test");

Task("Default")
.Does(() => {
   Information("Hello Cake!");
});

RunTarget(target);
