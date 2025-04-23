#!/bin/bash
# One time commands
# brew install ollama
# ollama pull llama2

if [ "$#" -ne 1 ]; then
    echo "Usage: $0 <input_file>"
    exit 1
fi

INPUT_FILE=$1

if [ ! -f "$INPUT_FILE" ]; then
    echo "Error: File $INPUT_FILE does not exist"
    exit 1
fi

# Read the file content
# CONTENT=$(cat changes.txt)
CONTENT=$(cat "$INPUT_FILE")

# Process with ollama
ollama run llama2 "Process this text and provide analysis: $CONTENT" 

# or github copilot
# Install GitHub Copilot CLI
# npm install -g @githubnext/github-copilot-cli

# Use Copilot CLI
gh copilot suggest "Process this file: $CONTENT"