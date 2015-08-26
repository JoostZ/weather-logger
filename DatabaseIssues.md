# Introduction #

This page is a collection of issues found during the implementation of the Data Base support.


# SQL Server Compact 4.0 #

I want to use SQL Server Compact for the database. This provides the possibility to use a DB without the need of a server running.

VS 2010 express came with support for SSCE 3.5 which, I assume, has support for NET 3.5. To make full use of the Entity Framework 4.0 I think I need to upgrade to SSCE 4.0. (I did find at least one issue with 3.5: it is not possible to use auto generated keys).

Fortunately SP1 for Visual Studio has support for SSCE 4.0.