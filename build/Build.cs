using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.SonarScanner;
using Nuke.Common.Utilities;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.SonarScanner.SonarScannerTasks;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    GitHubActionsImage.UbuntuLatest,
    OnPushBranches = new[] { "master", "develop", "main" },
    OnPullRequestBranches = new[] { "master", "develop", "main" },
    InvokedTargets = new[] { nameof(Test), nameof(Pack) },
    ImportSecrets = new[] { "NUGET_API_KEY", "SONAR_TOKEN" },
    EnableGitHubToken = true)]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Test);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly bool? Cover = true;
    [Parameter] readonly string NuGetApiKey;
    [Parameter] readonly string SonarToken;

    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion(NoFetch = true)] readonly GitVersion GitVersion;
    [Solution(GenerateProjects = true)] readonly Solution Solution;

    readonly string NuGetSource = "https://api.nuget.org/v3/index.json";
    readonly string SonarProjectKey = "ubiety_Ubiety.Xmpp.Core";

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "test";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath CoverageDirectory => ArtifactsDirectory / "coverage";
    AbsolutePath TestResultsDirectory => ArtifactsDirectory / "test-results";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            TestsDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            ArtifactsDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target SonarBegin => _ => _
        .Before(Compile)
        .OnlyWhenStatic(() => !string.IsNullOrEmpty(SonarToken))
        .Executes(() =>
        {
            SonarScannerBegin(s => s
                .SetToken(SonarToken)
                .SetProjectKey(SonarProjectKey)
                .SetOrganization("ubiety")
                .SetServer("https://sonarcloud.io")
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetOpenCoverPaths(CoverageDirectory / "coverage.opencover.xml")
                .SetCoverageExclusions("**/test/**,**/*Test*.cs,**/*Tests.cs")
                .SetDuplicationExclusions("**/test/**"));
        });

    Target SonarEnd => _ => _
        .After(Test)
        .DependsOn(SonarBegin)
        .OnlyWhenStatic(() => !string.IsNullOrEmpty(SonarToken))
        .AssuredAfterFailure()
        .Executes(() =>
        {
            SonarScannerEnd(s => s
                .SetToken(SonarToken));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            TestResultsDirectory.CreateOrCleanDirectory();
            CoverageDirectory.CreateOrCleanDirectory();

            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetNoBuild(true)
                .SetLoggers("trx")
                .SetResultsDirectory(TestResultsDirectory)
                .When(Cover == true, _ => _
                    .SetDataCollector("XPlat Code Coverage")
                    .SetProperty("CollectCoverage", true)
                    .SetProperty("CoverletOutputFormat", "opencover")
                    .SetProperty("CoverletOutput", CoverageDirectory / "coverage.opencover.xml")
                    .SetProperty("ExcludeByFile", "**/*Test*.cs")
                    .SetProperty("Exclude", "[*Test*]*")));
        });

    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            var mainProject = Solution.AllProjects.FirstOrDefault(x => x.Name == "Ubiety.Xmpp.Core");
            if (mainProject != null)
            {
                DotNetPack(s => s
                    .SetProject(mainProject)
                    .SetConfiguration(Configuration)
                    .SetOutputDirectory(ArtifactsDirectory)
                    .SetVersion(GitVersion.NuGetVersionV2)
                    .SetIncludeSymbols(true)
                    .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                    .EnableNoBuild());
            }
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Requires(() => NuGetApiKey)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .Executes(() =>
        {
            var packages = ArtifactsDirectory.GlobFiles("*.nupkg").Where(x => !x.ToString().Contains("symbols"));
            
            DotNetNuGetPush(s => s
                .SetApiKey(NuGetApiKey)
                .SetSource(NuGetSource)
                .EnableSkipDuplicate()
                .CombineWith(packages, (cs, v) => cs.SetTargetPath(v)));
        });

    Target GenerateWorkflows => _ => _
        .Executes(() =>
        {
            // This target generates GitHub Actions workflows
            Log.Information("Generating GitHub Actions workflows...");
        });

}
