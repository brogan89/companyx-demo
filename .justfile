# Removes all temp files and cleans project folder
@clean:
    rm *.csproj
    rm *.sln
    rm -rf Library
    rm -rf Temp
    rm -rf Logs
    rm -rf obj
