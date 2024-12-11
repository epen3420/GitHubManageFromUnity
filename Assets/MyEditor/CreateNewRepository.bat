@echo off

curl -X POST https://api.github.com/user/repos -H "Authorization: token %1" -H "Content-Type: application/json" -d '{"name": "%2", "description": "%3", "private": %4}'
