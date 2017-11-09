#!/usr/bin/env bash

set -e

for dir in Components/*Test/
do
    cd ${dir} && dotnet test && cd -;
done
