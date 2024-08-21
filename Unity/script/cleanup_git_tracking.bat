@echo off
echo Removing cached files...
git rm -r --cached .plastic/
git rm -r --cached Logs/
git rm -r --cached Library/
git rm -r --cached UserSettings/
git rm -r --cached Temp/
git rm -r --cached *.log
git rm -r --cached *.dwlt
git commit -m "Removed Logs, Library, and other ignored files from tracking"
echo Cleanup complete! Please push your changes.