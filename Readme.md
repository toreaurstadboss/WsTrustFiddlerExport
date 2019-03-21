FiddlerCustomWebTestExport - A custom export plugin for Fiddler to target Microsoft Visual Web Test and Load testing framework
-------------------------------------------------------------------------------------------------------------------------------

This repository contains a customized export plug-in for Visual Studio Web Testing and Load Framwork forresponses / sessions captured in Fiddler that exchanges the values 
in a request with a WS-Trust SAML token with key values that can be exchanged with values of your choice. In my business case, I 
call a STS to retrieve fresh security token, which a web test plugin will exchange as web test Context variable values before running the web test itself. This makes it possible 
to re-run the web test without an expired security token.This makes the tests durable.

Last update,
Tore Aurstad | tore.aurstad@ntebb.no | Twitter: @Tore_Aurstad
02.05.2018 