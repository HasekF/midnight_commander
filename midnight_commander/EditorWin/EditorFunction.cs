using System;
using System.Collections.Generic;
using System.Text;
using midnight_commander.PopUpWindows;

namespace midnight_commander.EditorWin
{
    public class EditorFunction
    {
        public PopUpWindow PopUp { get; set; }
        public List<string> TextLines { get; set; }
        public int SelectedLine { get; set; }
        public int SelectedChar { get; set; }
        public Select Select { get; set; }
        public event Action ClosePopUp;

        public List<string> Copy(Select select, List<string> textLines, int selectedChar, int selectedLine)
        {
            List<string> lines = GetSelectedLines(select, textLines);
            string end = "";
            for (int i = selectedLine; i <= selectedLine + (select.StopLine - select.StartLine); i++)
            {
                if (selectedLine == i)
                {
                    if (select.StartLine == select.StopLine)
                    {
                        textLines[i] = textLines[i].Insert(selectedChar, lines[i - selectedLine]);
                    }
                    else
                    {
                        textLines[i] = textLines[i].Insert(selectedChar, lines[i - selectedLine]);
                        end = textLines[i].Substring(selectedChar + lines[i - selectedLine].Length);
                        textLines[i] = textLines[i].Substring(0, textLines[i].Length - end.Length);
                    }
                }
                else if (i == selectedLine + (select.StopLine - select.StartLine))
                {
                    textLines.Insert(i, lines[i - selectedLine] + end);
                }
                else
                {
                    textLines.Insert(i, lines[i - selectedLine]);
                }
            }
            return textLines;
        }
        public List<string> Move(Select select, List<string> textLines, int selectedChar, int selectedLine)
        {
            //List<string> lines = GetSelectedLines(select, textLines);
            textLines = Copy(select, textLines, selectedChar, selectedLine);
            textLines = DeletePart(select, textLines);

            int deletedLines = 0;
            for (int i = select.StartLine; i <= select.StopLine; i++)
            {
                if (textLines.Count == i) { break; }
                if (textLines[i] == "") { textLines.RemoveAt(i); i--; deletedLines++; }
            }
            //selectedLine -= deletedLines;
            //select.StartChar = selectedChar;
            //select.StartLine = selectedLine;
            //select.StopChar = lines[lines.Count-1].Length;
            //select.StopLine = select.StartLine + lines.Count;

            return textLines;
        }
        public void Search (Select select, List<string> textLines)
        {
            Select = select;
            PopUp = new PopUpWindow();
            PopUp.Done += SearchString;
            TextLines = textLines;
            PopUp.CreateEditorPopUp(PopUpType.EDITOR_SEARCH);
            
        
        }
        public void DrawPopUp()
        {
            PopUp.Regenerate();
        }
        private void SearchString(List<string> input)
        {
            if(input[0] == "OK")
            {
                for (int i = SelectedLine; i < TextLines.Count-1; i++)
                {
                    if (TextLines[i].IndexOf(input[1], SelectedChar) != -1)
                    {
                        Select.Active = true;
                        Select.StartLine = i;
                        Select.StopLine = i;
                        Select.StartChar = TextLines[i].IndexOf(input[1], SelectedChar);
                        Select.StopChar = input[1].Length + Select.StartChar;
                        ClosePopUp();
                    }
                }
            }
        }
        public void HandleKey(ConsoleKeyInfo key)
        {
            PopUp.HandleKey(key);
        }
        public List<string> Delete(Select select, List<string> textLines)
        {

            textLines = DeletePart(select, textLines);
            textLines = CleanPart(select, textLines);
            return textLines;
        }
        //**********************************************************************************
        private List<string> DeletePart(Select select, List<string> textLines)
        {
            for (int i = select.StartLine; i <= select.StopLine; i++)
            {
                if (select.StartLine == i)
                {
                    if (select.StartLine == select.StopLine)
                    {
                        textLines[i] = textLines[i].Remove(select.StartChar, select.StopChar - select.StartChar);
                    }
                    else
                    {
                        textLines[i] = textLines[i].Remove(select.StartChar);
                    }
                }
                else if (i == select.StopLine)
                {
                    textLines[i] = textLines[i].Remove(0, select.StopChar);
                }
                else
                {
                    textLines[i] = "";
                }
            }
            return textLines;
        }
        private List<string> CleanPart(Select select, List<string> textLines)
        {
            for (int i = select.StartLine; i <= select.StopLine; i++)
            {
                if (textLines.Count == i) { break; }
                if (textLines[i] == "") { textLines.RemoveAt(i); i--; }
            }
            return textLines;
        }
        private List<string> GetSelectedLines(Select select, List<string> textLines)
        {
            List<string> selectedText = new List<string>();
            for (int i = select.StartLine; i <= select.StopLine; i++)
            {
                if(select.StartLine == i)
                {
                    if(select.StartLine == select.StopLine)
                    {
                        selectedText.Add(textLines[i].Substring(select.StartChar, select.StopChar));
                    }
                    else
                    {
                        selectedText.Add(textLines[i].Substring(select.StartChar));
                    }
                }
                else if(i == select.StopLine)
                {
                    selectedText.Add(textLines[i].Substring(0,select.StopChar));
                }
                else
                {
                    selectedText.Add(textLines[i]);
                }
            }
            return selectedText;
        }
    }
}
