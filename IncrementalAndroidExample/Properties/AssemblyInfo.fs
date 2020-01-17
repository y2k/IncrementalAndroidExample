namespace IncrementalAndroidExample

open System.Reflection
open System.Runtime.CompilerServices
open Android.App

// the name of the type here needs to match the name inside the ResourceDesigner attribute
// type Resources = IncrementalAndroidExample.Resource

[<assembly:Android.Runtime.ResourceDesigner("IncrementalAndroidExample.Resources", IsApplication = true)>]
[<assembly:AssemblyTitle("IncrementalAndroidExample")>]
[<assembly:AssemblyDescription("")>]
[<assembly:AssemblyConfiguration("")>]
[<assembly:AssemblyCompany("")>]
[<assembly:AssemblyProduct("")>]
[<assembly:AssemblyCopyright("")>]
[<assembly:AssemblyTrademark("")>]
[<assembly:AssemblyCulture("")>]
[<assembly:AssemblyVersion("1.0.0")>]

// The following attributes are used to specify the signing key for the assembly,
// if desired. See the Mono documentation for more information about signing.

//[<assembly: AssemblyDelaySign(false)>]
//[<assembly: AssemblyKeyFile("")>]

()
