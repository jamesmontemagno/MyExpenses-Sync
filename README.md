My Expenses
==========

My Expenses Cross Platform Demo from TechEd Europe 2014. This is a new Azure Mobile Serive + SQLite Offline Online sync that was based off the original [MyExpenses Application that I built on Channel 9](http://www.github.com/jamesmontemagno/myexpenses)

Videos are available on Channel 9:

[Part 1: Cross Platform Mobile Development with Xamarin](https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Cross-Platform-Development-With-Xamarin?WT.mc_id=friends-0000-jamont)

[Part 2: Using Portable Class Libraries with Xamarin](https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Using-Portable-Class-Libraries-with-Xamarin?WT.mc_id=friends-0000-jamont)

Expense taking cross platform application for Windows Phone, Android, and iOS built with Xamarin inside of Visual Studio 2013. Expenses are stored locally in a Sqlite-net database. You can add new expenses and edit or delete existing. All business logic is shared in one portable class library.

Written in C# with ([Xamarin](http://www.xamarin.com))  **Created in Visual Studio 2013**

Open Source Project by ([@JamesMontemagno](http://www.twitter.com/jamesmontemagno)) 

For Windows Phone you must install SQLite for Windows Phone Extension: https://visualstudiogallery.msdn.microsoft.com/cd120b42-30f4-446e-8287-45387a4f40b7?WT.mc_id=friends-0000-jamont

** For Azure Mobile Services Integration please read the setup at the bottom of this page! **

## How much code is shared?
I have included an "Analysis Project", which will count the shared lines of code. Up to 80% of code is shared across platforms. All of the Models, Services, View Models, and tons of helper classes are all found in one single PCL library. 

## What technology is used?
Everything is written in C# with Xamarin with a base PCL library. This project couldn't have been done without the following:

### Json.NET
https://components.xamarin.com/view/json.net - I use both the NuGet in the PCL and component for iOS for facade linking. One of the most wonderful Json libraries that I simply love. It is used to deserialize all information coming from the meetup.com APIs.

### HTTP Client Libraries
https://www.nuget.org/packages/Microsoft.Net.Http - Brings HTTP Client functionality to Windows Phone in PCL.

### Windows Phone Toolkit
http://phone.codeplex.com/ - Everyone's favorite WP toolkit!

### ANDHud
https://components.xamarin.com/view/AndHUD - Brings in a nice spinner for Xamarin.Android

### BTProgressHud
https://components.xamarin.com/view/btprogresshud - Great spinner for iOS

### MonoTouch.Dialog
http://docs.xamarin.com/guides/ios/user_interface/monotouch.dialog/ - A wonderful library for Xamarin.iOS to create user interfaces quick with not a lot of code.


### Azure Mobile Services Integration

* Create a new Azure Mobile Services Table Called "Expense"
* Follow this guide to setup the Table for Authentication: https://www.windowsazure.com/develop/mobile/tutorials/get-started-with-users-dotnet/?WT.mc_id=friends-0000-jamont

* Setup Azure Scripts for Insert, Read, Update: https://www.windowsazure.com/develop/mobile/tutorials/authorize-users-in-scripts-dotnet/?WT.mc_id=friends-0000-jamont

* Setup Twitter App for Authentication: http://www.dotnetcurry.com/ShowArticle.aspx?ID=860

* Optionally you can setup Facebook, Microsoft, or Google, however the sample is setup for Twitter

* Open "AzureService.cs" a shared file in MyEpenses.Android (or iOS or WindowsPhone) and 
* Comment Back In & Edit: MobileClient = new MobileServiceClient(
        
"https://"+"PUT-SITE-HERE" +".azure-mobile.net/",
        
"PUT-YOUR-API-KEY-HERE");

* This information can be found on Azure
        

## License

//The MIT License (MIT)
//Copyright (c) 2015 Xamarin
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//    FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//    COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//    IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//    CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

