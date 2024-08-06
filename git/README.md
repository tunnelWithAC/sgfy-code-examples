A post-commit hook in Git is a script that runs after the commit process is completed. Here's how you can set it up:

1. Navigate to the `.git/hooks` directory in your Git repository. If you can't see the`.git` directory, it might be hidden. In that case, use `cd .git/hooks`in your terminal to navigate to the hooks directory.

2. In the `hooks` directory, there are sample scripts for various hooks. To create a post-commit hook, you can create a new file named `post-commit` (without any extension).

3. Open the `post-commit` file in a text editor and add your script. The script can be written in any scripting language. Here's an example of a simple bash script:

```
#!/bin/sh
echo "A commit has been made!"
```

4. Save the `post-commit` file and make it executable. You can do this with the following command in your terminal:

```
chmod +x post-commit
```

Now, every time you make a commit in this repository, your post-commit hook will run and print "A commit has been made!" to the console.

Remember, the hook is local to your Git repository and won't be shared when others clone the repository. If you want to share hooks, you might want to look into a solution like putting the hooks in a separate directory and using a script to symlink or copy them into `.git/hooks`.