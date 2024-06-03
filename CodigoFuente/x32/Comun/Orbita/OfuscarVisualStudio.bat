@echo off

set Compilacion=%1
set Ruta=%2
set Proyecto=%3
set Extension=%4

echo %Extension%

IF NOT "%Compilacion%"=="Ofuscado" GOTO FIN
title OBFUSCATING FILES
echo -- OBFUSCATING FILES ------------------------------------------ 

cd %Ruta%
del ..\..\..\..\..\General\Dlls\x32\Orbita\%Proyecto%%Extension% /q/f
SmartAssembly.com /build .\%Proyecto%32.saproj
verpatch ..\..\..\..\..\General\Dlls\x32\Orbita\%Proyecto%%Extension% /s company "Orbita Software" /s copyright "Copyright © Orbita Ingenieria SW 2012"

title OBFUSCATION FINISHED
echo -- OBFUSCATION FINISHED --------------------------------------- 
:FIN
