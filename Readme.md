# WSTrustFiddlerWebTestExport

![alt text](labflask.png)



#### A custom export plugin for Fiddler to target Microsoft Visual Web Test and Load testing framework that provides durable Load and Web Tests against federated environments.
-------------------------------------------------------------------------------------------------------------------------------

This version targets Fiddler version *5.0.20182.28034* or similar (5.0.x or perhaps newer versions, I have not tested this out). It might work with similar versions to 5.0.x or newer, bundled with this source code is 
the Fiddler.exe file in the Fiddler folder of this repository.

This repository contains a customized export plug-in for Visual Studio Web Testing and Load Framwork for http(s) requests and their responses also known
in Fiddler as "sessions". These sessions captured in Fiddler is transformed - where the export pluging exchanges the values 
of the request with a parameterized WS-Trust SAML token. This security token contains key values that can be exchanged with values of your choice from the 
STS you are using in your business case (STS - Security Token Service).

In my business case, I call a STS first to retrieve fresh security token, which a web test plugin will exchange as web test Context variable values 
before running the web test itself. This makes it possible to re-run the web test without an expired security token.This makes the tests durable.

IMHO, this provides important functionality for Visual Studio Web Tests and Load Tests, in that you can do a recording of HTTP(S) requests and get 
a parameterized SAML token presentation that we can inject fresh SAML token parameters into.

Also note that this plugin targets the WS-Trust standard, if you want to support WS-Federation - you can freely fork this repository and create your own
parameterization mechanism.

I have started up with the original Fiddler Web Test export plugin, before I worked with this repository - 
so the export plugin is similar to the Fiddler Web Test export plugin. The initial code was worked with in 
May 2018, testetd with Fiddler v5.0. 

If you want to work with this code, you can contact with me at Twitter for example. Pull requests are welcome, especially if you are interested in working with parameterizing 
WS-Federation sessions in Fiddler for durable Web Tests and Load tests - please contact me. 

-------------------------------------------------------------------------------------------------------------------------------

Last update,
Tore Aurstad | tore.aurstad@gmail.com | Twitter: @Tore_Aurstad
21.03.2019 