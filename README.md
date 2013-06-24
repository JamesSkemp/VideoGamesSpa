Video Games SPA
=============

SPA (single page application) for tracking video games.

Powered by AngularJS and unofficial PlayStation Network (PSN) and Xbox Live (XBL) APIs, since there are no official ones we can use.

While I use Visual Studio for development, the goal of this project is so that anyone can drop it onto a site, after generating the data it uses.

Getting started
===

Open the template (/Default.html) and update/remove the Google Analytics account, as needed.

You'll also want to update the home page language, in /app/partials/home.html, but please consider leaving the link back to this GitHub project page in some way.

Missing features / TODOs
===

At this point the queries to generate the JSON necessary are unavailable (since they're a dozen LINQPad scripts).

However, I'm starting to work on making them live (as part of this project) and you can find the wrappers I've created for the unofficial APIs on these project pages:

* Wrapper (.NET) for psnapi.com.ar : https://github.com/JamesSkemp/PsnApiArWrapper
* Wrapper (.NET) for XboxLeaders.com : https://github.com/JamesSkemp/XboxLeadersWrapper
* Wrapper (.NET) for XboxApi.com : https://github.com/JamesSkemp/XboxApiWrapper
