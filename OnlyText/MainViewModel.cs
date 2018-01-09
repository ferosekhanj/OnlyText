using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlyText
{
    public class MainViewModel :INotifyPropertyChanged
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
            if (string.IsNullOrEmpty(JustText))
                JustText = "Start typing your text here... Press F1 for help...";

            CreateCommands();
        }
        private void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string justText;
        public string JustText
        {
            get => justText;
            set
            {
                if (justText == value)
                    return;
                justText = value;
                RaisePropertyChanged();
            }
        }

        int fontSize;
        public int FontSize
        {
            get => fontSize;
            set
            {
                if (fontSize == value)
                    return;
                fontSize = value;
                RaisePropertyChanged();
            }
        }

        bool spellCheck;
        public bool SpellCheck
        {
            get => spellCheck;
            set
            {
                if (spellCheck == value)
                    return;
                spellCheck = value;
                RaisePropertyChanged();
            }
        }

        Thickness padding;
        public Thickness Padding
        {
            get => padding;
            set
            {
                if (padding == value)
                    return;
                padding = value;
                RaisePropertyChanged();
            }
        }

        private void CreateCommands()
        {
            NewCommand = new RelayCommand<object>(_ => OnNewCommand());
            SaveCommand = new RelayCommand<object>(_ => OnSaveCommand());
            PreviewCommand = new RelayCommand<object>(_ => OnPreviewCommand());
            IncreaseFontSizeCommand = new RelayCommand<object>(_ => OnIncreaseFontSizeCommand());
            DecreaseFontSizeCommand = new RelayCommand<object>(_ => OnDecreaseFontSizeCommand());
            IncreasePaddingCommand = new RelayCommand<object>(_ => OnIncreasePaddingCommand());
            DecreasePaddingCommand = new RelayCommand<object>(_ => OnDecreasePaddingCommand());
            ToggleSpellCheckCommand = new RelayCommand<object>(_ => OnToggleSpellCheckCommand());
            ExitHelpTextCommand = new RelayCommand<object>(_ => OnExitHelpTextCommand());
            HelpTextCommand = new RelayCommand<object>(_ => OnHelpTextCommand());
        }

        public RelayCommand<object> NewCommand
        {
            get;
            set;
        }

        public void OnNewCommand()
        {
            myBackupText = null;
            JustText = "Start typing your text here... Press F1 for help...";
        }

        public RelayCommand<object> SaveCommand
        {
            get;
            set;
        }
        public void OnSaveCommand()
        {
            if (!string.IsNullOrEmpty(myBackupText))
            {
                OnExitHelpTextCommand();
            }

            myConvertor.ConvertAndSave(JustText);
            File.WriteAllText(myFilePath, JustText);
        }

        public RelayCommand<object> PreviewCommand
        {
            get;
            set;
        }
        public void OnPreviewCommand()
        {
            myConvertor.ConvertAndSave(JustText);
            Process.Start(myHtmlFilePath);
        }
        public RelayCommand<object> IncreaseFontSizeCommand
        {
            get;
            set;
        }
        public void OnIncreaseFontSizeCommand() => FontSize++;

        public RelayCommand<object> DecreaseFontSizeCommand
        {
            get;
            set;
        }
        public void OnDecreaseFontSizeCommand() => FontSize--;

        public RelayCommand<object> IncreasePaddingCommand
        {
            get;
            set;
        }
        public void OnIncreasePaddingCommand() => Padding = new Thickness(Padding.Left + 5, 5, Padding.Right + 5, 5);

        public RelayCommand<object> DecreasePaddingCommand
        {
            get;
            set;
        }
        public void OnDecreasePaddingCommand()
        {
            if (Padding.Left == 0)
                return;
            Padding = new Thickness(Padding.Left - 5, 5, Padding.Right - 5, 5);
        }
        public RelayCommand<object> ToggleSpellCheckCommand
        {
            get;
            set;
        }
        public void OnToggleSpellCheckCommand() => SpellCheck = !SpellCheck;

        public RelayCommand<object> ExitHelpTextCommand
        {
            get;
            set;
        }

        public void OnExitHelpTextCommand()
        {
            if (myBackupText == null)
                return;

            JustText = myBackupText;
            myBackupText = null;
        }

        public RelayCommand<object> HelpTextCommand
        {
            get;
            set;
        }
        string myBackupText;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnHelpTextCommand()
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

- Ferose Khan J
";
        }
    }
}
