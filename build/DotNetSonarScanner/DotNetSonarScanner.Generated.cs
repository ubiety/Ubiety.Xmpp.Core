// Generated from https://github.com/ubiety/common/blob/master/build/specifications/DotNetSonarScanner.json
// Generated with Nuke.CodeGeneration version LOCAL (Windows,.NETStandard,Version=v2.0)

using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Nuke.Common.Tools.DotNetSonarScanner
{
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class DotNetSonarScannerTasks
    {
        /// <summary>
        ///   Path to the DotNetSonarScanner executable.
        /// </summary>
        public static string DotNetSonarScannerPath =>
            ToolPathResolver.TryGetEnvironmentExecutable("DOTNETSONARSCANNER_EXE") ??
            ToolPathResolver.GetPathExecutable("dotnet");
        public static Action<OutputType, string> DotNetSonarScannerLogger { get; set; } = ProcessTasks.DefaultLogger;
        /// <summary>
        ///   The SonarScanner for MSBuild is the recommended way to launch a SonarQube or SonarCloud analysis for projects/solutions using MSBuild or dotnet command as build tool.
        /// </summary>
        public static IReadOnlyCollection<Output> DotNetSonarScanner(string arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Func<string, string> outputFilter = null)
        {
            var process = ProcessTasks.StartProcess(DotNetSonarScannerPath, arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, DotNetSonarScannerLogger, outputFilter);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary>
        ///   <p>The SonarScanner for MSBuild is the recommended way to launch a SonarQube or SonarCloud analysis for projects/solutions using MSBuild or dotnet command as build tool.</p>
        ///   <p>For more details, visit the <a href="https://www.sonarqube.org/">official website</a>.</p>
        /// </summary>
        public static IReadOnlyCollection<Output> DotNetSonarScannerBegin(DotNetSonarScannerBeginSettings toolSettings = null)
        {
            toolSettings = toolSettings ?? new DotNetSonarScannerBeginSettings();
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary>
        ///   <p>The SonarScanner for MSBuild is the recommended way to launch a SonarQube or SonarCloud analysis for projects/solutions using MSBuild or dotnet command as build tool.</p>
        ///   <p>For more details, visit the <a href="https://www.sonarqube.org/">official website</a>.</p>
        /// </summary>
        /// <remarks>
        ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
        ///   <ul>
        ///     <li><c>/d:sonar.coverage.exclusions</c> via <see cref="DotNetSonarScannerBeginSettings.CoverageExclusions"/></li>
        ///     <li><c>/d:sonar.cpd.exclusions</c> via <see cref="DotNetSonarScannerBeginSettings.DuplicationExclusions"/></li>
        ///     <li><c>/d:sonar.cs.dotcover.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.DotCoverPaths"/></li>
        ///     <li><c>/d:sonar.cs.nunit.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.NUnitTestReports"/></li>
        ///     <li><c>/d:sonar.cs.opencover.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.OpenCoverPaths"/></li>
        ///     <li><c>/d:sonar.cs.vscoveragexml.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.VisualStudioCoveragePaths"/></li>
        ///     <li><c>/d:sonar.cs.vstest.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.VSTestReports"/></li>
        ///     <li><c>/d:sonar.cs.xunit.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.XUnitTestReports"/></li>
        ///     <li><c>/d:sonar.host.url</c> via <see cref="DotNetSonarScannerBeginSettings.Server"/></li>
        ///     <li><c>/d:sonar.links.ci</c> via <see cref="DotNetSonarScannerBeginSettings.ContinuousIntegrationUrl"/></li>
        ///     <li><c>/d:sonar.links.homepage</c> via <see cref="DotNetSonarScannerBeginSettings.Homepage"/></li>
        ///     <li><c>/d:sonar.links.issue</c> via <see cref="DotNetSonarScannerBeginSettings.IssueTrackerUrl"/></li>
        ///     <li><c>/d:sonar.links.scm</c> via <see cref="DotNetSonarScannerBeginSettings.SCMUrl"/></li>
        ///     <li><c>/d:sonar.login</c> via <see cref="DotNetSonarScannerBeginSettings.Login"/></li>
        ///     <li><c>/d:sonar.password</c> via <see cref="DotNetSonarScannerBeginSettings.Password"/></li>
        ///     <li><c>/d:sonar.projectDescription</c> via <see cref="DotNetSonarScannerBeginSettings.Description"/></li>
        ///     <li><c>/d:sonar.sourceEncoding</c> via <see cref="DotNetSonarScannerBeginSettings.SourceEncoding"/></li>
        ///     <li><c>/d:sonar.verbose</c> via <see cref="DotNetSonarScannerBeginSettings.Verbose"/></li>
        ///     <li><c>/d:sonar.ws.timeout</c> via <see cref="DotNetSonarScannerBeginSettings.WebServiceTimeout"/></li>
        ///     <li><c>/k</c> via <see cref="DotNetSonarScannerBeginSettings.ProjectKey"/></li>
        ///     <li><c>/n</c> via <see cref="DotNetSonarScannerBeginSettings.Name"/></li>
        ///     <li><c>/o</c> via <see cref="DotNetSonarScannerBeginSettings.Organization"/></li>
        ///     <li><c>/v</c> via <see cref="DotNetSonarScannerBeginSettings.Version"/></li>
        ///   </ul>
        /// </remarks>
        public static IReadOnlyCollection<Output> DotNetSonarScannerBegin(Configure<DotNetSonarScannerBeginSettings> configurator)
        {
            return DotNetSonarScannerBegin(configurator(new DotNetSonarScannerBeginSettings()));
        }
        /// <summary>
        ///   <p>The SonarScanner for MSBuild is the recommended way to launch a SonarQube or SonarCloud analysis for projects/solutions using MSBuild or dotnet command as build tool.</p>
        ///   <p>For more details, visit the <a href="https://www.sonarqube.org/">official website</a>.</p>
        /// </summary>
        /// <remarks>
        ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
        ///   <ul>
        ///     <li><c>/d:sonar.coverage.exclusions</c> via <see cref="DotNetSonarScannerBeginSettings.CoverageExclusions"/></li>
        ///     <li><c>/d:sonar.cpd.exclusions</c> via <see cref="DotNetSonarScannerBeginSettings.DuplicationExclusions"/></li>
        ///     <li><c>/d:sonar.cs.dotcover.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.DotCoverPaths"/></li>
        ///     <li><c>/d:sonar.cs.nunit.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.NUnitTestReports"/></li>
        ///     <li><c>/d:sonar.cs.opencover.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.OpenCoverPaths"/></li>
        ///     <li><c>/d:sonar.cs.vscoveragexml.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.VisualStudioCoveragePaths"/></li>
        ///     <li><c>/d:sonar.cs.vstest.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.VSTestReports"/></li>
        ///     <li><c>/d:sonar.cs.xunit.reportsPaths</c> via <see cref="DotNetSonarScannerBeginSettings.XUnitTestReports"/></li>
        ///     <li><c>/d:sonar.host.url</c> via <see cref="DotNetSonarScannerBeginSettings.Server"/></li>
        ///     <li><c>/d:sonar.links.ci</c> via <see cref="DotNetSonarScannerBeginSettings.ContinuousIntegrationUrl"/></li>
        ///     <li><c>/d:sonar.links.homepage</c> via <see cref="DotNetSonarScannerBeginSettings.Homepage"/></li>
        ///     <li><c>/d:sonar.links.issue</c> via <see cref="DotNetSonarScannerBeginSettings.IssueTrackerUrl"/></li>
        ///     <li><c>/d:sonar.links.scm</c> via <see cref="DotNetSonarScannerBeginSettings.SCMUrl"/></li>
        ///     <li><c>/d:sonar.login</c> via <see cref="DotNetSonarScannerBeginSettings.Login"/></li>
        ///     <li><c>/d:sonar.password</c> via <see cref="DotNetSonarScannerBeginSettings.Password"/></li>
        ///     <li><c>/d:sonar.projectDescription</c> via <see cref="DotNetSonarScannerBeginSettings.Description"/></li>
        ///     <li><c>/d:sonar.sourceEncoding</c> via <see cref="DotNetSonarScannerBeginSettings.SourceEncoding"/></li>
        ///     <li><c>/d:sonar.verbose</c> via <see cref="DotNetSonarScannerBeginSettings.Verbose"/></li>
        ///     <li><c>/d:sonar.ws.timeout</c> via <see cref="DotNetSonarScannerBeginSettings.WebServiceTimeout"/></li>
        ///     <li><c>/k</c> via <see cref="DotNetSonarScannerBeginSettings.ProjectKey"/></li>
        ///     <li><c>/n</c> via <see cref="DotNetSonarScannerBeginSettings.Name"/></li>
        ///     <li><c>/o</c> via <see cref="DotNetSonarScannerBeginSettings.Organization"/></li>
        ///     <li><c>/v</c> via <see cref="DotNetSonarScannerBeginSettings.Version"/></li>
        ///   </ul>
        /// </remarks>
        public static IEnumerable<(DotNetSonarScannerBeginSettings Settings, IReadOnlyCollection<Output> Output)> DotNetSonarScannerBegin(CombinatorialConfigure<DotNetSonarScannerBeginSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
        {
            return configurator.Invoke(DotNetSonarScannerBegin, DotNetSonarScannerLogger, degreeOfParallelism, completeOnFailure);
        }
        /// <summary>
        ///   <p>The SonarScanner for MSBuild is the recommended way to launch a SonarQube or SonarCloud analysis for projects/solutions using MSBuild or dotnet command as build tool.</p>
        ///   <p>For more details, visit the <a href="https://www.sonarqube.org/">official website</a>.</p>
        /// </summary>
        public static IReadOnlyCollection<Output> DotNetSonarScannerEnd(DotNetSonarScannerEndSettings toolSettings = null)
        {
            toolSettings = toolSettings ?? new DotNetSonarScannerEndSettings();
            var process = ProcessTasks.StartProcess(toolSettings);
            process.AssertZeroExitCode();
            return process.Output;
        }
        /// <summary>
        ///   <p>The SonarScanner for MSBuild is the recommended way to launch a SonarQube or SonarCloud analysis for projects/solutions using MSBuild or dotnet command as build tool.</p>
        ///   <p>For more details, visit the <a href="https://www.sonarqube.org/">official website</a>.</p>
        /// </summary>
        /// <remarks>
        ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
        ///   <ul>
        ///     <li><c>/d:sonar.login</c> via <see cref="DotNetSonarScannerEndSettings.Login"/></li>
        ///     <li><c>/d:sonar.password</c> via <see cref="DotNetSonarScannerEndSettings.Password"/></li>
        ///   </ul>
        /// </remarks>
        public static IReadOnlyCollection<Output> DotNetSonarScannerEnd(Configure<DotNetSonarScannerEndSettings> configurator)
        {
            return DotNetSonarScannerEnd(configurator(new DotNetSonarScannerEndSettings()));
        }
        /// <summary>
        ///   <p>The SonarScanner for MSBuild is the recommended way to launch a SonarQube or SonarCloud analysis for projects/solutions using MSBuild or dotnet command as build tool.</p>
        ///   <p>For more details, visit the <a href="https://www.sonarqube.org/">official website</a>.</p>
        /// </summary>
        /// <remarks>
        ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
        ///   <ul>
        ///     <li><c>/d:sonar.login</c> via <see cref="DotNetSonarScannerEndSettings.Login"/></li>
        ///     <li><c>/d:sonar.password</c> via <see cref="DotNetSonarScannerEndSettings.Password"/></li>
        ///   </ul>
        /// </remarks>
        public static IEnumerable<(DotNetSonarScannerEndSettings Settings, IReadOnlyCollection<Output> Output)> DotNetSonarScannerEnd(CombinatorialConfigure<DotNetSonarScannerEndSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
        {
            return configurator.Invoke(DotNetSonarScannerEnd, DotNetSonarScannerLogger, degreeOfParallelism, completeOnFailure);
        }
    }
    #region DotNetSonarScannerBeginSettings
    /// <summary>
    ///   Used within <see cref="DotNetSonarScannerTasks"/>.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class DotNetSonarScannerBeginSettings : ToolSettings
    {
        /// <summary>
        ///   Path to the DotNetSonarScanner executable.
        /// </summary>
        public override string ToolPath => base.ToolPath ?? DotNetSonarScannerTasks.DotNetSonarScannerPath;
        public override Action<OutputType, string> CustomLogger => DotNetSonarScannerTasks.DotNetSonarScannerLogger;
        /// <summary>
        ///   Specifies the key of the analyzed project in SonarQube.
        /// </summary>
        public virtual string ProjectKey { get; internal set; }
        /// <summary>
        ///   Specifies the name of the analyzed project in SonarQube. Adding this argument will overwrite the project name in SonarQube if it already exists.
        /// </summary>
        public virtual string Name { get; internal set; }
        /// <summary>
        ///   Specifies the SonarQube organization.
        /// </summary>
        public virtual string Organization { get; internal set; }
        /// <summary>
        ///   Specifies the version of your project.
        /// </summary>
        public virtual string Version { get; internal set; }
        /// <summary>
        ///   The project description.
        /// </summary>
        public virtual string Description { get; internal set; }
        /// <summary>
        ///   The server URL. Default is http://localhost:9000
        /// </summary>
        public virtual string Server { get; internal set; }
        /// <summary>
        ///   Specifies the username or access token to authenticate with to SonarQube. If this argument is added to the begin step, it must also be added on the end step.
        /// </summary>
        public virtual string Login { get; internal set; }
        /// <summary>
        ///   Specifies the password for the SonarQube username in the `sonar.login` argument. This argument is not needed if you use authentication token. If this argument is added to the begin step, it must also be added on the end step.
        /// </summary>
        public virtual string Password { get; internal set; }
        /// <summary>
        ///   Sets the logging verbosity to detailed. Add this argument before sending logs for troubleshooting.
        /// </summary>
        public virtual bool? Verbose { get; internal set; }
        /// <summary>
        ///   Comma separated list of VSTest report files to include.
        /// </summary>
        public virtual IReadOnlyList<string> VSTestReports => VSTestReportsInternal.AsReadOnly();
        internal List<string> VSTestReportsInternal { get; set; } = new List<string>();
        /// <summary>
        ///   Comma separated list of NUnit report files to include.
        /// </summary>
        public virtual IReadOnlyList<string> NUnitTestReports => NUnitTestReportsInternal.AsReadOnly();
        internal List<string> NUnitTestReportsInternal { get; set; } = new List<string>();
        /// <summary>
        ///   Comma separated list of xUnit report files to include.
        /// </summary>
        public virtual IReadOnlyList<string> XUnitTestReports => XUnitTestReportsInternal.AsReadOnly();
        internal List<string> XUnitTestReportsInternal { get; set; } = new List<string>();
        /// <summary>
        ///   Comma separated list of files to exclude from coverage calculations. Supports wildcards (*, **, ?).
        /// </summary>
        public virtual IReadOnlyList<string> CoverageExclusions => CoverageExclusionsInternal.AsReadOnly();
        internal List<string> CoverageExclusionsInternal { get; set; } = new List<string>();
        /// <summary>
        ///   Comma separated list of Visual Studio Code Coverage reports to include. Supports wildcards (*, **, ?).
        /// </summary>
        public virtual IReadOnlyList<string> VisualStudioCoveragePaths => VisualStudioCoveragePathsInternal.AsReadOnly();
        internal List<string> VisualStudioCoveragePathsInternal { get; set; } = new List<string>();
        /// <summary>
        ///   Comma separated list of dotCover HTML-reports to include. Supports wildcards (*, **, ?).
        /// </summary>
        public virtual IReadOnlyList<string> DotCoverPaths => DotCoverPathsInternal.AsReadOnly();
        internal List<string> DotCoverPathsInternal { get; set; } = new List<string>();
        /// <summary>
        ///   Comma separated list of OpenCover reports to include. Supports wildcards (*, **, ?).
        /// </summary>
        public virtual IReadOnlyList<string> OpenCoverPaths => OpenCoverPathsInternal.AsReadOnly();
        internal List<string> OpenCoverPathsInternal { get; set; } = new List<string>();
        /// <summary>
        ///   Maximum time to wait for the response of a Web Service call (in seconds). Modifying this value from the default is useful only when you're experiencing timeouts during analysis while waiting for the server to respond to Web Service calls.
        /// </summary>
        public virtual int? WebServiceTimeout { get; internal set; }
        /// <summary>
        ///   Project home page.
        /// </summary>
        public virtual string Homepage { get; internal set; }
        /// <summary>
        ///   Link to Continuous integration
        /// </summary>
        public virtual string ContinuousIntegrationUrl { get; internal set; }
        /// <summary>
        ///   Link to Issue tracker.
        /// </summary>
        public virtual string IssueTrackerUrl { get; internal set; }
        /// <summary>
        ///   Link to project source repository
        /// </summary>
        public virtual string SCMUrl { get; internal set; }
        /// <summary>
        ///   Encoding of the source files. Ex: UTF-8 , MacRoman , Shift_JIS . This property can be replaced by the standard property project.build.sourceEncoding in Maven projects. The list of available encodings depends on your JVM.
        /// </summary>
        public virtual string SourceEncoding { get; internal set; }
        /// <summary>
        ///   Comma-delimited list of file path patterns to be excluded from duplication detection.
        /// </summary>
        public virtual IReadOnlyList<string> DuplicationExclusions => DuplicationExclusionsInternal.AsReadOnly();
        internal List<string> DuplicationExclusionsInternal { get; set; } = new List<string>();
        protected override Arguments ConfigureArguments(Arguments arguments)
        {
            arguments
              .Add("sonarscanner begin")
              .Add("/k:{value}", ProjectKey)
              .Add("/n:{value}", Name)
              .Add("/o:{value}", Organization)
              .Add("/v:{value}", Version)
              .Add("/d:sonar.projectDescription={value}", Description)
              .Add("/d:sonar.host.url={value}", Server)
              .Add("/d:sonar.login={value}", Login, secret: true)
              .Add("/d:sonar.password={value}", Password, secret: true)
              .Add("/d:sonar.verbose={value}", Verbose)
              .Add("/d:sonar.cs.vstest.reportsPaths={value}", VSTestReports, separator: ',')
              .Add("/d:sonar.cs.nunit.reportsPaths={value}", NUnitTestReports, separator: ',')
              .Add("/d:sonar.cs.xunit.reportsPaths={value}", XUnitTestReports, separator: ',')
              .Add("/d:sonar.coverage.exclusions={value}", CoverageExclusions, separator: ',')
              .Add("/d:sonar.cs.vscoveragexml.reportsPaths={value}", VisualStudioCoveragePaths, separator: ',')
              .Add("/d:sonar.cs.dotcover.reportsPaths={value}", DotCoverPaths, separator: ',')
              .Add("/d:sonar.cs.opencover.reportsPaths={value}", OpenCoverPaths, separator: ',')
              .Add("/d:sonar.ws.timeout={value}", WebServiceTimeout)
              .Add("/d:sonar.links.homepage={value}", Homepage)
              .Add("/d:sonar.links.ci={value}", ContinuousIntegrationUrl)
              .Add("/d:sonar.links.issue={value}", IssueTrackerUrl)
              .Add("/d:sonar.links.scm={value}", SCMUrl)
              .Add("/d:sonar.sourceEncoding={value}", SourceEncoding)
              .Add("/d:sonar.cpd.exclusions={value}", DuplicationExclusions, separator: ',');
            return base.ConfigureArguments(arguments);
        }
    }
    #endregion
    #region DotNetSonarScannerEndSettings
    /// <summary>
    ///   Used within <see cref="DotNetSonarScannerTasks"/>.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    [Serializable]
    public partial class DotNetSonarScannerEndSettings : ToolSettings
    {
        /// <summary>
        ///   Path to the DotNetSonarScanner executable.
        /// </summary>
        public override string ToolPath => base.ToolPath ?? DotNetSonarScannerTasks.DotNetSonarScannerPath;
        public override Action<OutputType, string> CustomLogger => DotNetSonarScannerTasks.DotNetSonarScannerLogger;
        /// <summary>
        ///   Specifies the username or access token to authenticate with to SonarQube. If this argument is added to the begin step, it must also be added on the end step.
        /// </summary>
        public virtual string Login { get; internal set; }
        /// <summary>
        ///   Specifies the password for the SonarQube username in the `sonar.login` argument. This argument is not needed if you use authentication token. If this argument is added to the begin step, it must also be added on the end step.
        /// </summary>
        public virtual string Password { get; internal set; }
        protected override Arguments ConfigureArguments(Arguments arguments)
        {
            arguments
              .Add("sonarscanner end")
              .Add("/d:sonar.login={value}", Login, secret: true)
              .Add("/d:sonar.password={value}", Password, secret: true);
            return base.ConfigureArguments(arguments);
        }
    }
    #endregion
    #region DotNetSonarScannerBeginSettingsExtensions
    /// <summary>
    ///   Used within <see cref="DotNetSonarScannerTasks"/>.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class DotNetSonarScannerBeginSettingsExtensions
    {
        #region ProjectKey
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.ProjectKey"/></em></p>
        ///   <p>Specifies the key of the analyzed project in SonarQube.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetProjectKey(this DotNetSonarScannerBeginSettings toolSettings, string projectKey)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.ProjectKey = projectKey;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.ProjectKey"/></em></p>
        ///   <p>Specifies the key of the analyzed project in SonarQube.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetProjectKey(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.ProjectKey = null;
            return toolSettings;
        }
        #endregion
        #region Name
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.Name"/></em></p>
        ///   <p>Specifies the name of the analyzed project in SonarQube. Adding this argument will overwrite the project name in SonarQube if it already exists.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetName(this DotNetSonarScannerBeginSettings toolSettings, string name)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Name = name;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.Name"/></em></p>
        ///   <p>Specifies the name of the analyzed project in SonarQube. Adding this argument will overwrite the project name in SonarQube if it already exists.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetName(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Name = null;
            return toolSettings;
        }
        #endregion
        #region Organization
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.Organization"/></em></p>
        ///   <p>Specifies the SonarQube organization.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetOrganization(this DotNetSonarScannerBeginSettings toolSettings, string organization)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Organization = organization;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.Organization"/></em></p>
        ///   <p>Specifies the SonarQube organization.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetOrganization(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Organization = null;
            return toolSettings;
        }
        #endregion
        #region Version
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.Version"/></em></p>
        ///   <p>Specifies the version of your project.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetVersion(this DotNetSonarScannerBeginSettings toolSettings, string version)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Version = version;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.Version"/></em></p>
        ///   <p>Specifies the version of your project.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetVersion(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Version = null;
            return toolSettings;
        }
        #endregion
        #region Description
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.Description"/></em></p>
        ///   <p>The project description.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetDescription(this DotNetSonarScannerBeginSettings toolSettings, string description)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Description = description;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.Description"/></em></p>
        ///   <p>The project description.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetDescription(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Description = null;
            return toolSettings;
        }
        #endregion
        #region Server
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.Server"/></em></p>
        ///   <p>The server URL. Default is http://localhost:9000</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetServer(this DotNetSonarScannerBeginSettings toolSettings, string server)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Server = server;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.Server"/></em></p>
        ///   <p>The server URL. Default is http://localhost:9000</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetServer(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Server = null;
            return toolSettings;
        }
        #endregion
        #region Login
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.Login"/></em></p>
        ///   <p>Specifies the username or access token to authenticate with to SonarQube. If this argument is added to the begin step, it must also be added on the end step.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetLogin(this DotNetSonarScannerBeginSettings toolSettings, string login)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Login = login;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.Login"/></em></p>
        ///   <p>Specifies the username or access token to authenticate with to SonarQube. If this argument is added to the begin step, it must also be added on the end step.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetLogin(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Login = null;
            return toolSettings;
        }
        #endregion
        #region Password
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.Password"/></em></p>
        ///   <p>Specifies the password for the SonarQube username in the `sonar.login` argument. This argument is not needed if you use authentication token. If this argument is added to the begin step, it must also be added on the end step.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetPassword(this DotNetSonarScannerBeginSettings toolSettings, string password)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Password = password;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.Password"/></em></p>
        ///   <p>Specifies the password for the SonarQube username in the `sonar.login` argument. This argument is not needed if you use authentication token. If this argument is added to the begin step, it must also be added on the end step.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetPassword(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Password = null;
            return toolSettings;
        }
        #endregion
        #region Verbose
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.Verbose"/></em></p>
        ///   <p>Sets the logging verbosity to detailed. Add this argument before sending logs for troubleshooting.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetVerbose(this DotNetSonarScannerBeginSettings toolSettings, bool? verbose)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Verbose = verbose;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.Verbose"/></em></p>
        ///   <p>Sets the logging verbosity to detailed. Add this argument before sending logs for troubleshooting.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetVerbose(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Verbose = null;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Enables <see cref="DotNetSonarScannerBeginSettings.Verbose"/></em></p>
        ///   <p>Sets the logging verbosity to detailed. Add this argument before sending logs for troubleshooting.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings EnableVerbose(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Verbose = true;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Disables <see cref="DotNetSonarScannerBeginSettings.Verbose"/></em></p>
        ///   <p>Sets the logging verbosity to detailed. Add this argument before sending logs for troubleshooting.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings DisableVerbose(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Verbose = false;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Toggles <see cref="DotNetSonarScannerBeginSettings.Verbose"/></em></p>
        ///   <p>Sets the logging verbosity to detailed. Add this argument before sending logs for troubleshooting.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ToggleVerbose(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Verbose = !toolSettings.Verbose;
            return toolSettings;
        }
        #endregion
        #region VSTestReports
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.VSTestReports"/> to a new list</em></p>
        ///   <p>Comma separated list of VSTest report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetVSTestReports(this DotNetSonarScannerBeginSettings toolSettings, params string[] vstestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VSTestReportsInternal = vstestReports.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.VSTestReports"/> to a new list</em></p>
        ///   <p>Comma separated list of VSTest report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetVSTestReports(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> vstestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VSTestReportsInternal = vstestReports.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.VSTestReports"/></em></p>
        ///   <p>Comma separated list of VSTest report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddVSTestReports(this DotNetSonarScannerBeginSettings toolSettings, params string[] vstestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VSTestReportsInternal.AddRange(vstestReports);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.VSTestReports"/></em></p>
        ///   <p>Comma separated list of VSTest report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddVSTestReports(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> vstestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VSTestReportsInternal.AddRange(vstestReports);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Clears <see cref="DotNetSonarScannerBeginSettings.VSTestReports"/></em></p>
        ///   <p>Comma separated list of VSTest report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ClearVSTestReports(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VSTestReportsInternal.Clear();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.VSTestReports"/></em></p>
        ///   <p>Comma separated list of VSTest report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveVSTestReports(this DotNetSonarScannerBeginSettings toolSettings, params string[] vstestReports)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(vstestReports);
            toolSettings.VSTestReportsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.VSTestReports"/></em></p>
        ///   <p>Comma separated list of VSTest report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveVSTestReports(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> vstestReports)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(vstestReports);
            toolSettings.VSTestReportsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        #endregion
        #region NUnitTestReports
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.NUnitTestReports"/> to a new list</em></p>
        ///   <p>Comma separated list of NUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetNUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, params string[] nunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NUnitTestReportsInternal = nunitTestReports.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.NUnitTestReports"/> to a new list</em></p>
        ///   <p>Comma separated list of NUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetNUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> nunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NUnitTestReportsInternal = nunitTestReports.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.NUnitTestReports"/></em></p>
        ///   <p>Comma separated list of NUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddNUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, params string[] nunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NUnitTestReportsInternal.AddRange(nunitTestReports);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.NUnitTestReports"/></em></p>
        ///   <p>Comma separated list of NUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddNUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> nunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NUnitTestReportsInternal.AddRange(nunitTestReports);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Clears <see cref="DotNetSonarScannerBeginSettings.NUnitTestReports"/></em></p>
        ///   <p>Comma separated list of NUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ClearNUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.NUnitTestReportsInternal.Clear();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.NUnitTestReports"/></em></p>
        ///   <p>Comma separated list of NUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveNUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, params string[] nunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(nunitTestReports);
            toolSettings.NUnitTestReportsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.NUnitTestReports"/></em></p>
        ///   <p>Comma separated list of NUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveNUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> nunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(nunitTestReports);
            toolSettings.NUnitTestReportsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        #endregion
        #region XUnitTestReports
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.XUnitTestReports"/> to a new list</em></p>
        ///   <p>Comma separated list of xUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetXUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, params string[] xunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.XUnitTestReportsInternal = xunitTestReports.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.XUnitTestReports"/> to a new list</em></p>
        ///   <p>Comma separated list of xUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetXUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> xunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.XUnitTestReportsInternal = xunitTestReports.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.XUnitTestReports"/></em></p>
        ///   <p>Comma separated list of xUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddXUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, params string[] xunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.XUnitTestReportsInternal.AddRange(xunitTestReports);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.XUnitTestReports"/></em></p>
        ///   <p>Comma separated list of xUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddXUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> xunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.XUnitTestReportsInternal.AddRange(xunitTestReports);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Clears <see cref="DotNetSonarScannerBeginSettings.XUnitTestReports"/></em></p>
        ///   <p>Comma separated list of xUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ClearXUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.XUnitTestReportsInternal.Clear();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.XUnitTestReports"/></em></p>
        ///   <p>Comma separated list of xUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveXUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, params string[] xunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(xunitTestReports);
            toolSettings.XUnitTestReportsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.XUnitTestReports"/></em></p>
        ///   <p>Comma separated list of xUnit report files to include.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveXUnitTestReports(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> xunitTestReports)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(xunitTestReports);
            toolSettings.XUnitTestReportsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        #endregion
        #region CoverageExclusions
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.CoverageExclusions"/> to a new list</em></p>
        ///   <p>Comma separated list of files to exclude from coverage calculations. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetCoverageExclusions(this DotNetSonarScannerBeginSettings toolSettings, params string[] coverageExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.CoverageExclusionsInternal = coverageExclusions.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.CoverageExclusions"/> to a new list</em></p>
        ///   <p>Comma separated list of files to exclude from coverage calculations. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetCoverageExclusions(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> coverageExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.CoverageExclusionsInternal = coverageExclusions.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.CoverageExclusions"/></em></p>
        ///   <p>Comma separated list of files to exclude from coverage calculations. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddCoverageExclusions(this DotNetSonarScannerBeginSettings toolSettings, params string[] coverageExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.CoverageExclusionsInternal.AddRange(coverageExclusions);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.CoverageExclusions"/></em></p>
        ///   <p>Comma separated list of files to exclude from coverage calculations. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddCoverageExclusions(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> coverageExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.CoverageExclusionsInternal.AddRange(coverageExclusions);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Clears <see cref="DotNetSonarScannerBeginSettings.CoverageExclusions"/></em></p>
        ///   <p>Comma separated list of files to exclude from coverage calculations. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ClearCoverageExclusions(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.CoverageExclusionsInternal.Clear();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.CoverageExclusions"/></em></p>
        ///   <p>Comma separated list of files to exclude from coverage calculations. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveCoverageExclusions(this DotNetSonarScannerBeginSettings toolSettings, params string[] coverageExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(coverageExclusions);
            toolSettings.CoverageExclusionsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.CoverageExclusions"/></em></p>
        ///   <p>Comma separated list of files to exclude from coverage calculations. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveCoverageExclusions(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> coverageExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(coverageExclusions);
            toolSettings.CoverageExclusionsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        #endregion
        #region VisualStudioCoveragePaths
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.VisualStudioCoveragePaths"/> to a new list</em></p>
        ///   <p>Comma separated list of Visual Studio Code Coverage reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetVisualStudioCoveragePaths(this DotNetSonarScannerBeginSettings toolSettings, params string[] visualStudioCoveragePaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VisualStudioCoveragePathsInternal = visualStudioCoveragePaths.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.VisualStudioCoveragePaths"/> to a new list</em></p>
        ///   <p>Comma separated list of Visual Studio Code Coverage reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetVisualStudioCoveragePaths(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> visualStudioCoveragePaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VisualStudioCoveragePathsInternal = visualStudioCoveragePaths.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.VisualStudioCoveragePaths"/></em></p>
        ///   <p>Comma separated list of Visual Studio Code Coverage reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddVisualStudioCoveragePaths(this DotNetSonarScannerBeginSettings toolSettings, params string[] visualStudioCoveragePaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VisualStudioCoveragePathsInternal.AddRange(visualStudioCoveragePaths);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.VisualStudioCoveragePaths"/></em></p>
        ///   <p>Comma separated list of Visual Studio Code Coverage reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddVisualStudioCoveragePaths(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> visualStudioCoveragePaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VisualStudioCoveragePathsInternal.AddRange(visualStudioCoveragePaths);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Clears <see cref="DotNetSonarScannerBeginSettings.VisualStudioCoveragePaths"/></em></p>
        ///   <p>Comma separated list of Visual Studio Code Coverage reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ClearVisualStudioCoveragePaths(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.VisualStudioCoveragePathsInternal.Clear();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.VisualStudioCoveragePaths"/></em></p>
        ///   <p>Comma separated list of Visual Studio Code Coverage reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveVisualStudioCoveragePaths(this DotNetSonarScannerBeginSettings toolSettings, params string[] visualStudioCoveragePaths)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(visualStudioCoveragePaths);
            toolSettings.VisualStudioCoveragePathsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.VisualStudioCoveragePaths"/></em></p>
        ///   <p>Comma separated list of Visual Studio Code Coverage reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveVisualStudioCoveragePaths(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> visualStudioCoveragePaths)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(visualStudioCoveragePaths);
            toolSettings.VisualStudioCoveragePathsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        #endregion
        #region DotCoverPaths
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.DotCoverPaths"/> to a new list</em></p>
        ///   <p>Comma separated list of dotCover HTML-reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetDotCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, params string[] dotCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DotCoverPathsInternal = dotCoverPaths.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.DotCoverPaths"/> to a new list</em></p>
        ///   <p>Comma separated list of dotCover HTML-reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetDotCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> dotCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DotCoverPathsInternal = dotCoverPaths.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.DotCoverPaths"/></em></p>
        ///   <p>Comma separated list of dotCover HTML-reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddDotCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, params string[] dotCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DotCoverPathsInternal.AddRange(dotCoverPaths);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.DotCoverPaths"/></em></p>
        ///   <p>Comma separated list of dotCover HTML-reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddDotCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> dotCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DotCoverPathsInternal.AddRange(dotCoverPaths);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Clears <see cref="DotNetSonarScannerBeginSettings.DotCoverPaths"/></em></p>
        ///   <p>Comma separated list of dotCover HTML-reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ClearDotCoverPaths(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DotCoverPathsInternal.Clear();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.DotCoverPaths"/></em></p>
        ///   <p>Comma separated list of dotCover HTML-reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveDotCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, params string[] dotCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(dotCoverPaths);
            toolSettings.DotCoverPathsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.DotCoverPaths"/></em></p>
        ///   <p>Comma separated list of dotCover HTML-reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveDotCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> dotCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(dotCoverPaths);
            toolSettings.DotCoverPathsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        #endregion
        #region OpenCoverPaths
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.OpenCoverPaths"/> to a new list</em></p>
        ///   <p>Comma separated list of OpenCover reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetOpenCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, params string[] openCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.OpenCoverPathsInternal = openCoverPaths.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.OpenCoverPaths"/> to a new list</em></p>
        ///   <p>Comma separated list of OpenCover reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetOpenCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> openCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.OpenCoverPathsInternal = openCoverPaths.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.OpenCoverPaths"/></em></p>
        ///   <p>Comma separated list of OpenCover reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddOpenCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, params string[] openCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.OpenCoverPathsInternal.AddRange(openCoverPaths);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.OpenCoverPaths"/></em></p>
        ///   <p>Comma separated list of OpenCover reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddOpenCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> openCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.OpenCoverPathsInternal.AddRange(openCoverPaths);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Clears <see cref="DotNetSonarScannerBeginSettings.OpenCoverPaths"/></em></p>
        ///   <p>Comma separated list of OpenCover reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ClearOpenCoverPaths(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.OpenCoverPathsInternal.Clear();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.OpenCoverPaths"/></em></p>
        ///   <p>Comma separated list of OpenCover reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveOpenCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, params string[] openCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(openCoverPaths);
            toolSettings.OpenCoverPathsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.OpenCoverPaths"/></em></p>
        ///   <p>Comma separated list of OpenCover reports to include. Supports wildcards (*, **, ?).</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveOpenCoverPaths(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> openCoverPaths)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(openCoverPaths);
            toolSettings.OpenCoverPathsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        #endregion
        #region WebServiceTimeout
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.WebServiceTimeout"/></em></p>
        ///   <p>Maximum time to wait for the response of a Web Service call (in seconds). Modifying this value from the default is useful only when you're experiencing timeouts during analysis while waiting for the server to respond to Web Service calls.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetWebServiceTimeout(this DotNetSonarScannerBeginSettings toolSettings, int? webServiceTimeout)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.WebServiceTimeout = webServiceTimeout;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.WebServiceTimeout"/></em></p>
        ///   <p>Maximum time to wait for the response of a Web Service call (in seconds). Modifying this value from the default is useful only when you're experiencing timeouts during analysis while waiting for the server to respond to Web Service calls.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetWebServiceTimeout(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.WebServiceTimeout = null;
            return toolSettings;
        }
        #endregion
        #region Homepage
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.Homepage"/></em></p>
        ///   <p>Project home page.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetHomepage(this DotNetSonarScannerBeginSettings toolSettings, string homepage)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Homepage = homepage;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.Homepage"/></em></p>
        ///   <p>Project home page.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetHomepage(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Homepage = null;
            return toolSettings;
        }
        #endregion
        #region ContinuousIntegrationUrl
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.ContinuousIntegrationUrl"/></em></p>
        ///   <p>Link to Continuous integration</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetContinuousIntegrationUrl(this DotNetSonarScannerBeginSettings toolSettings, string continuousIntegrationUrl)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.ContinuousIntegrationUrl = continuousIntegrationUrl;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.ContinuousIntegrationUrl"/></em></p>
        ///   <p>Link to Continuous integration</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetContinuousIntegrationUrl(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.ContinuousIntegrationUrl = null;
            return toolSettings;
        }
        #endregion
        #region IssueTrackerUrl
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.IssueTrackerUrl"/></em></p>
        ///   <p>Link to Issue tracker.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetIssueTrackerUrl(this DotNetSonarScannerBeginSettings toolSettings, string issueTrackerUrl)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.IssueTrackerUrl = issueTrackerUrl;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.IssueTrackerUrl"/></em></p>
        ///   <p>Link to Issue tracker.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetIssueTrackerUrl(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.IssueTrackerUrl = null;
            return toolSettings;
        }
        #endregion
        #region SCMUrl
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.SCMUrl"/></em></p>
        ///   <p>Link to project source repository</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetSCMUrl(this DotNetSonarScannerBeginSettings toolSettings, string scmurl)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.SCMUrl = scmurl;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.SCMUrl"/></em></p>
        ///   <p>Link to project source repository</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetSCMUrl(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.SCMUrl = null;
            return toolSettings;
        }
        #endregion
        #region SourceEncoding
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.SourceEncoding"/></em></p>
        ///   <p>Encoding of the source files. Ex: UTF-8 , MacRoman , Shift_JIS . This property can be replaced by the standard property project.build.sourceEncoding in Maven projects. The list of available encodings depends on your JVM.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetSourceEncoding(this DotNetSonarScannerBeginSettings toolSettings, string sourceEncoding)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.SourceEncoding = sourceEncoding;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerBeginSettings.SourceEncoding"/></em></p>
        ///   <p>Encoding of the source files. Ex: UTF-8 , MacRoman , Shift_JIS . This property can be replaced by the standard property project.build.sourceEncoding in Maven projects. The list of available encodings depends on your JVM.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ResetSourceEncoding(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.SourceEncoding = null;
            return toolSettings;
        }
        #endregion
        #region DuplicationExclusions
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.DuplicationExclusions"/> to a new list</em></p>
        ///   <p>Comma-delimited list of file path patterns to be excluded from duplication detection.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetDuplicationExclusions(this DotNetSonarScannerBeginSettings toolSettings, params string[] duplicationExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DuplicationExclusionsInternal = duplicationExclusions.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerBeginSettings.DuplicationExclusions"/> to a new list</em></p>
        ///   <p>Comma-delimited list of file path patterns to be excluded from duplication detection.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings SetDuplicationExclusions(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> duplicationExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DuplicationExclusionsInternal = duplicationExclusions.ToList();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.DuplicationExclusions"/></em></p>
        ///   <p>Comma-delimited list of file path patterns to be excluded from duplication detection.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddDuplicationExclusions(this DotNetSonarScannerBeginSettings toolSettings, params string[] duplicationExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DuplicationExclusionsInternal.AddRange(duplicationExclusions);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Adds values to <see cref="DotNetSonarScannerBeginSettings.DuplicationExclusions"/></em></p>
        ///   <p>Comma-delimited list of file path patterns to be excluded from duplication detection.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings AddDuplicationExclusions(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> duplicationExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DuplicationExclusionsInternal.AddRange(duplicationExclusions);
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Clears <see cref="DotNetSonarScannerBeginSettings.DuplicationExclusions"/></em></p>
        ///   <p>Comma-delimited list of file path patterns to be excluded from duplication detection.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings ClearDuplicationExclusions(this DotNetSonarScannerBeginSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.DuplicationExclusionsInternal.Clear();
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.DuplicationExclusions"/></em></p>
        ///   <p>Comma-delimited list of file path patterns to be excluded from duplication detection.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveDuplicationExclusions(this DotNetSonarScannerBeginSettings toolSettings, params string[] duplicationExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(duplicationExclusions);
            toolSettings.DuplicationExclusionsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Removes values from <see cref="DotNetSonarScannerBeginSettings.DuplicationExclusions"/></em></p>
        ///   <p>Comma-delimited list of file path patterns to be excluded from duplication detection.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerBeginSettings RemoveDuplicationExclusions(this DotNetSonarScannerBeginSettings toolSettings, IEnumerable<string> duplicationExclusions)
        {
            toolSettings = toolSettings.NewInstance();
            var hashSet = new HashSet<string>(duplicationExclusions);
            toolSettings.DuplicationExclusionsInternal.RemoveAll(x => hashSet.Contains(x));
            return toolSettings;
        }
        #endregion
    }
    #endregion
    #region DotNetSonarScannerEndSettingsExtensions
    /// <summary>
    ///   Used within <see cref="DotNetSonarScannerTasks"/>.
    /// </summary>
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static partial class DotNetSonarScannerEndSettingsExtensions
    {
        #region Login
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerEndSettings.Login"/></em></p>
        ///   <p>Specifies the username or access token to authenticate with to SonarQube. If this argument is added to the begin step, it must also be added on the end step.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerEndSettings SetLogin(this DotNetSonarScannerEndSettings toolSettings, string login)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Login = login;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerEndSettings.Login"/></em></p>
        ///   <p>Specifies the username or access token to authenticate with to SonarQube. If this argument is added to the begin step, it must also be added on the end step.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerEndSettings ResetLogin(this DotNetSonarScannerEndSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Login = null;
            return toolSettings;
        }
        #endregion
        #region Password
        /// <summary>
        ///   <p><em>Sets <see cref="DotNetSonarScannerEndSettings.Password"/></em></p>
        ///   <p>Specifies the password for the SonarQube username in the `sonar.login` argument. This argument is not needed if you use authentication token. If this argument is added to the begin step, it must also be added on the end step.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerEndSettings SetPassword(this DotNetSonarScannerEndSettings toolSettings, string password)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Password = password;
            return toolSettings;
        }
        /// <summary>
        ///   <p><em>Resets <see cref="DotNetSonarScannerEndSettings.Password"/></em></p>
        ///   <p>Specifies the password for the SonarQube username in the `sonar.login` argument. This argument is not needed if you use authentication token. If this argument is added to the begin step, it must also be added on the end step.</p>
        /// </summary>
        [Pure]
        public static DotNetSonarScannerEndSettings ResetPassword(this DotNetSonarScannerEndSettings toolSettings)
        {
            toolSettings = toolSettings.NewInstance();
            toolSettings.Password = null;
            return toolSettings;
        }
        #endregion
    }
    #endregion
}
