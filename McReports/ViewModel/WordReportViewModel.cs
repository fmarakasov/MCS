using MCDomain.DataAccess;
using CommonBase;
using System;

namespace McReports.ViewModel
{
    public abstract class WordReportViewModel : BaseReportViewModel
    {
        protected WordReportViewModel(IContractRepository repository)
            : base(repository)
        {
            
        }

        protected override void OnDispose()
        {
            ReleaseWord();
            base.OnDispose();
        }


        private static Microsoft.Office.Interop.Word.Application _word;

        public static Microsoft.Office.Interop.Word.Application Word
        {
            get { return _word ?? (_word = new Microsoft.Office.Interop.Word.Application()); }
        }

        private static Microsoft.Office.Interop.Word.Document _doc;
        public static Microsoft.Office.Interop.Word.Document CurrentDocument
        {
            get { return _doc; }
        }


        public bool IsComposite { get; set; }

        public override void SetReport()
        {
            if (!IsComposite || _doc == null)
                _doc = Word.Documents.Add(ReportTemplate);
            else
            {
                if (IsComposite)
                {
                    Word.Selection.EndKey(Microsoft.Office.Interop.Word.WdUnits.wdStory);
                    Word.Selection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                    Word.Selection.EndKey(Microsoft.Office.Interop.Word.WdUnits.wdStory);
                    Word.Selection.InsertFile(ReportTemplate);
                }
            }
            
            BuildReport();      
        }

        public static void ReleaseWord()
        {
            _word = null;
            _doc = null;
        }

        public override void ShowReport()
        {
            Word.ActiveWindow.Activate();
            Word.Visible = true;
            base.ShowReport();
        }

        protected void InsertBigFieldIntoReport(string text, string replacetext)
        {
            const int maxLenth = 250;

            while (replacetext.Length > maxLenth)
            {
                var maxindex = maxLenth - text.Length;
                ReplaceText(text, replacetext.Substring(0, maxindex) + text);
                replacetext = replacetext.Remove(0, maxindex);
            }

            ReplaceText(text, replacetext);

        }

        protected void ReplaceText(string text, string replacetext)
        {
            const int maxLenth = 250;
            if (replacetext == null) replacetext = "";
            if (replacetext.Length > maxLenth) InsertBigFieldIntoReport(text, replacetext);
            else
            {
                Word.Selection.Find.ClearFormatting();
                Word.Selection.Find.Replacement.ClearFormatting();
                Word.Selection.Find.Text = text;
                Word.Selection.Find.MatchCase = true;
                Word.Selection.Find.MatchWholeWord = true;
                Word.Selection.Find.Forward = true;
                Word.Selection.Find.Replacement.Text = replacetext;

                try
                {
                    Word.Selection.Find.Execute(text, true, true, false, false, false, true,
                                                Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue, false, replacetext,
                                                Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll, false, false, false,
                                                false);
                }
                catch (Exception e)
                {
                    //TODO: Исправить Execute, что бы принимал длинные строки
                    System.Diagnostics.Debug.WriteLine("Исключение при выполнении замещении текста в объекте Word.Selection: " + e.Message);
                }
            }
        }


        //protected override bool CanSave()
        //{
        //    return IsComposite;
        //}

        protected override void Save()
        {
            Word.ActiveDocument.SaveAs(OutputFullFileName);
        }

        protected override string DefaultExt
        {
            get { return ".doc"; }
        }
    }
}