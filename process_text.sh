#!/bin/bash

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
CONTENT=$(cat "$INPUT_FILE")

# Process with ollama
ollama run llama2 "Process this text and provide analysis: $CONTENT" 