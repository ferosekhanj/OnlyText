using Commander;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlyText
{
    [ImplementPropertyChanged]
    public class MainViewModel
    {
        string myFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Post.txt");
        string myHtmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Post.htm");

        TextToHtml myConvertor;

        public MainViewModel()
        {
            myConvertor = new TextToHtml(myHtmlFilePath);
            FontSize = 14;
            Padding = new Thickness(20, 5, 20, 5);
            if (File.Exists(myFilePath))
            {
                JustText = File.ReadAllText(myFilePath);
            }
            if(string.IsNullOrEmpty(JustText))
                JustText = "Start typing your text here... Press F1 for help...";
        }

        public string JustText { get; set; }

        public int FontSize { get; set; }

        public bool SpellCheck { get; set; }

        public Thickness Padding { get; set; }

        [OnCommand("NewCommand")]
        public void NewCommand()
        {
            myBackupText = null;
            JustText = "Start typing your text here... Press F1 for help...";
        }

        [OnCommand("SaveCommand")]
        public void SaveCommand()
        {
            if (!string.IsNullOrEmpty(myBackupText))
            {
                ExitHelpTextCommand();
            }

            myConvertor.ConvertAndSave(JustText);
            File.WriteAllText(myFilePath, JustText);
        }

        [OnCommand("PreviewCommand")]
        public void PreviewCommand()
        {
            myConvertor.ConvertAndSave(JustText);
            Process.Start(myHtmlFilePath);
        }
        [OnCommand("IncreaseFontSizeCommand")]
        public void IncreaseFontSizeCommand()
        {
            FontSize++;
        }
        [OnCommand("DecreaseFontSizeCommand")]
        public void DecreaseFontSizeCommand()
        {
            FontSize--;
        }
        [OnCommand("IncreasePaddingCommand")]
        public void IncreasePaddingCommand()
        {
            Padding = new Thickness(Padding.Left+5,5,Padding.Right+5,5);
        }
        [OnCommand("DecreasePaddingCommand")]
        public void DecreasePaddingCommand()
        {
            if (Padding.Left == 0)
                return;
            Padding = new Thickness(Padding.Left - 5, 5, Padding.Right - 5, 5);
        }
        [OnCommand("ToggleSpellCheckCommand")]
        public void ToggleSpellCheckCommand()
        {
            SpellCheck = !SpellCheck;
        }

        [OnCommand("ExitHelpTextCommand")]
        public void ExitHelpTextCommand()
        {
            if (myBackupText == null)
                return;

            JustText = myBackupText;
            myBackupText = null;
        }

        string myBackupText;
        [OnCommand("HelpTextCommand")]
        public void HelpTextCommand()
        {
            if (!string.IsNullOrEmpty(myBackupText))
                return;

            myBackupText = JustText;
            JustText = @"!Only Text Help
---

Press Esc to exit this page and get back to your content. Press f5 to view this page as html.

!Introduction

This is a simple text editor that lets you concentrate only on your content. It runs in full screen. Your content is available at <My Documents Folder>\Post.txt as text and as <My Documents Folder>\Post.htm as html. You format your text easily and the editor takes care of converting it to html. See the formatting section below. In addition to this a basic spell-check too is supported.

!Formatting

!!Heading

Precede the content with !
For eg

!This is level one heading
!!This is level two heading
!!!This is level three heading
!!!!This is level four heading
!!!!!This is level five heading
!!!!!!This is level six heading

!!List

Precede the content with a #

# This is a level one list
## This is a sub list under level one list
# This is level one list item again

!!Horizontal Ruler

For horizontal ruler just add --- in a line
--- 

!!Preformatted content

Enclose the content between {{ }} 

{{

Any content here will appear as is

}}


!!Shortcut Keys

{{

Ctrl + s - Save the current content.
Ctrl + n - Start a new content.

Ctrl + x - Cut.
Ctrl + c - Copy.
Ctrl + v - Paste.

Ctrl + z - Undo.
Ctrl + y - Redo.
Alt + f4 - Close application.
 
     Esc - Exit help
     F1  - Display this help text.
     F2  - Increase the font size.
     F3  - Decrease the font size.
     F4  - Toggle spellchecking.
     F5  - Preview the content as HTML.
     F6  - Increase the text area.
     F7  - Decrease the text area.
}}

";
        }
    }
}
