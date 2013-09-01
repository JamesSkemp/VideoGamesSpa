Video Games SPA
=============

SPA (single page application) for tracking video games.

Powered by AngularJS and unofficial PlayStation Network (PSN) and Xbox Live (XBL) APIs, since there are no official ones we can use.

While I use Visual Studio for development, the goal of this project is so that anyone can drop it onto a site, after generating the data it uses.

Getting started
===

Open the template (/Default.html) and update/remove the Google Analytics account, as needed.

You'll also want to update the home page language, in /app/partials/home.html, but please consider leaving the link back to this GitHub project page in some way.

Generating the necessary data
===

After updating the template to your taste, you'll need to actually generate your own data.

First, you'll need one or more of the following wrappers.

* Wrapper (.NET) for unofficial PSN API : https://github.com/JamesSkemp/PsnWrapper
* Wrapper (.NET) for XboxLeaders.com : https://github.com/JamesSkemp/XboxLeadersWrapper
* Wrapper (.NET) for XboxApi.com : https://github.com/JamesSkemp/XboxApiWrapper

Follow the instructions for whichever one(s) you select to pull data from the APIs.

Once you've done that you can use the included library to compile the data. An example is below:

	// Setup our spa.
	var spa = new Spa();
	spa.SpaDirectory = @"C:\Users\James\Desktop\Video games\";
	// PlayStation Network
	var psnGenerator = new VideoGamesSpa.ApiParser.PsnWrapper.Generator();
	psnGenerator.ApiOutputDirectory = @"C:\Users\James\Projects\GitHub\VideoGamesSpa\_output\strivinglife\psnwrapper\";
	psnGenerator.XmlNameFormat = "{0}";
	// Xbox Live option 1
	var xblApiGenerator = new VideoGamesSpa.ApiParser.XboxApi.Generator();
	xblApiGenerator.ApiOutputDirectory = @"C:\Users\James\Projects\GitHub\VideoGamesSpa\_output\strivinglife\xboxapi\";
	xblApiGenerator.XmlNameFormat = "{0}";
	// Optional
	xblApiGenerator.HiddenAchievementsDirectory = @"C:\Users\James\Projects\GitHub\XblAchievements\";
	// Optional
	xblApiGenerator.OfflineAchievementsXmlPath = @"C:\Users\James\Projects\GitHub\VideoGamesSpa\OfflineAchievements.xml";
	// Xbox Live option 2
	var xblLeadersGenerator = new VideoGamesSpa.ApiParser.XboxLeaders.Generator();
	xblLeadersGenerator.ApiOutputDirectory = @"C:\Users\James\Projects\GitHub\VideoGamesSpa\_output\strivinglife\xboxleaders\";
	xblLeadersGenerator.XmlNameFormat = "{0}";
	// Optional
	xblLeadersGenerator.HiddenAchievementsDirectory = @"C:\Users\James\Projects\GitHub\XblAchievements\";
	// Optional
	xblLeadersGenerator.OfflineAchievementsXmlPath = @"C:\Users\James\Projects\GitHub\VideoGamesSpa\OfflineAchievements.xml";
	
	// Add the following PlayStation Network generator, if applicable.
	spa.Generators.Add(psnGenerator);
	// Add one of the following Xbox Live generators, if applicable.
	spa.Generators.Add(xblApiGenerator);
	//spa.Generators.Add(xblLeadersGenerator);
	
	spa.GenerateAll();

Additional querying
===

While you use the Spa object to query against, you can also parse the output files after compiling and then run any queries you'd like.

For example:

	// Setup our data parser.
	var spaData = new SpaData();
	// Set where compiled files have been saved to.
	spaData.SpaDirectory = @"C:\Users\James\Desktop\Video games\";
	// Parse the XML files saved in the directory and populate the model.
	spaData.LoadData();
	
	// Example query: get all unearned achievements that are worth more than 0, but less than or equal to 20, gamerscore.
	var possibleAchievements = spaData.XblAchievements
		.Where (a => !a.Earned.HasValue)
		.Select (a => new { a.Title, a.Description, a.GameTitle, Gamerscore = int.Parse(a.GamerScore) })
		.Where (a => a.Gamerscore > 0 && a.Gamerscore <= 20)
		.Dump();

