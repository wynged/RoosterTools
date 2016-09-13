using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace RoosterTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]


    public class CreateDetailItemsFromCAD : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            string folderToMakeDetails = @"D:\_code\VisualStudio\RoosterTools\TestFiles\CadBlocks";
            string detailFamilyTemplatePath = @"C:\ProgramData\Autodesk\RAC 2015\Family Templates\English_I\Detail Item.rft";

            List<string> cadPaths = FileOperations.GetFilePathsRecursively(folderToMakeDetails, "*.dwg");

            string resultString = "";

            foreach(string s in cadPaths)
            {
                Document doc = commandData.Application.Application.NewFamilyDocument(detailFamilyTemplatePath);

                Transaction t = new Transaction(doc, "AddCAD");
                t.Start();

                DWGImportOptions importOpts = new DWGImportOptions();
                importOpts.AutoCorrectAlmostVHLines = true;
                importOpts.ColorMode = ImportColorMode.BlackAndWhite;
                importOpts.Placement = ImportPlacement.Origin;
                DocumentPreviewSettings set = doc.GetDocumentPreviewSettings();
                ElementId previewId = doc.GetDocumentPreviewSettings().PreviewViewId;
                View starting = doc.GetElement(new ElementId(23)) as View;

                ElementId elemId = null;
                doc.Import(s, importOpts, starting, out elemId);

                t.Commit();

                string newpath = s.Replace(".dwg", ".rfa");
                newpath = newpath.Replace(".DWG", ".rfa"); 
                SaveAsOptions opts = new SaveAsOptions();
                opts.OverwriteExistingFile = true;
                doc.SaveAs(newpath);
                doc.Close(false);
                resultString += s + "\n";
            }

            TaskDialog.Show("results", resultString);

            return Result.Succeeded;
        }
    }
}
