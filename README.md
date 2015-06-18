OnlyText
========

[![Build status](https://ci.appveyor.com/api/projects/status/567ct14my06097xf?svg=true)](https://ci.appveyor.com/project/ferosekhanj/onlytext)

Somehow I felt a need for a simple editor that will allow me to concentrate on my writing. I checked out some of the distraction free editors and finally decided to write it on my own. During a lunch break I tested my wpf and wrote this. I am publishing this to github so that someone with a similar need can pull and use it from here. In my opinion wpf is really powerful and has high productivity compared to the previous UI technologies. The steep learning curve is worth every second.

It is a simple text editor that lets you concentrate only on your content. It runs in full screen. Your content is available at My Documents\Post.txt as text and as My Documents\Post.htm as html. You format your text easily by adding some markers (similar to writing in a wiki) and the editor takes care of converting it to html. See the formatting section below. In addition to this a basic spell-check too is supported (thanks to wpf).

Intentionally I have left out the following features

- No file support
- No search/replace support
- No window decoration
- No complex formatting. All you have is heading, list, ruler & pre formatted text

#Shortcut Keys

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


#Formatting

##Heading

Precede the content with !
you can added from a single ! to six !!!!!! depeding on the level of heading

##List

Precede the content with a #
You can nest list inside other by adding multiple # like ##

##Horizontal Ruler

For horizontal ruler just add --- in a separate line

##Preformatted content

Enclose the content between {{ }} 

{{

Any content here will appear as is

}}


     
     


