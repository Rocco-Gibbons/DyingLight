#!/bin/sh
printf '\033c\033]0;%s\a' Dying Light
base_path="$(dirname "$(realpath "$0")")"
"$base_path/Dying Light.x86_64" "$@"
