set ROOT=..\..\Build\web\handymd\

copy /Y app.yaml %ROOT%
copy /Y favicon.ico %ROOT%
copy /Y handymd.html %ROOT%

appcfg.py update %ROOT%

pause
