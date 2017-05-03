# Badges

[![Build status](https://ci.appveyor.com/api/projects/status/hnjj82rhfae58yue/branch/master?svg=true)](https://ci.appveyor.com/project/kungfux/business-accounting/branch/master)

# Business Accounting Overview

Business Accounting is a standalone desktop application written on C#/WPF that aims for the accounting of funds for small businesses. All data are being stored locally using SQLite database.

## Features

* Keep track of income and expenses;
* Manage employees card files;
* Keep track of salary;
* Build charts.

## Screenshots

![Screenshot1](https://cloud.githubusercontent.com/assets/10548881/25675193/41c852f8-3046-11e7-8dfb-35ed550ee9c2.png)
![Screenshot2](https://cloud.githubusercontent.com/assets/10548881/25675189/3d46183c-3046-11e7-99e4-e4d850deabba.png)
![Screenshot3](https://cloud.githubusercontent.com/assets/10548881/25675207/49457f10-3046-11e7-9c86-0901bc8c01e9.png)
![Screenshot4](https://cloud.githubusercontent.com/assets/10548881/25675213/4b64e362-3046-11e7-8056-4da16bc4dcaa.png)

## Configuration

It is possible to configure the location of database file and default dates for the Date input controls via editing Business Accounting.exe.config file.

![Screenshot5](https://cloud.githubusercontent.com/assets/10548881/25675219/4d9a0d38-3046-11e7-9a20-06de993f4428.png)

## Installation instructions

Pre-compiled binaries and installation wizard may be found [here](https://github.com/kungfux/business-accounting/releases)

In order to build from scratch, it is needed to build the project in usual C# way and make new database using .bat [script](https://github.com/kungfux/business-accounting/blob/master/Database/makefile.bat)
