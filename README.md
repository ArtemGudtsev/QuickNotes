# QuickNotes
CLI to make quick notes directly from shell

## How to use

Create alias for tool:

$ alias qnotes=/e/src/local/QuickNotes/QuickNotes/bin/Debug/netcoreapp3.0/qnotes.exe

Use tool in quick mode (allow to leave just one message):

$ qnotes  
[23:10:14] Just one note about something

Use tool in mode of diary:

$ qnotes -a  
[23:12:01] This is diary mode  
[23:12:07] You can leave as much messages here as you want  
[23:12:20] You should type single 'x' to exit from this mode  
[23:12:42] 'x' will not be written to qnote file  
[23:12:54] let's try  
[23:12:57] x  
