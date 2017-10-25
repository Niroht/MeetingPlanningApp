# MeetingPlanningApp

This application was built in Visual Studio Community 2017 using the .NET framework. To run, either run within Visual Studio, or build the application and run the MeetingPlanningApp executable from the build directory ({Solution Directory}/MeetingPlanningApp/Bin/{Build type})

This solution uses several existing frameworks such as Unity and MVVMLight which should automatically be downloaded using NuGet package manager on first build. In the event the NuGet packages do not download and install automatically, open NuGet package manager (Tools -> NuGet Package Manager -> Manage NuGet Packages For Solution...) and click the "Restore" button on the notification that appears.

## Testing
This application uses the NUnit framework for its unit tests. Running them requires the NUnit test adapter (https://marketplace.visualstudio.com/items?itemName=NUnitDevelopers.NUnit3TestAdapter), Resharper, or another extension that detects NUnit tests.
