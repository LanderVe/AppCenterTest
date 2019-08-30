#!/usr/bin/env bash
#
# For Xamarin, change some constants located in some class of the app.
# In this sample, suppose we have an AppConstant.cs class in shared folder with follow content:
#
# namespace AppCenterTest
# {
#     public class AppConstant
#     {
#         public const string ApiUrl = "https://CMS_MyApp-Eur01.com/api";
#     }
# }
# 
# Suppose in our project exists two branches: master and develop. 
# We can release app for production API in master branch and app for test API in develop branch. 
# We just need configure this behaviour with environment variable in each branch :)
# 
# The same thing can be perform with any class of the app.
#
# AN IMPORTANT THING: FOR THIS SAMPLE YOU NEED DECLARE API_URL ENVIRONMENT VARIABLE IN APP CENTER BUILD CONFIGURATION.

# Check if APP_SECRET is defined in App Center
if [ -z "$APP_SECRET" ]
then
    echo "You need define the APP_SECRET variable in App Center"
    exit
fi

# Look for MainActivity.cs
MAINACTIVITY_FILE=$APPCENTER_SOURCE_DIRECTORY/AppCenterTest/MainActivity.cs

if [ -e "$MAINACTIVITY_FILE" ]
then
    # Replace placeholder with env variable
    echo "Updating APP_SECRET to $APP_SECRET in MainActivity.cs"
    sed -i '' 's#APP_SECRET = "[-a-z0-9]*"#APP_SECRET = "'$APP_SECRET'"#' $MAINACTIVITY_FILE

    echo "File content:"
    cat $MAINACTIVITY_FILE
fi

# Look for AndroidManifest.xml
MANIFEST_FILE=$APPCENTER_SOURCE_DIRECTORY/AppCenterTest/Properties/AndroidManifest.xml

if [ -e "$MANIFEST_FILE" ]
then
    # Replace placeholder with env variable
    echo "Updating APP_SECRET to $APP_SECRET in AndroidManifest.xml"
    sed -i '' 's#msal[-a-z0-9]*#msal'$APP_SECRET'#' $MANIFEST_FILE

    echo "File content:"
    cat $MANIFEST_FILE
fi