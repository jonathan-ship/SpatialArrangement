@echo off
REM cd Assembly
svn add *.dll
svn add *.sql
svn commit -m "Auto Committed: EOBA Development Common Assemblies"
REM cd ..
REM cd Schema
REM svn add *
REM svn commit -m "Auto Committed: EOBA Development Common Schema"
echo on